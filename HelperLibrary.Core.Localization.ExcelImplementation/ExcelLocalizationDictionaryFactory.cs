/* 
 * FileName:    ExcelLocalizationDictionaryFactory.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2017/2/14 11:36:59
 * Version:     v1.0
 * Description:
 * */

using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Localization
{
    /// <summary>
    /// 
    /// </summary>
    public class ExcelLocalizationDictionaryFactory : ILocalizationDictionaryFactory
    {
        private readonly string filePath;
        private readonly ILocalizationColumnSelector columnSelector;

        private DataFormatter formatter = new DataFormatter();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">localization file path, XLS or XLSX</param>
        /// <param name="columnSelector">a column selector for identifying columns.
        /// If null was passed, then NamedLocalizationColumnSelector will be used</param>
        public ExcelLocalizationDictionaryFactory(string filePath, ILocalizationColumnSelector columnSelector = null)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");
            if (columnSelector == null)
            {
                columnSelector = NamedLocalizationColumnSelector.Instance;
            }
            this.filePath = filePath;
            this.columnSelector = columnSelector;
        }

        /// <summary>
        /// create localization dictionnary for specified culture。
        /// </summary>
        /// <param name="cultureName">culture name , for example "zh-CN"</param>
        /// <returns></returns>
        public ILocalizationDictionary CreateLocalizationDictionary(string cultureName)
        {
            if (cultureName == null)
                throw new ArgumentNullException("cultureName");

            var list = LoadLocalizationData(cultureName);
            return new LocalizationDictionary(cultureName, list);
        }

        private IEnumerable<LocalizationItem> LoadLocalizationData(string cultureName)
        {
            string xlsPath = Path.GetFullPath(filePath);

            if (!File.Exists(xlsPath))
                throw new FileNotFoundException("localization file not found", xlsPath);

            var wb = WorkbookFactory.Create(filePath);
            var items = new List<LocalizationItem>();
            var enumerator = wb.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var sheet = (ISheet)enumerator.Current;
                var rows = ReadSheet(sheet);
                if (!rows.Any())
                {
                    // sheet is empty
                    continue;
                }
                var firstLine = rows.First();

                Tuple<int, int> dataIndexes = GetIndexes(firstLine, cultureName);
                int keyCol = dataIndexes.Item1;
                int cultureCol = dataIndexes.Item2;
                rows.Remove(firstLine);
                foreach (var row in rows)
                {
                    string key = row[keyCol];
                    string value = row[cultureCol];

                    var item = new LocalizationItem(sheet.SheetName, key, value);
                    items.Add(item);
                }
            }
            return items;
        }

        private Tuple<int, int> GetIndexes(string[] firstLine, string cultureName)
        {
            int keyColumn = -1;
            int cultureColumn = -1;

            for (int i = 0; i < firstLine.Length; i++)
            {
                var columnInfo = new ColumnInfo(i, firstLine[i]);

                if (keyColumn < 0 && columnSelector.IsKeyColumn(columnInfo))
                    keyColumn = i;

                if (cultureColumn < 0 && columnSelector.IsCultureColumn(cultureName, columnInfo))
                    cultureColumn = i;
            }
            if (keyColumn < 0)
            {
                throw new InvalidDataException("data format is not correct, 'Key' column not found");
            }
            if (cultureColumn < 0)
            {
                throw new InvalidDataException("data format is not correct, column for specified culture not found. The specified culture is " + cultureName);
            }
            return Tuple.Create(keyColumn, cultureColumn);
        }

        private List<string[]> ReadSheet(ISheet sheet)
        {
            var dict = new List<string[]>();
            var rowEnumerator = sheet.GetRowEnumerator();
            
            while (rowEnumerator.MoveNext())
            {
                var row = (IRow)rowEnumerator.Current;
                var rowContents = row.Cells.Select(c => formatter.FormatCellValue(c)).ToArray();
                dict.Add(rowContents);
            }
            return dict;
        }
    }
}

using HelperLibrary.Core.Db;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Core.Db
{   
    public class Program
    {
        private static Dictionary<string, string> tableCreation = new Dictionary<string, string>()
        {
            ["MySql"]       = @"CREATE TABLE users (
                                    id INT NOT NULL AUTO_INCREMENT,
                                    Name VARCHAR(50) NOT NULL DEFAULT '0',
                                    Age INT NOT NULL DEFAULT '0',
                                    PRIMARY KEY(id)
                                );",
            ["SqlServer"]   = @"CREATE TABLE users (
                                    id INT NOT NULL IDENTITY(1, 1),
                                    Name VARCHAR(50) NOT NULL DEFAULT '0',
                                    Age INT NOT NULL DEFAULT '0',
                                    PRIMARY KEY(id)
                                );",
            ["SQLite"] = @"CREATE TABLE `users` (
                                    `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                    `Name` Text NOT NULL,
                                    `Age` INTEGER NOT NULL
                                );",
        };


        private static void Main(string[] args)
        {
            /* 此示例代码需要连接一个MySql数据库，请自行搭建一个并创建好数据库实例。
             * 然后修改配置文件中的连接字符串)。
             */
            const string dbName = "SQLite";
            IDbConnectionFactory connectionFactory = GetConnectionFactory(dbName);

            Console.WriteLine("连接字符串:{0}", connectionFactory.ConnectionString);

            var dbInvoker = new DbOperationInvoker(connectionFactory);

            //string sql = @"select table_name from INFORMATION_SCHEMA.TABLES 
            //                where table_type='base table' and table_name='users'";

            //dbInvoker.ExecuteNonQuery("create database mydb");

            string sql = @"select name from sqlite_master 
                            where type='table' and name='users'";

            if (dbInvoker.ExecuteScalar<string>(sql) != null)
            {
                // 如果已经存在，则先删除数据表
                sql = "DROP TABLE users;";
                try
                {
                    dbInvoker.ExecuteNonQuery(sql);
                    Console.WriteLine("删除旧表成功");
                }
                catch (Exception)
                {
                    Console.WriteLine("删除旧表失败");
                }
            }

            // 创建数据表
            sql = tableCreation[dbName];
            try
            {

                dbInvoker.ExecuteNonQuery(sql);
                Console.WriteLine("创建表users成功");
            }
            catch (Exception)
            {
                Console.WriteLine("创建表users失败");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\n=====Insert插入数据测试=====");

            // insert语句，使用了命名参数传值
            sql = @"INSERT INTO users (Name, Age) VALUES(@name, @age);";
            var parameters = new Dictionary<string, object>()
            {
                {"@name", "John"},
                {"@age", 23},
            };
            // 可选参数较多，可以使用命名参数的方式传递参数
            if (dbInvoker.ExecuteNonQuery(sql, useTransaction: true, parameters: parameters) == 1)
            {
                Console.WriteLine("写入数据成功");
            }
            else
            {
                Console.WriteLine("写入数据失败");
            }
            Console.WriteLine("=====修改参数，插入新的行=====");
            parameters["@name"] = "Tom";
            parameters["@age"] = 32;
            if (dbInvoker.ExecuteNonQuery(sql, parameters) == 1)
            {
                Console.WriteLine("写入数据成功");
            }
            else
            {
                Console.WriteLine("写入数据失败");
            }
            Console.WriteLine("=====修改参数，插入新的行=====");
            parameters["@name"] = "Jack";
            parameters["@age"] = 12;
            if (dbInvoker.ExecuteNonQuery(sql, parameters) == 1)
            {
                Console.WriteLine("写入数据成功");
            }
            else
            {
                Console.WriteLine("写入数据失败");
            }

            Console.WriteLine("=====不使用SQL直接把对象插入到数据库中（新增功能）=====");

            var dave = new Person()
            {
                Name = "Dave",
                Age = 18
            };

            if (dbInvoker.InsertEntity(dave) == 1)
            {
                Console.WriteLine("写入数据成功");
            }
            else
            {
                Console.WriteLine("写入数据失败");
            }

            Console.WriteLine("===============\n");

            Console.WriteLine("\n======查询全部数据====");
            sql = @"SELECT id, Name, Age FROM users;";
            var dataSet = dbInvoker.ExecuteQuery(sql);
            var dt = dataSet.Tables[0];
            // 映射数据表为对象集合
            List<Person> list = dt.ToEntities<Person>().ToList();

            // 也可以像下面这样使用ORM风格的扩展方法
            // list = dbInvoker.QueryMany<Person>(sql).ToList();
            Console.WriteLine("===============");
            Console.WriteLine("id\tName\tAge");
            foreach (var entity in list)
            {
                Console.WriteLine("{0}\t{1}\t{2}", entity.Id, entity.Name, entity.Age);
            }
            Console.WriteLine("===============\n");

            int index = 1;
            Console.WriteLine("=====只映射一条记录，比如第{0}条=====", index + 1);
            var second = dt.Rows[index].ToEntity<Person>();
            Console.WriteLine("id\tName\tAge");

            Console.WriteLine("{0}\t{1}\t{2}", second.Id, second.Name, second.Age);

            Console.WriteLine("===============\n");

            Console.WriteLine("=====不使用SQL命令直接修改Tom的年龄=====");
            second.Age = 28;
            if (dbInvoker.UpdateEntity(second, new[] { nameof(Person.Age) }) == 1)
            {
                Console.WriteLine("修改成功");
            }
            else
            {
                Console.WriteLine("修改失败");
            }

            Console.WriteLine("=====重新查询=====");
            sql = "SELECT * FROM users WHERE name = @name";
            parameters = new Dictionary<string, object>()
            {
                {"@name", "Tom"}
            };
            var result = dbInvoker.ExecuteQuery(sql, parameters);
            var tom = result.Tables[0].Rows[0].ToEntity<Person>();
            Console.WriteLine("id\tName\tAge");

            Console.WriteLine("{0}\t{1}\t{2}", tom.Id, tom.Name, tom.Age);
            Console.WriteLine("===============\n");

            Console.WriteLine("=====不使用SQL删除Tom的记录=====");
            // 注意删除是根据主键值查找删除的，此处为id属性。
            if (dbInvoker.DeleteEntity(tom) == 1)
            {
                Console.WriteLine("删除成功");
            }
            else
            {
                Console.WriteLine("删除失败");
            }

            Console.WriteLine("\n======查询全部数据====");
            sql = @"SELECT id, Name, Age FROM users;";
            dataSet = dbInvoker.ExecuteQuery(sql);
            dt = dataSet.Tables[0];
            // 映射数据表为对象集合
            list = dt.ToEntities<Person>().ToList();

            Console.WriteLine("===============");
            Console.WriteLine("id\tName\tAge");
            foreach (var entity in list)
            {
                Console.WriteLine("{0}\t{1}\t{2}", entity.Id, entity.Name, entity.Age);
            }
            Console.WriteLine("===============\n");

            Console.WriteLine();
            Console.WriteLine("按Enter键结束...");
            Console.ReadLine();
        }

        private static IDbConnectionFactory GetConnectionFactory(string databaseName)
        {
            if (databaseName == "MySql")
            {
                string connString = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
                return new MySqlDbConnectionFactory(connString);
            }
            if (databaseName == "SqlServer")
            {
                string connString = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                return new SqlServerDbConnectionFactory(connString);
            }
            if (databaseName == "SQLite")
            {
                string connString = ConfigurationManager.ConnectionStrings["SQLite"].ConnectionString;
                return new SqliteDbConnectionFactory(connString);
            }
            // 将来也可以实现其他的数据库，例如SQL Server， Oracle DB等
            throw new NotSupportedException("尚不支持此类型:" + databaseName);
        }
    }
}

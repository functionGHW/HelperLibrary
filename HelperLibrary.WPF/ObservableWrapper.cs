using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.WPF
{
    public class ObservableWrapper : DynamicObject, INotifyPropertyChanged
    {
        private readonly object model;
        private readonly Type modelType;

        public ObservableWrapper(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.model = model;
            this.modelType = model.GetType();
        }

        public object Model { get { return model; } }

        public event PropertyChangedEventHandler PropertyChanged;

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            SetModelValue(indexes[0].ToString(), value);
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = GetModelValue(indexes[0].ToString());
            return true;
        }

        public new Type GetType()
        {
            return modelType;
        }


        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void SetModelValue(string propName, object value)
        {
            var prop = modelType.GetProperty(propName);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(model, value);
                OnPropertyChanged(propName);
                return;
            }
            throw new InvalidOperationException();
        }

        private object GetModelValue(string propName)
        {
            var prop = modelType.GetProperty(propName);
            if (prop != null && prop.CanRead)
            {
                return prop.GetValue(model);
            }
            throw new InvalidOperationException();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetModelValue(binder.Name);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SetModelValue(binder.Name, value);
            return true;
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            return base.TryInvoke(binder, args, out result);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var m = modelType.GetMethod(binder.Name);
            if (m != null)
            {
                result = m.Invoke(model, args);
                return true;
            }
            return base.TryInvokeMember(binder, args, out result);
        }

        public override string ToString()
        {
            return model.ToString();
        }

        public override bool Equals(object obj)
        {
            return model.Equals(obj);
        }

        public override int GetHashCode()
        {
            return model.GetHashCode();
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (model.GetType() == binder.Type)
            {
                result = model;
                return true;
            }
            return base.TryConvert(binder, out result);
        }
    }
}

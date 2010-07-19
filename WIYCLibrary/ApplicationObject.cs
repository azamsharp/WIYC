namespace WIYCLibrary
{
    using System;

    [Serializable]
    public class ApplicationObject : IObject
    {
        private string _key;
        private double _size;
        private string _typeName;
        private object _value;

        public ApplicationObject(string key, object value, string typeName)
        {
            this._key = key;
            this._value = value;
            this._typeName = typeName;
        }

        public string Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        public double Size
        {
            get
            {
                return this._size;
            }
            set
            {
                this._size = value;
            }
        }

        public string TypeName
        {
            get
            {
                return this._typeName;
            }
            set
            {
                this._typeName = value;
            }
        }

        public object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
    }
}


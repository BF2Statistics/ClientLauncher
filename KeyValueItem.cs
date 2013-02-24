using System;

namespace BF2statisticsLauncher
{
    class KeyValueItem
    {
        private string _key = "";
        private string _value = "";

        public string Key
        { 
            get{ return _key; } 
        }

        public string Value
        {
            get { return _value; }
        }

        public KeyValueItem(string Key, string Value)
        {
            this._key = Key;
            this._value = Value;
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}

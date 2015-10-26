using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LoveBank.Common.Plugins
{
    [Serializable]
    public class CheckBoxItem
    {
        public CheckBoxItem() {
            
        }

        public CheckBoxItem(string key,string value) {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public bool Checked { get; set; }
    }

    [Serializable]
    public class CheckBoxGroup : Dictionary<string, CheckBoxItem>
    {
       
        public CheckBoxGroup Add(string key,string value) {
            if(this.ContainsKey(key)) {
                this[key].Value = value;
            }else {
                this.Add(key,new CheckBoxItem(key,value));
            }
            return this;
        }

        public void SetChecked(string key,bool isSelect) {
            
            if (!this.ContainsKey(key)) return;

            this[key].Checked = isSelect;
        }
    }

    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        public SerializableDictionary() {
            
        }

        public SerializableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> old)
        {
            this.AddRange(old);
        } 

        #region IXmlSerializable 成员

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));
            var isEmpty = reader.IsEmptyElement;
            reader.Read();
            if (isEmpty)
                return;
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                var key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                var value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                this.Add(key, value);
                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();

        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));
            foreach (var key in this.Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
                writer.WriteStartElement("value");
                var value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

        }

        #endregion
    }
}

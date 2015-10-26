using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoveBank.Common.Plugins {

    public class DynamicConfig {

        #region AttributeType enum

        public enum AttributeType {
            Text = 0,
            Select = 1,
            Image = 2,
            RichText = 3,
            Password = 4,
            TextArea = 5,
            CheckBox = 6
        }

        #endregion

        public DynamicConfig() {
        }

        public DynamicConfig(string key,string title, AttributeType input, object value = null) {
            Key = key;
            Title = title;
            Type = input;
            Value = value;
        }

        public string Title { get; set; }

        public AttributeType Type { get; set; }

        public string Key { get; set; }

        public object Value { get; set; }

        public static string ConvertJson(IDictionary<string, DynamicConfig> config) {
            return JsonConvert.SerializeObject(config);
        }

        public static IDictionary<string, DynamicConfig> ConvertObject(string config) {
            var data = JsonConvert.DeserializeObject<IDictionary<string, DynamicConfig>>(config);

            foreach (var item in data) {
                if (item.Value.Type == AttributeType.CheckBox) {
                    item.Value.Value = JsonConvert.DeserializeObject<CheckBoxGroup>(item.Value.Value.ToString());
                }
            }

            return data;
        }
    }
}
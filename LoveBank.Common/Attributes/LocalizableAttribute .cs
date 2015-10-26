using System;

namespace LoveBank.Common
{
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
    public class LocalizableAttribute : Attribute
    {
        public LocalizableAttribute(string defaultValue)
        {
            Default = defaultValue;
        }
        public LocalizableAttribute(Type language)
        {
            LanguageType = language;
        }
        public LocalizableAttribute(Type language, String prefix)
            : this(language)
        {
            Prefix = prefix;
        }

        public Type LanguageType
        {
            get;
            set;
        }

        public string Prefix
        {
            get;
            set;
        }

        public string Default { get; set; }
    }
}

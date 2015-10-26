using System;

namespace LoveBank.Common
{
    public class ParamPair : IComparable<ParamPair>
    {
        public ParamPair()
        {
        }

        public ParamPair(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }


        public int CompareTo(ParamPair other)
        {
            if (null == Name)
            {
                if (null == other.Name)
                {
                    if (null == Value)
                        return null == other.Value ? 0 : -1;
                    return null == other.Value ? 1 : String.Compare(Value, other.Value, StringComparison.Ordinal);
                }
                return -1;
            }
            var equal = String.Compare(Name, other.Name, StringComparison.Ordinal);

            if (0 == equal)
                equal = String.Compare(Value, other.Value, StringComparison.Ordinal);

            return equal;
        }
    }
}
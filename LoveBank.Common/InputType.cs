namespace LoveBank.Common
{
    public static class InputType
    {

        public const int Text = 0;

        public const int Select = 1;

        public const int Image = 2;

        public const int RichText = 3;

        public const int Password = 4;

        public const int TextArea = 5;

        public const int CheckBox = 6;

        public static string GetName(int type)
        {
            switch (type)
            {
                case Text:
                    return "Text";
                case Select:
                    return "Select";
                case Image:
                    return "Image";
                case RichText:
                    return "RichText";
                case Password:
                    return "Password";
                case TextArea:
                    return "TextArea";
                case CheckBox:
                    return "CheckBox";
                default:
                    return "Text";
            }
        }

       public static int[] GetTypes()
       {
           return new[]
                      {
                          Text,Select,Image,RichText,Password,TextArea,CheckBox
                      };
       }
    }
}

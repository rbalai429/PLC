using System.ComponentModel;

namespace HPPlc.Models.WhatsApp
{
    public enum TemplateTypeEnum
    {
        [Description("plc_free_week_1")]
        plc_free_week_1 = 1,
        [Description("plc11")]
        plc11 = 2,
        [Description("plc12")]
        plc12 = 3
    }

    public static class MyEnumExtensions
    {
        public static string ToDescriptionString(this TemplateTypeEnum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
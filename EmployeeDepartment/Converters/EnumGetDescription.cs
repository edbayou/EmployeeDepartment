using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDepartment.Converter
{
    public static class EnumGetDescription
    {
        public static string GetDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (descriptionAttributes.Length == 0)
            {
                return enumValue.ToString();
            }
            else
            {
                DescriptionAttribute attrib = descriptionAttributes[0] as DescriptionAttribute;
                return attrib.Description;
            }

         
        }
    }
}

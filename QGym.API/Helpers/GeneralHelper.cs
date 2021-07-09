using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QGym.API.Helpers
{
    public class GeneralHelper
    {
        public string GetOnlyNumber(string str)
        {
            string nums = string.Empty;
            /* //   Regresa solo el primer bloque
            Match m = Regex.Match(str, "(\\d+)"); 

            if (m.Success)
                nums = m.Value;
            */

            // "(?:- *)?\\d+(?:\\.\\d+)?" Regresa numeros con negativo y punto decimal
            Regex regex = new Regex("(\\d+)"); 

            MatchCollection matches = regex.Matches(str);

            foreach (Match m in matches)
                nums += m.Value;

            return nums;
        }

        public string AplayRegex(string str, string regexFormat)
        {

            Match v = Regex.Match(str, regexFormat);
            string result = string.Empty;

            if (v.Success)
                result = v.Value;

            return result;
        }
    }
}

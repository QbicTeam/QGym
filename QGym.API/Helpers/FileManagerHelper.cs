using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QGym.API.Helpers
{
    public class FileManagerHelper
    {
        public void RecordLogFile(string methodName, object objectSend, Exception ex, string valuesUri = null)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string logTest = basePath + string.Format("LogError{0}.txt",DateTime.Today.ToString("yyyyMMdd"));
            string textAdd = "----------------------------------------------------------------------" + Environment.NewLine
                + DateTime.Now.ToString("yyyyMMdd HH:mm") + Environment.NewLine
                + "Method: " + methodName + Environment.NewLine
                + "   Sended Information: (Serializado)"
                + Environment.NewLine + (valuesUri != null ? "Values in Uri: " + valuesUri + ";  " : "") 
                + JsonConvert.SerializeObject(objectSend) + Environment.NewLine
                + "Error:" + Environment.NewLine
                + ex.Source + Environment.NewLine
                + ex.Message + Environment.NewLine
                + JsonConvert.SerializeObject(ex);

            System.IO.File.AppendAllLines(logTest, new String[] { textAdd });
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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


        private void SampleToCreateCSVFile()
        {
            
            // Funciona... genera un archivo .csv y lo graba en disco.
            var filepath = @"C:\Temp\TestReport.csv";
            using (StreamWriter writer = new StreamWriter(new FileStream(filepath,
            FileMode.Create, FileAccess.Write)))
            {
                writer.WriteLine("sep=,");
                writer.WriteLine("Encabezados");
                writer.WriteLine("Titulo 1, Titulo 2");
                writer.WriteLine("Hello, Goodbye");
                writer.WriteLine("Registro 2-1, Registro 2-2");
                writer.WriteLine("Registro 3-1, Registro 3-2");
            }
        }

        private void SampleToReturnFileToDownload()
        {

            // HttpGet Metod return type: Task<FileResult>
            // using Microsoft.AspNetCore.Mvc;

            /*
            var builder = new StringBuilder();
            builder.AppendLine("Id,Username");
            foreach (var user in users)
            {
                builder.AppendLine($"{user.Id},{user.Username}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "users.csv");
            */


            /*
            string fileName = "RepoteTest.csv";

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write("Hello, World!");
            writer.Flush();
            stream.Position = 0;


            // return File(stream, "text/csv", fileName);
            */
        }

    }
}

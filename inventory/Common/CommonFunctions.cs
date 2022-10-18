using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace inventory.Common
{
    public class CommonFunctions
    {
        public void BindParameter(MySqlCommand oCommand, string parameterName, System.Data.DbType dbType, Object value)
        {
            oCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = parameterName,
                DbType = dbType,
                Value = value

            });
        }
        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public void CreateLog(string txtLog)
        {
            try
            {
                // Set a variable to the Documents path.
                string docPath = Environment.CurrentDirectory + "/Logs/";

                if (!Directory.Exists(docPath))
                {
                    Directory.CreateDirectory(docPath);
                }

                // Append text to an existing file named "WriteLines.txt".
                string filename = DateTime.Today.ToString("yyyyMMdd") + ".log";

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, filename), true))
                {
                    txtLog = "Date Time:" + DateTime.Today.ToString() + "\nLog:" + txtLog + "\n";
                    outputFile.WriteLine(txtLog);
                }
            }
            catch (Exception ex)
            
            {

            }

        }
    }
}

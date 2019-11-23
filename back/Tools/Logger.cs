using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Backend.Tools
{
    public class FileLogger : ILogger
    {
        private string _path;
        public FileLogger()
        {
            string fileName = "logs.log";
            _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+"\\" + fileName;
            SetUp();
        }

        public void AddErrorLog(string message)
        {
                try
                {
                    File.AppendAllText(_path,$"[ERROR] {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} Message: {message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
               
        }

        public void AddInfoLog(string message)
        {
                try
                {
                    File.AppendAllText(_path, $"[INFO] {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} Message: {message}");
                   
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            
        }

        public void SetUp()
        {
            if (!File.Exists(_path))
            {
                File.Create(_path);
            }
        }
    }
}

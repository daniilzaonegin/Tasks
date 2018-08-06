using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Model
{

    public class DataBaseLogger : IDisposable
    {
        private static StreamWriter _logFile;
        private string _filename;
        private static object lockObj = new object();
        public DataBaseLogger(string filename)
        {
            _filename = filename;
        }

        public void Setup()
        {
            if (_logFile == null)
            {
                _logFile = new StreamWriter(new FileStream(_filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite), Encoding.UTF8);
            }
        }

        public void LogToFile(string msg)
        {

            lock (lockObj)
            {
                if (_logFile != null)
                {
                    _logFile.WriteLine(msg);
                    _logFile.Flush();
                }
            }

        }

        public void Dispose()
        {
            _logFile.Close();
        }
    }
}

using System.Collections.Generic;
using System.IO;

namespace Noobot.Examples.Web.Logging
{
    public class FileLog : IMemoryLog
    {
        private readonly List<string> _log;
        private readonly object _lock;

        public FileLog()
        {
            _log = new List<string>();
            _lock = new object();

            File.Create("./log.txt");
        }

        public void Log(string data)
        {
            lock (_lock)
            {
                _log.Add(data);
            }

            StreamWriter writer = new StreamWriter("./log.txt");

            writer.WriteLine(data);
            writer.Close();
        }

        public string[] FullLog()
        {
            lock (_lock)
            {
                return _log.ToArray();
            }
        }
    }
}

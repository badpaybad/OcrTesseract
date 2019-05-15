using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Bigdata.OrcTesseract
{
    public class CommandLineHandleManagement
    {
        static ConcurrentDictionary<string, Action<string>> _map = new ConcurrentDictionary<string, Action<string>>();
        static ConcurrentDictionary<string, string> _mapHelp = new ConcurrentDictionary<string, string>();

        static List<string> _recentCmds = new List<string>();
        static CommandLineHandleManagement()
        {
            _map["quit"] = (args) =>
             {
                 Environment.Exit(0);
             };
            _map["help"] = (args) =>
             {
                 Console.WriteLine("---Menu:ListCommand---");
                 foreach (var kv in _map)
                 {
                     Console.WriteLine($"+-Cmd: {kv.Key}");
                     if (_mapHelp.ContainsKey(kv.Key))
                         Console.WriteLine($"|-- {_mapHelp[kv.Key]}");
                 }
                 Console.WriteLine("---Menu:ListCommand---");
             };
            Console.WriteLine("type 'help' for list command support");
        }
        public void Register(string cmd, Action<string> handle, string helpGuider = "")
        {
            cmd = cmd.ToLower();
            _map[cmd] = handle;
            _mapHelp[cmd] = helpGuider;
        }

        public void Do(string cmdLine)
        {
            try
            {
                _recentCmds.Add(cmdLine);

                var arr = cmdLine.Split(' ');
                var cmd = arr[0].ToLower();
                int startIndex = cmdLine.IndexOf(cmd);
                var args = startIndex < 0 ? string.Empty : cmdLine.Substring(startIndex+cmd.Length).Trim();
                if (_map.TryGetValue(cmd, out Action<string> a))
                {
                    a(args);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }
        }
    }
}

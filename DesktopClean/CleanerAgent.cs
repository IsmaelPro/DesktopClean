using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DesktopClean
{
    public class FileInfo
    {
        public string Name { get; set; }
        public string Extension { get; set; }
    }

    public class CleanerAgent
    {
        private readonly string _path;

        private static Timer _atimer;
        private static Stopwatch _stopWatch;

        public CleanerAgent()
        {
            _path = @"coloque aqui o caminho que deseja monitorar";
            _atimer = new System.Timers.Timer(1000);
            _atimer.Elapsed += CleanDirectory;
            _atimer.AutoReset = true;
            _atimer.Enabled = true;
            _stopWatch = Stopwatch.StartNew();
        }

        private void CleanDirectory(Object s, ElapsedEventArgs e)
        {
            try
            {
                var directory = Directory.EnumerateFileSystemEntries(_path).ToList();
                _atimer.Stop();
                _atimer.Interval = 3600000;
                string targetPath = @"Coloque aqui a pasta onde deseja salvar os arquivos";
                Directory.CreateDirectory(targetPath);
                //var fileList = new ConcurrentBag<FileInfo>(); feature para a próxima versão
                if (directory.Count() > 0)
                {
                    var count = 0;
                    foreach (var item in directory)
                    {
                        Console.WriteLine($"running at line: {count}");
                        var obj = item.Remove(0, _path.Length).Split('.');
                        var fileInfo = new FileInfo();
                        if (obj.Count() > 1)
                        {
                            fileInfo.Name = obj[0].Remove(0, 1);
                            fileInfo.Extension = obj[1];

                            //fileList.Add(fileInfo); Será atualizado na próxima versão
                        }
                        if (item.Contains("xls") || item.Contains("xlsx") || item.Contains("csv"))
                        {
                            var fileName = item.Remove(0, _path.Length);
                            var destFile = targetPath + fileName;
                            File.Move(item, destFile);
                        }
                        count++;
                    };
                }
                Console.WriteLine($"Arquivos salvos em: {targetPath}");
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _atimer.Start();
                _stopWatch = Stopwatch.StartNew();
            }
        }

        public static void Service()
        {
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("-----------  Desktop Cleaner SERVICE  ------------");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("-----------------------------------------------------");
        }

        public void Start()
        {
            Service();
        }

        public void Stop()
        {
        }
    }
}
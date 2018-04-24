using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CopyDirectories
{
    class Program
    {
        static void Main(string[] args)
        {
            //line 1: source path
            //line 2: destination path
            //line 3: target type
            var config = File.ReadAllLines(Environment.CurrentDirectory + "\\config.txt");
            foreach (var item in config)
            {
                Console.WriteLine(item);
            }

            //GetUniqueFileTypes(config); Uncomment if looking for all filetypes in directory

            //create directories in new location, but without files
            foreach (string dirPath in Directory.GetDirectories(config[0], "*", SearchOption.AllDirectories))
            {

                Directory.CreateDirectory(dirPath.Replace(config[0], config[1]));
                Console.WriteLine(dirPath);
            }

            //copy album cover images to new directory
            foreach (string newPath in Directory.GetFiles(config[0], "*.jpg", SearchOption.AllDirectories))
            {
                File.Copy(config[0], newPath.Replace(config[0], config[1]), true);
            }

            using (var engine = new Engine())
            {
                var fileextensions = new List<string> { "flac", "ape", "FLAC", "wv", "wave", "m4a" };

                foreach (var item in fileextensions)
                {
                    foreach (string dirPath in Directory.GetFiles(config[0], $"*.{item}", SearchOption.AllDirectories))
                    {
                        var inputFile = dirPath;

                        //Construct path for new File
                        var outputFile = dirPath.Replace(config[0], config[1]);
                        outputFile = outputFile.Replace($".{item}", config[2]);
                        //Construct path end

                        Console.WriteLine($"Now Processing {dirPath}");

                        var inFileMF = new MediaFile { Filename = inputFile };
                        var outFileMF = new MediaFile { Filename = outputFile };
                        engine.Convert(inFileMF, outFileMF);
                    }
                }
            }

            Console.WriteLine("All Done!");
            Console.ReadKey();

        }


        private static void GetUniqueFileTypes(string[] config)
        {
            foreach (string dirPath in Directory.GetDirectories(config[0], "*", SearchOption.AllDirectories))
            {
                HashSet<string> filetypes = new HashSet<string>();

                DirectoryInfo dirinfo = new DirectoryInfo(dirPath);
                FileInfo[] Files = dirinfo.GetFiles();
                foreach (var item in Files)
                {
                    if (!filetypes.Contains(item.Extension))
                    {
                        filetypes.Add(item.Extension);
                        Console.WriteLine(item.Extension);
                    }

                }
            }
        }
    }
}

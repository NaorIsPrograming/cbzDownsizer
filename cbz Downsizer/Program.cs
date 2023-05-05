using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.IO.Compression;
using System.Collections.Generic;

namespace CBZ_Downsizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("please enter the path of the main folder where all of your cbz's are");
            string InitialDirectory = Console.ReadLine();
            Console.WriteLine("please enter the folder where you want all of your new cbz's");
            string newDirectory=Console.ReadLine();
            string[] files= Directory.GetFiles(InitialDirectory);
            Console.WriteLine("please enter your resize ration");
            double ratio = double.Parse(Console.ReadLine());
            foreach(string file in files)
            {
                string fileName=Path.GetFileName(file);
                CreateNewCbz(file, newDirectory, ratio, fileName);
            }

                
            
        }
        static void CreateNewCbz(string oldLocation,string newLocation, double ResizeRatio,string name)
        {
            using (FileStream newFile = new FileStream(Path.Combine(newLocation,name), FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(newFile, ZipArchiveMode.Update))
                {
                    Cbz cbz = new Cbz(oldLocation, 1.5, archive);
                }
            }
        }


    }
}

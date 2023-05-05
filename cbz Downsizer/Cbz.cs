using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Xml.Linq;
using System.Resources;

namespace CBZ_Downsizer 
{
    internal class Cbz
    {
        public string PreviousFilePath { get; private set; }
        public string NewFilePath { get; private set; }
        public ZipArchive PreviousCbz;
        public ZipArchive NewCbz;
        public double ResizeRatio;
        private FileStream fileStream;

        public Cbz(string previousFilePath, string newFilePath, int resizeRatio)
        {
            fileStream = new FileStream(newFilePath, FileMode.Create);
            PreviousFilePath = previousFilePath;
            NewFilePath = newFilePath;
            PreviousCbz = ZipFile.Open(previousFilePath, ZipArchiveMode.Read);
            NewCbz = new ZipArchive(fileStream, ZipArchiveMode.Update);
            CopyCbzStructure();
            ResizeRatio = resizeRatio;

        }
        public void Save()
        {
            ResizeCBZ();

        }
        private void ResizeCBZ()
        {
            List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG" };
            foreach (ZipArchiveEntry entry in PreviousCbz.Entries)
            {
                if (!entry.FullName.EndsWith('/'))
                {
                    if (ImageExtensions.Contains(Path.GetExtension(entry.Name).ToUpper()))
                    {
                        Image image = Image.Load(entry.Open());
                        ResizeAndSave(image, entry);
                    }
                    else
                    {
                        using (Stream data = entry.Open())
                        {
                            data.CopyTo(NewCbz.GetEntry(entry.FullName).Open());

                        }
                    }
                }


            }

        }
        void ResizeAndSave(Image image, ZipArchiveEntry oldEntry)
        {
            ZipArchiveEntry newEntry = NewCbz.GetEntry(oldEntry.FullName.Replace(Path.GetExtension(oldEntry.FullName), ".jpeg"));
            if(newEntry != null)
            {
                image.Mutate(x => x.Resize(new Size(Convert.ToInt32(Convert.ToDouble(image.Width) / ResizeRatio), 0)));
                MemoryStream buffer = new MemoryStream();
                Console.WriteLine(newEntry.FullName);
                image.SaveAsJpeg(buffer);
                Stream stream = newEntry.Open();
                buffer.Seek(0, SeekOrigin.Begin);
                buffer.CopyTo(stream);
                stream.Close();
            }


        }
        private void CopyCbzStructure()
        {
            foreach (ZipArchiveEntry entry in PreviousCbz.Entries)
            {
                if (Path.GetExtension(entry.FullName) != null && Path.GetExtension(entry.FullName) != "")
                {
                    NewCbz.CreateEntry(entry.FullName.Replace(Path.GetExtension(entry.FullName), ".jpeg"));
                    Console.WriteLine(entry.FullName.Replace(Path.GetExtension(entry.FullName), ".jpeg"));
                }
                else NewCbz.CreateEntry(entry.FullName);


            }
        }
    }
    
        
    
}

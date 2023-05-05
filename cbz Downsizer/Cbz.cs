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
using System.Diagnostics;
using System.Buffers;

namespace CBZ_Downsizer 
{
    internal class Cbz
    {
        public string PreviousFilePath { get; private set; }
        public ZipArchive PreviousCbz;
        public ZipArchive NewCbz;
        public double ResizeRatio;

        public Cbz(string previousFilePath, double resizeRatio,ZipArchive newArchive)
        {
            PreviousFilePath = previousFilePath;
            PreviousCbz = ZipFile.Open(previousFilePath, ZipArchiveMode.Read);
            NewCbz = newArchive;
            ResizeRatio = resizeRatio;
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
                        using (BufferedStream data = new BufferedStream(entry.Open()))
                        {
                            using (BufferedStream newEntryStream = new BufferedStream(NewCbz.CreateEntry(entry.FullName).Open()))
                            {
                                data.Position = 0;
                                data.CopyTo(newEntryStream);
                            } 

                        }
                    }
                }
                else
                {
                }


            }

        }
        void ResizeAndSave(Image image, ZipArchiveEntry oldEntry)
        {
            ZipArchiveEntry newEntry = NewCbz.CreateEntry(oldEntry.FullName.Replace(Path.GetExtension(oldEntry.FullName), ".jpeg"));
            if(newEntry != null)
            {
                image.Mutate(x => x.Resize(new Size(Convert.ToInt32(Convert.ToDouble(image.Width) / ResizeRatio), 0)));
                using (BufferedStream buffer = new BufferedStream(newEntry.Open()))
                {
                    image.SaveAsJpeg(buffer);
                    Console.WriteLine(newEntry.FullName);
                }

            }
        }


        }
      
}
    
        
    


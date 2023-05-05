using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.IO.Compression;
using System.Collections.Generic;

namespace CBZ_Resizer
{
    class Program
    {
        static void Main(string[] args)
        {
            string oldLocation = "C:\\Coding\\Git\\CBZ Resizer\\CBZ Resizer\\Resources\\CBZ'S\\Bleach.cbz";
            string newLocation = "C:\\Coding\\Git\\CBZ Resizer\\CBZ Resizer\\Resources\\CBZ'S\\BleachNew.cbz";
            Cbz cbz = new Cbz(oldLocation, newLocation, 2);
            cbz.Save();

        }
        
        public static string RemoveQuotes(string str)
        {
            if (str[0]== '"') 
            {
                str=str.Substring(1, str.Length - 2);
            }
            
            return str;
        }
    }
}

using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace WPFEncryptionTool2PROA04
{
    public static class FileHelper
    {
        public static string FolderIndex = Path.Combine(Environment.CurrentDirectory, "DefaultFolders.csv");

       

        public static void WriteToCsv<T>(IEnumerable<T> collection)
        {
            using (var fs = new FileStream(FolderIndex, FileMode.OpenOrCreate))
            {
                using (var sw = new StreamWriter(fs))
                {
                    using (var csv = new CsvWriter(sw, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(collection);
                    }
                }
            }
        }

        public static List<T> ReadCsv<T>(string filepath)
        {
            using (var reader = new StreamReader(filepath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<T>().ToList();  
            }
        }
      
    }
}

using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using WPFEncryptionTool2PROA04.Models;

namespace WPFEncryptionTool2PROA04
{
    public class FileHelper
    {
        public static FileResult<T> WriteCsv<T>(IEnumerable<T> collection, string filepath)
        {
            FileResult<T> result = new FileResult<T>();

            try
            {
                if (ValidateCollection<T>(collection))
                {
                    using (var fs = new FileStream(filepath, FileMode.OpenOrCreate))
                    {
                        using (var sw = new StreamWriter(fs))
                        {
                            using (var csv = new CsvWriter(sw, CultureInfo.InvariantCulture))
                            {
                                csv.WriteRecords(collection);

                            }

                            sw.Close();
                            result.Succeeded = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Errors.Append(ex.Message);
            }

            return result;
        }

        public static FileResult<T> ReadCsv<T>(string filepath)
        {
            FileResult<T> result = new FileResult<T>();

            try
            {
                if (File.Exists(filepath))
                {
                    using (var reader = new StreamReader(filepath))
                    {
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            result.records = csv.GetRecords<T>().ToList();
                        }

                        reader.Close();
                        result.Succeeded = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
                /*result.Succeeded = false;                
                result.Errors.Append(ex.Message.ToString());*/
            }

            return result;
        }               

        public static string GetFolderPath(string folderName)
        {
            return
                ReadCsv<Folder>(Folders.FolderIndex).records
                .FirstOrDefault(x => x.Name == folderName).Path;
        }

        public static IEnumerable<string> GetKeyNames(string folder)
        {
            var files = new List<string>();
                      
            foreach (var record in Directory.EnumerateFiles(folder).ToList())
            {
                files.Add(Path.GetFileName(record).Split('.')[0]);
            }
            
            return files;
        }

        private static bool ValidateCollection<T>(IEnumerable<T> collection)
        {
            bool validation = true;

            if (collection == null)
            {
                validation = false;
            }

            if (collection.Count() < 1)
            {
                validation = false;
            }

            return validation;

        }




    }
}

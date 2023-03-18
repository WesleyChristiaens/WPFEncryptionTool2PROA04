using CsvHelper;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using WPFEncryptionTool2PROA04.Models;
using static System.Net.WebRequestMethods;

namespace WPFEncryptionTool2PROA04
{
    public class FileHelper
    {
        public static SaveResult WriteCsv<T>(IEnumerable<T> collection, string filepath)
        {
            SaveResult result = new SaveResult();

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

        public static SaveResult WriteCsv<T>(T model, string filepath)
        {
            SaveResult result = new SaveResult();

            try
            {
                if (model != null)
                {
                    using (var fs = new FileStream(filepath, FileMode.OpenOrCreate))
                    {
                        using (var sw = new StreamWriter(fs))
                        {
                            using (var csv = new CsvWriter(sw, CultureInfo.InvariantCulture))
                            {
                                csv.WriteRecord<T>(model);
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
                if (System.IO.File.Exists(filepath))
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
                result.Succeeded = false;
                result.Errors.Append(ex.Message.ToString());
            }

            return result;
        }

        public static string GetFolderPath(string folderName)
        {
            string path = "";

            if (System.IO.File.Exists(Folders.FolderIndex) && !string.IsNullOrEmpty(System.IO.File.ReadAllText(Folders.FolderIndex)) )
            {
                path = ReadCsv<Folder>(Folders.FolderIndex).records
                .FirstOrDefault(x => x.Name == folderName).Path;
            }

            return path;
        }

        public static IEnumerable<string> GetDirectoryContent(string folder)
        {
            var files = new List<string>();

            if (!string.IsNullOrEmpty(folder))
            {
                foreach (var record in Directory.EnumerateFiles(folder).ToList())
                {
                    files.Add(Path.GetFileName(record).Split('.')[0]);
                }
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

        public static SaveResult StoreAesKey(AesKey generatedKey)
        {
            SaveResult result = new SaveResult();

            try
            {
                var path = GetFolderPath(Folders.GeneratedAESKeys);

                if (!string.IsNullOrEmpty(path))
                {
                    if (!GetDirectoryContent(path).ToList().Contains(generatedKey.Name))
                    {
                        WriteCsv(generatedKey, Path.Combine(path, generatedKey.Name));
                        result.Succeeded = true;
                    }
                    else
                    {
                        result.Errors.Add("You already have a key with this name");
                    }
                }
                else
                {
                    result.Errors.Add("Please set standard folder first");
                }
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Errors.Append(ex.ToString());
            }

            return result;
        }

        public static SaveResult StoreRSAKeyPair(RsaKeyPair rkp)
        {
            SaveResult result = new SaveResult();

            try
            {
                var pathpublic = GetFolderPath(Folders.RSAPublicKeys);
                var pathprivate = GetFolderPath(Folders.RSAPrivateKeys);

                if (pathpublic != string.Empty && pathprivate != string.Empty && rkp.PublicKey != string.Empty &&
                    rkp.PrivateKey != string.Empty)
                {
                    if ((!GetDirectoryContent(pathpublic).ToList().Contains(rkp.Name)) ||
                        (!GetDirectoryContent(pathprivate).Contains(rkp.Name)))
                    {
                        System.IO.File.WriteAllText(Path.Combine(pathpublic, rkp.Name), rkp.PublicKey);
                        System.IO.File.WriteAllText(Path.Combine(pathprivate, rkp.Name), rkp.PrivateKey);
                        result.Succeeded = true;
                    }
                    else
                    {
                        result.Succeeded = false;
                        result.Errors.Add("You already have a key with this name");
                    }
                }
                else
                {
                    result.Succeeded = false;
                    result.Errors.Add(
                        $"Please set standard folders for {nameof(Folders.RSAPublicKeys)} & {nameof(Folders.RSAPrivateKeys)} first, using File -> Options");
                }
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Errors.Add(ex.ToString());
            }

            return result;
        }
    }
}


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace WPFEncryptionTool2PROA04
{
    public class FileHelper
    {
        public string FolderPath { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public string FilePath => Path.Combine(FolderPath, FileName);

        public FileHelper()
        {
            FolderPath = "";
            FileName = "";
            Content = "";
        }

        public FileHelper WithFolder(string folderpath)
        {
            FolderPath = folderpath;
            return this;
        }

        public FileHelper WithFileName(string fileName)
        {
            FileName = fileName;
            return this;
        }

        public FileHelper WithContent(string content)
        {
            Content = content;
            return this;
        }

        public FileHelper SaveToFile()
        {
            if (string.IsNullOrEmpty(FolderPath))
            {
                MessageBox.Show("Please set standard folder first.");
                var wpf = new WpfOptions();
                wpf.ShowDialog();
                return null;
            }

            if (Content is null)
            {
                MessageBox.Show("No content supplied for storage");
                return null;
            }

            if (string.IsNullOrEmpty(FileName))
            {
                MessageBox.Show("Please specify a Filename");
                return null;
            }

            try
            {
                using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter sr = new StreamWriter(fs))
                    {
                        sr.Write(Content);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went horribly wrong while saving...");
            }

            return this;
        }


        public FileHelper ReadFromFile()
        {
            if (string.IsNullOrEmpty(FolderPath))
            {
                MessageBox.Show("Please set standard folder first.");
                var wpf = new WpfOptions();
                wpf.ShowDialog();
                return null;
            }

            if (string.IsNullOrEmpty(FileName))
            {
                MessageBox.Show("Please specify FileName");
                return null;
            }

            using (FileStream fs = new FileStream(FilePath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    Content = sr.ReadToEnd();
                }
            }

            return this;
        }

        public IEnumerable<string> GetDirectoryContent()
        {
            if (string.IsNullOrEmpty(FolderPath))
            {
                MessageBox.Show("Please specify directory.");
                return null;
            }

            if (!Directory.Exists(FolderPath))
            {
                throw new DirectoryNotFoundException();
            }

            var files = new List<string>();

            foreach (var record in Directory.EnumerateFiles(FolderPath).ToList())
            {
                files.Add(Path.GetFileName(record).Split('.')[0]);
            }

            return files;
        }
    }
}
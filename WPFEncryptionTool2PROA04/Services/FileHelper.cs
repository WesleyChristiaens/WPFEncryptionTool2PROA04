using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;

namespace WPFEncryptionTool2PROA04
{
    public class FileHelper
    {
        public string FilePath => Path.Combine(_folderPath, _fileName);

        private string _folderPath { get; set; }
        private string _fileName { get; set; }
        private string _content { get; set; }
        private bool _isXml { get; set; }

        public FileHelper()
        {
            _folderPath = "";
            _fileName = "";
            _content = "";
        }

        public FileHelper WithFolder(string folderpath)
        {
            _folderPath = folderpath;
            return this;
        }

        public FileHelper WithFileName(string fileName)
        {
            _fileName = fileName;
            return this;
        }

        public FileHelper WithContent(string content)
        {
            _content = content;
            return this;
        }

        public FileHelper IsXmlFile(bool value)
        {
            _isXml = value;
            return this;
        }

        public FileHelper SaveToFile()
        {
            if (string.IsNullOrEmpty(_folderPath))
            {
                MessageBox.Show("Please set standard folder first.");
                var wpf = new WpfOptions();
                wpf.ShowDialog();
                return null;
            }

            if (_content is null)
            {
                MessageBox.Show("No content supplied for storage");
                return null;
            }

            if (string.IsNullOrEmpty(_fileName))
            {
                MessageBox.Show("Please specify a Filename");
                return null;
            }

            try
            {
                if (_isXml)
                {
                    _fileName += ".xml";

                    using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        using (XmlTextWriter xr = new XmlTextWriter(fs, Encoding.UTF8))
                        {
                            xr.WriteString(_content);
                        }
                    }
                }

                _fileName += ".txt";

                using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter sr = new StreamWriter(fs))
                    {
                        sr.Write(_content);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went horribly wrong while saving...");
            }

            return this;
        }


        public string ReadFromFile()
        {
            if (string.IsNullOrEmpty(_folderPath))
            {
                MessageBox.Show("Please set standard folder first.");
                var wpf = new WpfOptions();
                wpf.ShowDialog();
                return null;
            }

            if (string.IsNullOrEmpty(_fileName))
            {
                MessageBox.Show("Please specify FileName");
                return null;
            }

            if (_isXml)
            {
                using (FileStream fs = new FileStream(FilePath, FileMode.Open))
                {
                    using (XmlTextReader xr = new XmlTextReader(fs))
                    {
                        _content = xr.ReadString();
                    }
                }
            }

            using (FileStream fs = new FileStream(FilePath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    _content = sr.ReadToEnd();
                }
            }

            return _content;
        }

        public IEnumerable<string> GetDirectoryContent()
        {
            if (string.IsNullOrEmpty(_folderPath))
            {
                MessageBox.Show("Please specify directory.");
                return null;
            }

            if (!Directory.Exists(_folderPath))
            {
                throw new DirectoryNotFoundException();
            }

            var files = new List<string>();

            foreach (var record in Directory.EnumerateFiles(_folderPath).ToList())
            {
                files.Add(Path.GetFileName(record).Split('.')[0]);
            }

            return files;
        }
    }
}
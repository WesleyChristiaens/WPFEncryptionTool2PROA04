using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFEncryptionTool2PROA04.Models
{
    public class FileResult<T>
    {
        public  bool Succeeded { get; set; }

        public  IEnumerable<string> Errors { get; set; }

        public  IEnumerable<T> records { get; set; }

        public FileResult()
        {
            Errors = new List<string>();
            records = new List<T>();
            Succeeded= false;
        }
    }
}

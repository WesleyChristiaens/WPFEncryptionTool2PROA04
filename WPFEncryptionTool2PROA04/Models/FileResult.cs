using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFEncryptionTool2PROA04.Models
{
    public class FileResult<T> : SaveResult
    {
        public  IEnumerable<T> records { get; set; }

        public FileResult()
        {            
            records = new List<T>();
            Succeeded= false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFEncryptionTool2PROA04.Models
{
    public class SaveResult
    {
        public bool Succeeded { get; set; }

        public List<string> Errors { get; set; }

        public SaveResult()
        {
            Errors = new List<string>();
        }
    }
}

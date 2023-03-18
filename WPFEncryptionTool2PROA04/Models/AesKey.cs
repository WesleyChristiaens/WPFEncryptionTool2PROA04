using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFEncryptionTool2PROA04.Models
{
    public class AesKey : Key
    {
       public string Key { get; set; }

        public string InitiationVector { get; set; }

    }
}

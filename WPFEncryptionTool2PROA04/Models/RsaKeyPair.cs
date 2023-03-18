using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFEncryptionTool2PROA04.Models
{
    public class RsaKeyPair : Key
    {        
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}

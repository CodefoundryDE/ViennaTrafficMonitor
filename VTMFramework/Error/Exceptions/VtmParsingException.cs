using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtmFramework.Error.Exceptions {
    public class VtmParsingException : Exception {

        private string _path;

        public string Path { get; set; }

        public VtmParsingException(string message, string path, Exception ex)
            : base(message, ex) {
            _path = path;
        }
    }
}

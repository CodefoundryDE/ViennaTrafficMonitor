using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VtmFramework.Error.Exceptions
{
    [Serializable]
    public class VtmParsingException : Exception
    {

        public string Path { get; set; }

        public VtmParsingException(string message, string path, Exception ex)
            : base(message, ex)
        {
            Path = path;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Path", Path);
        }
    }
}

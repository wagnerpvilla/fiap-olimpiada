using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OlimpiadaAPI
{
    public class SignedUrlModel
    {
        public string SignedUrl { get; set; }
        public string ImageUrl { get; set; }
        public string FileName { get; internal set; }
    }
}

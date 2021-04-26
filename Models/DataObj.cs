using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChartsUsingMdx.Models
{
    public class DataObj
    {
        public string HeaderName { get; set; }
        public List<float> Values { get; set; }
        public List<string> Parameters { get; set; }
    }
}

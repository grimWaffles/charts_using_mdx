using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChartsUsingMdx.Models
{
    public class DataObj
    {
        public string Name { get; set; }
        public List<float> Data { get; set; }
        public List<string> Parameters { get; set; }
    }
}

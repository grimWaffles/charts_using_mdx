using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChartsUsingMdx.Models
{
    public class DataCube
    {
        public string Name { get; set; }
        public string Updated_at { get; set; }
        public List<string> Kpis { get; set; }
        public List<string> Measures { get; set; }
        public List<string> Dimensions { get; set; }
    }
}

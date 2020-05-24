using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Takehome.API.VirtualModels
{
    public class MunroVM
    {
        public string Name { get; set; }
        public decimal Height { get; set; }
        public string Hill_Category { get; set; }
        public string Grid_Reference { get; set; }
    }
}

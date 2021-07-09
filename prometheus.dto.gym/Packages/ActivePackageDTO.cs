using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Packages
{
    public class ActivePackageDTO
    {
        public int Id { get; set; }
        public Double Price { get; set; }
        public string Period { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public bool ForSale { get; set; }
    }
}

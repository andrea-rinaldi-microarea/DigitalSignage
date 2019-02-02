using System;
using System.Collections.Generic;

namespace DigitalSignage.Models
{
    public partial class ZcMenuDetail
    {
        public string MenuheaderCode { get; set; }
        public short Day { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public double? SalesPrice { get; set; }
        public short MenuId { get; set; }
        public string Background { get; set; }
        public string Picture { get; set; }
        public int? Estimatedquantity { get; set; }
    }
}

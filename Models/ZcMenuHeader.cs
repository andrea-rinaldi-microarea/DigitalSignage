using System;
using System.Collections.Generic;

namespace DigitalSignage.Models
{
    public partial class ZcMenuHeader
    {
        public Guid? Tbguid { get; set; }
        public string MenuheaderCode { get; set; }
        public short? Week { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Description { get; set; }
        public string Storage { get; set; }
        public string Background { get; set; }
    }
}

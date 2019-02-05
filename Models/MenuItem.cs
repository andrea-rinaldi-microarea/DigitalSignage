namespace DigitalSignage.Models
{
    public class MenuItem {
        public int MenuId {get;set;}
        public short Day {get; set;}
        public string Description {get;set;}
        public double? SalesPrice {get;set;}
        public string Picture {get;set;}
    }
}
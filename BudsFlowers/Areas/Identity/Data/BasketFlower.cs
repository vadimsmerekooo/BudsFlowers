using System.Xml.Serialization;

namespace BudsFlowers.Areas.Identity.Data
{
    public class BasketFlower
    {
        public string FlowerId { get; set; }
        public int Count { get; set; }
        [XmlIgnore]
        public Flower? Flower { get; set; }
        public double GetTotalPrice() => Flower.GetTotalPrice() * Count;
        public double GetPrice() => Flower.Price * Count;
    }
}

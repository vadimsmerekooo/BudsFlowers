namespace BudsFlowers.Areas.Identity.Data
{
    public class BasketFlower
    {
        public string FlowerId { get; set; }
        public int Count { get; set; }
        public virtual Flower? Flower { get; set; }
    }
}

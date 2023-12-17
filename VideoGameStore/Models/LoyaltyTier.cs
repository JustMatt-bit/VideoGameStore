namespace VideoGameStore.Models
{
    public class LoyaltyTier
    {
        public int TierId { get; set; }
        public string Name { get; set; }
        public int PointsFrom { get; set; }
        public int PointsTo { get; set; }
        public string Description { get; set; }
        public double DiscountCoefficient { get; set; }
    }
}

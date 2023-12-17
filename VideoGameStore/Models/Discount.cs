namespace VideoGameStore.Models
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public double Percent { get; set; }
        public string? FkAccount { get; set; }
    }
}

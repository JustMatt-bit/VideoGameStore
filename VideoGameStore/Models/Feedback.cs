namespace VideoGameStore.Models
{
    public class Feedback
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string text { get; set; }
        public float rating { get; set; }
        public int rating_count { get; set; }
        public int is_flagged { get; set; }
        public string? account_name { get; set; }
        public int fk_product { get; set; }
        public int? replying_to_id { get; set; }
    }
}

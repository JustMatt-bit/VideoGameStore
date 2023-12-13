namespace VideoGameStore.Models
{
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string? referal_code { get; set; }
        public DateTime creation_date { get; set; }
        public int loyalty_progress { get; set; } 
        public int fk_user_type { get; set; }
        public int fk_loyalty_tier { get; set; }
    }
}

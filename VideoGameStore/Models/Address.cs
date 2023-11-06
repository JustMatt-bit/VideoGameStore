namespace VideoGameStore.Models
{
    public class Address
    {
        public int id { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public int building { get; set; }
        public string postal_code { get; set; }
        public string fk_account { get; set; }
    }
}

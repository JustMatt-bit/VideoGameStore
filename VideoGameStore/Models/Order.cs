using Org.BouncyCastle.Asn1.X509;

namespace VideoGameStore.Models
{
    public class Order
    {
        public int id { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime completion_date { get; set; }
        public float price { get; set; }
        public string comment { get; set; }
        public float parcel_price { get; set; }
        public string fk_account { get; set; }
        public int? fk_address { get; set; }
        public int fk_status { get; set; }
        public int? fk_discount { get; set; }
        public string AddressV { get; set; }
        public string StatusV { get; set; }
        public int DiscountV { get; set; }
    }
}

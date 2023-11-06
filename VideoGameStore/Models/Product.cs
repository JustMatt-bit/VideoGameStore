namespace VideoGameStore.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public float price { get; set; }
        public int stock { get; set; }
        public string description { get; set; }
        public DateTime release_date { get; set; }
        public bool being_sold { get; set; }
        public int fk_game_type { get; set; } // Gražins tik fk int'ą
        public string? game_type_name { get; set; } // Optional variable, kuriame galima saugoti pilną pavadinimą (frontendo reikmėms)
        public int fk_developer { get; set; } // Gražins tik fk int'ą
        public string? developer_name { get; set; } // Optional variable, kuriame galima saugoti pilną pavadinimą (frontendo reikmėms)
        public string fk_account { get; set; }
        public int? units_in_cart { get; set; }
    }
}

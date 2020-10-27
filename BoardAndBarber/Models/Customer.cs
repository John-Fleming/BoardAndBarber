using System;

namespace BoardAndBarber.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string FavoriteBarber { get; set; }
        public string Notes { get; set; }
    }
}

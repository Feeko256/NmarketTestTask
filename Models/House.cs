using System.Collections.Generic;

namespace NmarketTestTask.Models
{
    public class House
    {
        public string Name { get; set; }
        public HouseDescription Description { get; set; }

        public List<Flat> Flats { get; set; }
    }
}

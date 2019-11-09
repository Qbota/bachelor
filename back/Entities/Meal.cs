using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Comments { get; set; }
        public double AverageRate { get; set; }
        public double Price { get; set; }
        public int RestaurantId { get; set; }
        public IEnumerable<int> Rates { get; set; }
    }
}

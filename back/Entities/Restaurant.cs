using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<Meal> Menu { get; set; }
        public bool HasMeal(string meal)
        {
            if (Menu.Where(x => x.Name == meal).Count() >= 1)
                return true;
            else
                return false;
        }
        public override string ToString() => $"0,\"{Name}\",{Lat.ToString(System.Globalization.CultureInfo.InvariantCulture)},{Lng.ToString(System.Globalization.CultureInfo.InvariantCulture)},1";
        
    }
}

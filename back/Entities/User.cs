using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public bool IsActive { get; set; }
        public override string ToString() {
            if(Restaurant == null)
            {
                return $"0,\"{Email}\",\"{Password}\",{Convert.ToInt32(IsAdmin)},0,1";
            }
            else
            {
                return $"0,\"{Email}\",\"{Password}\",{Convert.ToInt32(IsAdmin)}," +
            $"(select id from restaurants where restaurant_lat={Restaurant.Lat.ToString(System.Globalization.CultureInfo.InvariantCulture)} and restaurant_lng={Restaurant.Lng.ToString(System.Globalization.CultureInfo.InvariantCulture)}),1";
            }
        }
        
    }
}

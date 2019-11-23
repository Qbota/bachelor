using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class Comment
    {
        public int Rate { get; set; }
        public string Text { get; set; }
        public int MealId { get; set; }
        public override string ToString() => $"0,\"{Text}\",\"{Rate}\",\"{MealId}\"";
    }
}

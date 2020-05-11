using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKent.Models
{
    public class Lift
    {
        public int Id { get; set; }
        public string State { get; set; }
        public string Direction { get; set; }
        public int Person { get; set; }
        public int Floor { get; set; }
    }
}

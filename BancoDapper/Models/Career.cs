using System;
using System.Collections.Generic;

namespace BancoDapper.Models
{
    public class Career
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IList<CareerItem> Items{ get; set; }


        public Career()
        {
            Items = new List<CareerItem>();
        }
    }
}

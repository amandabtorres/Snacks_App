using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snacks_App.Models
{
    public class OrdersByUser
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public DateTime DateOrder { get; set; }
    }
}

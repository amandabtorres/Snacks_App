using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snacks_App.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Detail { get; set; }
        public string? UrlImage { get; set; }
        public string? UrlImagem => AppConfig.BaseUrl + UrlImage;
    }
}

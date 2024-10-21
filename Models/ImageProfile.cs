using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snacks_App.Models
{
    public class ImageProfile
    {
        public string? UrlImage { get; set; }
        public string? UrlImagem => AppConfig.BaseUrl + UrlImage;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snacks_App.Services
{
    public static class ServiceFactory
    {
        public static FavoriteService CreateFavoritosService()
        {
            return new FavoriteService();
        }
    }
}

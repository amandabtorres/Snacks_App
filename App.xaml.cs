using Snacks_App.Pages;
using Snacks_App.Services;

namespace Snacks_App
{
    public partial class App : Application
    {
        private readonly ApiService _apiService;

        public App(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            MainPage = new NavigationPage(new RegisterPage(_apiService));
        }
    }
}

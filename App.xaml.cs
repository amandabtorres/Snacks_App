using Snacks_App.Pages;
using Snacks_App.Services;
using Snacks_App.Validations;

namespace Snacks_App
{
    public partial class App : Application
    {
        private readonly ApiService _apiService;
        private readonly IValidator _validator;

        public App(ApiService apiService, IValidator validator)
        {
            InitializeComponent();
            _apiService = apiService;
            _validator = validator;
            MainPage = new NavigationPage(new RegisterPage(_apiService, _validator));
        }
    }
}

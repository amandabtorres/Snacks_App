using Snacks_App.Services;
using Snacks_App.Validations;

namespace Snacks_App.Pages;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;

    public LoginPage(ApiService apiService, IValidator validator)
	{
		InitializeComponent();
        _apiService = apiService;
        _validator = validator;
    }

    private async void TapRegister_TappedAsync(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage(_apiService, _validator));
    }

    private async void BtnSignIn_ClickedAsync(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(EntEmail.Text))
        {
            await DisplayAlert("Erro", "Informe o email", "Cancelar");
            return;
        }

        if (string.IsNullOrEmpty(EntPassword.Text))
        {
            await DisplayAlert("Erro", "Informe a senha", "Cancelar");
            return;
        }

        var response = await _apiService.Login(EntEmail.Text, EntPassword.Text);

        if (!response.HasError)
        {
            Application.Current!.MainPage = new AppShell(_apiService, _validator);
        }
        else
        {
            await DisplayAlert("Erro", "Algo deu errado", "Cancelar");
        }
    }
}
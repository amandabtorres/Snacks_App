using Snacks_App.Services;

namespace Snacks_App.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly ApiService _apiService;
    public RegisterPage(ApiService apiService)
	{
		InitializeComponent();
        _apiService = apiService;
    }

    private async void BtnSignup_ClickedAsync(object sender, EventArgs e)
    {
        var response = await _apiService.RegisterUser(EntName.Text, EntEmail.Text,
                                                         EntPhone.Text, EntPassword.Text);

        if (!response.HasError)
        {
            await DisplayAlert("Aviso", "Sua conta foi criada com sucesso !!", "OK");
            await Navigation.PushAsync(new LoginPage(_apiService));
        }
        else
        {
            await DisplayAlert("Erro", "Algo deu errado!!!", "Cancelar");
        }

    }

    private async void TapLogin_TappedAsync(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new LoginPage(_apiService));
    }
}
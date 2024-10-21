using Snacks_App.Services;

namespace Snacks_App.Pages;

public partial class MyAccountPage : ContentPage
{
    private readonly ApiService _apiService;

    private const string NomeUsuarioKey = "usuarionome";
    private const string EmailUsuarioKey = "usuarioemail";
    private const string TelefoneUsuarioKey = "usuariotelefone";
    public MyAccountPage(ApiService apiService)
	{
		InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        CarregarInformacoesUsuario();
        ImgBtnPerfil.Source = await GetImageProfileAsync();
    }

    private void CarregarInformacoesUsuario()
    {
        LblNomeUsuario.Text = Preferences.Get(NomeUsuarioKey, string.Empty);
        EntNome.Text = LblNomeUsuario.Text;
        EntEmail.Text = Preferences.Get(EmailUsuarioKey, string.Empty);
        EntFone.Text = Preferences.Get(TelefoneUsuarioKey, string.Empty);
    }

    private async Task<string?> GetImageProfileAsync()
    {
        string imagemPadrao = AppConfig.PerfilImagemPadrao;

        var (response, errorMessage) = await _apiService.GetImageUserProfile();

        if (errorMessage is not null)
        {
            switch (errorMessage)
            {
                case "Unauthorized":
                    await DisplayAlert("Erro", "Não autorizado", "OK");
                    return imagemPadrao;
                default:
                    await DisplayAlert("Erro", errorMessage ?? "Não foi possível obter a imagem.", "OK");
                    return imagemPadrao;
            }
        }

        if (response?.UrlImagem is not null)
        {
            return response.UrlImagem;
        }
        return imagemPadrao;
    }
        

    private async void BtnSalvar_Clicked(object sender, EventArgs e)
    {
        // Salva as informa  es alteradas pelo usu rio nas prefer ncias
        Preferences.Set(NomeUsuarioKey, EntNome.Text);
        Preferences.Set(EmailUsuarioKey, EntEmail.Text);
        Preferences.Set(TelefoneUsuarioKey, EntFone.Text);
        await DisplayAlert("Informações Salvas", "Suas informações foram salvas com sucesso!", "OK");
    }
}
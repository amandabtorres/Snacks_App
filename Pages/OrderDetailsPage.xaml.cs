using Snacks_App.Services;
using Snacks_App.Validations;

namespace Snacks_App.Pages;

public partial class OrderDetailsPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private bool _loginPageDisplayed = false;

    public OrderDetailsPage(int pedidoId,
                              decimal precoTotal,
                              ApiService apiService,
                              IValidator validator)
	{
		InitializeComponent();
        _apiService = apiService;
        _validator = validator;
        LblPrecoTotal.Text = " R$" + precoTotal;

        GetOrderDetails(pedidoId);
    }

    private async void GetOrderDetails(int pedidoId)
    {
        try
        {
            var (pedidoDetalhes, errorMessage) = await _apiService.GetOrderDetails(pedidoId);

            if (errorMessage == "Unauthorized" && !_loginPageDisplayed)
            {
                await DisplayLoginPage();
                return;
            }

            if (pedidoDetalhes is null)
            {
                await DisplayAlert("Erro", errorMessage ?? "Não foi possível obter detalhes do pedido.", "OK");
                return;
            }
            else
            {
                CvPedidoDetalhes.ItemsSource = pedidoDetalhes;
            }
        }
        catch (Exception)
        {
            await DisplayAlert("Erro", "Ocorreu um erro ao obter os detalhes. Tente novamente mais tarde.", "OK");
        }
    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;
        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }
}
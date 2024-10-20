using Snacks_App.Models;
using Snacks_App.Services;
using Snacks_App.Validations;
using System.Collections.ObjectModel;

namespace Snacks_App.Pages;

public partial class CartPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private bool _loginPageDisplayed = false;

    private ObservableCollection<ShoppingCartItem> ItemsShoppingCart = new ObservableCollection<ShoppingCartItem>();

    public CartPage(ApiService apiService, IValidator validator)
	{
		InitializeComponent();
        _apiService = apiService;
        _validator = validator;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetItemsShoppingCart();

        bool enderecoSalvo = Preferences.ContainsKey("endereco");
        if (enderecoSalvo)
        {
            string nome = Preferences.Get("nome", string.Empty);
            string endereco = Preferences.Get("endereco", string.Empty);
            string telefone = Preferences.Get("telefone", string.Empty);

            //formatar os dados conforme desejo na label
            LblEndereco.Text = $"{nome}\n{endereco}\n{telefone}";
        }
        else
        {
            LblEndereco.Text = "Informe o seu endere�o";
        }
    }

    private async Task<bool> GetItemsShoppingCart()
    {
        try
        {
            var usuarioId = Preferences.Get("usuarioid", 0);
            var (itensCarrinhoCompra, errorMessage) = await
                     _apiService.GetItemsShoppingCart(usuarioId);

            if (errorMessage == "Unauthorized" && !_loginPageDisplayed)
            {
                // Redirecionar para a p?gina de login
                await DisplayLoginPage();
                return false;
            }

            if (itensCarrinhoCompra == null)
            {
                await DisplayAlert("Erro", errorMessage ?? "N�o foi possivel obter os itens do carrinho de compra.", "OK");
                return false;
            }

            ItemsShoppingCart.Clear();
            foreach (var item in itensCarrinhoCompra)
            {
                ItemsShoppingCart.Add(item);
            }

            CvCarrinho.ItemsSource = ItemsShoppingCart;
            UpdatePriceTotal(); // Atualizar o preco total ap?s atualizar os itens do carrinho

            if (!ItemsShoppingCart.Any())
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro inesperado: {ex.Message}", "OK");
            return false;
        }        
    }

    private void UpdatePriceTotal()
    {
        try
        {
            var precoTotal = ItemsShoppingCart.Sum(item => item.Price * item.Quantity);
            LblPrecoTotal.Text = precoTotal.ToString();
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", $"Ocorreu um erro ao atualizar o pre?o total: {ex.Message}", "OK");
        }
    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;

        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }

    private async void BtnDecrementar_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ShoppingCartItem itemCarrinho)
        {
            if (itemCarrinho.Quantity == 1) return;
            else
            {
                itemCarrinho.Quantity--;
                UpdatePriceTotal();
                await _apiService.UpdateQuantityItemCart(itemCarrinho.ProductId, "diminuir");
            }
        }

    }

    private async void BtnIncrementar_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ShoppingCartItem itemCarrinho)
        {
            itemCarrinho.Quantity++;
            UpdatePriceTotal();
            await _apiService.UpdateQuantityItemCart(itemCarrinho.ProductId, "aumentar");
        }
    }

    private async void BtnDeletar_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.BindingContext is ShoppingCartItem itemCarrinho)
        {
            bool resposta = await DisplayAlert("Confirma  o",
                          "Tem certeza que deseja excluir este item do carrinho?", "Sim", "N o");
            if (resposta)
            {
                ItemsShoppingCart.Remove(itemCarrinho);
                UpdatePriceTotal();
                await _apiService.UpdateQuantityItemCart(itemCarrinho.ProductId, "apagar");
            }
        }
    }

    private void BtnEditaEndereco_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddressPage());
    }
}
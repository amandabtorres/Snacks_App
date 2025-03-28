using Snacks_App.Models;
using Snacks_App.Services;
using Snacks_App.Validations;

namespace Snacks_App.Pages;

public partial class ProductDetailsPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private int _produtoId;
    private bool _loginPageDisplayed = false;

    private FavoriteService _favoritesService = new FavoriteService();
    private string? _imageUrl;

    public ProductDetailsPage(int produtoId,
                              string produtoNome,
                              ApiService apiService,
                              IValidator validator)
	{
		InitializeComponent();
        _apiService = apiService;
        _validator = validator;
        _produtoId = produtoId;
        Title = produtoNome ?? "Detalhe do Produto";
    }

    // M�todo chamado quando a p�gina aparece
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetProductDetails(_produtoId);
        AtualizaFavoritoButton();
    }

    private async Task<Product?> GetProductDetails(int produtoId)
    {
        var (produtoDetalhe, errorMessage) = await _apiService.GetProductDetails(produtoId);

        if (errorMessage == "Unauthorized" && !_loginPageDisplayed)
        {
            await DisplayLoginPage();
            return null;
        }

        // Verificar se houve algum erro na obten��o das produtos
        if (produtoDetalhe == null)
        {
            // Lidar com o erro, exibir mensagem ou logar
            await DisplayAlert("Erro", errorMessage ?? "N�o foi poss�vel obter o produto.", "OK");
            return null;
        }

        if (produtoDetalhe != null)
        {
            // Atualizar as propriedades dos controles com os dados do produto
            ImagemProduto.Source = produtoDetalhe.UrlImagem;
            LblProdutoNome.Text = produtoDetalhe.Name;
            LblProdutoPreco.Text = produtoDetalhe.Price.ToString();
            LblProdutoDescricao.Text = produtoDetalhe.Detail;
            LblPrecoTotal.Text = produtoDetalhe.Price.ToString();
            _imageUrl = produtoDetalhe.UrlImagem;
        }
        else
        {
            await DisplayAlert("Erro", errorMessage ?? "N�o foi poss�vel obter os detalhes do produto.", "OK");
            return null;
        }
        return produtoDetalhe;
    }

    private async void ImagemBtnFavorito_Clicked(object sender, EventArgs e)
    {
        try
        {
            var existeFavorito = await _favoritesService.ReadAsync(_produtoId);
            if (existeFavorito is not null)
            {
                await _favoritesService.DeleteAsync(existeFavorito);
            }
            else
            {
                var produtoFavorito = new FavoriteProduct()
                {
                    ProductId = _produtoId,
                    IsFavorite = true,
                    Detail = LblProdutoDescricao.Text,
                    Name = LblProdutoNome.Text,
                    Price = Convert.ToDecimal(LblProdutoPreco.Text),
                    ImageUrl = _imageUrl
                };

                await _favoritesService.CreateAsync(produtoFavorito);
            }
            AtualizaFavoritoButton();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
        }

    }

    private async void AtualizaFavoritoButton()
    {
        var existeFavorito = await
               _favoritesService.ReadAsync(_produtoId);

        if (existeFavorito is not null)
            ImagemBtnFavorito.Source = "heartfill";
        else
            ImagemBtnFavorito.Source = "heart";
    }


    private void BtnRemove_Clicked(object sender, EventArgs e)
    {
        if (int.TryParse(LblQuantidade.Text, out int quantidade) && decimal.TryParse(LblProdutoPreco.Text, out decimal precoUnitario))
        {
            // Decrementa a quantidade, e n o permite que seja menor que 1
            quantidade = Math.Max(1, quantidade - 1);
            LblQuantidade.Text = quantidade.ToString();

            // Calcula o pre o total
            var precoTotal = quantidade * precoUnitario;
            LblPrecoTotal.Text = precoTotal.ToString();
        }
        else
        {
            // Tratar caso as convers es falhem
            DisplayAlert("Erro", "Valores inv�lidos", "OK");
        }
    }

    private void BtnAdiciona_Clicked(object sender, EventArgs e)
    {
        if (int.TryParse(LblQuantidade.Text, out int quantidade) && decimal.TryParse(LblProdutoPreco.Text, out decimal precoUnitario))
        {
            // Incrementa a quantidade
            quantidade++;
            LblQuantidade.Text = quantidade.ToString();

            // Calcula o pre o total
            var precoTotal = quantidade * precoUnitario;
            LblPrecoTotal.Text = precoTotal.ToString(); // Formata como moeda
        }
        else
        {
            // Tratar caso as convers es falhem
            DisplayAlert("Erro", "Valores inv�lidos", "OK");
        }
    }

    private async void BtnIncluirNoCarrinho_Clicked(object sender, EventArgs e)
    {
        try
        {
            var carrinhoCompra = new ShoppingCart()
            {
                Quantity = Convert.ToInt32(LblQuantidade.Text),
                UnitPrice = Convert.ToDecimal(LblProdutoPreco.Text),
                Total = Convert.ToDecimal(LblPrecoTotal.Text),
                ProductId = _produtoId,
                ClientId = Preferences.Get("usuarioid", 0)
            };
            var response = await _apiService.AddItemInCart(carrinhoCompra);
            if (response.Data)
            {
                await DisplayAlert("Sucesso", "Item adicionado ao carrinho !", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Erro", $"Falha ao adicionar item: {response.ErrorMessage}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
        }
    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;

        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }
}
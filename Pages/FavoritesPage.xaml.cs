using Snacks_App.Models;
using Snacks_App.Services;
using Snacks_App.Validations;

namespace Snacks_App.Pages;

public partial class FavoritesPage : ContentPage
{
    private readonly FavoriteService _favoriteService;
    private readonly ApiService _apiService;
    private readonly IValidator _validator;

    public FavoritesPage(ApiService apiService, IValidator validator)
	{
		InitializeComponent();
        _favoriteService = ServiceFactory.CreateFavoritosService();
        _apiService = apiService;
        _validator = validator;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetProductsFavorites();
    }
    private async Task GetProductsFavorites()
    {
        try
        {
            var produtosFavoritos = await _favoriteService.ReadAllAsync();

            if (produtosFavoritos is null || produtosFavoritos.Count == 0)
            {
                CvProdutos.ItemsSource = null;//limpa a lista atual
                LblAviso.IsVisible = true; //mostra o aviso
            }
            else
            {
                CvProdutos.ItemsSource = produtosFavoritos;
                LblAviso.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro inesperado: {ex.Message}", "OK");
        }
    }

    private void CvProdutos_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as FavoriteProduct;

        if (currentSelection == null) return;

        Navigation.PushAsync(new ProductDetailsPage(currentSelection.ProductId,
                                                     currentSelection.Name!,
                                                     _apiService, _validator));

        ((CollectionView)sender).SelectedItem = null;
    }
}
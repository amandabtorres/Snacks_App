namespace Snacks_App.Pages;

public partial class CartEmptyPage : ContentPage
{
	public CartEmptyPage()
	{
		InitializeComponent();
	}

    private async void BtnRetornar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
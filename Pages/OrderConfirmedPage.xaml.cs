namespace Snacks_App.Pages;

public partial class OrderConfirmedPage : ContentPage
{
	public OrderConfirmedPage()
	{
		InitializeComponent();
	}

    private async void BtnRetornar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
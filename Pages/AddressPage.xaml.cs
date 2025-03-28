namespace Snacks_App.Pages;

public partial class AddressPage : ContentPage
{
	public AddressPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadSavedData();
    }

    private void LoadSavedData()
    {
        if (Preferences.ContainsKey("nome"))
            EntNome.Text = Preferences.Get("nome", string.Empty);

        if (Preferences.ContainsKey("endereco"))
            EntEndereco.Text = Preferences.Get("endereco", string.Empty);

        if (Preferences.ContainsKey("telefone"))
            EntTelefone.Text = Preferences.Get("telefone", string.Empty);
    }

    private void BtnSalvar_Clicked(object sender, EventArgs e)
    {
        Preferences.Set("nome", EntNome.Text);
        Preferences.Set("endereco", EntEndereco.Text);
        Preferences.Set("telefone", EntTelefone.Text);
        Navigation.PopAsync();
    }
}
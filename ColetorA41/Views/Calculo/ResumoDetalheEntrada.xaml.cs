using ColetorA41.ViewModel;
using CommunityToolkit.Maui.Views;

namespace ColetorA41.Views.Calculo;

public partial class ResumoDetalheEntrada : ContentPage
{
    private readonly CalculoViewModel vm;
    public ResumoDetalheEntrada(CalculoViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;

        this.vm = viewModel;
    }

    protected override bool OnBackButtonPressed()
    {
        var mensa = new Mensagem("info", "Navegação", "Utilize a navegação incluída no cálculo");
        Shell.Current.CurrentPage.ShowPopup(mensa);
        return true;
    }

}
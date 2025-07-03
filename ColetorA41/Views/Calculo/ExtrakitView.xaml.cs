using System.Globalization;
using ColetorA41.ViewModel;
using CommunityToolkit.Maui.Views;

namespace ColetorA41.Views.Calculo;

public partial class ExtrakitView : ContentPage
{
    private readonly CalculoViewModel vm;
    public ExtrakitView(CalculoViewModel viewModel)
	{
		InitializeComponent();
        this.BindingContext = viewModel;
        this.vm = viewModel;
    }

    protected override void OnAppearing()
    {

        base.OnAppearing();
        //this.vm.ObterExtrakit();
    }

    protected override bool OnBackButtonPressed()
    {
        var mensa = new Mensagem("info", "Navegação", "Utilize a navegação incluída no cálculo");
        Shell.Current.CurrentPage.ShowPopup(mensa);
        return true;
    }

    //FAS
    public class CodigoToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var codigo = value?.ToString() ?? string.Empty;

            if (codigo.StartsWith("8"))
                return Colors.Red;  // Cor vermelha se começar com 8

            return Colors.Black;   // Senão, cor preta
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}


using System.Globalization;
using ColetorA41.ViewModel;
using CommunityToolkit.Maui.Views;

namespace ColetorA41.Views.Calculo;

//FAS
public class CodigoToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var codigo = value?.ToString() ?? string.Empty;

        //if (codigo.StartsWith("8"))
        if (codigo == "0")
            return Colors.Black;  // Cor vermelha se começar com 8

        return Colors.Red;   // Senão, cor preta
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


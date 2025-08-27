using ColetorA41.ViewModel;
using CommunityToolkit.Maui.Core.Views;
using CommunityToolkit.Maui.Views;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;

namespace ColetorA41.Views.Calculo;

public partial class EstabTec : ContentPage
{
    private readonly CalculoViewModel _vm;
    public EstabTec(CalculoViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        try
        {
            Debug.WriteLine("EstabTec: Iniciando carregamento de estabelecimentos");
            
            // Verificar se o ViewModel está inicializado
            if (_vm == null)
            {
                Debug.WriteLine("EstabTec: ViewModel é null!");
                return;
            }

            // Verificar se a lista está inicializada
            if (_vm.listaEstab == null)
            {
                Debug.WriteLine("EstabTec: listaEstab é null!");
                return;
            }

            // Limpar lista antes de carregar
            _vm.listaEstab.Clear();
            
            Debug.WriteLine("EstabTec: Chamando ObterEstabelecimentos");
            
            // Carregar estabelecimentos
            await _vm.ObterEstabelecimentos();
            
            Debug.WriteLine($"EstabTec: Carregamento concluído. Itens: {_vm.listaEstab.Count}");
            
            // Forçar atualização da UI
            OnPropertyChanged(nameof(_vm.listaEstab));
            
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"EstabTec: Erro no carregamento: {ex.Message}");
            
            var erro = new Mensagem("erro", "Erro Carregamento", 
                $"Erro ao carregar estabelecimentos: {ex.Message}");
            await Shell.Current.CurrentPage.ShowPopupAsync(erro);
        }
    }

    protected override bool OnBackButtonPressed()
    {
        var mensa = new Mensagem("info", "Navegação", "Utilize a navegação incluída no cálculo");
        Shell.Current.CurrentPage.ShowPopup(mensa);
        return true;
    }
   
}
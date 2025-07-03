
using ColetorA41.ViewModel;
using ColetorA41.Views;
using ColetorA41.Views.Calculo;
using ColetorA41.Views.Monitor;


namespace ColetorA41
{
    public partial class AppShell : Shell
    {
       

        public AppShell(AppShellViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            
            //Adicionar Rotas das Views
            Routing.RegisterRoute(nameof(Loading)       , typeof(Loading));
            Routing.RegisterRoute(nameof(Login)         , typeof(Login));
            Routing.RegisterRoute(nameof(EstabTec)      , typeof(EstabTec));
            Routing.RegisterRoute(nameof(DadosNF)       , typeof(DadosNF));
            Routing.RegisterRoute(nameof(ExtrakitView)  , typeof(ExtrakitView));
            Routing.RegisterRoute(nameof(Resumo)        , typeof(Resumo));
            Routing.RegisterRoute(nameof(ResumoDetalhe) , typeof(ResumoDetalhe));
            Routing.RegisterRoute(nameof(ResumoDetalheEntrada), typeof(ResumoDetalheEntrada)); //FAS
            Routing.RegisterRoute(nameof(ResumoDetalhePago), typeof(ResumoDetalhePago));
            Routing.RegisterRoute(nameof(ResumoDetalheItem), typeof(ResumoDetalheItem));
            Routing.RegisterRoute(nameof(LeituraENC)    , typeof(LeituraENC));
            Routing.RegisterRoute(nameof(LoginAlmoxa)   , typeof(LoginAlmoxa));
            Routing.RegisterRoute(nameof(LoadingCalculo), typeof(LoadingCalculo));
            Routing.RegisterRoute(nameof(Erro), typeof(Erro));
            Routing.RegisterRoute(nameof(Processos), typeof(Processos));
            Routing.RegisterRoute(nameof(ResumoFinal), typeof(ResumoFinal));
            Routing.RegisterRoute(nameof(Embalagem), typeof(Embalagem));
            Routing.RegisterRoute(nameof(EmbalagemLoading), typeof(EmbalagemLoading));
            Routing.RegisterRoute(nameof(EmbalagemPrimeiraNota), typeof(EmbalagemPrimeiraNota));
            Routing.RegisterRoute(nameof(Reparo), typeof(Reparo));
            Routing.RegisterRoute(nameof(ReparoEdicaoItemReparo), typeof(ReparoEdicaoItemReparo));
            Routing.RegisterRoute(nameof(LeituraPagtos), typeof(LeituraPagtos));

        }

       
    }
}

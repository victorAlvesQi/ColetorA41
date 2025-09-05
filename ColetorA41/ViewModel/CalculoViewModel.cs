using System.Collections.ObjectModel;
using System.Diagnostics;
using ColetorA41.Extensions;
using ColetorA41.Models;
using ColetorA41.Services;
using ColetorA41.Views;
using ColetorA41.Views.Calculo;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;


namespace ColetorA41.ViewModel
{
    public partial class CalculoViewModel : BaseViewModel
    {
        private readonly TotvsService _service;
        private readonly TotvsService46 _service46;
        private readonly IConfiguration _config;
        private int controle = 0;

        private bool _temMaisPaginas = true;
        private bool _carregandoPagina = false;

        public CalculoViewModel(TotvsService totvsService, 
                                TotvsService46 totvsService46,
                                IConfiguration config
                                )
        {
            _service = totvsService;
            _service46 = totvsService46;
            _config = config;
          
        }


        #region Getter-Setter
        private Estabelecimento _estabSelecionado;


        /*
         * Victor - Ajuste de thread
         * 
        public Estabelecimento EstabSelecionado
        {
            get =>  _estabSelecionado;
            set
            {

                if (_estabSelecionado != value)
                {
                    _estabSelecionado = value;
                    //Task.Run(async () =>
                    //{

                        IsBusy = false;
                        this.BuscaTecnico = string.Empty;
                        this.CriterioBuscaTecnico = string.Empty;
                        this.listaTecnico.Clear();
                    //await this.CarregarTecnicosEstabelecimento();
                    //await this.ObterTransporte();
                    this.CarregarTecnicosEstabelecimento();
                    this.ObterTransporte();

                    // });

                }
            }
        }

        */

        public Estabelecimento EstabSelecionado
        {
            get => _estabSelecionado;
            set
            {

                if (_estabSelecionado != value)
                {
                    _estabSelecionado = value;
                    //Task.Run(async () =>
                    //{

                    OnPropertyChanged();
                    _ = OnEstabSelecionadoChangedAsync();

                }
            }
        }

        private async Task OnEstabSelecionadoChangedAsync()
        {
            try
            {
                IsBusy = true; ;
                this.BuscaTecnico = string.Empty;
                this.CriterioBuscaTecnico = string.Empty;

                ResetarPaginacaoTecnicos();

                await this.CarregarTecnicosEstabelecimento();
                await this.ObterTransporte();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Tecnico _tecnicoSelecionado;

        /*
       * Victor - Ajuste de thread
       * 

          public Tecnico TecnicoSelecionado
          {
              get => _tecnicoSelecionado;
              set
              {
                  if (_tecnicoSelecionado != value)
                  {
                      _tecnicoSelecionado = value;
                      Task.Run(async () => await this.ObterEntrega());

                  }
              }
          }
        */
        public Tecnico TecnicoSelecionado
        {
            get => _tecnicoSelecionado;
            set
            {
                if (_tecnicoSelecionado != value)
                {
                    _tecnicoSelecionado = value;
                    OnPropertyChanged();

                    _ = OnTecnicoSelecionadoChangedAsync(_tecnicoSelecionado.codTec);

                }
            }
        }

        public async Task OnTecnicoSelecionadoChangedAsync(int pCodTecnico)
        {
            await ObterEntrega(pCodTecnico);
        }

        #endregion

        #region Lista Compartilhadas
       public ObservableCollection<Estabelecimento> listaEstab { get; private set; } = new();
      public ObservableRangeCollection<Tecnico> listaTecnico { get; private set; } = new();
      public ObservableCollection<Enc> listaEnc { get; private set; } = new();
      public ObservableCollection<Transporte> listaTranspCompleta { get; private set; } = new();
      public ObservableCollection<Transporte> listaTransporteEntra { get; private set; } 
      public ObservableCollection<Transporte> listaTransporteSai { get; private set; } 
      public ObservableCollection<Entrega> listaEntrega { get; private set; } = new();
      public ObservableCollection<Extrakit> listaExtrakit { get; private set; } = new();
      public ObservableCollection<object> listaExtrakitSelecionados { get; set; } = new();
      public ObservableCollection<object> listaPagtosSelecionados { get; set; } = new();
      public ObservableCollection<Extrakit> listaExtrakitNaoSelecionados { get; set; } = new();
      public ObservableCollection<Ficha> listaCalculo { get; private set; } = new();
      public ObservableCollection<Semsaldo> listaSemSaldo { get; private set; } = new();
      public ObservableCollection<Models.Resumo> listaResumo { get; private set; } = new ();
      public ObservableRangeCollection<ItemFicha> listaItensFicha { get; private set; } = new();
      public ObservableCollection<ItemFicha> listaPagtos { get; private set; }  = new();
      #endregion

      #region Listas Locais
      #endregion

      #region Variaveis Compartilhadas

      [ObservableProperty]
      string labelErro = "";

      [ObservableProperty]
      int qtdePendentesPagto = 0;

      [ObservableProperty]
      int qtdeTotalPagto = 0;

      [ObservableProperty]
      int nrProcessSelecionado;

      [ObservableProperty]
      string etiquetaEnc;

      //Dados da Nota
      [ObservableProperty]
      string serieEntra;

      [ObservableProperty]
      string serieSaida;

      [ObservableProperty]
      string rpw;

      [ObservableProperty]
      string entrega;

      [ObservableProperty]
      int qtdeETSelecionadas;

      [ObservableProperty]
      int qtdeETNaoSelecionadas;

      [ObservableProperty]
      Transporte transpEntraSelecionado;

      [ObservableProperty]
      ParamEstabel parametroSelecionado;

      [ObservableProperty]
      Transporte transpSaidaSelecionado;

      [ObservableProperty]
      Entrega entregaSelecionada;

      [ObservableProperty]
      ItemFicha itemFichaSelecionada;

      [ObservableProperty]
      string tipoFichaSelecionado;

      [ObservableProperty]
      int usuarioAlmoxa_;

      [ObservableProperty]
      string senhaAlmoxa;

      [ObservableProperty]
      int tipoCalculo;

      [ObservableProperty]
      string numEnc;

      [ObservableProperty]
      string itemPagto;

      [ObservableProperty]
      string lblAprovar;

      [ObservableProperty]
      string lblAprovarSemEntrada;

      [ObservableProperty]
      string rowIdOS = "";

      [ObservableProperty]
      string labelLoading;

      //Login Almoxarifado
      [ObservableProperty]
      LabelResumo fichas = new();

      [ObservableProperty]
      string criterioBuscaTecnico;

      [ObservableProperty]
      string criterioBuscaItemFicha = "";

      [ObservableProperty]
      bool isTotal = false;

      [ObservableProperty]
      bool isBtnAprovar = true;

      [ObservableProperty]
      bool isBtnAprovarSS = true;

      [ObservableProperty]
      bool isParcial = true;

      [ObservableProperty]
      bool isET = false;

      [ObservableProperty]
      int iRadio=1;

      #endregion

      #region Variaveis Locais


      #endregion

      #region Funcoes Compartilhadas


      private string buscaTransporteEntra;
      public string? BuscaTransporteEntra
      {
          get
          {
              return buscaTransporteEntra;
          }
          set
          {
              if (buscaTransporteEntra == value) return;

              buscaTransporteEntra = value;

              if (buscaTransporteEntra == string.Empty )
              {
                  BuscarTranspEntra(string.Empty);
              }
          }
      }

      private string buscaTransporteSai;
      public string? BuscaTransporteSai
      {
          get
          {
              return buscaTransporteSai;
          }
          set
          {
              if (buscaTransporteSai == value) return;
              buscaTransporteSai = value;
              if (buscaTransporteSai == string.Empty)
              {
                   BuscarTranspSai(string.Empty);

              }
          }
      }


      [RelayCommand]
      async Task BuscarTranspEntra(string criterio)
      {
          IsBusy = true;
          this.listaTransporteEntra.Clear();
          if (string.IsNullOrEmpty(criterio))
          {
              foreach (var item in this.listaTranspCompleta)
              {
                  this.listaTransporteEntra.Add(item);
              }
          }
          else
          {
              //Localizar registros
              var listaEncontrada = this.listaTranspCompleta.Where(x => x.identific.ToUpper().Contains(criterio.ToUpper()));

              //Caso existe apenas 1 setar 
              this.TranspEntraSelecionado = null;

              //Popular Lista
              foreach (var item in listaEncontrada)
              {
                  this.listaTransporteEntra.Add(item);
              }

              if (listaEncontrada.Count() == 1)
              {
                  this.TranspEntraSelecionado = listaEncontrada.First();
              }

          }
          IsBusy = false;
      }

      [RelayCommand]
      async Task BuscarTranspSai(string criterio)
      {
          IsBusy = true;
          this.listaTransporteSai.Clear();
          if (string.IsNullOrEmpty(criterio))
          {
              foreach (var item in this.listaTranspCompleta)
              {
                  this.listaTransporteSai.Add(item);
              }
          }
          else
          {
              //Localizar registros
              var listaEncontrada = this.listaTranspCompleta.Where(x => x.identific.ToUpper().Contains(criterio.ToUpper()));

              //Caso existe apenas 1 setar 
              this.TranspSaidaSelecionado = null;

              //Popular Lista
              foreach (var item in listaEncontrada)
              {
                  this.listaTransporteSai.Add(item);
              }

              if (listaEncontrada.Count() == 1)
              {
                  this.TranspSaidaSelecionado = listaEncontrada.First();
              }

          }
          IsBusy = false;
      }

      [RelayCommand]
      void DownloadVersao()
      {
          var obj = _service.DownloadVersao();
          //Pendencia:
          //Transformar obj em Stream e depois gerar um arquivo
          //Obter permissao Android para geracao arquivo

      }
      [RelayCommand]
      void DetalheFicha(object obj)
      {

      }

      //Extrakit
      [RelayCommand]
      void SelecionarTodosExtrakit()
      {
          this.listaExtrakitSelecionados.Clear();

          foreach (var item in this.listaExtrakit)
          {
              this.listaExtrakitSelecionados.Add(item);
          }
      }

      [RelayCommand]
      void SelecionarNenhumExtrakit()
      {
          this.listaExtrakitSelecionados.Clear();
      }

      [RelayCommand]
      async Task BuscarTecnico(string criterio)
      {
           await Task.Run(async ()=> {
              IsBusy = true;
              CriterioBuscaTecnico = criterio;
              await ObterTecnicosEstab();
              IsBusy = false;
              });
      }

      [RelayCommand]
      async Task BuscarItemFicha(string criterio)
      {
          // Victor alves - COmentado p/ erro de busca nas fichas e lentidão
          //await Task.Run(async () => {

          IsBusy = true;

          CriterioBuscaItemFicha = criterio;

          // Victor Alves - add revisao de congelar tela
          listaItensFicha.Clear();

          _temMaisPaginas = true;
          // Fim - Victor Alves - add revisao de congelar tela

          await CarregarFichas();

          IsBusy = false;
          //});


      }

      [RelayCommand]
      public async void ListaETSelecionada()
      {
          this.listaExtrakitNaoSelecionados.Clear();
          foreach(var item in this.listaExtrakit)
          {
              if (this.listaExtrakitSelecionados.OfType<Extrakit>().Where(x=>x.cRowId==item.cRowId).FirstOrDefault() == null)
              {
                  this.listaExtrakitNaoSelecionados.Add(item);
              }
          }
          QtdeETSelecionadas = listaExtrakitSelecionados.OfType<Extrakit>().Sum(s => s.qtSaldo);
          QtdeETNaoSelecionadas = listaExtrakitNaoSelecionados.Sum(s => s.qtSaldo);
      }

      [RelayCommand]
      public async void ListaPagtosSelecionada()
      {
          foreach (var item in listaPagtos)
          {

              var encontrado = this.listaPagtosSelecionados.OfType<ItemFicha>().Where(x => x.id == item.id && x.leituraPagto == false).FirstOrDefault();
              if(encontrado != null)
                 encontrado.leituraPagto = true;
          }

          this.QtdePendentesPagto = this.listaPagtos.Where(item => item.leituraPagto).Count();

      }

      [RelayCommand]
      public async Task LoginAlmoxa()
      {
          try
          {
              this.IsBusy = true;
              var ok = await _service.LoginAlmoxa(this.NrProcessSelecionado,
                                                  this.EstabSelecionado.codEstab,
                                                  this.UsuarioAlmoxa_,
                                                  this.SenhaAlmoxa);
              if (ok.senhaValida)
              {
                  //await ChamarResumo();
                  await ChamarLoadingCalculo();
              }
              else
              {
                  this.IsBusy = false;
                  var erro = new Mensagem("erro", "Erro Login", ok.mensagem);
                  await Shell.Current.CurrentPage.ShowPopupAsync(erro);
                  return;

              }
          }
          catch (Exception ex)
          {

              throw;
          }


      }

      [RelayCommand]
      private void SelectionChanged()
      {
          foreach (var p in listaExtrakitSelecionados)
          {
              if (p is Extrakit person)
              {
                  Console.WriteLine($"{person.itCodigo} is selected");
              }
          }
      }

      [RelayCommand]
      async Task ChamarEstabTec()
      {
          try
          {
              Debug.WriteLine("CalculoViewModel: Iniciando ChamarEstabTec");

              // Verificar se Shell.Current está disponível
              if (Shell.Current == null)
              {
                  Debug.WriteLine("CalculoViewModel: Shell.Current é null!");
                  var erro = new Mensagem("erro", "Erro Navegação", "Shell.Current não está disponível.");
                  await Application.Current.MainPage.DisplayAlert("Erro", "Shell.Current é null", "OK");
                  return;
              }

              Debug.WriteLine("CalculoViewModel: Shell.Current disponível, tentando navegar para EstabTec");

              // Tentar navegar para EstabTec
              await Shell.Current.GoToAsync($"{nameof(EstabTec)}");

              Debug.WriteLine("CalculoViewModel: Navegação para EstabTec concluída");

          }
          catch (Exception ex)
          {
              Debug.WriteLine($"CalculoViewModel: Erro na navegação para EstabTec: {ex.Message}");

              var erro = new Mensagem("erro", "Erro Navegação", $"Erro ao navegar para EstabTec: {ex.Message}");
              await Application.Current.MainPage.DisplayAlert("Erro", $"Erro na navegação: {ex.Message}", "OK");
          }
      }

      [RelayCommand]
      async Task ChamarResumoDetalhe(string tipoFicha)
      {
          if (tipoFichaSelecionado != tipoFicha)
          {
              //listaItensFicha.Clear();
              tipoFichaSelecionado = tipoFicha;

          }


          // Victor Alves - add revisao de congelar tela
          listaItensFicha.Clear();

          _temMaisPaginas = true;
          // Fim - Victor Alves - add revisao de congelar tela

          await this.CarregarFichas();
          await Shell.Current.GoToAsync($"{nameof(ResumoDetalhe)}");
      }

      //FAS
      [RelayCommand]
      async Task ChamarResumoDetalheEntrada(string tipoFicha)
      {
          if (tipoFichaSelecionado != tipoFicha)
          {
              //listaItensFicha.Clear();
              tipoFichaSelecionado = tipoFicha;

          }

          // Victor Alves - add revisao de congelar tela
          listaItensFicha.Clear();

          _temMaisPaginas = true;
          // Fim - Victor Alves - add revisao de congelar tela

          //v01 listaItensFicha.Clear();
          await this.CarregarFichas();
          await Shell.Current.GoToAsync($"{nameof(ResumoDetalheEntrada)}"); //FAS
      }

      /*junim
      [RelayCommand]
      async Task ChamarResumoDetalhe(string tipoFicha)
      {
          if (tipoFichaSelecionado != tipoFicha)
          {
              listaItensFicha.Clear();
              tipoFichaSelecionado = tipoFicha;

          }
          listaItensFicha.Clear();
          await this.CarregarFichas();
          await Shell.Current.GoToAsync($"{nameof(ResumoDetalhe)}");
      }
      */
        [RelayCommand]
        async Task ChamarResumoDetalhePagto(string tipoFicha)
        {
            if (tipoFichaSelecionado != tipoFicha)
            {
                listaItensFicha.Clear();
                tipoFichaSelecionado = tipoFicha;

            }

            await this.CarregarFichasPagto();
            await Shell.Current.GoToAsync($"{nameof(ResumoDetalhePago)}");
        }

        [RelayCommand]
        async Task CarregarFichas()
        {
            if (!_temMaisPaginas || _carregandoPagina) return;
            
            _carregandoPagina = true;

            Debug.WriteLine("Carregando ficha " + Environment.StackTrace);
            
            IsBusy = true;

            //this.listaItensFicha.Clear();
            //Adicionar Extrakit fora do processo na selecao do Geral
            if (this.TipoFichaSelecionado == "Geral" && this.listaItensFicha.Count <= 0)
            {
                this.controle = 0; //FAS
                int count = 0;

                foreach (var item in this.listaExtrakitNaoSelecionados)
                {
                    count++;

                    listaItensFicha.Add(new ItemFicha
                    {
                        seqItem = count,
                        itCodigo = item.itCodigo,
                        itPrincipal = item.itCodigo,
                        tipo = item.tipo,
                        descItem = item.descItem,
                        qtPagar = 0,
                        qtRenovar = 0,
                        qtRuim = item.qtRuim,
                        qtSaldo = item.qtSaldo,
                        qtExtrakit = item.qtSaldo,
                        notaAnt = item.nroDocto,
                        material = "etforaprocesso"
                    });

                    //Sera preciso descontar esta qtidade de registros 
                    this.controle = this.controle + 1;
                }

            }

            //Itens sem saldo nao sera paginado
            if (this.TipoFichaSelecionado == "SemSaldo" && this.listaItensFicha.Count > 0)
                return;

            if (this.TipoFichaSelecionado == "Geral")
            {
                
            }
            else
            {
                if ((listaItensFicha.Count() - this.controle) < 20)
                {
                    //this.listaItensFicha.Clear();
                }
            }
            // if (IsBusy) return;
            
          //  IsBusy = true;

            var lista = await _service.ObterItensCalculoMobile(TipoCalculo,
                                                                TipoFichaSelecionado, 
                                                                NrProcessSelecionado, 
                                                                listaItensFicha.Count() - this.controle, 
                                                                20, 
                                                                BuscaItemFicha);

            // Adicionar Victor Teste
            if (lista != null && lista.items != null) { 
                int iCont = 0;
                foreach(var row in lista.items)
                {
                    iCont++;
                    row.seqItem = iCont;
                }
            }

            if (lista?.items != null && lista.items.Any())
            {
                listaItensFicha.AddRange(lista.items);


                if (lista.items.Count < 20)
                {
                    _temMaisPaginas = false;
                }
            }
            else
            {
                _temMaisPaginas = false;
            }


            IsBusy = false;
            _carregandoPagina = false;
        }

        async Task CarregarFichasPagto()
        {

           
        }

        private bool _temMaisTecnicos = true;
      //  private bool _carregamentoInicial = true;

        private void ResetarPaginacaoTecnicos()
        {
            _temMaisTecnicos = true;
            IsBusy = false;

           // _carregamentoInicial = true;
            listaTecnico.Clear();
        }


        [RelayCommand]
        async Task CarregarTecnicosEstabelecimento()
        {
         /*   if (_carregamentoInicial)
            {
                _carregamentoInicial = false;
                return;
            }
         */

            if (IsBusy || !_temMaisTecnicos) 
                return;
         
            try
            {
                IsBusy = true;

                await CarregarTecnicosEstabelecimentoSemBusy();
            }
            catch (Exception ex)
            {
                var msg = new Mensagem("erro", "Erro", ex.Message);
                await Shell.Current.ShowPopupAsync(msg);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task CarregarTecnicosEstabelecimentoSemBusy()
        {
            if (!_temMaisTecnicos || _estabSelecionado == null)
                return;

            try
            {
                // Erro aqui na busca
                var lista = await _service.ObterTecEstabMobile(this._estabSelecionado.codEstab, CriterioBuscaTecnico, listaTecnico.Count(), 20);

                if (lista != null && lista.Any())
                {
                    listaTecnico.AddRange(lista);

                    if (lista.Count < 20)
                    {
                        _temMaisTecnicos = false;
                    }
                }
                else
                {
                    _temMaisTecnicos = false;
                }


            }
            catch (Exception ex)
            {
                var msg = new Mensagem("erro", "Erro", ex.Message);
                await Shell.Current.ShowPopupAsync(msg);
            }
        }

        [RelayCommand]
        async Task ChamarDadosNF(string botaoVoltar="false")
        {
            if ((this.EstabSelecionado == null) || (this.TecnicoSelecionado == null))
            {
                var erro = new Mensagem("erro", "Erro Validação", "Dados do estabelecimento e técnicos não preenchidos corretamente!");
                await Shell.Current.CurrentPage.ShowPopupAsync(erro);
                return;
            }
            
            try
            {
                if (botaoVoltar == "false")
                {
                    //Zerar campos de busca
                    this.buscaTransporteEntra = string.Empty;
                    this.buscaTransporteSai = string.Empty;
                        
                    await this.ObterParametrosEstab();
                    await this.ObterDados();
                }

                await Shell.Current.GoToAsync($"{nameof(DadosNF)}");

            }
            catch (Exception ex)
            {
                IsBusy = false;
                var msg = new Mensagem("erro", "Erro", ex.Message);
                await Shell.Current.ShowPopupAsync(msg);
              
            }
           
            
        }

        [RelayCommand]
        async Task ChamarExtrakit(string botaoVoltar="false")
        {
            IsBusy = true;
            if (botaoVoltar == "false")
            {
                QtdeETNaoSelecionadas = 0;
                QtdeETNaoSelecionadas = 0;
                await this.ObterExtrakit();
                this.ListaETSelecionada();
            }
            await Shell.Current.GoToAsync($"{nameof(ExtrakitView)}");
            IsBusy = false;
        }

        [RelayCommand]
        async Task ChamarMainPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            
        }

        [RelayCommand]
        async Task ChamarLeituraENC()
        {
            await Shell.Current.GoToAsync($"{nameof(LeituraENC)}");
            await this.ObterEncs();
        }

        [RelayCommand]
        async Task ChamarLoginAlmoxa()
        {
            //Verificar se existe alguma etiqueta sem leitura

            if (listaEnc != null)
            {
                var lpendente = listaEnc.Where(o => o.flag != "OK").FirstOrDefault();
                if (lpendente != null)
                {

                    var erro = new Mensagem("erro", "Fichas Pendentes", "Verifique a leitura das etiquetas antes de continuar!");
                    await Shell.Current.CurrentPage.ShowPopupAsync(erro);
                    return;
                }
            }

            this.UsuarioAlmoxa_ = 0;
            this.SenhaAlmoxa = string.Empty;

            await Shell.Current.GoToAsync($"{nameof(LoginAlmoxa)}");
        }

        [RelayCommand]
        async Task LeituraENC(string numEnc)
        {

            await Task.Run(() =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    this.IsBusy = true;
                    if (string.IsNullOrEmpty(numEnc))
                    {
                        this.IsBusy = false;
                        return;
                    }

                    var item = await _service46.LeituraEnc(this.EstabSelecionado.codEstab, this.TecnicoSelecionado.codTec, numEnc, this.NrProcessSelecionado.ToString());
                    item.numEnc = numEnc;
                    if (item.flag.ToUpper() == "ERRO")
                    {
                        this.IsBusy = false;
                        var erro = new Mensagem("erro", "Erro Enc", item.mensagem);
                        await Shell.Current.CurrentPage.ShowPopupAsync(erro);
                        // this.listaEnc.Add(new Enc { flag = "Erro Leitura", numEnc = numEnc, itCodigo = item.mensagem });
                        this.NumEnc = string.Empty;

                        return;
                    }

                    //this.listaEnc.Add(item);
                    this.listaEnc.Clear();
                    await this.ObterEncs();
                    this.NumEnc = string.Empty;
                });
            });
        }

        [RelayCommand]
        async Task LeituraItemPagto(string itemPagto)
        {

            this.IsBusy = true;
           
            if (string.IsNullOrEmpty(itemPagto))
            {
                this.IsBusy = false;
                return;
            }

            var item = this.listaPagtos.Where(x => x.itCodigo.ToUpper() == itemPagto.ToUpper() && x.leituraPagto == false).FirstOrDefault();
            if (item != null)
            {
                item.leituraPagto = true;
            }
            await Task.Delay(1000);


            //Atualizar pendencia na tela
            this.QtdePendentesPagto = this.listaPagtos.Where(item => item.leituraPagto).Count();
            this.IsBusy = false;
            this.ItemPagto = string.Empty;
        }


        [RelayCommand]
        async Task EliminarEnc(Enc objEnc)
        {
            IsBusy = true;
            await _service46.Desmarcar(objEnc.cRowId, objEnc.cItemRowId);
            var item = this.listaEnc.Where(x => x.numEnc == objEnc.numEnc).First();
            this.listaEnc.Remove(item);
            IsBusy = false;
        }

       
        [RelayCommand]
        async Task EliminarPagtoSelecionado(ItemFicha obj)
        {
           

            IsBusy = true;
            var item = this.listaPagtos.Where(o => o.cRowId == obj.cRowId).First();

            var itemCalculo = this.listaCalculo.Where(o => o.tipo == this.TipoCalculo).FirstOrDefault();
            if (itemCalculo != null)
            {
                itemCalculo.qtGeral -= item.qtPagar;
            }

            await this._service.EliminarPorId(item.id, this._estabSelecionado.codEstab, this._tecnicoSelecionado.codTec);

            // Fichas.Geral = Fichas.Geral - item.qtPagar;
            this.listaPagtos.Remove(item);

            //Mostrar Acompanhamento
            this.QtdePendentesPagto = this.listaPagtos.Where(item => item.leituraPagto).Count();
            this.QtdeTotalPagto = this.listaPagtos.Count();


            await AtualizarLabelsContadores(TipoCalculo);
            IsBusy = false;
           


        }

        [RelayCommand]
        async Task EliminarPagto(ItemFicha obj)
        {
          
            var mensa = new MensagemSimNao("Eliminar Pagamento", "Deseja eliminar registro de pagamento ?");
            var result = await Shell.Current.CurrentPage.ShowPopupAsync(mensa);
            if (result is bool ok)
            {

                if (ok)
                {
                    IsBusy = true;
                    var item = this.listaPagtos.Where(o => o.cRowId == obj.cRowId).First();

                    var itemCalculo = this.listaCalculo.Where(o => o.tipo == this.TipoCalculo).FirstOrDefault();
                    if (itemCalculo != null)
                    {
                        itemCalculo.qtGeral -= item.qtPagar;
                    }

                    await this._service.EliminarPorId(item.id, this._estabSelecionado.codEstab, this._tecnicoSelecionado.codTec);

                    // Fichas.Geral = Fichas.Geral - item.qtPagar;
                    this.listaPagtos.Remove(item);

                    //Mostrar Acompanhamento
                    this.QtdePendentesPagto = this.listaPagtos.Where(item => item.leituraPagto).Count();
                    this.QtdeTotalPagto = this.listaPagtos.Count();


                    await AtualizarLabelsContadores(TipoCalculo);
                    IsBusy = false;
                }
            }
         
        }

       
        [RelayCommand]
        async Task EliminarTodosPagtos()
        {
            
            var mensa = new MensagemSimNao("Eliminar Pagamentos", "Deseja eliminar todos os registros de pagamentos não lidos ?");
            var result = await Shell.Current.CurrentPage.ShowPopupAsync(mensa);
            if (result is bool ok)
            {

                if (ok)
                {
                    IsBusy = true;
                    var itemCalculo = this.listaCalculo.Where(o => o.tipo == this.TipoCalculo).FirstOrDefault();
                   

                    for (int i = listaPagtos.Count - 1; i >= 0; i--)
                    {
                        var item = listaPagtos[i];
                        if (!item.leituraPagto)
                        {
                            //Para funcionamento online da rotina de leitura o tratamento sera offline
                            await this._service.EliminarPorId(item.id, this._estabSelecionado.codEstab, this._tecnicoSelecionado.codTec);
                            if (itemCalculo != null)
                            {
                                itemCalculo.qtGeral -= item.qtPagar;
                            }
                           // Fichas.Geral = Fichas.Geral - item.qtPagar;
                            listaPagtos.RemoveAt(i);
                        }
                    }
                   
                  
                   // this.listaPagtos.Clear();
                    await AtualizarLabelsContadores(TipoCalculo);

                    //Mostrar Acompanhamento
                    this.QtdePendentesPagto = this.listaPagtos.Where(item => item.leituraPagto).Count();
                    this.QtdeTotalPagto = this.listaPagtos.Count();


                    IsBusy = false;
                }
            }
            

        }

        [RelayCommand]
        async Task DetalheItemFicha(ItemFicha obj)
        {
            IsBusy = true;
            ItemFichaSelecionada = obj;
            await Shell.Current.GoToAsync($"{nameof(ResumoDetalheItem)}");
            IsBusy = false;

        }

        [RelayCommand]
        async Task ChamarLoadingCalculo()
        {
            try
            {
                await Task.Delay(500);
                this.IsBusy = true;
                //Apagar os calculos anteriores
                this.Fichas = new();
                LabelLoading = "Gerando Resumo Cálculo";
                await Shell.Current.GoToAsync($"{nameof(LoadingCalculo)}");
                await this.PrepararCalculo();

                //Ajustar a lista de pagamentos
                foreach (var item in listaPagtos)
                {
                    item.qtPagarEdicao = item.qtPagar;
                }

                //Mostrar Acompanhamento
                this.QtdePendentesPagto = this.listaPagtos.Where(item=>item.leituraPagto).Count();
                this.QtdeTotalPagto = this.listaPagtos.Count();


                this.IsBusy = false;
                if (this.listaPagtos.Count() == 0)
                {
                    await Shell.Current.GoToAsync($"{nameof(Views.Calculo.Resumo)}");
                }
                else
                {
                    await Shell.Current.GoToAsync($"{nameof(LeituraPagtos)}");
                }
            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message , "OK");
               
            }
            

        }

        [RelayCommand]
        async Task ChamarLeituraPagtos()
        {

            if(this.listaPagtos.Count() == 0)
            {
                await this.ChamarExtrakit("true");
            }
            else
            {
                await Shell.Current.GoToAsync($"{nameof(LeituraPagtos)}");
            }
            
        }

        [RelayCommand]
        async Task ChamarResumo()
        {

            //Quantidade Informada deve ser menor ou igual qtde calculada
            var listaErros = string.Empty;
            foreach (var item in this.listaPagtos)
            {
                if (item.qtPagarEdicao > item.qtPagar)
                {
                    listaErros += $"Item: {item.itCodigo}, qtde Info: {item.qtPagarEdicao} maior que calculada: {item.qtPagar}.\n\n";
                }
            }
            if (listaErros.Length > 5)
            {
                var msg = new Mensagem("ERROR", "Erro de Quantidade", listaErros);
                await Shell.Current.CurrentPage.ShowPopupAsync(msg);
                return;
            }
         

            //A tela do resumo só podera ser apresentada se todos os pagamentos foram lidos
            var lpendente = this.listaPagtos.Where(item => !item.leituraPagto).FirstOrDefault();
            if (lpendente != null)
            {
                var msg = new Mensagem("ERROR", "Leituras Pendentes","Verificar lista de pagamentos !");
                await Shell.Current.CurrentPage.ShowPopupAsync(msg);
                return;
            }

            ObservableRangeCollection<ItemFicha> listaModificada = new();
            foreach (var item in listaPagtos)
            {
                item.qtPagar = item.qtPagarEdicao;
                listaModificada.Add(item);
            }

            //Mandar a lista de pagamentos alteradas no front para o back atualizar
            var resp = this._service.AjustarListaPagtosMobile(new AcertoPagamentoRequest
            {
                nrProcess = this.NrProcessSelecionado,
                items = listaModificada?.ToList()
            });
                
            IsTotal = TipoCalculo == 1;
            IsParcial = TipoCalculo == 2 ;
            IsET = TipoCalculo == 3;

            await this.AtualizaLblBotoes(TipoCalculo);
            await this.AtualizarLabelsContadores(TipoCalculo);
            await Shell.Current.GoToAsync($"{nameof(Views.Calculo.Resumo)}");
        }
       
        [RelayCommand]
        async Task GerarInforme()
        {
            var mensa = new MensagemSimNao("Gerar Informe", "Deseja gerar o Informe de OS ?");
            var result = await Shell.Current.CurrentPage.ShowPopupAsync(mensa);
            if (result is bool ok)
            {
                if (ok)
                {

                    IsBusy = true;
                    LabelLoading = "Gerando Arquivo de Informe";
                    await Shell.Current.GoToAsync($"{nameof(LoadingCalculo)}");

                    var informe = await _service46.ImprimirOS(RowIdOS);
                    if (informe != null)
                    {

                        IsBusy = false;
                        var erro = new Mensagem("ok", "Impressão OS", $"NumPedExec: {informe.NumPedExec} \nArquivo: {informe.Arquivo}");
                        await Shell.Current.CurrentPage.ShowPopupAsync(erro);
                        await Shell.Current.GoToAsync($"{nameof(Views.Calculo.Resumo)}");
                    }
                }
            }
            IsBusy = false;
        }

        [RelayCommand]
        async Task ChamarDetalheResumo()
        {
            //await Shell.Current.GoToAsync($"{nameof(ResumoDetalhe)}");
            await Shell.Current.GoToAsync("..");
        }        

        [RelayCommand]
        async Task RadioTipoCalculo()
        {
            IsBusy = true;
            var tipo = TipoCalculo;
            await this.AtualizaLblBotoes(tipo);
            await this.AtualizarLabelsContadores(tipo);
            IsBusy = false;
        }

        [RelayCommand]
        async Task AprovarCalculo(string tipoAprovacao)
        {
            var mensa = new MensagemSimNao("Execução Cálculo", "Confirma a execução do cálculo ?");
            var result = await Shell.Current.CurrentPage.ShowPopupAsync(mensa);
            if (result is bool ok)
            {
                if (ok)
                {
                    var paramTela = new ParamCalculo
                    {
                         opcao = TipoCalculo,
                         tipoAprovacao= Convert.ToInt32(tipoAprovacao),
                         nrProcess= NrProcessSelecionado,
                         codEstab=EstabSelecionado.codEstab,
                         codEmitente= TecnicoSelecionado.codTec,
                         codEntrega=EntregaSelecionada.codEntrega,
                         serieEntra=SerieEntra,
                         serieSai=SerieSaida,
                         codTranspEntra=TranspEntraSelecionado.codTransp,
                         codTranspSai=TranspSaidaSelecionado.codTransp,
                    };

                    //Executar o calculo
                    var resp = await this._service.AprovarCalculo(paramTela);
                    if(resp.type != null && resp.type == "error")
                    {
                        var mensaCalc = new Mensagem("error", "Cálculo Execução", "Erro execução do cálculo");
                        await Shell.Current.CurrentPage.ShowPopupAsync(mensaCalc);
                        return;

                    }
                    else
                    {
                        var mensaCalc = new Mensagem("ok", "Cálculo Execução", "Execução do cálculo realizada com sucesso! Processo RPW: " + resp.rpw);
                        await Shell.Current.CurrentPage.ShowPopupAsync(mensaCalc);
                        await ChamarEstabTec();
                    }

                }
            }
            IsBusy = false;
        }
        #endregion

        #region Funcoes Locais        

        public void resetGeral()
        {
            EstabSelecionado = null;
            TecnicoSelecionado = null;
            listaExtrakitSelecionados.Clear();
            listaTecnico.Clear();
            listaExtrakit.Clear();

        }

        private string buscaItemFicha;
        public string? BuscaItemFicha
        {
            get
            {
                return buscaItemFicha;
            }
            set
            {
                buscaItemFicha = value;
                if (buscaItemFicha.Length > 2)
                {
                    // Task.Run(async () => { await BuscarTecnico(searchText); }).Wait();
                }
                if (buscaItemFicha == "")
                {
                    Task.Run(async () => { await BuscarTecnico(""); }).Wait();
                }
            }
        }

        private string buscaTecnico;

        /* Victor Alvs - ajuste async
        public string? BuscaTecnico
        {
            get
            {
                return buscaTecnico;
            }
            set
            {
                SetProperty(ref buscaTecnico, value);

                if (buscaTecnico == value) return;

                buscaTecnico = value;
               
                if (buscaTecnico == string.Empty)
                {
                    Task.Run(async () => { await BuscarTecnico(""); });
                }
            }
        }

        */

        public string? BuscaTecnico
        {
            get
            {
                return buscaTecnico;
            }
            set
            {
                if (SetProperty(ref buscaTecnico, value))
                {
                    if (String.IsNullOrEmpty(buscaTecnico))
                    {
                        _ = BuscarTecnico("");
                    }
                }
            }
        }

        public async Task ObterEstabelecimentos()
        {
            try
            {
                Debug.WriteLine("CalculoViewModel: Iniciando ObterEstabelecimentos");
                
                this.IsBusy = true;
                
                // Verificar se o serviço está inicializado
                if (_service == null)
                {
                    Debug.WriteLine("CalculoViewModel: _service é null!");
                    var erro = new Mensagem("erro", "Erro Inicialização", "Serviço não inicializado.");
                    await Shell.Current.CurrentPage.ShowPopupAsync(erro);
                    return;
                }
                
                // Adicionar timeout para evitar travamento
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                
                Debug.WriteLine("CalculoViewModel: Chamando _service.ObterEstabelecimentos()");
                var lista = await _service.ObterEstabelecimentos().WaitAsync(cts.Token);
                
                Debug.WriteLine($"CalculoViewModel: Resposta recebida. Lista é null: {lista == null}");
                
                if (lista == null || !lista.Any())
                {
                    Debug.WriteLine("CalculoViewModel: Lista vazia ou nula");
                    var erro = new Mensagem("erro", "Erro Carregamento", "Não foi possível carregar os estabelecimentos. Verifique sua conexão de rede.");
                    await Shell.Current.CurrentPage.ShowPopupAsync(erro);
                    return;
                }

                Debug.WriteLine($"CalculoViewModel: Populando lista com {lista.Count} itens");
                
                this.listaEstab.Clear();
                foreach (var item in lista.OrderBy(x => x.identific))
                {
                    this.listaEstab.Add(item);
                }
                
                // CORREÇÃO: Forçar notificação da propriedade para atualizar a UI
                OnPropertyChanged(nameof(listaEstab));


                // Victor Alves - direciona tela Estab
                /* 
                if (this._estabSelecionado == null)
                {
                    ChamarEstabTec();
                }
                */

                Debug.WriteLine($"CalculoViewModel: Lista populada com {this.listaEstab.Count} itens");
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("CalculoViewModel: Timeout na requisição");
                var erro = new Mensagem("erro", "Timeout de Rede", "A requisição demorou muito. Verifique sua conexão de rede.");
                await Shell.Current.CurrentPage.ShowPopupAsync(erro);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CalculoViewModel: Erro: {ex.Message}");
                var erro = new Mensagem("erro", "Erro de Rede", $"Erro ao carregar estabelecimentos: {ex.Message}");
                await Shell.Current.CurrentPage.ShowPopupAsync(erro);
            }
            finally
            {
                this.IsBusy = false;
                Debug.WriteLine("CalculoViewModel: ObterEstabelecimentos concluído");
            }
        }

        public async Task VerificarVersao()
        {
            this.IsBusy = true;
            IsError = await _service.VerificarVersaoMobile(AppInfo.Current.VersionString);
            this.IsBusy = false;
        }

        public async Task ObterTecnicosEstab()
        {
            if (this._estabSelecionado == null) 
                return;

            try
            {
                this.IsBusy = true;

                var lista = await _service.ObterTecEstabMobile(this._estabSelecionado.codEstab, CriterioBuscaTecnico, 0, 20);
                if (lista != null)
                {
                    this.listaTecnico.Clear();
                    this.listaTecnico.AddRange(lista);
                }

                //// Victor Alves - Melhoria de performance ao clicar no tecnico
                //if (this.listaTecnico != null && this.listaTecnico.Any())
                //{
                //    foreach (var row in this.listaTecnico)
                //    {
                //        await ObterEntrega(row.codTec);
                //    }
                //}

            }
            catch (Exception ex)
            {
                var erro = new Mensagem("erro", "Erro", ex.Message);
                await Shell.Current.CurrentPage.ShowPopupAsync(erro);
            }
            finally
            {
                this.IsBusy = false;
            }
        }
        public async Task ObterTransporte()
        {
            try
            {
              //  this.IsBusy = true;
                var lista = await _service.ObterTransportes();

                this.listaTranspCompleta.Clear();
                foreach (var item in lista.OrderBy(x => x.identific))
                {
                    this.listaTranspCompleta.Add(item);
                }

                listaTransporteEntra = new(this.listaTranspCompleta);
                listaTransporteSai = new(this.listaTranspCompleta);
            }
            finally
            {
             //   this.IsBusy = false;
            }
        }

        public async Task ObterEntrega(int pCodTec)
        {
            this.IsBusy = true;
            //var lista = await _service.ObterEntrega(this.TecnicoSelecionado.codTec, this.EstabSelecionado.codEstab);

            var lista = await _service.ObterEntrega(pCodTec, this.EstabSelecionado.codEstab);

            this.listaEntrega.Clear();
            foreach (var item in lista.OrderBy(x => x.identific))
            {
                this.listaEntrega.Add(item);
            }
            this.IsBusy = false;
        }
        public async Task ObterParametrosEstab()
        {
            this.IsBusy = true;
            var parametro = await _service.ObterParametrosEstab(this._estabSelecionado.codEstab);
            if (parametro != null)
            {
                await Task.Delay(2000);
                this.TranspEntraSelecionado = this.listaTranspCompleta[0]; 
                this.TranspSaidaSelecionado = this.listaTranspCompleta[0]; 
                this.EntregaSelecionada = this.listaEntrega.Where(item => item.codEntrega == parametro.codEntrega).FirstOrDefault();
                this.SerieEntra = parametro.serieEntra;
                this.SerieSaida = parametro.serieSai;
                this.Entrega = parametro.codEntrega;
                this.Rpw = parametro.rpw;
                
                TranspEntraSelecionado = this.listaTranspCompleta.Where(item => item.codTransp == parametro.codTranspEntra).FirstOrDefault();
                TranspSaidaSelecionado = this.listaTranspCompleta.Where(item => item.codTransp == parametro.codTranspSai).FirstOrDefault();

            }
            this.IsBusy = false;
        }
        public async Task ObterExtrakit()
        {
            this.IsBusy = true;
            this.listaExtrakit.Clear();
            var lista = await _service.ObterExtrakit(this.EstabSelecionado.codEstab, this.TecnicoSelecionado.codTec, this.NrProcessSelecionado);
            if (lista != null) {
                foreach (var item in lista)
                {
                    this.listaExtrakit.Add(item);
                }
            }
            this.IsBusy = false;
        }
        public async Task ObterDados()
        {
            try
            {
                this.IsBusy = true;
                //Gerar Numero de Processo se for preciso

                RowIdOS = await _service46.ObterDados(this.EstabSelecionado.codEstab, this.TecnicoSelecionado.codTec);

                //Obter Numero Gerado
                this.NrProcessSelecionado = await _service.ObterNrProcesso(this.EstabSelecionado.codEstab, this.TecnicoSelecionado.codTec);

                this.IsBusy = false;

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.IsBusy = false;
            }
        }
        async Task AtualizaLblBotoes(int tipo)
        {
            switch (tipo)
            {
                case 1:
                    {
                        LblAprovar = "Renovação Total";
                        LblAprovarSemEntrada = "Renovação Total";
                    }
                    break;

                case 2:
                    {
                        LblAprovar = "Renovação Parcial";
                        LblAprovarSemEntrada = "Renovação Parcial";
                    }
                    break;
                case 3:
                    {
                        LblAprovar = "ExtraKit";
                        LblAprovarSemEntrada = "Extrakit";
                    }
                    break;
            }
        }
        public async Task PrepararCalculo()
        {
            this.IsBusy = true;
            this.IsCalculated = true;

            //Gerar Numero de Processo se for preciso
            var calculo = await _service.PrepararCalculoMobile(this.EstabSelecionado.codEstab,
                                                               this.TecnicoSelecionado.codTec,
                                                               this.NrProcessSelecionado,
                                                               this.listaExtrakitSelecionados.OfType<Extrakit>().ToList());

            
            //Montar Fichar
            this.listaCalculo.Clear();
            foreach (var item in calculo.items)
            {
                this.listaCalculo.Add(item);
            }

            //Montar SemSaldo
            this.listaSemSaldo.Clear();
            foreach (var item in calculo.semsaldo)
            {
                this.listaSemSaldo.Add(item);
            }

            //Montar Pagamentos
            this.listaPagtos.Clear();
            foreach (var item in calculo.pagto.OrderBy(x=>x.codLocaliza))
            {
                if (!item.soEntrada)
                this.listaPagtos.Add(item);
            }

            //Chamar Tela
            await Task.Delay(500);
            TipoCalculo = 2;
            IsTotal = false;
            IsParcial = true;
            IsET = false;

            await this.AtualizaLblBotoes(2);
            await this.AtualizarLabelsContadores(2);
            

            this.IsBusy = false;
            //await this.ChamarResumo();

        }
        public async Task AtualizarLabelsContadores(int tipoCalculo)
        {

            TipoCalculo = tipoCalculo;

            var item = this.listaCalculo.Where(x => x.tipo == tipoCalculo).FirstOrDefault();
            if (item == null) return;

            //Extrakits nao selecionados
            var totalET = 0;
            foreach (var x in listaExtrakitNaoSelecionados)
            {
                totalET += x.qtDisp;
            }

            //Geral
            Fichas.Geral = item.qtGeral + totalET;

            //Gera Extrakit - Labels
            Fichas.GeralExtrakit = totalET;

            //Pagamento
            if (tipoCalculo == 3)
            {
                Fichas.Pagto = listaPagtos.Where(o=>o.soEntrada).Sum(o => o.qtPagarEdicao);
            }
            else
            {
                Fichas.Pagto = listaPagtos.Sum(o => o.qtPagarEdicao);
            }

            //Renovacoes
            Fichas.Renovacao = item.qtReno;

            //Somente Entrada
            Fichas.SomenteEntrada = item.qtSoEntra;

            //Extrakit
            Fichas.Extrakit = item.qtExtrakit;

            //Sem Saldo
            Fichas.SemSaldo = item.qtSemSaldo;

            IsBtnAprovar = true;
            IsBtnAprovarSS = true;

            if (TipoCalculo >= 2)
            {

                var resp = await this._service.AlteracaoTipoCalculoMobile(TipoCalculo.ToString(), this.NrProcessSelecionado.ToString());
                IsBtnAprovar = resp.btnAprovar;
                IsBtnAprovarSS = resp.btnAprovarSS;

                if (!IsBtnAprovar && !IsBtnAprovarSS)
                {
                    var mensa = new Mensagem("error", "Tipo de Cálculo", "Existe OS Informada para Nota Fiscal de Kit. Usar as opções Renovação Total ou Renovação Parcial");
                    await Shell.Current.CurrentPage.ShowPopupAsync(mensa);
                }
            }


            this.IsBusy = false;

        }
        public async Task ObterEncs()
        {
            this.IsBusy = true;
            var lista = await _service46.ObterEncs(this.EstabSelecionado.codEstab, this.TecnicoSelecionado.codTec);
            this.listaEnc.Clear();
            if (lista != null)
            {
                foreach (var item in lista)
                {
                    this.listaEnc.Add(item);
                }
            }
            this.IsBusy = false;
           

        }

        #endregion
    }
}

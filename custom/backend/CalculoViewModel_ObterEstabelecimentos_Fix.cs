using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColetorA41.Models;
using ColetorA41.Services;
using ColetorA41.Views;

namespace ColetorA41.ViewModel
{
    public partial class CalculoViewModelFix
    {
        private readonly TotvsService _service;
        private readonly TotvsService46 _service46;

        public CalculoViewModelFix(TotvsService totvsService, TotvsService46 totvsService46)
        {
            _service = totvsService;
            _service46 = totvsService46;
        }

        /// <summary>
        /// Versão corrigida do método ObterEstabelecimentos com melhor tratamento de erro
        /// </summary>
        public async Task ObterEstabelecimentos()
        {
            try
            {
                this.IsBusy = true;
                
                // Adicionar timeout para evitar travamento
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                
                var lista = await _service.ObterEstabelecimentos().WaitAsync(cts.Token);
                
                if (lista == null || !lista.Any())
                {
                    var erro = new Mensagem("erro", "Erro Carregamento", "Não foi possível carregar os estabelecimentos. Verifique sua conexão.");
                    await Shell.Current.CurrentPage.ShowPopupAsync(erro);
                    return;
                }

                this.listaEstab.Clear();
                foreach (var item in lista.OrderBy(x => x.identific))
                {
                    this.listaEstab.Add(item);
                }
            }
            catch (OperationCanceledException)
            {
                var erro = new Mensagem("erro", "Timeout", "A requisição demorou muito para responder. Verifique sua conexão.");
                await Shell.Current.CurrentPage.ShowPopupAsync(erro);
            }
            catch (Exception ex)
            {
                var erro = new Mensagem("erro", "Erro", $"Erro ao carregar estabelecimentos: {ex.Message}");
                await Shell.Current.CurrentPage.ShowPopupAsync(erro);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        /// <summary>
        /// Versão alternativa que verifica conectividade antes de fazer a requisição
        /// </summary>
        public async Task ObterEstabelecimentosComVerificacao()
        {
            try
            {
                this.IsBusy = true;
                
                // Verificar conectividade primeiro
                if (!IsInternetAvailable())
                {
                    var erro = new Mensagem("erro", "Sem Conexão", "Verifique sua conexão com a internet.");
                    await Shell.Current.CurrentPage.ShowPopupAsync(erro);
                    return;
                }

                // Timeout mais curto para evitar travamento
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                
                var lista = await _service.ObterEstabelecimentos().WaitAsync(cts.Token);
                
                if (lista == null || !lista.Any())
                {
                    var erro = new Mensagem("erro", "Dados Vazios", "Nenhum estabelecimento encontrado.");
                    await Shell.Current.CurrentPage.ShowPopupAsync(erro);
                    return;
                }

                this.listaEstab.Clear();
                foreach (var item in lista.OrderBy(x => x.identific))
                {
                    this.listaEstab.Add(item);
                }
            }
            catch (OperationCanceledException)
            {
                var erro = new Mensagem("erro", "Timeout", "A requisição demorou muito. Tente novamente.");
                await Shell.Current.CurrentPage.ShowPopupAsync(erro);
            }
            catch (Exception ex)
            {
                var erro = new Mensagem("erro", "Erro", $"Erro ao carregar estabelecimentos: {ex.Message}");
                await Shell.Current.CurrentPage.ShowPopupAsync(erro);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        /// <summary>
        /// Verifica se há conectividade com a internet
        /// </summary>
        private bool IsInternetAvailable()
        {
            try
            {
                var accessType = Connectivity.NetworkAccess;
                return accessType == NetworkAccess.Internet;
            }
            catch
            {
                return false;
            }
        }
    }
} 
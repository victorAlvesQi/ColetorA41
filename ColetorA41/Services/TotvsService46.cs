using ColetorA41.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColetorA41.Services
{
    public class TotvsService46 : BaseService
    {
        public TotvsService46(IHttpClientFactory httpClientFactory, IConfiguration config) : base(httpClientFactory, config)
        {
        }

        public async Task<string> ObterDados(string codEstabel, int codTecnico, string senha = "moto", string origem = "calculo")
        {
            var param = new NameValueCollection {
                {"codEstabel", codEstabel },
                {"codUsuario", codTecnico.ToString() },
                {"senha", senha },
                {"origem", origem }
            };

            var response = await GetAsync<OrdemServicoResponse>("apiesaa046/ObterDadosMobile", param);

            if (response.type == "error")
            {
                throw new Exception($"Code: {response.code}\nDetail: {response.detailedMessage}\nMessage: {response.message}");
            }

            return response.cRowId;
            
        }

        public async Task<Enc> LeituraEnc(string codEstabel, int codTecnico, string numEnc, string nrProcesso)
        {
            var param = new NameValueCollection {
                {"codEstabel", codEstabel },
                {"codTecnico", codTecnico.ToString() },
                {"numEnc", numEnc },
                {"nrProcesso", nrProcesso }
            };
            var response = await GetAsync<EncResponse>("apiesaa046/LeituraEnc", param);
            return response.items.FirstOrDefault();
        }

        public async Task<List<Enc>> ObterEncs(string codEstabel, int codTecnico)
        {
            var param = new NameValueCollection {
                {"codEstabel", codEstabel },
                {"codTecnico", codTecnico.ToString() },
            };
            try
            {
                var response = await GetAsync<EncResponse>("apiesaa046/ObterEncs", param);
                return response.items;
            }
            catch (Exception ex)
            {

                return null;
            }
            
        }

        public async Task<bool> Desmarcar(string cRowId, string cItemRowId)
        {
            var param = new NameValueCollection {
                {"cRowId", cRowId },
                {"cItemRowId", cItemRowId }
            };
            var response = await GetAsync<EncResponse>("apiesaa046/Desmarcar", param);
            return true;
        }

        public async Task<InformeResponse> ImprimirOS(string cRowId)
        {
            var param = new NameValueCollection {
                {"iExecucao", "2" },
                {"cRowId", cRowId }
            };
            var response = await GetAsync<InformeResponse>("apiesaa046/ImprimirOS", param);
            return response;
        }

        public async Task<Serie> ValidarSerie(string itCodigo, string numSerieItem)
        {
            var param = new NameValueCollection {
                {"itCodigo", itCodigo },
                {"numSerieItem", numSerieItem }
            };
            var response = await GetAsync<Serie>("apiesaa046/ValidarSerie", param);
            return response;
        }

    }
}

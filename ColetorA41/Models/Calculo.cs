using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ColetorA41.Models
{
    public class ParamCalculo
    {
        public int opcao { get; set; }
        public int tipoAprovacao { get; set; }
        public string? codEstab { get; set; }
        public int codEmitente { get; set; }
        public int nrProcess { get; set; }
        public string? serieEntra { get; set; }
        public string? serieSai { get; set; }
        public int codTranspEntra { get; set; }
        public int codTranspSai { get; set; }
        public string? codEntrega { get; set; }

    }

    public class EliminarPagtoRequest
    {
        public int id { get; set; } = 0;
        public string? codEstabel { get; set; }
        public int codTecnico { get; set; }
    }

    public class ParamCalculoRequest
    {
        public ParamCalculo paramsTela { get; set; }
    }

    public class CalculoResponse:BaseModel
    {
        public int rpw { get; set; }
    }

    public class Resumo
    {
        public int id { get; set; }
        public string? titulo { get; set; }
        public string? descricao { get; set; }

        public int quantidade { get; set; }

        public List<Resumo> resumos { get; set; }

    }

    public class Ficha
    {
        public int tipo { get; set; }
        public int qtSemSaldo { get; set; }
        public int qtGeral { get; set; }
        public int qtSoEntra { get; set; }
        public int qtPagto { get; set; }
        public int qtExtrakit { get; set; }
        public int qtReno { get; set; }


    }

    public class TipoCalculoResponse
    {
        public bool btnAprovar { get; set; }
        public bool btnAprovarSS { get; set; }
    }

        public class PrepararCalculoRequest
    {
        public string? CodEstab { get; set; }
        public int CodTecnico { get; set; }
        public int NrProcess { get; set; }
        public List<Extrakit> Extrakit { get; set; }
    }

    public class PrepararCalculoResponse:BaseModel
    {
        public List<Ficha> items { get; set; }
        public List<Semsaldo> semsaldo { get; set; }

        public List<ItemFicha> pagto { get; set; }
    }

    public class AcertoPagamentoRequest
    {
        public int nrProcess { get; set; }
        public List<ItemFicha> items { get; set; }
    }

    public class BaseResponse : BaseModel { }

    public class Semsaldo
    {
        public int qtPagar { get; set; }
        public int qtSaldo { get; set; }
        public string? kit { get; set; }
        public string? itCodigo { get; set; }
        public string? descItem { get; set; }
        public int qtMascara { get; set; }
    }

    public class Calculo: INotifyPropertyChanged
    {

        public int seqItem { get; set; }

        private bool leituraPagto1 = false;

        public bool soEntrada { get; set; }
        public string? tipo { get; set; }
        public int qtPagar { get; set; }
        public int qtPagarEdicao { get; set; } = 0;
        public string? itPrincipal { get; set; }
        public bool temPagto { get; set; }
        public bool leituraPagto { get => leituraPagto1; set => SetProperty(ref leituraPagto1, value); }
        public int qtRenovar { get; set; }
        public string? itCodigo { get; set; }
        public int numOS { get; set; }
        public int qtMascara { get; set; }
        public string? material { get; set; }
        public int seqOrdem { get; set; }
        public string? cRowId { get; set; }
        public int qtRuim { get; set; }
        public int qtSaldo { get; set; }
        public string? localiz { get; set; }
        public string? kit { get; set; }
        public string? codLocaliza { get; set; }
        public int qtTerc { get; set; }
        public int qtExtrakit { get; set; }
        public string? descItem { get; set; }
        public int qtDAT { get; set; }
        public int id { get; set; }
        public string? notaAnt { get; set; }

        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }


    public class AprovarCalculoRequest
    {
        public string? CodEstab { get; set; }
        public int CodTecnico { get; set; }
        public int NrProcess { get; set; }
    }

    public class ExecutarCalculoRequest
    {
        public ParamsTela paramTela { get; set; }
    }

    public class ParamsTela
    {
        public int opcao { get; set; }
        public int tipoAprovacao { get; set; }
        public string? codEstab { get; set; }
        public int codEmitente { get; set; }
        public int nrProcess { get; set; }
        public string? serieEntra { get; set; }
        public string? serieSai { get; set; }
        public string? codTranspEntra { get; set; }
        public string? codTranspSai { get; set; }
        public string? codEntrega { get; set; }
    }

    public class ExecutarCalculoResponse
    {

    }



}

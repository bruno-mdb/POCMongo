using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models.MongoDb
{
    public class TipoPedido : BaseModel
    {
        public enum EGrupo : short
        {
            [Description("Revenda CD")]
            RevendaCD = 101,
            [Description("Revenda EDL")]
            RevendaEDL = 102,

            [Description("Não revenda CD")]
            NaoRevendaCD = 201,
            [Description("Não revenda EDL")]
            NaoRevendaEDL = 202,

            [Description("Outros")]
            Outros = 301
        }

        public enum ERevendaNaoRevenda : short
        {
            [Description("Revenda")]
            Revenda = 1,
            [Description("Não revenda")]
            NaoRevenda = 2
        }

        public enum EEntradaSaida : short
        {
            [Description("Entrada")]
            Entrada = 1,
            [Description("Saída")]
            Saida = 2
        }

        #region Campos personalizados

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public EGrupo Grupo { get; set; }
        public ERevendaNaoRevenda RevendaNaoRevenda { get; set; }
        public EEntradaSaida EntradaSaida { get; set; }
        public bool Financeiro { get; set; }
        public bool EmiteNF { get; set; }
        public bool Estoque { get; set; }
        public bool Contabiliza { get; set; }
        public bool PortalAdm { get; set; }
        public bool PortalLoja { get; set; }
        public bool PortalFornecedor { get; set; }
        public bool PortalLog { get; set; }
        public bool Logistica { get; set; }
        public bool PDV { get; set; }
        public bool Precifica { get; set; }
        public bool PedidoSaida { get; set; }

        #endregion

        #region Campos calculados

        public string Integracoes
        {
            get
            {
                var interacao = new List<string>();
                string result = "";

                if (Financeiro)
                {
                    interacao.Add("Financeiro");
                }
                if (EmiteNF)
                {
                    interacao.Add("emite NF");
                }
                if (Estoque)
                {
                    interacao.Add("estoque");
                }
                if (Contabiliza)
                {
                    interacao.Add("contabiliza");
                }

                if (interacao.Count() == 0)
                {
                    interacao.Add("Sem integrações");
                    result = String.Join("", interacao);
                    return result;
                }
                else
                {
                    result += String.Join(", ", interacao.OrderBy(o => o));
                    return (result.First().ToString().ToUpper() + result.Substring(1));
                }
            }
        }

        public string Portais
        {
            get
            {
                var portais = new List<string>();
                string result = "";
                if (PortalAdm)
                {
                    portais.Add("administrativo");
                }
                if (PortalLoja)
                {
                    portais.Add("loja");
                }
                if (PortalLog)
                {
                    portais.Add("logística");
                }
                if (PortalFornecedor)
                {
                    portais.Add("fornecedor");
                }

                if (portais.Count() == 0)
                {
                    portais.Add("Sem portais");
                    result = String.Join("", portais);
                    return result;
                }
                else
                {
                    result += String.Join(", ", portais.OrderBy(o => o));
                    return (result.First().ToString().ToUpper() + result.Substring(1));
                }
            }
        }

        #endregion

        #region Relacionamentos

        //public virtual ICollection<FormularioPedido> FormularioPedidos { get; private set; }
        //public virtual ICollection<TipoPedidoRegra> TipoPedidoRegras { get; private set; }
        //public virtual ICollection<TipoPedidoFonteSuprimento> TipoPedidoFonteSuprimentos { get; private set; }
        //public virtual ICollection<PedidoEntrada> PedidoEntradas { get; private set; }
        //public virtual ICollection<PedidoSaida> PedidoSaidas { get; private set; }

        #endregion
    }
}

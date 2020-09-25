using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Demo.Models.PostgreSql
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

        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public EGrupo Grupo { get; private set; }
        public ERevendaNaoRevenda RevendaNaoRevenda { get; private set; }
        public EEntradaSaida EntradaSaida { get; private set; }
        public bool Financeiro { get; private set; }
        public bool EmiteNF { get; private set; }
        public bool Estoque { get; private set; }
        public bool Contabiliza { get; private set; }
        public bool PortalAdm { get; private set; }
        public bool PortalLoja { get; private set; }
        public bool PortalFornecedor { get; private set; }
        public bool PortalLog { get; private set; }
        public bool Logistica { get; private set; }
        public bool PDV { get; private set; }
        public bool Precifica { get; private set; }
        public bool PedidoSaida { get; private set; }

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

        public TipoPedido() : base(Guid.Empty) { }

        public TipoPedido(Guid aspNetUserInsertId, string nome, string descricao, EGrupo grupo, ERevendaNaoRevenda revendaNaoRevenda, EEntradaSaida entradaSaida,
                          bool financeiro, bool emiteNF, bool estoque, bool contabiliza, bool portalAdm, bool portalLoja, bool portalFornecedor, bool portalLog, bool logistica, bool pdv,
                          bool precifica, bool pedidoSaida) : base(aspNetUserInsertId)
        {
            Nome = nome;
            Descricao = descricao;
            Grupo = grupo;
            RevendaNaoRevenda = revendaNaoRevenda;
            EntradaSaida = entradaSaida;
            Financeiro = financeiro;
            EmiteNF = emiteNF;
            Estoque = estoque;
            Contabiliza = contabiliza;
            PortalAdm = portalAdm;
            PortalLoja = portalLoja;
            PortalFornecedor = portalFornecedor;
            PortalLog = portalLog;
            Logistica = logistica;
            PDV = pdv;
            Precifica = precifica;
            PedidoSaida = pedidoSaida;
        }

        public void Update(string nome, string descricao,
          EGrupo grupo, ERevendaNaoRevenda revendaNaoRevenda, EEntradaSaida entradaSaida,
          bool financeiro, bool emiteNF, bool estoque, bool contabiliza,
          bool logistica, bool pdv,
          bool portalAdm, bool portalLoja, bool portalFornecedor, bool portalLog,
          bool precifica, bool pedidoSaida)
        {
            Nome = nome;
            Descricao = descricao;
            Grupo = grupo;
            RevendaNaoRevenda = revendaNaoRevenda;
            EntradaSaida = entradaSaida;
            Financeiro = financeiro;
            EmiteNF = emiteNF;
            Estoque = estoque;
            Contabiliza = contabiliza;
            PortalAdm = portalAdm;
            PortalLoja = portalLoja;
            PortalFornecedor = portalFornecedor;
            PortalLog = portalLog;
            Logistica = logistica;
            PDV = pdv;
            Precifica = precifica;
            PedidoSaida = pedidoSaida;
        }

        public void UpdatePoc(string nome, EGrupo grupo)
        {
            Nome = nome;
            Grupo = grupo;
        }
    }
}

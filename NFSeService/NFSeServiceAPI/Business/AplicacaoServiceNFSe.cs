using LayoutService.Entities;
using LayoutService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;
using static LayoutService.Entities.FrgNFSe;
using static LayoutService.Models.NFSeModel;

namespace NFSeServiceAPI.Business
{
    public class AplicacaoServiceNFSe
    {
        private XmlDocument _documento { get; }
        private readonly string _metodo;
        private readonly ComandoTransmitir _comandoTransmitir;
        private Empresa empresa;
        private X509Certificate2 _certificado;

        #region CONSTRUTOR

        public AplicacaoServiceNFSe(XmlDocument documento, string metodo, ComandoTransmitir comandoTransmitir)
        {
            _documento = documento;
            _metodo = metodo;

            _comandoTransmitir = new ComandoTransmitir
            {
                FnTerminal = comandoTransmitir.FnTerminal,
                TDFe = new TDFe
                {
                    Tide = ObterIde(comandoTransmitir),
                    TServico = comandoTransmitir.TDFe.TServico,
                    TCondicaoPagamento = comandoTransmitir.TDFe.TCondicaoPagamento,
                    TPrestador = comandoTransmitir.TDFe.TPrestador,
                    TTomador = comandoTransmitir.TDFe.TTomador,
                    _TCertificado = comandoTransmitir.TDFe._TCertificado,
                }
            };

        }

        #endregion

        #region MÉTODOS

        private Tide ObterIde(ComandoTransmitir comandoTransmitir)
        {
            var identificacaoRps = new TIdentificacaoRps(comandoTransmitir.TDFe.Tide.FIdentificacaoRps.FAmbiente)
            {
                FNumero = comandoTransmitir.TDFe.Tide.FIdentificacaoRps.FNumero,
                FSerie = comandoTransmitir.TDFe.Tide.FIdentificacaoRps.FSerie,
                FTipo = 1 //RPS
            };

            var ide = new Tide
            {
                FIdentificacaoRps = identificacaoRps,
                FNumeroLote = comandoTransmitir.TDFe.Tide.FNumeroLote,
                FnProtocolo = comandoTransmitir.TDFe.Tide.FnProtocolo,
                DataEmissao = comandoTransmitir.TDFe.Tide.DataEmissao,
                DataEmissaoRps = comandoTransmitir.TDFe.Tide.DataEmissaoRps,
                FNaturezaOperacao = comandoTransmitir.TDFe.Tide.FNaturezaOperacao,
                FRegimeEspecialTributacao = comandoTransmitir.TDFe.Tide.FRegimeEspecialTributacao,
                FOptanteSimplesNacional = comandoTransmitir.TDFe.Tide.FOptanteSimplesNacional,
                FIncentivadorCultural = comandoTransmitir.TDFe.Tide.FIncentivadorCultural,
                FStatus = comandoTransmitir.TDFe.Tide.FStatus,
                FOutrasInformacoes = comandoTransmitir.TDFe.Tide.FOutrasInformacoes,
                _FMsgGeradaPeloSistema = comandoTransmitir.TDFe.Tide._FMsgGeradaPeloSistema,
                _FMsgComplementares = comandoTransmitir.TDFe.Tide._FMsgComplementares
            };

            return ide;
        }

        public void CarregarDados()
        {
            empresa = new Empresa();
            empresa.CNPJ = _comandoTransmitir.TDFe.TPrestador.FCnpj;
            empresa.Nome = _comandoTransmitir.TDFe.TPrestador.FNomeFantasia;
            empresa.UnidadeFederativaCodigo = Convert.ToInt32(_comandoTransmitir.TDFe.TPrestador.TEndereco.FCodigoMunicipio);

            if (_comandoTransmitir.TDFe._TCertificado.Arquivo != null)
            {
                empresa.UsaCertificado = true;
                empresa.CertificadoSenha = _comandoTransmitir.TDFe._TCertificado.SenhaCert;

                _certificado = new X509Certificate2(_comandoTransmitir.TDFe._TCertificado.Arquivo, _comandoTransmitir.TDFe._TCertificado.SenhaCert);

                empresa.X509Certificado = _certificado;
            }
            else
            {
                empresa.UsaCertificado = false;
            }

            empresa.UsuarioWS = _comandoTransmitir.TDFe.TPrestador.FIdentificacaoPrestador._FUsuario;
            empresa.SenhaWS = _comandoTransmitir.TDFe.TPrestador.FIdentificacaoPrestador.FSenha;

            if (_comandoTransmitir.TDFe.Tide.FIdentificacaoRps.FAmbiente == EnumAmbiente.Producao)
            {
                empresa.AmbienteCodigo = 1;
            }
            else
            {
                empresa.AmbienteCodigo = 2;
            }
        }

        public XmlDocument ExecutaMetodo()
        {
            // get cnpj do xml
            XmlDocument resposta = null;
            //int emp = Empresas.FindConfEmpresaIndex(cnpj, Components.TipoAplicativo.Nfse); // 2 = nfse

            switch (_metodo.ToUpper())
            {
                case "CANCELARNFSE":
                    var servicoCancelar = new TaskNFSeCancelar(_documento, _certificado, empresa);
                    resposta = servicoCancelar.ExecuteAPI();
                    break;

                case "CONSULTARLOTERPS":
                    var servCONSULTARLOTERPS = new TaskNFSeConsultarLoteRps(_documento, _certificado, empresa);
                    resposta = servCONSULTARLOTERPS.ExecuteAPI();
                    break;
                case "CONSULTARNFSE":
                    var servicoConsultarNFSe = new TaskNFSeConsultar(_documento, _certificado, empresa);
                    resposta = servicoConsultarNFSe.ExecuteAPI();
                    break;

                case "CONSULTARNFSEPORRPS":
                    var servicoConsultarNFSePorRPS = new TaskNFSeConsultarPorRps(_documento, _certificado, empresa);
                    resposta = servicoConsultarNFSePorRPS.ExecuteAPI();
                    break;

                case "CONSULTARSTATUSNFSE":
                    var servicoConsultarStatusNfse = new TaskConsultarStatusNFse(_documento, _certificado, empresa);
                    resposta = servicoConsultarStatusNfse.ExecuteAPI();

                    break;

                case "CONSULTASITUACAOLOTERPS":
                    var servicoConsultarSituacaoLoteRPS = new TaskNFSeConsultaSituacaoLoteRps(_documento, _certificado, empresa);
                    resposta = servicoConsultarSituacaoLoteRPS.ExecuteAPI();

                    break;

                case "RECEPCIONARLOTERPS":
                    var servicoRecepLote = new TaskNFSeRecepcionarLoteRps(_documento, _certificado, empresa);

                    resposta = servicoRecepLote.ExecuteAPI();
                    break;

                case "CONSULTARSTLOTE":
                    var servicoConsultaStLote = new TaskNFSeConsultaSituacaoLoteRps(_documento, _certificado, empresa);
                    resposta = servicoConsultaStLote.ExecuteAPI();
                    break;

                default:
                    break;
            }

            return resposta;
        }

        #endregion

    }
}

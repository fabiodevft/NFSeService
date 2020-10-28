using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;


namespace NFSeServiceAPI.Business
{
    public class AplicacaoServiceNFSe
    {
        private XmlDocument _documento { get; }
        private readonly string _metodo;
        private readonly ComandoTransmitir _comandoTransmitir;
        private Empresa empresa;
        private WSSoap wsSoap;
        private X509Certificate2 _certificado;
        private EnumProvedor _enumProvedor;
        
        #region CONSTRUTOR

        public AplicacaoServiceNFSe(XmlDocument documento, string metodo, ComandoTransmitir comandoTransmitir, EnumProvedor enumProvedor)
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

            _enumProvedor = enumProvedor;

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

            //Popula o WSSoap
            wsSoap = new WSSoap
            {
                EnderecoWeb = _comandoTransmitir.FSoap.EnderecoWeb,
                ActionWeb = _comandoTransmitir.FSoap.ActionWeb,
                VersaoSoap = "", 
                ContentType = "",                
            };


        }

        public XmlDocument ExecutaMetodo()
        {            
            string resposta = string.Empty;
            WebService service = null;

            if (!string.IsNullOrWhiteSpace(wsSoap.EnderecoWeb))
            {
                switch (_enumProvedor)
                {
                    case EnumProvedor.ELv2:
                        service = new .ServiceElv2()




                                //wsSoap.EnderecoWeb, "",
                                //_enumProvedor, 
                                //empresa.UsuarioWS, 
                                //empresa.SenhaWS, 
                                //empresa.X509Certificado);
                        break;
                }   
            }            
            
            switch (_metodo.ToUpper())
            {                
                case "CANCELARNFSE":
                    resposta = service.CancelarNfse();
                    break;

                case "CONSULTARLOTERPS":
                    resposta = service.ConsultarLoteRps();
                    break;

                case "CONSULTARNFSE":
                    resposta = service.ConsultarNfse();
                    break;

                case "CONSULTARNFSEPORRPS":
                    resposta = service.ConsultarNfsePorRps();
                    break;

                case "CONSULTARSTATUSNFSE":
                    resposta = service.ConsultarStatusNfse();
                    break;

                case "CONSULTASITUACAOLOTERPS":
                    resposta = service.ConsultarSituacaoLoteRps();
                    break;

                case "RECEPCIONARLOTERPS":
                    resposta = service.RecepcionarLoteRps();
                    break;

                case "CONSULTARSTLOTE":
                    resposta = service.ConsultarStLote();
                    break;

                default:
                    break;
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(resposta);

            return doc;
        }

        #endregion

    }
}

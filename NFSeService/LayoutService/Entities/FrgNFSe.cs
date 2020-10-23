using LayoutService.Enums;
using LayoutService.Interfaces;
using LayoutService.Provedores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using static LayoutService.Models.NFSeModel;

namespace LayoutService.Entities
{
    [Serializable, Browsable(false)]
    public class FrgNFSe : Notas
    {
        #region Variaveis

        private IProvedor _provedor;
        private ComandoTransmitir _documento;

        #endregion

        #region Properties

        public IProvedor Provedor
        {
            get { return this._provedor; }
        }

        public ComandoTransmitir Documento
        {
            get { return this._documento; }
            set { this._documento = value; }
        }

        #endregion Properties

        #region Construtor
        public FrgNFSe(EnumProvedor provedor)
        {
            _provedor = new Provedor(provedor);
        }

        #endregion Construtor

        #region Comando

        public class ComandoTransmitir
        {
            public string FnTerminal { get; set; }
            public int FCodigoCancelamento { get; set; }
            public TDFe TDFe { get; set; }
            public string FNumeroNF { get; set; }
            public DateTime FDataEmissao { get; set; }
            public string FCodigoVerificacao { get; set; }
        }

        public class ComandoAcessar
        {
            public ComandoAcessar() { }

            public ComandoAcessar(string terminal, string chaveAcesso, string xml)
            {
                FnTerminal = terminal;
                FChaveAcesso = chaveAcesso;
                FXml = xml;
            }

            public string FnTerminal { get; set; }
            public string FChaveAcesso { get; set; }
            public string FXml { get; set; }
            //public TServico TServico { get; set; }
            public List<TItemServico> TItemServico { get; set; }
            public int FNaturezaOperacao { get; set; }
            public int FRegimeEspecialTributacao { get; set; }
            public string FOutrasInformacoes { get; set; }
            public TPrestador TPrestador { get; set; }
            public TTomador TTomador { get; set; }
        }

        public class ComandoCancelar
        {
            public string FnTerminal { get; set; }
            public string FChaveAcesso { get; set; }
            public int FCodigoCancelamento { get; set; }
            public string FXml { get; set; }
        }

        public class ComandoConsultarNFSeporRps
        {
            public string FnTerminal { get; set; }
            public TDFe TDFe { get; set; }
        }

        public class ComandoConfigurar
        {
            public string FnTerminal { get; set; }
            public string FNumCertificado { get; set; }
            public EnumAmbiente FAmbiente { get; set; }

            public string FPrefNome { get; set; }
            public string FPrefEndereco { get; set; }
            public string FPrefEndNumero { get; set; }
            public string FPrefBairro { get; set; }
            public string FPrefCep { get; set; }
            public string FPrefCidade { get; set; }
            public string FPrefUF { get; set; }
            public string FPrefFone { get; set; }
            public string FPrefEmail { get; set; }
            public TArquivo FPrefLogo { get; set; }
            public string FPrefShWebServices { get; set; }
            public string FPrefUsWebServices { get; set; }

            public string FEmitIBGEMunicipio { get; set; }
            public string FEmitRazaoSocial { get; set; }
            public string FEmitCNPJ { get; set; }
            public string FEmitIE { get; set; }
            public string FEmitIM { get; set; }
            public int FEmitIBGEUF { get; set; }
            public TArquivo FLogo { get; set; }
        }

        #endregion Comando

        public XmlDocument GeraXmlNota()
        {
            if (this.Provedor == null)
                throw new Exception("Provedor não cadastrado.");

            return this.Provedor.GeraXmlNota(this);
        }


        public XmlDocument GerarXmlConsulta(string numeroNFSe)
        {
            if (this.Provedor == null)
                throw new Exception("Provedor não cadastrado.");

            return this.Provedor.GerarXmlConsulta(this, numeroNFSe);
        }

        public XmlDocument GerarXmlConsulta(string numeroNFSe, DateTime emissao)
        {
            if (this.Provedor == null)
                throw new Exception("Provedor não cadastrado.");

            return this.Provedor.GerarXmlConsulta(this, numeroNFSe, emissao);
        }

        public XmlDocument GerarXmlConsulta(long numeroLote)
        {
            if (this.Provedor == null)
                throw new Exception("Provedor não cadastrado.");

            return this.Provedor.GerarXmlConsulta(this, numeroLote);
        }
                
        public RetornoTransmitir LerRetorno(XmlDocument xmlDocument, EnumOperacao op)
        {
            return LerRetorno(xmlDocument, op, "");
        }
        public XmlDocument GerarXmlConsultaNotaValida(string numeroNFSe, string hash)
        {
            if (this.Provedor == null)
                throw new Exception("Provedor não cadastrado.");

            return this.Provedor.GerarXmlConsultaNotaValida(this, numeroNFSe, hash);
        }

        public XmlDocument GerarXmlCancelaNota(string numeroNFSe, string motivo)
        {
            if (this.Provedor == null)
                throw new Exception("Provedor não cadastrado.");

            return this.Provedor.GerarXmlCancelaNota(this, numeroNFSe, motivo);
        }

        public XmlDocument GerarXmlCancelaNota(string numeroNFSe)
        {
            if (this.Provedor == null)
                throw new Exception("Provedor não cadastrado.");

            return this.Provedor.GerarXmlCancelaNota(this, numeroNFSe);
        }

        public XmlDocument GerarXmlCancelaNota(string numeroNFSe, string motivo, long numeroLote, string codigoVerificacao)
        {
            if (this.Provedor == null)
                throw new Exception("Provedor não cadastrado.");

            return this.Provedor.GerarXmlCancelaNota(this, numeroNFSe, motivo, numeroLote, codigoVerificacao);
        }
        public string TrataMensagemRetornoErrorUniNfe(string texto)
        {
            string textoRet = string.Empty;
            int pInicio;
            int pFim;

            pInicio = texto.IndexOf("Message|") + 8;

            if (pInicio == -1)
            {
                return texto == null ? "" : texto;
            }

            pFim = texto.IndexOf("StackTrace|") != -1 ? texto.IndexOf("StackTrace|") :
                    texto.IndexOf("Source|") != -1 ? texto.IndexOf("Source|") :
                    texto.IndexOf("Type|") != -1 ? texto.IndexOf("Type|") :
                    texto.IndexOf("TargetSite|") != -1 ? texto.IndexOf("TargetSite|") : texto.IndexOf("HashCode|");

            if (pFim == -1)
            {
                return texto == null ? "" : texto;
            }

            if (texto.Contains("The operation has timed out"))
            {
                textoRet = "O Webservice do município não retornou nenhuma resposta. Tente transmitir novamente mais tarde.";
            }
            else if (texto.Contains("The request failed with HTTP status 404: Not Found."))
            {
                textoRet = "O Webservice do município não está disponível. Entre em contato com a prefeitura ou tente transmitir novamente mais tarde.";
            }
            else if (texto.Contains("The request failed with HTTP status 503: Service Temporarily Unavailable."))
            {
                textoRet = "O Serviço de integração de notas do município está temporariamente indisponível. Entre em contato com a prefeitura ou tente transmitir novamente mais tarde.";
            }
            else
            {
                textoRet = texto.Substring(pInicio, (pFim - pInicio));
            }

            return textoRet;
        }

        public RetornoTransmitir LerRetorno(XmlDocument xmlResposta, EnumOperacao op, string numeroNFSe)
        {
            try
            {
                return this.Provedor.LerRetorno(this, xmlResposta);
            }
            catch (Exception)
            {
                throw new Exception("Provedor não cadastrado.");
            }
        }


    }
}

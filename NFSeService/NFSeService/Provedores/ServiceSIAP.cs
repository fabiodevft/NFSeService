using LayoutService.Entities;
using LayoutService.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace NFSeService.Entities.Provedores
{
    public class ServiceSIAP : WebService
    {        
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public X509Certificate Certificado { get; set; }
        public XmlDocument ArquivoXml { get; set; }
        public string NameSpaces
        {
            get
            {
                return "http://nfse.abrasf.org.br/";
            }
        }
        public string CabecMsg
        {
            get
            {
                return "<san:Cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><Versao>1.0</Versao><versaoDados>2.03</versaoDados></san:Cabecalho>";
            }
        }

        public string SoapAction { get; set; }


        public ServiceSIAP(WSSoap wsSoap, EnumProvedor provedor, string usuario, string senha, X509Certificate certificado, Empresa empresa)
           : base(wsSoap, provedor)
        {
            Usuario = usuario;
            Senha = senha;
            Certificado = certificado;

            switch (empresa.UnidadeFederativaCodigo)
            {
                case 1506807:
                    WSSoap.ActionSoap = "SantaremRPSaction/{metodo}.Execute";
                    break;
            }
        }

        /// <summary>
        /// Envelopa o XML (Soap)
        /// </summary>
        /// <param name="xml">XML a ser envelopado</param>
        /// <returns>Retorna o xml envelopado</returns>
        private string Envelopar(XmlDocument xml, string strCabecMsg, string metodo)
        {
            StringBuilder env = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

            env.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:san=\"SantaremRPS\">");
            env.Append("<soapenv:Header/>");
            env.Append("<soapenv:Body>");
            env.Append($"<san:{metodo}.Execute>");
            
            env.Append(strCabecMsg);
            env.Append(xml.OuterXml.ToString());
            
            env.Append($"</san:{metodo}.Execute>");
            env.Append("</soapenv:Body>");
            env.Append("</soapenv:Envelope>");

            return env.ToString();
        }
        public XmlDocument Assinar()
        {
            throw new NotImplementedException();
        }
        public Validacao Validar()
        {
            throw new NotImplementedException();
        }
        public override string RecepcionarLoteRps()
        {
            string retornar = string.Empty;

            string XMLRetorno = RequestWS(Envelopar(ArquivoXml, CabecMsg, "RecepcionarLoteRpsSincrono"), WSSoap.EnderecoWeb, 
                    WSSoap.ActionSoap.Replace("{metodo}", "ARECEPCIONARLOTERPSSINCRONO"));

            MemoryStream stream = Generico.StringXmlToStream(XMLRetorno);
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            XmlNodeList xmlList = doc.GetElementsByTagName("outputXML");
            retornar = Generico.TrataCaracteres(xmlList[0].OuterXml).Replace("<outputXML>", "").Replace("</outputXML>", "");

            return retornar.ToString();
        }
        
        public override string ConsultarLoteRps()
        {
            string retornar = string.Empty;
            
            string XMLRetorno = RequestWS(Envelopar(ArquivoXml, CabecMsg, "ConsultarLoteRps"), WSSoap.EnderecoWeb,
                    WSSoap.ActionSoap.Replace("{metodo}", "ACONSULTARLOTERPS"));

            MemoryStream stream = Generico.StringXmlToStream(XMLRetorno);
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            XmlNodeList xmlList = doc.GetElementsByTagName("outputXML");
            retornar = Generico.TrataCaracteres(xmlList[0].OuterXml).Replace("<outputXML>", "").Replace("</outputXML>", "");

            return retornar;
        }
       
    }
}

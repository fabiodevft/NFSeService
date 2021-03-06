﻿using LayoutService.Enums;
using NFSeService.Entities;
using NFSeService.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace NFSeService.Provedores
{
    public class ServiceElv2 : WebService, IServicoProvedor
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
                return "<cabecalho versao=\"2.04\" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"><versaoDados>2.04</versaoDados></cabecalho>";
            }
        }
        public ServiceElv2(WSSoap wsSoap, EnumProvedor provedor, string usuario, string senha, X509Certificate certificado)
            : base (wsSoap, provedor)
        {
            Usuario = usuario;
            Senha = senha;
            Certificado = certificado;
        }
        public string Envelopar(XmlDocument xml, string strCabecMsg, string metodo)
        {
            StringBuilder env = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

            env.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:nfse=\"http://nfse.abrasf.org.br\">");
            env.Append("<soapenv:Header/>");
            env.Append("<soapenv:Body>");
            env.Append($"<nfse:{metodo}>");
            env.Append($"<nfse:{metodo}Request>");
            env.Append("<nfseCabecMsg><![CDATA[" + strCabecMsg + "]]></nfseCabecMsg>");
            env.Append("<nfseDadosMsg><![CDATA[" + xml.OuterXml.ToString() + "]]></nfseDadosMsg>");
            env.Append($"</nfse:{metodo}Request>");
            env.Append($"</nfse:{metodo}>");
            env.Append("</soapenv:Body>");
            env.Append("</soapenv:Envelope>");

            return env.ToString();
        }
        public XmlDocument Assinar(XmlDocument document)
        {
            throw new NotImplementedException();
        }
        public Validacao Validar(XmlDocument document)
        {
            throw new NotImplementedException();
        }
        public override string RecepcionarLoteRps()
        {
            string retornar = string.Empty;

            string XMLRetorno = RequestWS(Envelopar(ArquivoXml, CabecMsg, "RecepcionarLoteRps"), WSSoap.EnderecoWeb, NameSpaces + "RecepcionarLoteRps");

            MemoryStream stream = Generico.StringXmlToStream(XMLRetorno);
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            XmlNodeList xmlList = doc.GetElementsByTagName("outputXML");
            retornar = Generico.TrataCaracteres(xmlList[0].OuterXml).Replace("<outputXML>", "").Replace("</outputXML>", "");

            return retornar.ToString();
        }
        public override string ConsultarNfsePorRps()
        {
            string retornar = string.Empty;

            string XMLRetorno = RequestWS(Envelopar(ArquivoXml, CabecMsg, "ConsultarNfsePorRps"), WSSoap.EnderecoWeb, this.NameSpaces + "ConsultarNfsePorRps");

            MemoryStream stream = Generico.StringXmlToStream(XMLRetorno);
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            XmlNodeList xmlList = doc.GetElementsByTagName("outputXML");
            retornar = Generico.TrataCaracteres(xmlList[0].OuterXml).Replace("<outputXML>", "").Replace("</outputXML>", "");

            return retornar;
        }
        public override string ConsultarLoteRps()
        {
            string retornar = string.Empty;

            string XMLRetorno = RequestWS(Envelopar(ArquivoXml, CabecMsg, "ConsultarLoteRps"), WSSoap.EnderecoWeb, this.NameSpaces + "ConsultarLoteRps");

            MemoryStream stream = Generico.StringXmlToStream(XMLRetorno);
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            XmlNodeList xmlList = doc.GetElementsByTagName("outputXML");
            retornar = Generico.TrataCaracteres(xmlList[0].OuterXml).Replace("<outputXML>", "").Replace("</outputXML>", "");

            return retornar;
        }
        public override string CancelarNfse()
        {
            string retornar = string.Empty;

            string XMLRetorno = RequestWS(Envelopar(ArquivoXml, CabecMsg, "CancelarNfse"), WSSoap.EnderecoWeb, this.NameSpaces + "CancelarNfse");

            MemoryStream stream = Generico.StringXmlToStream(XMLRetorno);
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            XmlNodeList xmlList = doc.GetElementsByTagName("outputXML");
            retornar = Generico.TrataCaracteres(xmlList[0].OuterXml).Replace("<outputXML>", "").Replace("</outputXML>", "");

            return retornar;
        }

    }
}

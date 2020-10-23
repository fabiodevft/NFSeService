using LayoutService.Entities;
using LayoutService.Enums;
using System;
using System.Xml;
using static LayoutService.Entities.Notas;

namespace LayoutService.Interfaces
{
    public interface IProvedor
    {
        EnumProvedor Nome { get; set; }
        XmlDocument GeraXmlNota(FrgNFSe nota);
        XmlDocument GerarXmlConsulta(FrgNFSe nota, string numeroNFSe);
        XmlDocument GerarXmlConsulta(FrgNFSe nota, string numeroNFSe, DateTime emissao);
        XmlDocument GerarXmlConsulta(FrgNFSe nota, long numeroLote);
        RetornoTransmitir LerRetorno(FrgNFSe nota, string arquivo);
        RetornoTransmitir LerRetorno(FrgNFSe nota, string arquivo, string numNF);
        RetornoTransmitir LerRetorno(FrgNFSe nota, XmlDocument xmlResposta);
        XmlDocument GerarXmlConsultaNotaValida(FrgNFSe nota, string numeroNFSe, string hash);
        XmlDocument GerarXmlCancelaNota(FrgNFSe nota, string numeroNFSe);
        XmlDocument GerarXmlCancelaNota(FrgNFSe nota, string numeroNFSe, string motivo);
        XmlDocument GerarXmlCancelaNota(FrgNFSe nota, string numeroNFSe, string motivo, long numeroLote, string codigoVerificacao);
        RetornoTransmitir ValidarNFSe(FrgNFSe nota);
        XmlDocument GerarXmlURLImpressao(FrgNFSe nota, string numeroNFSe);
    }
}

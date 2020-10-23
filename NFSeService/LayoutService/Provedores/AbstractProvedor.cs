using LayoutService.Entities;
using LayoutService.Enums;
using System;
using System.Xml;

namespace LayoutService.Provedores
{
    public class AbstractProvedor : Notas
    {
        #region Variaveis

        const string classeNaoImplementar = "Função não implementada na classe filha. Implemente na classe que está sendo criada.";
        private EnumProvedor _nome = EnumProvedor.NaoDefinido;

        #endregion Variaveis

        #region Propriedades

        public virtual EnumProvedor Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        #endregion Propriedades

        #region Métodos

        public virtual XmlDocument GeraXmlNota(FrgNFSe nota)
        {
            throw new NotImplementedException(classeNaoImplementar);
        }

        public virtual XmlDocument GerarXmlConsulta(FrgNFSe nota, string numeroNFSe)
        {
            throw new NotImplementedException(classeNaoImplementar);
        }

        public virtual XmlDocument GerarXmlConsulta(FrgNFSe nota, string numeroNFSe, DateTime emissao)
        {
            throw new NotImplementedException(classeNaoImplementar);
        }

        public virtual XmlDocument GerarXmlConsulta(FrgNFSe nota, long numeroLote)
        {
            throw new NotImplementedException(classeNaoImplementar);
        }

        public virtual RetornoTransmitir LerRetorno(FrgNFSe nota, string arquivo, string numNF)
        {
            throw new NotImplementedException(classeNaoImplementar);
        }

        public virtual RetornoTransmitir LerRetorno(FrgNFSe nota, string arquivo)
        {
            throw new NotImplementedException(classeNaoImplementar);
        }
        public virtual RetornoTransmitir LerRetorno(FrgNFSe nota, XmlDocument xmlResposta)
        {
            throw new NotImplementedException(classeNaoImplementar);
        }

        public virtual XmlDocument GerarXmlConsultaNotaValida(FrgNFSe nota, string numeroNFSe, string hash)
        {
            throw new NotImplementedException(classeNaoImplementar);
        }

        public virtual XmlDocument GerarXmlCancelaNota(FrgNFSe nota, string numeroNFSe)
        {
            throw new NotImplementedException(classeNaoImplementar);
        }

        public virtual XmlDocument GerarXmlCancelaNota(FrgNFSe nota, string numeroNFSe, string motivo)
        {
            throw new NotImplementedException(classeNaoImplementar);
        }

        public virtual XmlDocument GerarXmlCancelaNota(FrgNFSe nota, string numeroNFSe, string motivo, long numeroLote, string codigoVerificacao)
        {
            throw new NotImplementedException(classeNaoImplementar);
        }

        public virtual RetornoTransmitir ValidarNFSe(FrgNFSe nota)
        {
            throw new NotImplementedException(classeNaoImplementar);
        }

        public virtual XmlDocument GerarXmlURLImpressao(FrgNFSe nota, string numeroNFSe)
        {
            throw new NotImplementedException(classeNaoImplementar);
        }

        # endregion
    }
}

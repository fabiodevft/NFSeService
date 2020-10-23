using LayoutService.Entities;
using LayoutService.Enums;
using LayoutService.Interfaces;
using System;
using System.Xml;

namespace LayoutService.Provedores
{
    public class Provedor : AbstractProvedor, IProvedor
    {
        private IProvedor _IProvedor;

        #region Construtores

        internal Provedor()
        {
        }

        public Provedor(EnumProvedor nomeProvedor)
        {
            try
            {
                InstanciaProvedor(nomeProvedor);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        #endregion

        #region Propriedades da Interface

        public override EnumProvedor Nome
        {
            get { return _IProvedor.Nome; }
            set { _IProvedor.Nome = value; }
        }

        #endregion

        #region Métodos Privados

        private void InstanciaProvedor(EnumProvedor nomeProvedor)
        {
            try
            {
                switch (nomeProvedor)
                {
                    case EnumProvedor.SigCorpSigISS:
                        //_IProvedor = new Provedor_SigCorpSigISS();
                        break;
                    

                    default:
                        throw new Exception("Provedor não implementando: " + nomeProvedor.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execução da transação.", ex);
            }
        }

        #endregion

        #region Métodos de Interface

        public override XmlDocument GeraXmlNota(FrgNFSe nota)
        {
            return _IProvedor.GeraXmlNota(nota);
        }

        public override XmlDocument GerarXmlConsulta(FrgNFSe nota, string numeroNFSe)
        {
            return _IProvedor.GerarXmlConsulta(nota, numeroNFSe);
        }

        public override XmlDocument GerarXmlConsulta(FrgNFSe nota, string numeroNFSe, DateTime emissao)
        {
            return _IProvedor.GerarXmlConsulta(nota, numeroNFSe, emissao);
        }

        public override XmlDocument GerarXmlConsulta(FrgNFSe nota, long numeroLote)
        {
            return _IProvedor.GerarXmlConsulta(nota, numeroLote);
        }

        public override RetornoTransmitir LerRetorno(FrgNFSe nota, string arquivo)
        {
            return _IProvedor.LerRetorno(nota, arquivo);
        }

        public override RetornoTransmitir LerRetorno(FrgNFSe nota, string arquivo, string numNF)
        {
            return _IProvedor.LerRetorno(nota, arquivo, numNF);
        }
        public RetornoTransmitir LerRetorno(FrgNFSe nota, XmlDocument xmlResposta)
        {
            return _IProvedor.LerRetorno(nota, xmlResposta);
        }

        public override XmlDocument GerarXmlConsultaNotaValida(FrgNFSe nota, string numeroNFSe, string hash)
        {
            return _IProvedor.GerarXmlConsultaNotaValida(nota, numeroNFSe, hash);
        }

        public override XmlDocument GerarXmlCancelaNota(FrgNFSe nota, string numeroNFSe, string motivo)
        {
            return _IProvedor.GerarXmlCancelaNota(nota, numeroNFSe, motivo);
        }

        public override XmlDocument GerarXmlCancelaNota(FrgNFSe nota, string numeroNFSe)
        {
            return _IProvedor.GerarXmlCancelaNota(nota, numeroNFSe);
        }

        public override XmlDocument GerarXmlCancelaNota(FrgNFSe nota, string numeroNFSe, string motivo, long numeroLote, string codigoVerificacao)
        {
            return _IProvedor.GerarXmlCancelaNota(nota, numeroNFSe, motivo, numeroLote, codigoVerificacao);
        }

        public override RetornoTransmitir ValidarNFSe(FrgNFSe nota)
        {
            return _IProvedor.ValidarNFSe(nota);
        }

        public override XmlDocument GerarXmlURLImpressao(FrgNFSe nota, string numeroNFSe)
        {
            return _IProvedor.GerarXmlURLImpressao(nota, numeroNFSe);
        }

        #endregion

    }
}

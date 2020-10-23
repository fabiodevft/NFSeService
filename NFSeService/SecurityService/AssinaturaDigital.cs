using LayoutService.Entities;
using LayoutService.Enums;
using LayoutService.Models;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace SecurityService
{
    public class AssinaturaDigital
    {
        public bool AssinaturaValida { get; private set; }
        public bool TesteCertificado { get; set; }
        public XmlDocument ArquivoXml { get; set; }

        #region Métodos Publicos

        public void Assinar(ref XmlDocument conteudoXml, Empresa emp, InfoSchema infoSchema, EnumAlgorithmType algorithmType = EnumAlgorithmType.Sha1, bool comURI = true)
        {
            if (emp.UsaCertificado)
            {
                if (!string.IsNullOrEmpty(infoSchema.TagAssinatura0))
                {
                    if (!Assinado(conteudoXml, infoSchema.TagAssinatura0))
                        Assinar(ref conteudoXml, infoSchema.TagAssinatura0, infoSchema.TagAtributoId0, emp.X509Certificado, emp, algorithmType, "", comURI, "");
                }

                if (!string.IsNullOrEmpty(infoSchema.TagAssinatura))
                {
                    if (!Assinado(conteudoXml, infoSchema.TagAssinatura))
                        Assinar(ref conteudoXml, infoSchema.TagAssinatura, infoSchema.TagAtributoId, emp.X509Certificado, emp, algorithmType, "", comURI, "");
                }

                //Assinar o lote
                if (!string.IsNullOrEmpty(infoSchema.TagLoteAssinatura))
                { 
                    if (!Assinado(conteudoXml, infoSchema.TagLoteAssinatura))
                        Assinar(ref conteudoXml, infoSchema.TagLoteAssinatura, infoSchema.TagLoteAtributoId, emp.X509Certificado, emp, algorithmType, "", comURI, "");
                }
            }
        }

        #endregion

        #region Métodos Privados

        private void Assinar(ref XmlDocument conteudoXML, string tagAssinatura, string tagAtributoId, X509Certificate2 x509Cert, 
            Empresa empresa, EnumAlgorithmType algorithmType = EnumAlgorithmType.Sha1, string idAttributeName = "", bool definirURI = true, 
            string pinCertificado = "")
        {
            try
            {
                //if (x509Cert == null)
                //    throw new ExceptionCertificadoDigital(ErroPadrao.CertificadoNaoEncontrado);

                if (conteudoXML.GetElementsByTagName(tagAssinatura).Count == 0)
                {
                    throw new Exception("A tag de assinatura " + tagAssinatura.Trim() + " não existe no XML. (Código do Erro: 5)");
                }
                else if (conteudoXML.GetElementsByTagName(tagAtributoId).Count == 0)
                {
                    throw new Exception("A tag de assinatura " + tagAtributoId.Trim() + " não existe no XML. (Código do Erro: 4)");
                }
                else
                {
                    XmlNodeList lists = conteudoXML.GetElementsByTagName(tagAssinatura);
                    XmlNode listRPS = null;

                    /// Esta condição foi feita especificamente para prefeitura de Governador Valadares pois o AtribudoID e o Elemento assinado devem possuir o mesmo nome.
                    /// Talvez tenha que ser reavaliado.

                    #region Governador Valadares

                    if (tagAssinatura.Equals(tagAtributoId) && empresa.UnidadeFederativaCodigo == 3127701)
                    {
                        foreach (XmlNode item in lists)
                        {
                            if (listRPS == null)
                            {
                                listRPS = item;
                            }

                            if (item.Name.Equals(tagAssinatura))
                            {
                                lists = item.ChildNodes;
                                break;
                            }
                        }
                    }
                    #endregion Governador Valadares

                    foreach (XmlNode nodes in lists)
                    {
                        foreach (XmlNode childNodes in nodes.ChildNodes)
                        {
                            if (!childNodes.Name.Equals(tagAtributoId))
                                continue;

                            var reference = new Reference
                            {
                                Uri = ""
                            };

                            // pega o uri que deve ser assinada
                            var childElemen = (XmlElement)childNodes;

                            if (definirURI)
                            {
                                if (string.IsNullOrEmpty(idAttributeName))
                                {
                                    if (childElemen.GetAttributeNode("Id") != null)
                                    {
                                        idAttributeName = "Id";
                                    }
                                    else if (childElemen.GetAttributeNode("id") != null)
                                    {
                                        idAttributeName = "id";
                                    }
                                }
                                else
                                {
                                    reference.Uri = "#" + childElemen.GetAttributeNode(idAttributeName).Value;
                                }
                            }

                            var signedXml = new SignedXml(conteudoXML);

                            if (!string.IsNullOrWhiteSpace(pinCertificado))
                            {
                                //x509Cert.SetPinPrivateKey(pinCertificado);
                            }

                            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                            reference.AddTransform(new XmlDsigC14NTransform());

                            switch (algorithmType)
                            {
                                case EnumAlgorithmType.Sha256:
                                    signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
                                    signedXml.SigningKey = x509Cert.GetRSAPrivateKey();
                                    signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
                                    reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
                                    break;
                                default:
                                    signedXml.SigningKey = x509Cert.PrivateKey;
                                    signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
                                    reference.DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";
                                    break;
                            }

                            // Add an enveloped transformation to the reference.
                            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                            reference.AddTransform(new XmlDsigC14NTransform());

                            signedXml.AddReference(reference);

                            var keyInfo = new KeyInfo();
                            keyInfo.AddClause(new KeyInfoX509Data(x509Cert));
                            signedXml.KeyInfo = keyInfo;
                            signedXml.ComputeSignature();

                            var xmlDigitalSignature = signedXml.GetXml();

                            if (tagAssinatura.Equals(tagAtributoId) && empresa.UnidadeFederativaCodigo == 3127701)
                            {
                                ///Desenvolvido especificamente para prefeitura de governador valadares
                                listRPS.AppendChild(conteudoXML.ImportNode(xmlDigitalSignature, true));
                            }
                            else
                            {
                                // Gravar o elemento no documento XML
                                nodes.AppendChild(conteudoXML.ImportNode(xmlDigitalSignature, true));
                            }
                        }
                    }
                }
            }
            catch (CryptographicException ex)
            {
                AssinaturaValida = false;
                throw;
            }
        }

        #endregion

        #region Assinado()

        /// <summary>
        /// Verificar se o XML já tem assinatura
        /// </summary>
        /// <param name="conteudoXML">Conteúdo do XML</param>
        /// <param name="tagAssinatura">Tag de assinatura onde vamos pesquisar</param>
        /// <returns>true = Já está assinado</returns>
        private bool Assinado(XmlDocument conteudoXML, string tagAssinatura)
        {
            bool retorno = false;

            try
            {
                if (conteudoXML.GetElementsByTagName(tagAssinatura)[0].LastChild.Name == "Signature")
                    retorno = true;
            }
            catch { }

            return retorno;
        }

        #endregion Assinado()

    }
}

﻿using LayoutService.Entities;
using LayoutService.Enums;
using LayoutService.Interfaces;
using LayoutService.Models;
using LayoutService.Util;
using System;
using System.Collections.Generic;
using System.Xml;

namespace LayoutService.Provedores
{
    public class Provedor_ELv2 : AbstractProvedor, IProvedor
    {
        public Provedor_ELv2()
        {
            this.Nome = EnumProvedor.ELv2;
        }

        /// <summary>
        /// Cria o documento xml e retorna a TAG principal
        /// </summary>
        /// <param name="strNomeMetodo">Ex.: ConsultarNfseRpsEnvio</param>
        /// <param name="doc">Referencia do objeto que será o documento</param>
        /// <returns>retorna o node principal</returns>
        private XmlElement CriaHeaderXml(string strNomeMetodo, ref XmlDocument doc)
        {
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            var gerarNotaNode = doc.CreateElement(strNomeMetodo);
            var nsAttributeTipos = doc.CreateAttribute("xmlns", "http://www.w3.org/2000/xmlns/");
            nsAttributeTipos.Value = "http://www.abrasf.org.br/nfse.xsd";
            gerarNotaNode.Attributes.Append(nsAttributeTipos);

            doc.AppendChild(gerarNotaNode);
            return gerarNotaNode;
        }
        private XmlElement CriaHeaderXml(string prefixo, string strNomeMetodo, ref XmlDocument doc)
        {
            var gerarNotaNode = doc.CreateElement(prefixo, strNomeMetodo, "http://www.abrasf.org.br/nfse.xsd");

            doc.AppendChild(gerarNotaNode);
            return gerarNotaNode;
        }

        #region XML
        public override XmlDocument GeraXmlNota(FrgNFSe nota)
        {
            var doc = new XmlDocument();

            #region EnviarLoteRpsEnvio

            var nodeEnviarLoteRpsEnvio = CriaHeaderXml("EnviarLoteRpsEnvio", ref doc);

            #region LoteRps       

            var nodeLoteRps = Extensions.CriarNo(doc, nodeEnviarLoteRpsEnvio, "LoteRps", "", "Id", "L" + nota.Documento.TDFe.Tide.FNumeroLote);

            var vsAttribute = doc.CreateAttribute("versao");
            vsAttribute.Value = "2.04";
            nodeLoteRps.Attributes.Append(vsAttribute);

            Extensions.CriarNoNotNull(doc, nodeLoteRps, "NumeroLote", nota.Documento.TDFe.Tide.FNumeroLote.ToString());

            #region Prestador
            var nodePrestador = Extensions.CriarNo(doc, nodeLoteRps, "Prestador");

            var CPFCNPJNode = Extensions.CriarNo(doc, nodePrestador, "CpfCnpj");
            Extensions.CriarNoNotNull(doc, CPFCNPJNode, "Cnpj", nota.Documento.TDFe.TPrestador.FCnpj);

            Extensions.CriarNoNotNull(doc, nodePrestador, "InscricaoMunicipal", nota.Documento.TDFe.TPrestador.FInscricaoMunicipal);

            #endregion fim - Prestador

            Extensions.CriarNoNotNull(doc, nodeLoteRps, "QuantidadeRps", "1");

            #region ListaRps
            var nodeListarps = Extensions.CriarNo(doc, nodeLoteRps, "ListaRps");

            #region Rps
            var rpsNode = Extensions.CriarNo(doc, nodeListarps, "Rps");

            #region InfDeclaracaoPrestacaoServico
            var nodeInfDeclaracaoPrestacaoServico = Extensions.CriarNo(doc, rpsNode, "InfDeclaracaoPrestacaoServico");

            #region RPS
            var nodeRPS = Extensions.CriarNo(doc, nodeInfDeclaracaoPrestacaoServico, "Rps");

            vsAttribute = doc.CreateAttribute("Id");
            vsAttribute.Value = "rps" + nota.Documento.TDFe.Tide.FIdentificacaoRps.FNumero;
            nodeRPS.Attributes.Append(vsAttribute);

            #region "IdentificacaoRps"

            var nodeIdentificacaoRps = Extensions.CriarNo(doc, nodeRPS, "IdentificacaoRps");
            Extensions.CriarNoNotNull(doc, nodeIdentificacaoRps, "Numero", nota.Documento.TDFe.Tide.FIdentificacaoRps.FNumero);
            Extensions.CriarNoNotNull(doc, nodeIdentificacaoRps, "Serie", nota.Documento.TDFe.Tide.FIdentificacaoRps.FSerie);
            Extensions.CriarNoNotNull(doc, nodeIdentificacaoRps, "Tipo", nota.Documento.TDFe.Tide.FIdentificacaoRps.FTipo.ToString());

            #endregion

            Extensions.CriarNoNotNull(doc, nodeRPS, "DataEmissao", nota.Documento.TDFe.Tide.DataEmissaoRps.ToString("yyyy-MM-dd"));
            Extensions.CriarNoNotNull(doc, nodeRPS, "Status", ((int)nota.Documento.TDFe.Tide.FStatus).ToString());

            #endregion
            Extensions.CriarNoNotNull(doc, nodeInfDeclaracaoPrestacaoServico, "Competencia", nota.Documento.TDFe.Tide.DataEmissaoRps.ToString("yyyy-MM-dd"));

            #region Servico
            var nodeServico = Extensions.CriarNo(doc, nodeInfDeclaracaoPrestacaoServico, "Servico");

            #region Valores
            var nodeServicoValores = Extensions.CriarNo(doc, nodeServico, "Valores");

            Extensions.CriarNoNotNull(doc, nodeServicoValores, "ValorServicos", Generico.FormataValor(nota.Documento.TDFe.TServico.FValores.FValorServicos, 2));
            Extensions.CriarNoNotNull(doc, nodeServicoValores, "ValorDeducoes", Generico.FormataValor(nota.Documento.TDFe.TServico.FValores.FValorDeducoes, 2));
            Extensions.CriarNoNotNull(doc, nodeServicoValores, "ValorPis", Generico.FormataValor(nota.Documento.TDFe.TServico.FValores.FValorPis, 2));
            Extensions.CriarNoNotNull(doc, nodeServicoValores, "ValorCofins", Generico.FormataValor(nota.Documento.TDFe.TServico.FValores.FValorCofins, 2));
            Extensions.CriarNoNotNull(doc, nodeServicoValores, "ValorInss", Generico.FormataValor(nota.Documento.TDFe.TServico.FValores.FValorInss, 2));
            Extensions.CriarNoNotNull(doc, nodeServicoValores, "ValorIr", Generico.FormataValor(nota.Documento.TDFe.TServico.FValores.FValorIr, 2));
            Extensions.CriarNoNotNull(doc, nodeServicoValores, "ValorCsll", Generico.FormataValor(nota.Documento.TDFe.TServico.FValores.FValorCsll, 2));
            Extensions.CriarNoNotNull(doc, nodeServicoValores, "OutrasRetencoes", Generico.FormataValor(nota.Documento.TDFe.TServico.FValores.FvalorOutrasRetencoes, 2));
            Extensions.CriarNoNotNull(doc, nodeServicoValores, "ValorIss", Generico.FormataValor(nota.Documento.TDFe.TServico.FValores.FValorIss, 2));
            Extensions.CriarNoNotNull(doc, nodeServicoValores, "Aliquota", nota.Documento.TDFe.TServico.FValores.FAliquota > 0 ? Generico.FormataValor(nota.Documento.TDFe.TServico.FValores.FAliquota, 2) : "0.00");
            Extensions.CriarNoNotNull(doc, nodeServicoValores, "DescontoIncondicionado", Generico.FormataValor(nota.Documento.TDFe.TServico.FValores.FDescontoIncondicionado, 2));
            Extensions.CriarNoNotNull(doc, nodeServicoValores, "DescontoCondicionado", Generico.FormataValor(nota.Documento.TDFe.TServico.FValores.FDescontoCondicionado, 2));

            #endregion FIM - Valores

            Extensions.CriarNoNotNull(doc, nodeServico, "IssRetido", Generico.ImpostoRetido((EnumNFSeSituacaoTributaria)nota.Documento.TDFe.TServico.FValores.FIssRetido, 1));

            if (Generico.ImpostoRetido((EnumNFSeSituacaoTributaria)nota.Documento.TDFe.TServico.FValores.FIssRetido, 1) == "1")
            {
                Extensions.CriarNoNotNull(doc, nodeServico, "ResponsavelRetencao", nota.Documento.TDFe.TServico.FResponsavelRetencao.ToString());
            }

            Extensions.CriarNoNotNull(doc, nodeServico, "ItemListaServico", nota.Documento.TDFe.TServico.FItemListaServico);
            Extensions.CriarNoNotNull(doc, nodeServico, "CodigoCnae", Generico.RetornarApenasNumeros(nota.Documento.TDFe.TServico.FCodigoCnae));
            Extensions.CriarNoNotNull(doc, nodeServico, "CodigoTributacaoMunicipio", Generico.RetornaApenasLetrasNumeros(nota.Documento.TDFe.TServico.FCodigoTributacaoMunicipio));
            Extensions.CriarNoNotNull(doc, nodeServico, "Discriminacao", Generico.TratarString(nota.Documento.TDFe.TServico.FDiscriminacao));
            Extensions.CriarNoNotNull(doc, nodeServico, "CodigoMunicipio", nota.Documento.TDFe.TServico.FMunicipioIncidencia);

            Extensions.CriarNoNotNull(doc, nodeServico, "ExigibilidadeISS", nota.Documento.TDFe.TServico.FExigibilidadeISS.ToString());
            Extensions.CriarNoNotNull(doc, nodeServico, "MunicipioIncidencia", nota.Documento.TDFe.TServico.FMunicipioIncidencia);

            #endregion FIM - Servico

            #region PrestadorNota

            var nodePrestadorNota = Extensions.CriarNo(doc, nodeInfDeclaracaoPrestacaoServico, "Prestador");

            var CPFCNPJPrestadorNode = Extensions.CriarNo(doc, nodePrestadorNota, "CpfCnpj");
            Extensions.CriarNoNotNull(doc, CPFCNPJPrestadorNode, "Cnpj", nota.Documento.TDFe.TPrestador.FCnpj);
            Extensions.CriarNoNotNull(doc, nodePrestadorNota, "InscricaoMunicipal", nota.Documento.TDFe.TPrestador.FInscricaoMunicipal);

            #endregion FIM - Prestador

            #region TomadorServico
            var nodeTomadorServico = Extensions.CriarNo(doc, nodeInfDeclaracaoPrestacaoServico, "TomadorServico");

            #region IdentificacaoTomador

            var nodeIdentificacaoTomador = Extensions.CriarNo(doc, nodeTomadorServico, "IdentificacaoTomador");
            var CPFCNPJTomador = Extensions.CriarNo(doc, nodeIdentificacaoTomador, "CpfCnpj");
            if (nota.Documento.TDFe.TTomador.TIdentificacaoTomador.FPessoa == "F")
            {
                Extensions.CriarNoNotNull(doc, CPFCNPJTomador, "Cpf", nota.Documento.TDFe.TTomador.TIdentificacaoTomador.FCpfCnpj);
            }
            else
            {
                Extensions.CriarNoNotNull(doc, CPFCNPJTomador, "Cnpj", nota.Documento.TDFe.TTomador.TIdentificacaoTomador.FCpfCnpj);
            }

            #endregion IdentificacaoTomador

            Extensions.CriarNoNotNull(doc, nodeTomadorServico, "RazaoSocial", Generico.TratarString(nota.Documento.TDFe.TTomador.FRazaoSocial));

            #region Endereco

            var nodeTomadorEndereco = Extensions.CriarNo(doc, nodeTomadorServico, "Endereco");

            Extensions.CriarNoNotNull(doc, nodeTomadorEndereco, "Endereco", Generico.TratarString(nota.Documento.TDFe.TTomador.TEndereco.FEndereco));
            Extensions.CriarNoNotNull(doc, nodeTomadorEndereco, "Numero", nota.Documento.TDFe.TTomador.TEndereco.FNumero);
            //Extensions.CriarNoNotNull(doc, nodeTomadorEndereco, "Complemento", Generico.TratarString(nota.Documento.TDFe.TTomador.TEndereco.FComplemento));
            Extensions.CriarNoNotNull(doc, nodeTomadorEndereco, "Bairro", Generico.TratarString(nota.Documento.TDFe.TTomador.TEndereco.FBairro));
            Extensions.CriarNoNotNull(doc, nodeTomadorEndereco, "CodigoMunicipio", nota.Documento.TDFe.TTomador.TEndereco.FCodigoMunicipio);
            Extensions.CriarNoNotNull(doc, nodeTomadorEndereco, "Uf", nota.Documento.TDFe.TTomador.TEndereco.FUF);
            Extensions.CriarNoNotNull(doc, nodeTomadorEndereco, "Cep", Generico.RetornarApenasNumeros(nota.Documento.TDFe.TTomador.TEndereco.FCEP));

            #endregion FIM - Endereco

            #region Contato

            var nodeTomadorContato = Extensions.CriarNo(doc, nodeTomadorServico, "Contato");
            Extensions.CriarNoNotNull(doc, nodeTomadorContato, "Telefone", Generico.RetornarApenasNumeros(nota.Documento.TDFe.TTomador.TContato.FDDD + nota.Documento.TDFe.TTomador.TContato.FFone));
            //Extensions.CriarNoNotNull(doc, nodeTomadorContato, "Email", nota.Documento.TDFe.TTomador.TContato.FEmail);

            #endregion FIM - Contato

            #endregion FIM - Tomador

            //Extensions.CriarNoNotNull(doc, nodeInfDeclaracaoPrestacaoServico, "RegimeEspecialTributacao", nota.Documento.TDFe.Tide.FRegimeEspecialTributacao.ToString());
            Extensions.CriarNoNotNull(doc, nodeInfDeclaracaoPrestacaoServico, "OptanteSimplesNacional", nota.Documento.TDFe.Tide.FOptanteSimplesNacional.ToString());
            Extensions.CriarNoNotNull(doc, nodeInfDeclaracaoPrestacaoServico, "IncentivoFiscal", nota.Documento.TDFe.Tide.FIncentivadorCultural.ToString());

            #endregion FIM - nodeInfDeclaracaoPrestacaoServico  

            #endregion FIM - Rps

            #endregion FIM - ListaRps

            #endregion LoteRps

            #endregion EnviarLoteRpsEnvio

            return doc;
        }
        public override XmlDocument GerarXmlConsulta(FrgNFSe nota, long numeroLote)
        {
            var doc = new XmlDocument();

            string[] prefixo = { "ns1", "http://www.abrasf.org.br/nfse.xsd" };

            var gerarNotaNode = CriaHeaderXml("ns1", "ConsultarLoteRpsEnvio", ref doc);

            var PrestadorNode = Extensions.CriarNo(doc, gerarNotaNode, "Prestador", "", prefixo);
            #region CpfCnpj
            var CPFCNPJnota = Extensions.CriarNo(doc, PrestadorNode, "CpfCnpj", "", prefixo);
            Extensions.CriarNoNotNull(doc, CPFCNPJnota, "Cnpj", nota.Documento.TDFe.TPrestador.FCnpj, prefixo);

            #endregion FIM - CpfCnpj

            Extensions.CriarNoNotNull(doc, PrestadorNode, "InscricaoMunicipal", nota.Documento.TDFe.TPrestador.FInscricaoMunicipal, prefixo);
            doc.AppendChild(gerarNotaNode);

            Extensions.CriarNoNotNull(doc, gerarNotaNode, "Protocolo", nota.Documento.TDFe.Tide.FnProtocolo?.ToString() ?? "", prefixo);

            return doc;
        }
        public override XmlDocument GerarXmlCancelaNota(FrgNFSe nota, string numeroNFSe, string motivo)
        {
            var doc = new XmlDocument();
            var gerarNotaNode = CriaHeaderXml("CancelarNfseEnvio", ref doc);

            var PedidoNode = Extensions.CriarNo(doc, gerarNotaNode, "Pedido", "");

            #region "InfPedidoCancelamento"
            var InfPedidoCancelamentoNode = Extensions.CriarNo(doc, PedidoNode, "InfPedidoCancelamento", "", "Id", "C_" + numeroNFSe);

            #region "tcIdentificacaoNfse"

            var IdentificacaoNfseNode = Extensions.CriarNo(doc, InfPedidoCancelamentoNode, "IdentificacaoNfse");

            Extensions.CriarNo(doc, IdentificacaoNfseNode, "Numero", numeroNFSe);

            var CPFCNPJNode = Extensions.CriarNo(doc, IdentificacaoNfseNode, "CpfCnpj");
            Extensions.CriarNoNotNull(doc, CPFCNPJNode, "Cnpj", nota.Documento.TDFe.TPrestador.FCnpj);

            long _FInscricaoMunicipal;

            Extensions.CriarNo(doc, IdentificacaoNfseNode, "InscricaoMunicipal", nota?.Documento?.TDFe?.TPrestador?.FInscricaoMunicipal?.ToString().Trim() ?? "");
            Extensions.CriarNo(doc, IdentificacaoNfseNode, "CodigoMunicipio", nota.Documento.TDFe.TPrestador.TEndereco.FCodigoMunicipio);

            #endregion "tcIdentificacaoNfse"

            var motivoAux = "2";
            switch (motivo.ToLower().Trim())
            {
                case "erro na emissão":
                    motivoAux = "1";
                    break;
                case "serviço não prestado":
                    motivoAux = "2";
                    break;
                case "duplicidade da nota":
                    motivoAux = "4";
                    break;
            }

            Extensions.CriarNo(doc, InfPedidoCancelamentoNode, "CodigoCancelamento", motivoAux); // tsCodigoCancelamentoNfse

            #endregion "InfPedidoCancelamento"

            return doc;
        }

        #endregion fim XML
        public override RetornoTransmitir LerRetorno(FrgNFSe nota, XmlDocument xmlResposta)
        {
            var sucesso = false;
            var cancelamento = false;
            var numeroNF = "";
            var numeroRPS = "";
            DateTime? dataEmissaoRPS = null;
            var situacaoRPS = "";
            var codigoVerificacao = "";
            var protocolo = "";
            long numeroLote = nota.Documento.TDFe.Tide.FNumeroLote;
            var descricaoProcesso = "";
            var descricaoErro = "";
            var area = EnumAreaLeitura.Nenhum;
            var codigoErroOuAlerta = "";
            var _EnumRespostaRPS = EnumRespostaRPS.Nenhum;
            var linkImpressaoAux = string.Empty;

            var numNF = nota.Documento.TDFe.Tide.FNumeroLote.ToString();

            using (XmlReader x = new XmlNodeReader(xmlResposta))
            {
                while (x.Read())
                {
                    if (x.NodeType == XmlNodeType.Element && area != EnumAreaLeitura.Erro)
                    {
                        switch (_EnumRespostaRPS)
                        {
                            case EnumRespostaRPS.Nenhum:
                                #region "EnumRespostaRPS"    
                                {
                                    switch (x.Name.ToString().ToLower())
                                    {                                       
                                        case "retorno": // Consultar RPS
                                            _EnumRespostaRPS = EnumRespostaRPS.ConsultarNfseRpsResposta; break;
                                    }
                                    break;

                                }
                            #endregion   "EnumRespostaRPS"
                            case EnumRespostaRPS.EnviarLoteRpsResposta:
                                {
                                    switch (x.Name.ToString().ToLower())
                                    {
                                        case "protocolo":
                                            protocolo = x.ReadString();
                                            sucesso = true;
                                            break;
                                        case "listamensagemretorno":
                                        case "mensagemretorno":
                                            area = EnumAreaLeitura.Erro;
                                            break;
                                    }
                                    break;
                                }
                            case EnumRespostaRPS.ConsultarNfseRpsResposta:
                                {
                                    switch (x.Name.ToString().ToLower())
                                    {
                                        case "codigo":
                                            if (x.ReadString().ToLower().Contains("sucesso"))
                                            {
                                                sucesso = true;
                                            }
                                            else
                                            {
                                                area = EnumAreaLeitura.Erro;
                                            }

                                            break;
                                        case "numero_nfse":
                                            if (numeroNF.Equals(""))
                                            {
                                                numeroNF = x.ReadString();
                                            }
                                            else if (numeroRPS.Equals(""))
                                            {
                                                numeroRPS = x.ReadString();
                                                //long.TryParse(numeroRPS, out numeroLote);
                                            }
                                            break;

                                        case "data_nfse":
                                            DateTime emissao;
                                            DateTime.TryParse(x.ReadString(), out emissao);
                                            dataEmissaoRPS = emissao;
                                            break;
                                        case "situacao_codigo_nfse":
                                            if (x.ReadString().Equals("2"))
                                            {
                                                sucesso = true;
                                                situacaoRPS = "C";
                                            }
                                            break;

                                        case "datahoracancelamento":
                                            if (cancelamento)
                                            {
                                                sucesso = true;
                                                situacaoRPS = "C";
                                            }
                                            break;
                                        case "cod_verificador_autenticidade":
                                            codigoVerificacao = x.ReadString();
                                            break;
                                        case "link_nfse":
                                            linkImpressaoAux = x.ReadString();
                                            break;
                                        case "listamensagemretorno":
                                        case "mensagemretorno":
                                            area = EnumAreaLeitura.Erro;
                                            break;
                                    }
                                    break;
                                }

                            case EnumRespostaRPS.CancelarNfseResposta:
                                {
                                    switch (x.Name.ToString().ToLower())
                                    {
                                        case "cancelamento":
                                            cancelamento = true;
                                            break;
                                        case "datahoracancelamento":
                                            if (cancelamento)
                                            {
                                                sucesso = true;
                                                situacaoRPS = "C";
                                            }
                                            break;
                                        case "numero":
                                            if (numeroNF.Equals(""))
                                            {
                                                numeroNF = x.ReadString();
                                            }
                                            else if (numeroRPS.Equals(""))
                                            {
                                                numeroRPS = x.ReadString();
                                                //long.TryParse(numeroRPS, out numeroLote);
                                            }
                                            break;
                                        case "listamensagemretorno":
                                        case "mensagemretorno":
                                            area = EnumAreaLeitura.Erro;
                                            break;
                                    }
                                    break;
                                }
                        }
                    }

                    #region Erro
                    if (area == EnumAreaLeitura.Erro)
                    {
                        if (x.NodeType == XmlNodeType.Element && x.Name == "codigo")
                        {
                            codigoErroOuAlerta = x.ReadString();
                        }
                        else if (x.NodeType == XmlNodeType.Element && x.Name == "mensagem")
                        {
                            if (string.IsNullOrEmpty(descricaoErro))
                            {
                                descricaoErro = string.Concat("[", codigoErroOuAlerta, "] ", x.ReadString());
                                codigoErroOuAlerta = "";
                            }
                            else
                            {
                                descricaoErro = string.Concat(descricaoErro, "\n", "[", codigoErroOuAlerta, "] ", x.ReadString());
                                codigoErroOuAlerta = "";
                            }
                        }
                        else if (x.NodeType == XmlNodeType.Element && x.Name == "correcao")
                        {
                            var correcao = x.ReadString().ToString().Trim() ?? "";
                            if (correcao != "") { descricaoErro = string.Concat(descricaoErro, " ( Sugestão: " + correcao + " ) "); }
                        }
                    }
                    #endregion Erro

                }
                x.Close();
            }

            var dhRecbto = "";
            var error = "";
            var success = "";

            if (dataEmissaoRPS != null && dataEmissaoRPS.Value != null)
            {
                nota.Documento.TDFe.Tide.DataEmissaoRps = dataEmissaoRPS.Value;
                nota.Documento.TDFe.Tide.DataEmissao = dataEmissaoRPS.Value;
                dhRecbto = dataEmissaoRPS.Value.ToString();
            }

            var xMotivo = descricaoErro != "" ? string.Concat(descricaoProcesso, "[", descricaoErro, "]") : descricaoProcesso;
            if ((sucesso && !string.IsNullOrEmpty(numeroNF)) || (!string.IsNullOrEmpty(numNF) && Generico.MesmaNota(numeroNF, numNF) && situacaoRPS != ""))
            {
                sucesso = true;
                success = "Sucesso";
            }
            else
            {
                var msgRetornoAux = xMotivo;

                if ((msgRetornoAux.Contains("O numero do lote do contribuinte informado, já existe.") ||
                        msgRetornoAux.Contains("O número do lote do contribuinte informado, já existe."))
                        && msgRetornoAux.Contains("Protocolo:"))
                {
                    var protocoloAux = msgRetornoAux.Substring(msgRetornoAux.LastIndexOf("Protocolo: ") + 10);
                    protocoloAux = Generico.RetornarApenasNumeros(protocoloAux);

                    if (!String.IsNullOrEmpty(protocoloAux))
                    {
                        protocolo = protocoloAux;
                        xMotivo = String.Empty;
                    }

                }

                error = xMotivo;
                if (string.IsNullOrEmpty(xMotivo))
                {
                    if (protocolo != "")
                        error = "Não foi possível finalizar a transmissão. Aguarde alguns minutos e execute um consulta para finalizar a operação. Protocolo gerado: " + protocolo.ToString().Trim();
                    else
                        error = "Não foi possível finalizar a transmissão. Tente novamente mais tarde ou execute uma consulta.";
                }
            }

            var cStat = "";
            var xml = "";

            if (sucesso && situacaoRPS != "C")
            {
                cStat = "100";
                nota.Documento.TDFe.Tide.FStatus = EnumNFSeRPSStatus.srNormal;
                xMotivo = "NFSe Normal";
            }
            else if (sucesso && situacaoRPS == "C")
            {
                cStat = "101";
                nota.Documento.TDFe.Tide.FStatus = EnumNFSeRPSStatus.srCancelado;
                xMotivo = "NFSe Cancelada";
            }
            //if (cStat == "100" || cStat == "101")
            //{
            //var xmlRetorno = nota.MontarXmlRetorno(nota, numeroNF, protocolo);
            xml = xmlResposta.OuterXml;
            //}

            return new RetornoTransmitir(error, success)
            {

                chave = numeroNF != "" && numeroNF != "0" ?
                            GerarChaveNFSe(nota.Documento.TDFe.TPrestador.FIdentificacaoPrestador.FEmitIBGEUF, nota.Documento.TDFe.Tide.DataEmissaoRps, nota.Documento.TDFe.TPrestador.FCnpj, numeroNF, 56) : "",
                cStat = cStat,
                xMotivo = xMotivo,
                numero = numeroNF,
                nProt = protocolo,
                xml = xml,
                digVal = codigoVerificacao,
                NumeroLote = numeroLote,
                NumeroRPS = numeroRPS,
                DataEmissaoRPS = dataEmissaoRPS,
                dhRecbto = dhRecbto,
                CodigoRetornoPref = codigoErroOuAlerta,
                LinkImpressao = linkImpressaoAux

            };
        }

        #region Leitura de Lote

        #endregion

        #region Schemas

        public List<InfoSchema> GetSchema()
        {
            List<InfoSchema> list = new List<InfoSchema>();

            list.Add(new InfoSchema()
            {
                Tag = "LoteRps",
                Ibge = "",
                ID = list.Count + 1,
                ArquivoXSD = "NFSe\\EL\\el-nfse.xsd",
                Descricao = "XML de Lote RPS",
                TagAssinatura = "",
                TagAtributoId = "",
                TagLoteAssinatura = "",
                TagLoteAtributoId = "",
                TargetNameSpace = "http://www.el.com.br/nfse/xsd/el-nfse.xsd"
            });

            return list;
        }


        #endregion

    }
}

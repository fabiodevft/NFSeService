using LayoutService.Entities;
using LayoutService.Enums;
using Microsoft.AspNetCore.Mvc;
using NFSeServiceAPI.Business;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using static LayoutService.Entities.FrgNFSe;
using static LayoutService.Entities.Notas;
using ActionNameAttribute = System.Web.Http.ActionNameAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace NFSeServiceAPI.Controllers
{
    public class AnaliseNFSeController : ApiController
    {
        [HttpPost]
        [ActionName("AnaliseRecepcionarLoteRPS")]
        public HttpResponseMessage RecepcionarLoteRPS(ComandoTransmitir comando)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var provedor = comando.TDFe.TProvedor;

                    var nota = new FrgNFSe(provedor)
                    {
                        Documento = comando
                    };

                    XmlDocument xml = null;
                    xml = nota.GeraXmlNota();

                    XmlDocument xmlAux = Util.EncodingXML(xml);

                    //retornar resposta
                    var retornoXml = xmlAux.OuterXml;

                    return Request.CreateResponse<string>(HttpStatusCode.OK, retornoXml);

                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (System.Exception erro)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Message);
            }
        }

        [HttpPost]
        [ActionName("AnaliseCancelarNfse")]
        public HttpResponseMessage CancelarNfse(ComandoTransmitir comando)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var provedor = comando.TDFe.TProvedor;

                    var nota = new FrgNFSe(provedor)
                    {
                        Documento = comando
                    };

                    XmlDocument xml = null;

                    switch (provedor)
                    {
                        case EnumProvedor.DSF:
                        case EnumProvedor.CECAM:
                            xml = nota.GerarXmlCancelaNota(comando.FNumeroNF, comando.FCodigoCancelamento.ToString(), nota.Documento.TDFe.Tide.FNumeroLote, comando.FCodigoVerificacao);
                            break;
                        case EnumProvedor.Paulistana:
                        case EnumProvedor.Metropolis:
                        case EnumProvedor.Thema:
                        case EnumProvedor.BHISS:
                        case EnumProvedor.IssOnline:
                        case EnumProvedor.Natalense:
                        case EnumProvedor.CARIOCA:
                        case EnumProvedor.PRONIM:
                            xml = nota.GerarXmlCancelaNota(comando.FNumeroNF);
                            break;
                        case EnumProvedor.Goiania:
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "err007003");
                        default:
                            xml = nota.GerarXmlCancelaNota(comando.FNumeroNF, comando.FCodigoCancelamento.ToString());
                            break;
                    }

                    XmlDocument xmlAux = Util.EncodingXML(xml);

                    var retornoXml = xmlAux.OuterXml;

                    return Request.CreateResponse<string>(HttpStatusCode.OK, retornoXml);

                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (System.Exception erro)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Message);
            }
        }

        [HttpPost]
        [ActionName("AnaliseConsultarLoteRps")]
        public HttpResponseMessage ConsultarLoteRps(ComandoTransmitir comando)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var provedor = comando.TDFe.TProvedor;

                    var nota = new FrgNFSe(provedor)
                    {
                        Documento = comando
                    };

                    XmlDocument xml = null;

                    xml = nota.GerarXmlConsulta(nota.Documento.TDFe.Tide.FNumeroLote);
                    XmlDocument xmlAux = Util.EncodingXML(xml);

                    var retornoXml = xmlAux.OuterXml;

                    return Request.CreateResponse<string>(HttpStatusCode.OK, retornoXml);

                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (System.Exception erro)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Message);
            }
        }

        [HttpPost]
        [ActionName("AnaliseConsultarNfse")]
        public HttpResponseMessage ConsultarNfse(ComandoTransmitir comando)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var provedor = comando.TDFe.TProvedor;

                    var nota = new FrgNFSe(provedor)
                    {
                        Documento = comando
                    };

                    XmlDocument xml = null;
                    xml = nota.GerarXmlConsulta(comando.FNumeroNF, comando.FDataEmissao);
                    XmlDocument xmlAux = Util.EncodingXML(xml);

                    var retornoXml = xmlAux.OuterXml;

                    return Request.CreateResponse<string>(HttpStatusCode.OK, retornoXml);

                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (System.Exception erro)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Message);
            }
        }

        [HttpPost]
        [ActionName("AnaliseConsultarNfsePorRps")]
        public HttpResponseMessage ConsultarNfsePorRps(ComandoTransmitir comando)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var provedor = comando.TDFe.TProvedor;

                    var nota = new FrgNFSe(provedor)
                    {
                        Documento = comando
                    };

                    XmlDocument xml = null;
                    xml = nota.GerarXmlConsulta(nota.Documento.TDFe.Tide.FIdentificacaoRps.FNumero, nota.Documento.TDFe.Tide.DataEmissao);

                    XmlDocument xmlAux = Util.EncodingXML(xml);

                    var retornoXml = xmlAux.OuterXml;

                    return Request.CreateResponse<string>(HttpStatusCode.OK, retornoXml);

                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (System.Exception erro)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Message);
            }
        }

        [HttpPost]
        [ActionName("AnaliseConsultarSituacaoLoteRps")]
        public HttpResponseMessage ConsultarSituacaoLoteRps(ComandoTransmitir comando)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var provedor = comando.TDFe.TProvedor;

                    var nota = new FrgNFSe(provedor)
                    {
                        Documento = comando
                    };

                    XmlDocument xml = null;
                    xml = nota.GerarXmlConsulta(nota.Documento.TDFe.Tide.FNumeroLote);

                    XmlDocument xmlAux = Util.EncodingXML(xml);

                    var retornoXml = xmlAux.OuterXml;

                    return Request.CreateResponse<string>(HttpStatusCode.OK, retornoXml);

                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (System.Exception erro)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Message);
            }
        }

        [HttpPost]
        [ActionName("AnaliseConsultarStLote")]
        public HttpResponseMessage ConsultarStLote(ComandoTransmitir comando)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var provedor = comando.TDFe.TProvedor;

                    var nota = new FrgNFSe(provedor)
                    {
                        Documento = comando
                    };

                    XmlDocument xml = null;
                    xml = nota.GerarXmlConsulta(nota.Documento.TDFe.Tide.FNumeroLote);

                    XmlDocument xmlAux = Util.EncodingXML(xml);

                    var retornoXml = xmlAux.OuterXml;

                    return Request.CreateResponse<string>(HttpStatusCode.OK, retornoXml);

                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (System.Exception erro)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Message);
            }
        }

    }
}

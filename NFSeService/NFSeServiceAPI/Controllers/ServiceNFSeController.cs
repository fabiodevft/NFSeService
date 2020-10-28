using LayoutService.Entities;
using LayoutService.Enums;
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
    public class ServiceNFSeController : ApiController
    {
        [HttpPost]
        [ActionName("RecepcionarLoteRPS")]
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

                    //chamarservico                    
                    AplicacaoServiceNFSe aplicacao = new AplicacaoServiceNFSe(xmlAux, "RecepcionarLoteRPS", comando, provedor);
                    aplicacao.CarregarDados();
                    var ret = aplicacao.ExecutaMetodo();

                    //retornar resposta
                    var resultadoOperacao = nota.LerRetorno(ret, EnumOperacao.Envio);

                    return Request.CreateResponse<RetornoTransmitir>(HttpStatusCode.OK, resultadoOperacao);

                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (System.Exception erro)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Message);
            }
        }

        [HttpPost]
        [ActionName("CancelarNfse")]
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

                    AplicacaoServiceNFSe aplicacao = new AplicacaoServiceNFSe(xmlAux, "CancelarNfse", comando, provedor);
                    aplicacao.CarregarDados();
                    var retCanc = aplicacao.ExecutaMetodo();

                    var resultadoOperacao = nota.LerRetorno(retCanc, EnumOperacao.Cancela);

                    return Request.CreateResponse<RetornoTransmitir>(HttpStatusCode.OK, resultadoOperacao);

                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (System.Exception erro)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Message);
            }
        }
                
        [HttpPost]
        [ActionName("ConsultarLoteRps")]
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

                    //chamarservico                    
                    AplicacaoServiceNFSe aplicacao = new AplicacaoServiceNFSe(xmlAux, "ConsultarLoteRps", comando, provedor);
                    aplicacao.CarregarDados();
                    var ret = aplicacao.ExecutaMetodo();

                    //retornar resposta
                    var resultadoOperacao = nota.LerRetorno(ret, EnumOperacao.ConsultaLote);

                    return Request.CreateResponse<RetornoTransmitir>(HttpStatusCode.OK, resultadoOperacao);

                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (System.Exception erro)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Message);
            }
        }

        [HttpPost]
        [ActionName("ConsultarNfse")]
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

                    //chamarservico                    
                    AplicacaoServiceNFSe aplicacao = new AplicacaoServiceNFSe(xmlAux, "ConsultarNfse", comando, provedor);
                    aplicacao.CarregarDados();
                    var ret = aplicacao.ExecutaMetodo();
                    //retornar resposta
                    var resultadoOperacao = nota.LerRetorno(ret, EnumOperacao.Consulta);

                    return Request.CreateResponse<RetornoTransmitir>(HttpStatusCode.OK, resultadoOperacao);

                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (System.Exception erro)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Message);
            }
        }

        [HttpPost]
        [ActionName("ConsultarNfsePorRps")]
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

                    //chamarservico                    
                    AplicacaoServiceNFSe aplicacao = new AplicacaoServiceNFSe(xmlAux, "ConsultarNfsePorRps", comando, provedor);
                    aplicacao.CarregarDados();
                    var ret = aplicacao.ExecutaMetodo();

                    //retornar resposta
                    var resultadoOperacao = nota.LerRetorno(ret, EnumOperacao.ConsultaRPS);

                    return Request.CreateResponse<RetornoTransmitir>(HttpStatusCode.OK, resultadoOperacao);

                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (System.Exception erro)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Message);
            }
        }

        [HttpPost]
        [ActionName("ConsultarSituacaoLoteRps")]
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

                    //chamarservico                    
                    AplicacaoServiceNFSe aplicacao = new AplicacaoServiceNFSe(xmlAux, "ConsultarSituacaoLoteRps", comando, provedor);
                    aplicacao.CarregarDados();
                    var ret = aplicacao.ExecutaMetodo();

                    //retornar resposta
                    var resultadoOperacao = nota.LerRetorno(ret, EnumOperacao.ConsultaStatus);

                    return Request.CreateResponse<RetornoTransmitir>(HttpStatusCode.OK, resultadoOperacao);

                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (System.Exception erro)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Message);
            }
        }

        [HttpPost]
        [ActionName("ConsultarStLote")]
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

                    //chamarservico                    
                    AplicacaoServiceNFSe aplicacao = new AplicacaoServiceNFSe(xmlAux, "ConsultarStLote", comando, provedor);
                    aplicacao.CarregarDados();
                    var ret = aplicacao.ExecutaMetodo();

                    //retornar resposta
                    var resultadoOperacao = nota.LerRetorno(ret, EnumOperacao.ConsultaStLote);

                    return Request.CreateResponse<RetornoTransmitir>(HttpStatusCode.OK, resultadoOperacao);

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

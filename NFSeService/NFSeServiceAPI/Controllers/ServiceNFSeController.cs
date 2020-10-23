using LayoutService.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using static LayoutService.Entities.FrgNFSe;
using ActionNameAttribute = System.Web.Http.ActionNameAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace NFSeServiceAPI.Controllers
{
    public class ServiceNFSeController : ApiController
    {
        /// <summary>
        /// Montagem e Envio de Lotes RPS para o WebService do município.
        /// </summary>
        /// <remarks>
        /// É recebido o objeto ComandoTransmitir (padrão dos documentos fiscais) e retornado o objeto RetornoTransmitir.  
        /// </remarks>
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

                    XmlDocument xmlAux = EncodingXML(xml);




                }

            }
            catch (System.Exception erro)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Message);
            }
        }


        private static XmlDocument EncodingXML(XmlDocument xml)
        {
            byte[] strXml = Encoding.UTF8.GetBytes(xml.OuterXml);

            XmlDocument xmlAux = new XmlDocument();
            xmlAux.PreserveWhitespace = true;
            xmlAux.LoadXml(Encoding.UTF8.GetString(strXml));

            return xmlAux;
        }

    }
}

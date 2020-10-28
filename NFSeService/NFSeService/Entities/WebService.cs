using LayoutService.Enums;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace NFSeService.Entities
{
    public abstract class WebService
    {
        public WSSoap WSSoap { get; set; }
        public EnumProvedor Provedor { get; set; }

        protected WebService(WSSoap wSSoap, EnumProvedor provedor)
        {
            WSSoap = WSSoap;
            Provedor = provedor;
        }

        public virtual string RecepcionarLoteRps()
        {
            throw new NotImplementedException();
        }
        public virtual string ConsultarSituacaoLoteRps()
        {
            throw new NotImplementedException();
        }
        public virtual string ConsultarNfsePorRps()
        {
            throw new NotImplementedException();
        }
        public virtual string ConsultarNfse()
        {
            throw new NotImplementedException();
        }
        public virtual string ConsultarLoteRps()
        {
            throw new NotImplementedException();
        }
        public virtual string ConsultarURLNfse()
        {
            throw new NotImplementedException();
        }
        public virtual string CancelarNfse()
        {
            throw new NotImplementedException();
        }
        public virtual string ConsultarStatusNfse()
        {
            throw new NotImplementedException();
        }
        public virtual string ConsultarStLote()
        {
            throw new NotImplementedException();
        }

        #region RequestWS

        public string RequestWS(string envelope, string url, string soapAction)
        {
            string XMLRetorno = string.Empty;

            HttpWebRequest request = CreateSOAPWebRequest(url, soapAction);

            XmlDocument SOAPReqBody = new XmlDocument();
            SOAPReqBody.LoadXml(envelope);

            var buffer = Encoding.UTF8.GetBytes(envelope);
            request.ContentLength = buffer.Length;

            //Envio
            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }

            //Resposta  
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream    
                    var ServiceResult = rd.ReadToEnd();
                    //writting stream result on console    
                    XMLRetorno = ServiceResult;
                }
            }

            return XMLRetorno;
        }

        private HttpWebRequest CreateSOAPWebRequest(string url, string sopaAction)
        {
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(url);
            Req.Headers.Add($"SOAPAction:{sopaAction}");
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Timeout = 6000;
            Req.Accept = "text/xml";
            Req.Method = "POST";

            return Req;
        }

        #endregion
    }
}

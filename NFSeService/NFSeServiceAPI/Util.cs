using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NFSeServiceAPI
{
    public static class Util
    {
        public static XmlDocument EncodingXML(XmlDocument xml)
        {
            byte[] strXml = Encoding.UTF8.GetBytes(xml.OuterXml);

            XmlDocument xmlAux = new XmlDocument();
            xmlAux.PreserveWhitespace = true;
            xmlAux.LoadXml(Encoding.UTF8.GetString(strXml));

            return xmlAux;
        }
    }
}

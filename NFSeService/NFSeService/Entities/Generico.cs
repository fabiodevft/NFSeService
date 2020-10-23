using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NFSeService.Entities
{
    public static class Generico
    {
        public static string TrataCaracteres(string value)
        {
            value = value.Replace("&#231;", "ç");
            value = value.Replace("&#227;", "ã");
            value = value.Replace("&amp;", "&");
            value = value.Replace("&lt;", "<");
            value = value.Replace("&gt;", ">");
            value = value.Replace("&quot;", "\"");
            value = value.Replace("&#39;", "'");

            return value;
        }
        public static MemoryStream StringXmlToStream(string strXml)
        {
            byte[] byteArray = new byte[strXml.Length];
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byteArray = encoding.GetBytes(strXml);
            MemoryStream memoryStream = new MemoryStream(byteArray);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

    }
}

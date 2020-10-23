using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NFSeService.Entities.Interfaces
{
    public interface IServicoProvedor
    {
        XmlDocument Assinar(XmlDocument document);
        string Envelopar(XmlDocument document);
        Validacao Validar(XmlDocument document);
    }
}

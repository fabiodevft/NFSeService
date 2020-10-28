using NFSeService.Entities;
using System.Xml;

namespace NFSeService.Interfaces
{
    public interface IServicoProvedor
    {
        XmlDocument Assinar(XmlDocument document);
        string Envelopar(XmlDocument document, string strCabecMsg, string metodo);
        Validacao Validar(XmlDocument document);
    }
}

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace LayoutService.Entities
{
    public class Empresa
    {
        public string CNPJ { get; set; }
        public int UnidadeFederativaCodigo { get; set; }
        public string Nome { get; set; }
        public bool UsaCertificado { get; set; }
        public X509Certificate2 X509Certificado { get; set; }
        public string UsuarioWS { get; set; }
        public string SenhaWS { get; set; }
        public string CertificadoSenha { get; set; }
        public int AmbienteCodigo { get; set; }

    }
}

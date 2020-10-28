namespace NFSeService.Entities
{
    public class WSSoap
    {

        private string _ActionSoap;
        private string _ContentType;
        private string _EnderecoWeb;
        private string _TagRetorno;
        private string _VersaoSoap;
        

        /// <summary>
        /// Web Action - Endereço com a ação/método que será executado no webservice
        /// </summary>
        public string ActionSoap
        {
            get => _ActionSoap;
            set => _ActionSoap = value;
        }

        /// <summary>
        /// Content Type - Tipo do conteúdo do SOAP
        /// </summary>
        public string ContentType
        {
            get => string.IsNullOrWhiteSpace(_ContentType) ? (_ContentType = "application/soap+xml; charset=utf-8;") : _ContentType;
            set => _ContentType = value;
        }

        /// <summary>
        /// Endereço do Webservice
        /// </summary>
        public string EnderecoWeb
        {
            get => _EnderecoWeb;
            set => _EnderecoWeb = value;
        }

        
        /// <summary>
        /// Versão do SOAP
        /// </summary>
        public string VersaoSoap
        {
            get => string.IsNullOrWhiteSpace(_VersaoSoap) ? (_VersaoSoap = "soap12") : _VersaoSoap;
            set => _VersaoSoap = value;
        }

    }
}

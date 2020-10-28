namespace NFSeService.Entities
{
    public class Validacao
    {
        public bool bArquivoValido { get; set; }
        public string MensagemErro { get; set; }

        public Validacao()
        {
        }

        public Validacao(bool bArquivoValido, string mensagemErro)
        {
            this.bArquivoValido = bArquivoValido;
            MensagemErro = mensagemErro;
        }


    }
}

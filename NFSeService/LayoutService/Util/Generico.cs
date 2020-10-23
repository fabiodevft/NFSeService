using LayoutService.Entities;
using LayoutService.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LayoutService.Util
{
    public static class Generico
    {
        public static string FormataValor(decimal valor)
        {
            var retorno = valor.ToString();
            if (Extensions.PossuiCasasDecimais(valor))
            {
                retorno = valor.ToString().Replace(",", ".");
            }
            else
            {
                retorno = decimal.Floor(valor).ToString();
            }

            return retorno;
        }
        public static string FormataValor(decimal valor, int casasDecimais)
        {
            var retorno = valor.ToString();
            if (Extensions.PossuiCasasDecimais(valor))
            {
                valor = Math.Round(valor, casasDecimais);
                retorno = valor.ToString().Replace(",", ".");
            }
            else
            {
                retorno = decimal.Floor(valor).ToString("#0.00").Replace(",", ".");
            }

            return retorno;
        }
        public static string ImpostoRetido(EnumNFSeSituacaoTributaria situacao, int tipo = 0)
        {
            var tipoRecolhimento = "2";
            if (situacao == EnumNFSeSituacaoTributaria.stRetencao)
            {
                tipoRecolhimento = "1";
            }

            return tipoRecolhimento;
        }
        public static bool MesmaNota(string numeroNF, string numNF)
        {
            long numero1 = 0;
            long numero2 = 0;
            return (long.TryParse(numeroNF, out numero1) == long.TryParse(numeroNF, out numero2));
        }
        public static string GetMunicipioIncidencia(FrgNFSe nota)
        {
            string resposta = string.Empty;

            if (nota.Documento.TDFe.TServico.FMunicipioIncidencia.Equals(nota.Documento.TDFe.TPrestador.TEndereco.FCodigoMunicipio))
                resposta = nota.Documento.TDFe.TPrestador.TEndereco.FSIAFI;
            else
                resposta = nota.Documento.TDFe.TTomador.TEndereco.FSIAFI;

            return resposta;
        }
        public static string RetornarApenasNumeros(string Texto)
        {
            string retorno = "";
            retorno = String.Join("", System.Text.RegularExpressions.Regex.Split(Texto, @"[^\d]"));
            return retorno;
        }
        public static string FormatCNPJ(string CNPJ)
        {
            return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
        }
        public static string RetornaApenasLetrasNumeros(string texto)
        {
            if (!string.IsNullOrEmpty(texto))
            {
                return Regex.Replace(texto, "[^a-zA-Z0-9]+", "");
            }
            return string.Empty;
        }
        public static string TratarString(string texto)
        {
            return RemoverCaracterEspecial(TrocarCaractercomAcentos(texto));
        }
        public static string RemoverCaracterEspecial(string texto)
        {
            return Regex.Replace(texto, "[^0-9a-zA-Z_+-.,!@#$%&*();\\/|<>:?= ]+", "");
        }
        public static string TrocarCaractercomAcentos(string texto)
        {
            if (texto != null && texto != "")
            {
                string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
                string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

                for (int i = 0; i < comAcentos.Length; i++)
                {
                    texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
                }
                return texto;
            }
            else
            {
                return "";
            }
        }
    }
}

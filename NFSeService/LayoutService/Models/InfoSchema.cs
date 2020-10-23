using System;
using System.Collections.Generic;
using System.Text;

namespace LayoutService.Models
{
    public class InfoSchema
    {
        /// <summary>
        /// TAG do XML que identifica qual XML é
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Ibge do municipio que tenha um layout especifico
        /// </summary>
        public string Ibge { get; set; }

        /// <summary>
        /// Identificador único numérico do XML
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Breve descrição do arquivo XML
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Nome do arquivo de schema para validar o XML
        /// </summary>
        public string ArquivoXSD { get; set; }

        /// <summary>
        /// Nome da tag do XML que será assinada
        /// </summary>
        public string TagAssinatura { get; set; }

        /// <summary>
        /// Nome da tag que tem o atributo ID
        /// </summary>
        public string TagAtributoId { get; set; }

        /// <summary>
        /// Nome da tag de lote do XML que será assinada
        /// </summary>
        public string TagLoteAssinatura { get; set; }

        /// <summary>
        /// Nome da tag de lote que tem o atributo ID
        /// </summary>
        public string TagLoteAtributoId { get; set; }

        /// <summary>
        /// Nome da tag do XML que será assinada (uma segunda tag que tem que ser assinada ex. SubstituirNfse Pelotas-RS)
        /// </summary>
        public string TagAssinatura0 { get; set; }
        /// <summary>
        /// Nome da tag que tem o atributo ID que será assinada, faz consunto com a TagAssinatura0
        /// </summary>
        public string TagAtributoId0 { get; set; }

        /// <summary>
        /// URL do schema de cada XML
        /// </summary>
        public string TargetNameSpace { get; set; }
    }
}

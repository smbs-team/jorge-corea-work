namespace PTASCRMHelpers
{
  using System;
  using Newtonsoft.Json;


  public partial class CrmOdataHelper
    {
    /// <summary>
    /// It represents a row in ptas_Fileattachmentmetadata
    /// </summary>
    public class Fileattachmentmetadata
        {

            [JsonProperty("ptas_name")]
            public string ptas_name { get; set; }

            [JsonProperty("ptas_icsdocumentid")]
            public string ptas_icsdocumentid { get; set; }

            [JsonProperty("ptas_icsfileid")]
            public string ptas_icsfileid { get; set; }

            [JsonProperty("ptas_icsentereddate")]
            public DateTime ptas_icsentereddate { get; set; }

            [JsonProperty("ptas_modifieddate")]
            public DateTime ptas_modifieddate { get; set; }

            [JsonProperty("ptas_icscreatedbyusername")]
            public string ptas_icscreatedbyusername { get; set; }

            [JsonProperty("ptas_icscheckedoutbyusername")]
            public string ptas_icscheckedoutbyusername { get; set; }

            [JsonProperty("ptas_icscheckedoutdate")]
            public DateTime ptas_icscheckedoutdate { get; set; }

            [JsonProperty("ptas_icsfullindex")]
            public string ptas_icsfullindex { get; set; }

            [JsonProperty("ptas_icsisinworkflow")]
            public int ptas_icsisinworkflow { get; set; }

            [JsonProperty("ptas_accountnumber")]
            public string ptas_accountnumber { get; set; }

            [JsonProperty("ptas_documenttype")]
            public string ptas_documenttype { get; set; }

            [JsonProperty("ptas_documentdate")]
            public DateTime ptas_documentdate { get; set; }

            [JsonProperty("ptas_loginuserid")]
            public string ptas_loginuserid { get; set; }

            [JsonProperty("ptas_scannerid")]
            public string ptas_scannerid { get; set; }

            [JsonProperty("ptas_recid")]
            public int ptas_recid { get; set; }

            [JsonProperty("ptas_pagecount")]
            public int ptas_pagecount { get; set; }

            [JsonProperty("ptas_scandatetime")]
            public DateTime ptas_scandatetime { get; set; }

            [JsonProperty("ptas_rollyear")]
            public int ptas_rollyear { get; set; }

            [JsonProperty("ptas_scandate")]
            public DateTime ptas_scandate { get; set; }

            [JsonProperty("ptas_originalfilename")]
            public string ptas_originalfilename { get; set; }

            [JsonProperty("ptas_documentsize")]
            public int ptas_documentsize { get; set; }

            [JsonProperty("ptas_fileextension")]
            public string ptas_fileextension { get; set; }

            [JsonProperty("ptas_insertdate")]
            public DateTime ptas_insertdate { get; set; }

            [JsonProperty("ptas_updatedate")]
            public DateTime ptas_updatedate { get; set; }

            [JsonProperty("ptas_isblob")]
            public bool ptas_isblob { get; set; }

            [JsonProperty("ptas_isilinx")]
            public bool ptas_isilinx { get; set; }

            [JsonProperty("ptas_bloburl")]
            public string ptas_bloburl { get; set; }

            [JsonProperty("ptas_repositoryname")]
            public string ptas_repositoryname { get; set; }

            [JsonProperty("ptas_seniorexemptionapplication")]
            public Guid ptas_seniorexemptionapplication { get; set; }

            [JsonProperty("ptas_seniorexemptionapplicationdetail")]
            public Guid ptas_seniorexemptionapplicationdetail { get; set; }
        }
    }
}

using System.Text.Json.Serialization;

namespace DeutschArtikelLearnApp.DTO.WiktApiResponseDTOS
{
    public class PosBlock
    {
        [JsonPropertyName("pos")]
        public string Pos { get; set; }
        [JsonPropertyName("lang_code")]
        public string Lang_Code { get; set; }
        [JsonPropertyName("forms")]
        public List<FormItem> Forms { get; set; }
    }
}

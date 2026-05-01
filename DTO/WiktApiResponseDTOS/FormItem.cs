using System.Text.Json.Serialization;

namespace DeutschArtikelLearnApp.DTO.WiktApiResponseDTOS
{
    public class FormItem
    {
        [JsonPropertyName("form")]
        public string Form { get; set; }
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }
        [JsonPropertyName("article")]
        public string Article { get; set; }
    }
}

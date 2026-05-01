using System.Text.Json.Serialization;

namespace DeutschArtikelLearnApp.DTO.WiktApiResponseDTOS
{
    public class WiktApiResponse
    {
        [JsonPropertyName("word")]
        public string Word { get; set; }
        [JsonPropertyName("edition")]
        public string Edition { get; set; }
        [JsonPropertyName("forms")]
        public List<PosBlock> Forms { get; set; }
    }
}

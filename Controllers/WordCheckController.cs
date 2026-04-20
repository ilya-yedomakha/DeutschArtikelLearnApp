using System.Text.Json;
using System.Text.RegularExpressions;
using DeutschArtikelLearnApp.DTO;
using DeutschArtikelLearnApp.Help.Result;
using DeutschArtikelLearnApp.Help.Result.ModelErrors;
using DeutschArtikelLearnApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeutschArtikelLearnApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordCheckController : ControllerBase
    {
        [HttpGet(Name = "GetTestWord")]
        public async Task<object> GetTestWord()
        {
            return await SingleWordDataRequest("Land");
        }


        [HttpPost(Name = "SingleWordCheck")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SingleWordCheck([FromBody] SingleWordRequest singleWordRequest)
        {

            var result = await SingleWordDataRequest(singleWordRequest.Word);
            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Model);
        }

        #region util methods
        private async Task<Result<RightForm,BaseReadDTO>> SingleWordDataRequest(String word)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) WiktionaryClient/1.0"
            );

            var json = await client.GetStringAsync(
                $"https://de.wiktionary.org/w/api.php?action=parse&page={word}&format=json&prop=wikitext"
            );

            var doc = JsonDocument.Parse(json);

            if(doc.RootElement.TryGetProperty("parse", out JsonElement parseElement) &&
               parseElement.TryGetProperty("wikitext", out JsonElement wikiElement) &&
               wikiElement.TryGetProperty("*",out JsonElement textElement))
            {
                var wikitext = textElement.GetString();

                var isNoun = wikitext.Contains("{{Wortart|Substantiv|Deutsch}}");

                if (!isNoun)
                {
                    return Result<RightForm, BaseReadDTO>
                        .Failure(RightFormError<RightForm>.IncorrectWord(word));
                }

                var substantivBlock = Regex.Match(wikitext, @"\{\{Deutsch Substantiv Übersicht.*?\}\}", RegexOptions.Singleline);

                if (!substantivBlock.Success)
                {
                    return Result<RightForm, BaseReadDTO>
                        .Failure(RightFormError<RightForm>.IncorrectWord(word));
                }

                var block = substantivBlock.Value;

                var genusMatch = Regex.Match(block, @"Genus=([mfn])");
                var pluralMatch = Regex.Match(block, @"Nominativ Plural 1=([^\n|}]+)");

                string article = genusMatch.Groups[1].Value switch
                {
                    "m" => "der",
                    "f" => "die",
                    "n" => "das",
                    _ => ""
                };

                var rightForm = new RightForm
                {
                    Word = word,
                    Article = article,
                    Plural = pluralMatch.Groups[1].Value
                };
                var res = Result<RightForm, BaseReadDTO>.Success();
                res.Model = rightForm;

                return res;
            } 
            else {
                return Result<RightForm, BaseReadDTO>.Failure(RightFormError<RightForm>.IncorrectWord(word));
            }

            
        }

        public class SingleWordRequest
        {
            public string Word { get; set; } = "";
        }

        public class RightForm : BaseModel
        {
            public string? Word { get; set; }
            public string? Article { get; set; }
            public string? Plural { get; set; }
        }
        #endregion util methods
    }
}

using System.Text.Json;
using System.Text.RegularExpressions;
using AutoMapper;
using DeutschArtikelLearnApp.DTO;
using DeutschArtikelLearnApp.DTO.Create;
using DeutschArtikelLearnApp.DTO.WiktApiResponseDTOS;
using DeutschArtikelLearnApp.Help.Result;
using DeutschArtikelLearnApp.Help.Result.ModelErrors;
using DeutschArtikelLearnApp.Model;
using DeutschArtikelLearnApp.Model.Lessons;
using DeutschArtikelLearnApp.Repositories;
using DeutschArtikelLearnApp.Repositories.Base;
using DeutschArtikelLearnApp.Services.Base;

namespace DeutschArtikelLearnApp.Services
{
    public class RightFormService : BaseService<RightForm, RightFormReadDTO>
    {
        public RightFormService(IMapper mapper, BaseRepository<RightForm> baseRepository, RightFormRepository rightFormRepository, LessonRepository lessonRepository) : base(mapper, baseRepository, lessonRepository, rightFormRepository)
        {
        }

        public async Task<Result<RightForm, RightFormReadDTO>> CreateRightForm(SingleRequestWord requestWord)
        {
            var word = requestWord.Word;

            if (string.IsNullOrEmpty(word))
            {
                //TODO: better error description
                return Result<RightForm, RightFormReadDTO>.Failure(ModelError<RightForm>.NullReference);
            }

            var createRightForm = _rightFormRepository.GetModel(word, true);



            Lesson? db_lesson = null;
            var lessonId = requestWord.LessonId;
            if (lessonId != null)
            {
                db_lesson = _lessonRepository.GetById((int)lessonId, true);
                if (db_lesson == null)
                {
                    return Result<RightForm, RightFormReadDTO>.Failure(ModelError<Lesson>.NullReference);
                }
            }

            if (createRightForm != null)
            {
                if (db_lesson != null)
                {
                    //lesson already has this word
                    if (createRightForm.Lessons.Contains(db_lesson))
                    {
                        return Result<RightForm, RightFormReadDTO>.Success();
                    }
                    //lesson and word are existing but they need to be bonded
                    else
                    {
                        db_lesson.RightForms.Add(createRightForm);
                        createRightForm.Lessons.Add(db_lesson);
                    }
                }

            }
            //word does not exist yet
            else
            {
                Result<RightForm, RightFormReadDTO> wikiResult = await SingleWordWiktapiDataRequest(word);
                if (wikiResult.Model == null || wikiResult.IsFailure)
                {
                    return wikiResult;
                }

                createRightForm = wikiResult.Model;
                if (db_lesson != null)
                {
                    db_lesson.RightForms.Add(createRightForm);
                    createRightForm.Lessons.Add(db_lesson);
                }

            }

            if (_rightFormRepository.AddModel(createRightForm))
            {
                return Result<RightForm, RightFormReadDTO>.Success();
            }

            return Result<RightForm, RightFormReadDTO>.Failure(ModelError<RightForm>.ServerError);
        }
        // OLD API
        //public async Task<Result<RightForm, RightFormReadDTO>> SingleWordDataRequest(String word)
        //{
        //    var client = new HttpClient();

        //    client.DefaultRequestHeaders.UserAgent.ParseAdd(
        //        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) WiktionaryClient/1.0"
        //    );

        //    var json = await client.GetStringAsync(
        //        $"https://de.wiktionary.org/w/api.php?action=parse&page={word}&format=json&prop=wikitext"
        //    );

        //    var doc = JsonDocument.Parse(json);

        //    if (doc.RootElement.TryGetProperty("parse", out JsonElement parseElement) &&
        //       parseElement.TryGetProperty("wikitext", out JsonElement wikiElement) &&
        //       wikiElement.TryGetProperty("*", out JsonElement textElement))
        //    {
        //        var wikitext = textElement.GetString();

        //        var isNoun = wikitext.Contains("{{Wortart|Substantiv|Deutsch}}");

        //        if (!isNoun)
        //        {
        //            return Result<RightForm, RightFormReadDTO>
        //                .Failure(RightFormError<RightForm>.IncorrectWord(word));
        //        }

        //        var substantivBlock = Regex.Match(wikitext, @"\{\{Deutsch Substantiv Übersicht.*?\}\}", RegexOptions.Singleline);

        //        if (!substantivBlock.Success)
        //        {
        //            return Result<RightForm, RightFormReadDTO>
        //                .Failure(RightFormError<RightForm>.IncorrectWord(word));
        //        }

        //        var block = substantivBlock.Value;

        //        var genusMatch = Regex.Match(block, @"Genus=([mfn])");
        //        var pluralMatch = Regex.Match(block, @"Nominativ Plural 1=([^\n|}]+)");

        //        string article = genusMatch.Groups[1].Value switch
        //        {
        //            "m" => "der",
        //            "f" => "die",
        //            "n" => "das",
        //            _ => ""
        //        };

        //        var rightForm = new RightForm
        //        {
        //            Name = word,
        //            Article = article,
        //            Plural = pluralMatch.Groups[1].Value
        //        };
        //        Result<RightForm, RightFormReadDTO> res = Result<RightForm, RightFormReadDTO>.Success();
        //        res.Model = rightForm;

        //        return res;
        //    }
        //    else
        //    {
        //        return Result<RightForm, RightFormReadDTO>.Failure(RightFormError<RightForm>.IncorrectWord(word));
        //    }


        //}

        public async Task<Result<RightForm, RightFormReadDTO>> SingleWordWiktapiDataRequest(string word)
        {
            var client = new HttpClient();

            var encodedWord = Uri.EscapeDataString(word);

            var url = $"https://api.wiktapi.dev/v1/de/word/{encodedWord}/forms?lang=de";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                return Result<RightForm, RightFormReadDTO>
                    .Failure(RightFormError<RightForm>.ServiceError(errorMessage));
            }

            var stream = await response.Content.ReadAsStreamAsync();

            var data = await JsonSerializer.DeserializeAsync<WiktApiResponse>(stream);

            if (data?.Forms == null)
            {
                return Result<RightForm, RightFormReadDTO>
                    .Failure(RightFormError<RightForm>.IncorrectWord(word));
            }
            // TODO: I don't know if there is a scenario where there are more than one correct noun form,
            //so I take the first one, where there are forms
            var realNounPosBlocks = data.Forms.Where(x => x.Pos == "noun" && x.Forms.Count != 0).FirstOrDefault();

            if (realNounPosBlocks == null)
            {
                //TODO: Change error to API error, there is no noun form for the given Substantive.
                return Result<RightForm, RightFormReadDTO>
                    .Failure(RightFormError<RightForm>.IncorrectWord(word));
            }

            var singleForm = realNounPosBlocks.Forms.Where(x => x.Tags.Contains("nominative") && x.Tags.Contains("singular")).FirstOrDefault();

            var pluralForm = realNounPosBlocks.Forms.Where(x => x.Tags.Contains("nominative") && x.Tags.Contains("plural")).FirstOrDefault();

            if (singleForm != null)
            {
                var result = Result<RightForm, RightFormReadDTO>.Success();

                var wordCompleteForm = new RightForm
                {
                    Name = singleForm.Form,
                    Article = singleForm.Article,
                    Plural = pluralForm != null && !string.IsNullOrEmpty(pluralForm.Form) ? pluralForm.Form : ""
                };
                result.Model = wordCompleteForm;
                result.ModelDTO = _mapper.Map<RightFormReadDTO>(wordCompleteForm);

                return result;
            }



            return Result<RightForm, RightFormReadDTO>
                .Failure(RightFormError<RightForm>.IncorrectWord(word));
        }
    }
}

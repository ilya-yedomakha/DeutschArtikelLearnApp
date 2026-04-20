using DeutschArtikelLearnApp.Models;

namespace DeutschArtikelLearnApp.Help.Result.ModelErrors
{
    public class RightFormError<T> where T : BaseModel
    {
        public static Error IncorrectWord(String word) => new Error(
            $"{typeof(T).Name}.IncorrectWord", $"The word: " + word + " is incorrect.");
    }
}

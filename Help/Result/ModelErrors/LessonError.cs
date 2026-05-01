using DeutschArtikelLearnApp.Model.Base;

namespace DeutschArtikelLearnApp.Help.Result.ModelErrors
{
    public class LessonError<T> where T : BaseModel
    {
        //public static Error IncorrectWord(int lessonId) => new Error(
        //    $"{typeof(T).Name}.IncorrectWord", $"The word: " + word + " is incorrect.");
    }
}

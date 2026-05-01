namespace DeutschArtikelLearnApp.DTO.Create
{
    public class SingleRequestWord
    {
        public required string Word { get; set; }

        public int? LessonId { get; set; }
    }
}

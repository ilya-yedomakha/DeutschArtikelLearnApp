using AutoMapper;
using DeutschArtikelLearnApp.DTO;
using DeutschArtikelLearnApp.DTO.Create;
using DeutschArtikelLearnApp.Help.Result;
using DeutschArtikelLearnApp.Help.Result.ModelErrors;
using DeutschArtikelLearnApp.Model;
using DeutschArtikelLearnApp.Model.Lessons;
using DeutschArtikelLearnApp.Repositories;
using DeutschArtikelLearnApp.Repositories.Base;
using DeutschArtikelLearnApp.Services.Base;

namespace DeutschArtikelLearnApp.Services
{
    public class LessonService : BaseService<Lesson, LessonReadDTO>
    {
        public LessonService(IMapper mapper, BaseRepository<Lesson> baseRepository, LessonRepository lessonRepository, RightFormRepository rightFormRepository) : base(mapper, baseRepository, lessonRepository, rightFormRepository)
        {
        }

        public Result<Lesson, LessonReadDTO> CreateLesson(LessonCreateDTO LessonDTO)
        {
            var Lesson = _mapper.Map<Lesson>(LessonDTO);
            if (Lesson == null)
            {
                return Result<Lesson, LessonReadDTO>.Failure(ModelError<Lesson>.NullReference);
            }

            if (HasDuplicate(Lesson))
            {
                return Result<Lesson, LessonReadDTO>.Failure(ModelError<Lesson>.SameTitle(Lesson.Name));
            }

            if (_lessonRepository.AddModel(Lesson))
            {
                return Result<Lesson, LessonReadDTO>.Success();
            }

            return Result<Lesson, LessonReadDTO>.Failure(ModelError<Lesson>.ServerError);
        }

        public bool HasDuplicate(Lesson lesson)
        {
            var duplicateLesson = _lessonRepository.GetModel(lesson.Name,false);
            if (duplicateLesson != null && lesson.level.Equals(duplicateLesson.level))
            {
                return true;
            }
            return false;
        }
    }
}

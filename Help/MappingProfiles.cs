using AutoMapper;
using DeutschArtikelLearnApp.DTO;
using DeutschArtikelLearnApp.DTO.Create;
using DeutschArtikelLearnApp.Model;
using DeutschArtikelLearnApp.Model.Lessons;

namespace DeutschArtikelLearnApp.Help
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RightForm, RightFormReadDTO>();
            CreateMap<Lesson, LessonReadDTO>();
            CreateMap<LessonCreateDTO, Lesson>();
        }
    }
}

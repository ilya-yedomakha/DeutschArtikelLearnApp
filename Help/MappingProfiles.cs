using AutoMapper;
using DeutschArtikelLearnApp.DTO;
using DeutschArtikelLearnApp.DTO.Create;
using DeutschArtikelLearnApp.Model;

namespace DeutschArtikelLearnApp.Help
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RightForm, RightFormReadDTO>();
        }
    }
}

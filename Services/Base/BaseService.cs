using AutoMapper;
using DeutschArtikelLearnApp.DTO;
using DeutschArtikelLearnApp.Help.Result;
using DeutschArtikelLearnApp.Help.Result.ModelErrors;
using DeutschArtikelLearnApp.Model.Base;
using DeutschArtikelLearnApp.Repositories;
using DeutschArtikelLearnApp.Repositories.Base;


namespace DeutschArtikelLearnApp.Services.Base
{
    public class BaseService<T,TDTO>
        where T : Entity
        where TDTO : BaseReadDTO
    {
        protected readonly IMapper _mapper;
        protected readonly BaseRepository<T> _baseRepository;
        protected readonly RightFormRepository _rightFormRepository;
        protected readonly LessonRepository _lessonRepository;
        public BaseService(
            IMapper mapper,
            BaseRepository<T> baseRepository,
            LessonRepository lessonRepository,
            RightFormRepository rightFormRepository)
        {
            _mapper = mapper;
            _baseRepository = baseRepository;
            _lessonRepository = lessonRepository;
            _rightFormRepository = rightFormRepository;
        }

        public Result<T, TDTO> GetById(int id, bool includes)
        {
            var model = _baseRepository.GetById(id, includes);
            if (model != null)
            {
                var res = Result<T, TDTO>.Success();
                res.Model = model;
                res.ModelDTO = _mapper.Map<TDTO>(model);
                return res;
            }
            else return Result<T, TDTO>.Failure(ModelError<T>.NotFound(id));
        }

        public Result<T, TDTO> GetAll(bool includes)
        {
            var models = _baseRepository.GetAll(includes);
            var res = Result<T, TDTO>.Success();
            res.Models = models.ToList();
            res.ModelDTOs = _mapper.Map<List<TDTO>>(models.ToList());
            return res;
        }

        public bool ModelExists(int id)
        {
            return _baseRepository.ModelExists(id);
        }
    }
}

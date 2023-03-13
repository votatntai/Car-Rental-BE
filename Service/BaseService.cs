using AutoMapper;
using Data;

namespace Service
{
    public class BaseService
    {
        protected IUnitOfWork _unitOfWork;
        protected IMapper _mapper;
        public BaseService(IUnitOfWork unitOfWork, IMapper  mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}

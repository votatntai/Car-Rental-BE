using AutoMapper;
using Data;

namespace Service
{
    public class BaseService
    {
        protected IUnitOfWork _unitOfWork;
        protected IMapper _mapper;
        private IUnitOfWork unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public BaseService(IUnitOfWork unitOfWork, IMapper  mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}

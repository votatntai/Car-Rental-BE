using AutoMapper;
using Data;
using Service;
using Service.Interfaces;
using Utility.Helpers.Models;

public class VNPayService : BaseService, IVNPayService
{
    private new readonly IMapper _mapper;

    public VNPayService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        _mapper = mapper;
    }

    public async Task<bool> AddRequest(VnPayRequestModel model)
    {
        return true;
    }

    public async Task<bool> AddResponse(VnPayResponseModel model)
    {
        return true;
    }
}
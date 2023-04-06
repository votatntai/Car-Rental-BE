﻿using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IFeedBackService
    {
        Task<ListViewModel<FeedBackViewModel>> GetFeedBacks(FeedBackFilterModel filter, PaginationRequestModel pagination);
        Task<FeedBackViewModel> GetFeedBack(Guid id);
        Task<FeedBackViewModel> CreateFeedBack(Guid customerId, FeedBackCreateModel model);
        Task<FeedBackViewModel> UpdateFeedBack(Guid id, FeedBackUpdateModel model);
        Task<bool> DeleteFeedBack(Guid id);
    }
}
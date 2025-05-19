using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface ITestimonialService
{
    Task<IPagedList<TestimonialListItemViewModel>> GetPagedTestimonialsAsync(TestimonialFilterViewModel filter, int pageNumber, int pageSize);

    Task<TestimonialViewModel?> GetTestimonialByIdAsync(int id);

    Task<OperationResult<int>> CreateTestimonialAsync(TestimonialViewModel viewModel);

    Task<OperationResult> UpdateTestimonialAsync(TestimonialViewModel viewModel);

    Task<OperationResult> DeleteTestimonialAsync(int id);
}

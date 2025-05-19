using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class TestimonialIndexViewModel
{
    public IPagedList<TestimonialListItemViewModel> Testimonials { get; set; } = default!;
    public TestimonialFilterViewModel Filter { get; set; } = new();
}
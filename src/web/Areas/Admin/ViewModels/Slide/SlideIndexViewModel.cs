using X.PagedList;

namespace web.Areas.Admin.ViewModels.Slide;

public class SlideIndexViewModel
{
    public IPagedList<SlideListItemViewModel> Slides { get; set; } = default!;
    public SlideFilterViewModel Filter { get; set; } = new();
}
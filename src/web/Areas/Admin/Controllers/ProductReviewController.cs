using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Constants;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = "AdminAccess")]
public partial class ProductReviewController : Controller
{
    private readonly IProductReviewService _productReviewService;
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductReviewController> _logger;
    private readonly IValidator<ProductReviewViewModel> _productReviewViewModelValidator;


    public ProductReviewController(
        IProductReviewService productReviewService,
        IProductService productService,
        IMapper mapper,
        ILogger<ProductReviewController> logger,
        IValidator<ProductReviewViewModel> productReviewViewModelValidator)
    {
        _productReviewService = productReviewService ?? throw new ArgumentNullException(nameof(productReviewService));
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _productReviewViewModelValidator = productReviewViewModelValidator ?? throw new ArgumentNullException(nameof(productReviewViewModelValidator));
    }

    // GET: Admin/ProductReview
    public async Task<IActionResult> Index(ProductReviewFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new ProductReviewFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IPagedList<ProductReviewListItemViewModel> reviewsPaged = await _productReviewService.GetPagedProductReviewsAsync(filter, pageNumber, currentPageSize);

        filter.ProductOptions = await _productService.GetProductSelectListAsync(filter.ProductId);
        filter.StatusOptions = GetReviewStatusSelectList(filter.Status);
        filter.RatingOptions = GetRatingOptions(filter.MinRating, filter.MaxRating);


        ProductReviewIndexViewModel viewModel = new()
        {
            Reviews = reviewsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/ProductReview/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        ProductReviewViewModel? viewModel = await _productReviewService.GetProductReviewByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("ProductReview not found for editing. ID: {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                 new ToastData("Lỗi", "Không tìm thấy đánh giá để cập nhật.", ToastType.Error)
             );
            return RedirectToAction(nameof(Index));
        }

        PopulateViewModelSelectLists(viewModel);

        return View(viewModel);
    }

    // POST: Admin/ProductReview/Edit/5 (Only update status)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductReviewViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await _productReviewViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await _productReviewService.RefillProductReviewViewModelFromDbAsync(viewModel);
            PopulateViewModelSelectLists(viewModel);

            return View(viewModel);
        }

        var updateResult = await _productReviewService.UpdateProductReviewStatusAsync(viewModel.Id, viewModel.Status);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật trạng thái đánh giá thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in updateResult.Errors)
            {
                if (error.Contains("trạng thái", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Status), error);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            if (!updateResult.Errors.Any() && !string.IsNullOrEmpty(updateResult.Message))
            {
                ModelState.AddModelError(string.Empty, updateResult.Message);
            }

            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", updateResult.Message ?? "Đã xảy ra lỗi hệ thống khi cập nhật đánh giá.", ToastType.Error)
            );
            await _productReviewService.RefillProductReviewViewModelFromDbAsync(viewModel);
            PopulateViewModelSelectLists(viewModel);

            return View(viewModel);
        }
    }

    // POST: Admin/ProductReview/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _productReviewService.DeleteProductReviewAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa đánh giá thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Đã xảy ra lỗi hệ thống khi xóa đánh giá.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
    }

    private void PopulateViewModelSelectLists(ProductReviewViewModel viewModel)
    {
        viewModel.StatusOptions = GetReviewStatusSelectList(viewModel.Status);
    }

    private List<SelectListItem> GetReviewStatusSelectList(ReviewStatus? selectedStatus)
    {
        var items = Enum.GetValues(typeof(ReviewStatus))
            .Cast<ReviewStatus>()
            .Select(t => new SelectListItem
            {
                Value = ((int)t).ToString(),
                Text = t.GetDisplayName(),
                Selected = selectedStatus.HasValue && t == selectedStatus.Value
            })
             .OrderBy(t => t.Text)
            .ToList();

        return items;
    }

    private List<SelectListItem> GetRatingOptions(int? minSelected, int? maxSelected)
    {
        var items = new List<SelectListItem>();
        items.Add(new SelectListItem { Value = "", Text = "Tất cả", Selected = !minSelected.HasValue && !maxSelected.HasValue });

        for (int i = 1; i <= 5; i++)
        {
            items.Add(new SelectListItem { Value = i.ToString(), Text = $"{i} sao", Selected = (minSelected == i) });
        }
        return items;
    }
}

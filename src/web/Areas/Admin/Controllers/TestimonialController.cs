using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Constants;
using shared.Enums;
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Roles = "Admin")]
public partial class TestimonialController : Controller
{
    private readonly ITestimonialService _testimonialService;
    private readonly IMapper _mapper;
    private readonly ILogger<TestimonialController> _logger;
    private readonly IValidator<TestimonialViewModel> _testimonialViewModelValidator;

    public TestimonialController(
        ITestimonialService testimonialService,
        IMapper mapper,
        ILogger<TestimonialController> logger,
        IValidator<TestimonialViewModel> testimonialViewModelValidator)
    {
        _testimonialService = testimonialService ?? throw new ArgumentNullException(nameof(testimonialService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _testimonialViewModelValidator = testimonialViewModelValidator ?? throw new ArgumentNullException(nameof(testimonialViewModelValidator));
    }

    // GET: Admin/Testimonial
    public async Task<IActionResult> Index(TestimonialFilterViewModel filter, int page = 1, int pageSize = 10)
    {
        filter ??= new TestimonialFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<TestimonialListItemViewModel> testimonialsPaged = await _testimonialService.GetPagedTestimonialsAsync(filter, pageNumber, currentPageSize);

        filter.StatusOptions = GetStatusOptions(filter.IsActive);
        filter.RatingOptions = GetRatingOptions(filter.Rating);

        TestimonialIndexViewModel viewModel = new()
        {
            Testimonials = testimonialsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Testimonial/Create
    public IActionResult Create()
    {
        TestimonialViewModel viewModel = new()
        {
            IsActive = true,
            Rating = 5,
            OrderIndex = 0
        };
        return View(viewModel);
    }

    // POST: Admin/Testimonial/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TestimonialViewModel viewModel)
    {
        var validationResult = await _testimonialViewModelValidator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var createResult = await _testimonialService.CreateTestimonialAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm đánh giá thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in createResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            if (!createResult.Errors.Any() && !string.IsNullOrEmpty(createResult.Message))
            {
                ModelState.AddModelError(string.Empty, createResult.Message);
            }

            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm đánh giá của '{viewModel.ClientName}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // GET: Admin/Testimonial/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        TestimonialViewModel? viewModel = await _testimonialService.GetTestimonialByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("Testimonial không tồn tại khi chỉnh sửa. ID: {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy đánh giá để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }

    // POST: Admin/Testimonial/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TestimonialViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var validationResult = await _testimonialViewModelValidator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var updateResult = await _testimonialService.UpdateTestimonialAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật đánh giá thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            if (!updateResult.Errors.Any() && !string.IsNullOrEmpty(updateResult.Message))
            {
                ModelState.AddModelError(string.Empty, updateResult.Message);
            }

            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật đánh giá của '{viewModel.ClientName}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // POST: Admin/Testimonial/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _testimonialService.DeleteTestimonialAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa đánh giá thành công.", ToastType.Success)
            );
            return Json(new { success = true, message = deleteResult.Message });
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa đánh giá.", ToastType.Error)
            );
            return Json(new { success = false, message = deleteResult.Message });
        }
    }
}

public partial class TestimonialController
{
    private List<SelectListItem> GetStatusOptions(bool? selectedValue)
    {
        return new List<SelectListItem>
        {
            new() { Value = "", Text = "Tất cả trạng thái", Selected = !selectedValue.HasValue },
            new() { Value = "true", Text = "Đang hiển thị", Selected = selectedValue.HasValue && selectedValue.Value },
            new() { Value = "false", Text = "Đang ẩn", Selected = selectedValue.HasValue && !selectedValue.Value }
        };
    }

    private List<SelectListItem> GetRatingOptions(int? selectedValue)
    {
        var items = new List<SelectListItem>
        {
             new() { Value = "", Text = "Tất cả Rating", Selected = !selectedValue.HasValue }
        };

        items.AddRange(Enumerable.Range(1, 5).Select(i => new SelectListItem
        {
            Value = i.ToString(),
            Text = $"{i} sao",
            Selected = selectedValue.HasValue && selectedValue.Value == i
        }));

        return items;
    }
}

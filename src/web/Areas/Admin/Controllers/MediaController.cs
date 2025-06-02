using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Constants;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = PermissionConstants.AdminAccess)]
public partial class MediaController : Controller
{
    private readonly IMediaService _mediaService;

    public MediaController(
        IMediaService mediaService)
    {
        _mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
    }

    // GET: Admin/Media
    [Authorize(Policy = PermissionConstants.MediaView)]
    public async Task<IActionResult> Index(MediaFilterViewModel filter, int page = 1, int pageSize = 20)
    {
        filter ??= new MediaFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 20;

        IPagedList<MediaFileViewModel> mediaFilesPaged = await _mediaService.GetPagedMediaFilesAsync(filter, pageNumber, currentPageSize);

        filter.MediaTypeOptions = GetMediaTypeSelectList(filter.MediaType, "-- Tất cả loại Media --");

        MediaIndexViewModel viewModel = new()
        {
            MediaFiles = mediaFilesPaged,
            Filter = filter
        };
        return View(viewModel);
    }


    [HttpGet]
    public async Task<IActionResult> SelectMediaModalContent(MediaFilterViewModel filter)
    {
        filter ??= new MediaFilterViewModel();
        var files = await _mediaService.GetFilesForModalAsync(filter.MediaType, filter.SearchTerm, limit: 100);

        var viewModel = new SelectMediaModalViewModel
        {
            Files = files,
            Filter = new MediaFilterViewModel
            {
                MediaType = filter.MediaType,
                SearchTerm = filter.SearchTerm,
                MediaTypeOptions = GetMediaTypeSelectList(filter.MediaType)
            }
        };
        return PartialView("Components/Media/__Media.ModalContent", viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetFiles(MediaType? mediaType, string? searchTerm)
    {
        var files = await _mediaService.GetFilesForModalAsync(mediaType, searchTerm, limit: 100);
        return Json(files);
    }

    [HttpPost]
    [RequestSizeLimit(100 * 1024 * 1024)] // 100 MB
    [Authorize(Policy = PermissionConstants.MediaUpload)]
    public async Task<IActionResult> UploadFile(IFormFile file, string? altText, string? description)
    {
        if (file == null || file.Length == 0)
        {
            return Json(new UploadResponseViewModel { Success = false, Message = "Không có tập tin được tải lên." });
        }

        var result = await _mediaService.UploadFileAsync(file, altText, description);

        if (result.Success && result.Data != null)
        {
            return Json(new UploadResponseViewModel { Success = true, File = result.Data, Message = result.Message });
        }
        else
        {
            return Json(new UploadResponseViewModel { Success = false, Message = result.Message ?? "Lỗi khi tải tập tin." });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = PermissionConstants.MediaDelete)]
    public async Task<IActionResult> DeleteFile(int id)
    {
        var result = await _mediaService.DeleteFileAsync(id);
        if (result.Success)
        {
            return Json(new { success = true, message = result.Message ?? "Đã xóa tập tin thành công." });
        }
        else
        {
            return Json(new { success = false, message = result.Message ?? "Không thể xóa tập tin." });
        }
    }

    // GET: Admin/Media/Edit/5
    [Authorize(Policy = PermissionConstants.MediaEdit)]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var viewModel = await _mediaService.GetFileViewModelByIdAsync(id);
        if (viewModel == null)
        {
            TempData[TempDataConstants.ToastMessage] = System.Text.Json.JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy tập tin.", ToastType.Error));
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }

    // POST: Admin/Media/Edit/5
    // [Authorize(Policy = PermissionConstants.MediaEdit)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MediaFileViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest();
        }

        var result = await _mediaService.UpdateMediaFileDetailsAsync(viewModel);

        if (result.Success)
        {
            TempData[TempDataConstants.ToastMessage] = System.Text.Json.JsonSerializer.Serialize(
                new ToastData("Thành công", result.Message ?? "Cập nhật thông tin tập tin thành công.", ToastType.Success));
            return RedirectToAction(nameof(Index));
        }
        else
        {
            ModelState.AddModelError(string.Empty, result.Message ?? "Lỗi khi cập nhật thông tin tập tin.");
            var originalViewModel = await _mediaService.GetFileViewModelByIdAsync(id);
            if (originalViewModel != null) viewModel.PresignedUrl = originalViewModel.PresignedUrl; else viewModel.PresignedUrl = null;

            return View(viewModel);
        }
    }
}

public partial class MediaController
{
    private List<SelectListItem> GetMediaTypeSelectList(MediaType? selectedType, string defaultOptionText = "-- Chọn loại Media --")
    {
        var items = Enum.GetValues(typeof(MediaType))
            .Cast<MediaType>()
            .Select(t => new SelectListItem
            {
                Value = ((int)t).ToString(),
                Text = t.GetDisplayName(),
                Selected = selectedType.HasValue && t == selectedType.Value
            })
            .ToList();

        items.Insert(0, new SelectListItem { Value = "", Text = defaultOptionText, Selected = !selectedType.HasValue });
        return items;
    }
}
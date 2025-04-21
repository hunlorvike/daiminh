using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.Services;
using web.Areas.Admin.ViewModels.Media;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class MediaController : Controller
{
    private readonly IMediaService _mediaService;
    private readonly IMapper _mapper;
    private readonly ILogger<MediaController> _logger;
    private readonly IValidator<MediaFolderViewModel> _folderValidator;

    public MediaController(
        IMediaService mediaService,
        IMapper mapper,
        ILogger<MediaController> logger,
        IValidator<MediaFolderViewModel> folderValidator)
    {
        _mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _folderValidator = folderValidator ?? throw new ArgumentNullException(nameof(folderValidator));
    }

    public IActionResult Index()
    {
        var viewModel = new MediaIndexViewModel
        {
            Filter = new MediaFilterViewModel { MediaTypeOptions = GetMediaTypeSelectList(null) }
        };
        return View(viewModel);
    }


    [HttpGet]
    public async Task<IActionResult> SelectMediaModalContent(int? folderId, MediaType? mediaTypeFilter = null)
    {
        var breadcrumbs = await _mediaService.GetFolderBreadcrumbsAsync(folderId);
        var folders = await _mediaService.GetChildFoldersAsync(folderId ?? 0);
        var files = await _mediaService.GetFilesByFolderAsync(folderId, mediaTypeFilter);

        var folderVMs = _mapper.Map<List<MediaFolderViewModel>>(folders);
        var fileVMs = _mapper.Map<List<MediaFileViewModel>>(files);

        foreach (var fileVm in fileVMs)
        {
            var fileEntity = await _mediaService.GetFileByIdAsync(fileVm.Id);
            if (fileEntity != null)
            {
                fileVm.PublicUrl = _mediaService.GetFilePublicUrl(fileEntity);
            }
        }


        var viewModel = new SelectMediaModalViewModel
        {
            CurrentFolderId = folderId,
            Breadcrumbs = _mapper.Map<List<MediaFolderViewModel>>(breadcrumbs),
            Folders = folderVMs,
            Files = fileVMs,
            Filter = new MediaFilterViewModel
            {
                MediaType = mediaTypeFilter,
                MediaTypeOptions = GetMediaTypeSelectList(mediaTypeFilter)
            }
        };

        return PartialView("Components/_SelectMediaModalContent", viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetFolders(int? parentId)
    {
        var folders = await _mediaService.GetChildFoldersAsync(parentId ?? 0);
        var folderVMs = _mapper.Map<List<MediaFolderViewModel>>(folders);
        return Json(folderVMs);
    }

    [HttpGet]
    public async Task<IActionResult> GetFiles(int? folderId, MediaType? mediaType)
    {
        var files = await _mediaService.GetFilesByFolderAsync(folderId, mediaType);
        var fileVMs = _mapper.Map<List<MediaFileViewModel>>(files);

        foreach (var fileVm in fileVMs)
        {
            var fileEntity = await _mediaService.GetFileByIdAsync(fileVm.Id);
            if (fileEntity != null)
            {
                fileVm.PublicUrl = _mediaService.GetFilePublicUrl(fileEntity);
            }
        }
        return Json(fileVMs);
    }

    [HttpGet]
    public async Task<IActionResult> GetBreadcrumbs(int? folderId)
    {
        var breadcrumbs = await _mediaService.GetFolderBreadcrumbsAsync(folderId);
        var breadcrumbVMs = _mapper.Map<List<MediaFolderViewModel>>(breadcrumbs);
        return Json(breadcrumbVMs);
    }


    [HttpPost]
    public async Task<IActionResult> CreateFolder([FromBody] MediaFolderViewModel viewModel)
    {
        var validationResult = await _folderValidator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            return Json(new { success = false, errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        try
        {
            var folder = await _mediaService.CreateFolderAsync(viewModel.ParentId, viewModel.Name);
            var folderVM = _mapper.Map<MediaFolderViewModel>(folder);
            return Json(new { success = true, folder = folderVM });
        }
        catch (InvalidOperationException ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating folder.");
            return Json(new { success = false, message = "Đã xảy ra lỗi khi tạo thư mục." });
        }
    }

    [HttpPost]
    [RequestSizeLimit(100 * 1024 * 1024)]
    public async Task<IActionResult> UploadFile(int? folderId, IFormFile file, string? altText, string? description)
    {
        if (file == null || file.Length == 0)
        {
            return Json(new UploadResponseViewModel { Success = false, Message = "Không có tập tin được tải lên." });
        }

        try
        {
            var uploadedFile = await _mediaService.UploadFileAsync(folderId, file, altText, description);
            var fileVM = _mapper.Map<MediaFileViewModel>(uploadedFile);
            fileVM.PublicUrl = _mediaService.GetFilePublicUrl(uploadedFile);
            return Json(new UploadResponseViewModel { Success = true, File = fileVM });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file {FileName}", file.FileName);
            return Json(new UploadResponseViewModel { Success = false, Message = "Đã xảy ra lỗi khi tải tập tin lên." });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFile(int id)
    {
        try
        {
            var success = await _mediaService.DeleteFileAsync(id);
            if (success)
            {
                return Json(new { success = true, message = "Đã xóa tập tin thành công." });
            }
            else
            {
                return Json(new { success = false, message = "Không tìm thấy tập tin." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file ID {FileId}", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa tập tin." });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFolder(int id)
    {
        try
        {
            var success = await _mediaService.DeleteFolderAsync(id);
            if (success)
            {
                return Json(new { success = true, message = "Đã xóa thư mục thành công." });
            }
            else
            {
                return Json(new { success = false, message = "Không thể xóa thư mục vì nó không rỗng." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting folder ID {FolderId}", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa thư mục." });
        }
    }

    private List<SelectListItem> GetMediaTypeSelectList(MediaType? selectedType)
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
        return items;
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
public partial class MediaController : Controller
{
    private readonly IMediaService _mediaService;
    private readonly IMapper _mapper;
    private readonly ILogger<MediaController> _logger;

    public MediaController(
        IMediaService mediaService,
        IMapper mapper,
        ILogger<MediaController> logger)
    {
        _mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    [HttpGet]
    public async Task<IActionResult> SelectMediaModalContent(MediaType? mediaTypeFilter = null)
    {
        var files = await _mediaService.GetFilesAsync(mediaTypeFilter);

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
            Files = fileVMs,
            Filter = new MediaFilterViewModel
            {
                MediaType = mediaTypeFilter,
                MediaTypeOptions = GetMediaTypeSelectList(mediaTypeFilter)
            }
        };

        return PartialView("Components/Media/__Media.ModalContent", viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetFiles(MediaType? mediaType)
    {
        var files = await _mediaService.GetFilesAsync(mediaType);
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


    [HttpPost]
    [RequestSizeLimit(100 * 1024 * 1024)]
    public async Task<IActionResult> UploadFile(IFormFile file, string? altText, string? description)
    {
        if (file == null || file.Length == 0)
        {
            return Json(new UploadResponseViewModel { Success = false, Message = "Không có tập tin được tải lên." });
        }

        try
        {
            var uploadedFile = await _mediaService.UploadFileAsync(file, altText, description);
            var fileVM = _mapper.Map<MediaFileViewModel>(uploadedFile);
            fileVM.PublicUrl = _mediaService.GetFilePublicUrl(uploadedFile);
            return Json(new UploadResponseViewModel { Success = true, File = fileVM });
        }
        catch (Exception)
        {
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
        catch (Exception)
        {
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa tập tin." });
        }
    }

}

public partial class MediaController
{

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
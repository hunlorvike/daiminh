using core.Common.Constants;
using core.Common.Extensions;
using Core.Common.Models;
using core.Entities;
using core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Models.Tag;
using web.Areas.Admin.Requests.Tag;

namespace web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public partial class TagController : Controller
    {
        private readonly TagService _tagService;
        private readonly IServiceProvider _serviceProvider;

        public TagController(TagService tagService, IServiceProvider serviceProvider)
        {
            _tagService = tagService;
            _serviceProvider = serviceProvider;
        }

        // Helper method for getting the validator
        private IValidator<T> GetValidator<T>() where T : class
            => _serviceProvider.GetRequiredService<IValidator<T>>();
    }

    // Controller actions for Tag management (Create, Edit, Delete, etc.)
    public partial class TagController
    {
        // GET: Admin/Tag
        public async Task<IActionResult> Index()
        {
            List<Tag> response = await _tagService.GetAllAsync();
            List<TagViewModel> viewModels = response.Select(r => new TagViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Slug = r.Slug,
                CreatedAt = r.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                UpdatedAt = r.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                DeletedAt = r.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") // Nullable DeletedAt
            }).ToList();
            return View(viewModels);
        }

        // GET: Admin/Tag/Create
        public IActionResult Create()
        {
            return PartialView("_Create.Modal", new TagRequest());
        }

        // GET: Admin/Tag/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Tag? response = await _tagService.GetByIdAsync(id);
            if (response == null) return NotFound();
            TagRequest request = new()
            {
                Id = response.Id,
                Name = response.Name,
                Slug = response.Slug,
            };
            return PartialView("_Edit.Modal", request);
        }

        // GET: Admin/Tag/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Tag? response = await _tagService.GetByIdAsync(id);
            if (response == null) return NotFound();
            TagDeleteRequest request = new()
            {
                Id = response.Id,
            };
            return PartialView("_Delete.Modal", request);
        }
    }

    // Controller actions for Post Requests (Create, Edit, Delete)
    public partial class TagController
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagRequest model)
        {
            var validator = GetValidator<TagRequest>();
            if (await this.ValidateAndReturnView(validator, model))
            {
                return PartialView("_Create.Modal", model);
            }

            try
            {
                Tag newTag = new Tag()
                {
                    Name = model.Name ?? string.Empty,
                    Slug = model.Slug ?? string.Empty,
                    CreatedAt = DateTime.UtcNow, // Set CreatedAt timestamp
                    UpdatedAt = DateTime.UtcNow // Set UpdatedAt timestamp
                };

                var response = await _tagService.AddAsync(newTag);

                switch (response)
                {
                    case SuccessResponse<Tag> successResponse:
                        ViewData["SuccessMessage"] = successResponse.Message;
                        return RedirectToAction("Index", "Tag", new { area = "Admin" });
                    case ErrorResponse errorResponse:
                        foreach (var error in errorResponse.Errors) ModelState.AddModelError(error.Key, error.Value);
                        return PartialView("_Create.Modal", model);
                    default:
                        return PartialView("_Create.Modal", model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_Create.Modal", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagRequest model)
        {
            var validator = GetValidator<TagRequest>();
            if (await this.ValidateAndReturnView(validator, model))
            {
                return PartialView("_Edit.Modal", model);
            }

            try
            {
                Tag updateTag = new Tag()
                {
                    Name = model.Name ?? string.Empty,
                    Slug = model.Slug ?? string.Empty,
                    UpdatedAt = DateTime.UtcNow // Set UpdatedAt timestamp on edit
                };

                var response = await _tagService.UpdateAsync(model.Id, updateTag);

                switch (response)
                {
                    case SuccessResponse<Tag> successResponse:
                        ViewData["SuccessMessage"] = successResponse.Message;
                        return RedirectToAction("Index", "Tag", new { area = "Admin" });
                    case ErrorResponse errorResponse:
                        foreach (var error in errorResponse.Errors)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }

                        return PartialView("_Edit.Modal", model);
                    default:
                        return PartialView("_Edit.Modal", model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_Edit.Modal", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TagDeleteRequest model)
        {
            try
            {
                var response = await _tagService.DeleteAsync(model.Id);

                switch (response)
                {
                    case SuccessResponse<Tag> successResponse:
                        ViewData["SuccessMessage"] = successResponse.Message;
                        return RedirectToAction("Index", "Tag", new { area = "Admin" });
                    case ErrorResponse errorResponse:
                        foreach (var error in errorResponse.Errors)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }

                        return PartialView("_Delete.Modal", model);
                    default:
                        return PartialView("_Delete.Modal", model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_Delete.Modal", model);
            }
        }
    }
}

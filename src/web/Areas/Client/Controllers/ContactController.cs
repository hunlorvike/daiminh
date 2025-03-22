using AutoMapper;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Client.Requests.Contact;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("/lien-he")]
public partial class ContactController(
    ApplicationDbContext context,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache);

public partial class ContactController
{
    public IActionResult Index()
    {
        return View();
    }
}

public partial class ContactController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContactCreateRequest model)
    {
        var validator = GetValidator<ContactCreateRequest>();

        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var contact = _mapper.Map<Contact>(model);
            await context.Contacts.AddAsync(contact);
            await context.SaveChangesAsync();

            var successResponse = new SuccessResponse<Contact>(contact, "Thêm liên hệ thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Contact", new { area = "Client" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Contact", new { area = "Client" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error creating contact.", ex);
        }
    }
}
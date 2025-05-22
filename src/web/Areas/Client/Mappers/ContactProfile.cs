using AutoMapper;
using domain.Entities;
using web.Areas.Client.ViewModels;

namespace web.Areas.Client.Mappers;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        // ViewModel -> Entity (For Create POST)
        CreateMap<ContactViewModel, Contact>();
    }
}
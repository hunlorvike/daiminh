using core.Entities;
using core.Interfaces;

namespace core.Services;

public class ContactService(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Contact> GetContactByIdAsync(int id)
    {
        var contactRepository = _unitOfWork.GetRepository<Contact, int>();
        return await contactRepository.FindByIdAsync(id);
    }

    public async Task CreateContactAsync(Contact contact)
    {
        var contactRepository = _unitOfWork.GetRepository<Contact, int>();
        await contactRepository.AddAsync(contact);
        await _unitOfWork.SaveChangesAsync();
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMSApi.Core.Interfaces;
using CMSApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CMSApi.DAL;

namespace CMSApi.DAL.Repositories

{

    public class ContactRepository : IContactRepository
    {
        private readonly MyAppDbContext _context;

        public ContactRepository(MyAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task<Contact> GetByIdAsync(int contactId)
        {
            return await _context.Contacts.FindAsync(contactId);
        }

        public async Task AddAsync(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contact contact)
        {
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Contact contact)
        {
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }
    }

}

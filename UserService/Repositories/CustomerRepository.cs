using Core.BaseImplementations;
using Database.BaseImplementations;
using Microsoft.EntityFrameworkCore;
using UserService.Entities;
using UserService.Repositories.EF;
using UserService.Repositories.Interfaces;

namespace UserService.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(UserServiceContext dbContext) : base(dbContext)
        {

        }
    }
}

using TMP_API.Context;
using TMP_API.Models.Customers;
using TMP_API.Repository.IRepository;

namespace TMP_API.Repository;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }
}

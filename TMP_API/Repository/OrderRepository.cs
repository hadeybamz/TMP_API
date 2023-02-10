using TMP_API.Data;
using TMP_API.Entities;
using TMP_API.Repository.IRepository;

namespace TMP_API.Repository;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }
}

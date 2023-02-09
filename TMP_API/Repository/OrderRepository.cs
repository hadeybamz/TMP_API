using TMP_API.Context;
using TMP_API.Models.Orders;
using TMP_API.Repository.IRepository;

namespace TMP_API.Repository;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }
}

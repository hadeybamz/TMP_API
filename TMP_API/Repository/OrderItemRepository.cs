using TMP_API.Data;
using TMP_API.Entities;
using TMP_API.Repository.IRepository;

namespace TMP_API.Repository;

public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }
}

using TMP_API.Data;
using TMP_API.Entities;
using TMP_API.Repository.IRepository;

namespace TMP_API.Repository;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

}

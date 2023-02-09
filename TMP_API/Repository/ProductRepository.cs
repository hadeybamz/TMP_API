using TMP_API.Context;
using TMP_API.Models.Products;
using TMP_API.Repository.IRepository;

namespace TMP_API.Repository;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

}

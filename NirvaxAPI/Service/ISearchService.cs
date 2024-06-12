using BusinessObject.Models;

namespace WebAPI.Service
{
    public interface ISearchService
    {
        Task<(IEnumerable<Owner> owners, IEnumerable<Product> products)> SearchAsync(string searchTerm, double? minPrice = null, double? maxPrice = null, int? categoryId = null, int? brandId = null, int? sizeId = null);
    }
}

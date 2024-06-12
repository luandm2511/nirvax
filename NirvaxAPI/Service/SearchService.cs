using BusinessObject.Models;
using DataAccess.IRepository;

namespace WebAPI.Service
{
    public class SearchService : ISearchService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOwnerRepository _ownerRepository;

        public SearchService(IProductRepository productRepository, IOwnerRepository ownerRepository)
        {
            _productRepository = productRepository;
            _ownerRepository = ownerRepository;
        }

        public async Task<(IEnumerable<Owner> owners, IEnumerable<Product> products)> SearchAsync(string? searchTerm, double? minPrice = null, double? maxPrice = null, int? categoryId = null, int? brandId = null, int? sizeId = null)
        {
            var ownersTask = _ownerRepository.SearchOwnersAsync(searchTerm);
            var productsTask = _productRepository.SearchProductsAsync(searchTerm, minPrice, maxPrice, categoryId, brandId, sizeId);

            await Task.WhenAll(ownersTask, productsTask);

            var owners = await ownersTask;
            var products = await productsTask;

            return (owners, products);
        }
    }
}

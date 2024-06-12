using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Service;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? searchTerm, double? minPrice = null, double? maxPrice = null, int? categoryId = null, int? brandId = null, int? sizeId = null)
        {
            var results = await _searchService.SearchAsync(searchTerm, minPrice, maxPrice, categoryId, brandId, sizeId);
            return Ok(results);
        }
    }
}

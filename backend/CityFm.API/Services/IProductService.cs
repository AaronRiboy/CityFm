using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityFm.API.Models;

namespace CityFm.API.Services
{
    public interface IProductService
    {
        Task<List<ProductViewModel>> GetProductsAsync(CancellationToken ct);
    }
}
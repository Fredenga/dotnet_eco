using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.DTO.Conversions
{
    public static class ProductConversion
    {
        public static Product ToEntity(ProductDTO productDTO) => new()
        {
            Id = productDTO.Id,
            Name = productDTO.Name,
            Quantity = productDTO.Quantity,
            Price = productDTO.Price
        };

        public static IEnumerable<ProductDTO>? FromEntity(Product product, IEnumerable<Product> products)

        {
            if (product is not null || products is null)
            {
                var _products = products.Select(p =>
                
                    new ProductDTO(p.Id, p.Name, p.Quantity, p.Price)
                ).ToList();
                return  _products;
            }
            return null;
        }
    }
}

using ProductApi.Application.Interfaces;
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

        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<Product> products)
        {
            // Case 1: If product is null or products is not empty, return products as a collection of ProductDTOs
            if (products != null && !products.Any())
            {
                var productDTOs = products.Select(p => new ProductDTO(p.Id, p.Name!, p.Quantity, p.Price));
                return (null, productDTOs);
            }

            // Case 2: If product is not null, return a single ProductDTO and null for the product list
            if (product != null)
            {
                var productDTO = new ProductDTO(product.Id, product.Name!, product.Quantity, product.Price);
                return (productDTO, null);
            }

            // Case 3: If both product and products are null, return (null, null)
            return (null, null);
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTO;
using ProductApi.Application.DTO.Conversions;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using SharedLib.Responses;
using System.Reflection.Metadata.Ecma335;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductsController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            //get all products from repo
            var products = await productInterface.GetAllAsync();
            if (!products.Any())
            {
                return NotFound("No products detected in the database");
            }
            //convert data from entity to DTO and return it
            return Ok(products);
            //var (_, list) = ProductConversion.FromEntity(null!, products);
            //return list!.Any() ? Ok(list) : NotFound("No product found");
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            //get single product from the repo
            var product = await productInterface.FindByIDAsync(id);
            if (product is null)
            {
                return NotFound("Product requested not found");
            }
            
            return Ok(product);
            
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO product)
        {
            //check model state if all data annotations are passed
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //convert to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.CreateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO product)
        {
            //check model state if all data annotations are passed
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //convert to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.UpdateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO product)
        {
            //convert to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.DeleteAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }

}

using OrderApi.Application.DTOs;
using System.ComponentModel.DataAnnotations;
namespace OrderApi.Application.DTOs
{
    public record OrderDetailDTO
    (
        int ID,
        [Required, Range(1, int.MaxValue)] int ProductID,
        [Required, Range(1, int.MaxValue)] int ClientID,
        [Required, EmailAddress] string Email,
        [Required] string ClientName,
        [Required] string ProductName,
        [Required] string Address,
        [Required] string Telephone,
        [Required, Range(1, int.MaxValue)] int PurchaseQuantity,
        [Required, DataType(DataType.Currency)] decimal UnitPrice,
        [Required, DataType(DataType.Currency)] decimal TotalPrice,
        DateTime OrderDate
    );
}

                
                
                

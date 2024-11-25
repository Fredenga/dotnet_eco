using System.ComponentModel.DataAnnotations;
namespace OrderApi.Application.DTOs
{
    public record OrderDTO
    (
        int ID,
        [Required, Range(1, int.MaxValue)] int ProductID,
        [Required, Range(1, int.MaxValue)] int ClientID,
        [Required, Range(1, int.MaxValue)] int PurchaseQuantity,
        DateTime OrderDate
    );
}

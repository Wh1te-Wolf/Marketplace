using Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ProductCartService.DTO
{
    public class ProductCartItemDTO
    {
        public Guid UUID { get; set; }

        public Guid ProductUUID { get; set; }

        public int Quantity { get; set; }

        public bool Included { get; set; }

    }
}

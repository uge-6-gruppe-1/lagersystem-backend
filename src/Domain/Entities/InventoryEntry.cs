using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entities
{
    [PrimaryKey(nameof(ProductId), nameof(LocationId))]
    public class InventoryEntry
    {
        public Guid ProductId { get; set; }

        public Guid LocationId { get; set; }

        [Required]
        public int Quantity { get; set; } = 0;

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } = null!;

        [ForeignKey(nameof(LocationId))]
        public virtual Location Location { get; set; } = null!;
    }
}
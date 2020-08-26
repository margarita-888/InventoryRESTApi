using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryRESTApi.Models
{
    public class InventoryItem
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "nvarchar(35)")]
        [StringLength(35)]
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100)]
        [Required]
        public string Description { get; set; }

        [Range(typeof(double), "1", "5000")]
        [Required]
        public double Price { get; set; }

        [Range(typeof(double), "0", "5000")]
        [Required]
        public double DeliveryPrice { get; set; }

        public List<InventoryItemOption> InventoryItemOptions { get; set; }

        public override string ToString()
        {
            return $"ItemId: {Id}, Name: {Name}, Description: {Description}, Price: {Price}, Shipping: {DeliveryPrice}";
        }
    }
}
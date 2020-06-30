using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsRESTApi.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        [Column(TypeName = "nvarchar(9)")]
        [StringLength(9)]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(23)")]
        [StringLength(23)]
        public string Description { get; set; }

        public Product Product { get; set; }
    }
}
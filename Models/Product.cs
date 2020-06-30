using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsRESTApi.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        [Column(TypeName = "nvarchar(17)")] // To create an nvarchar type of lenght 17 in the db
        [StringLength(17)] // Provides input validation
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(35)")]
        [StringLength(35)]
        public string Description { get; set; }

        [Range(typeof(decimal), "1", "5000")]
        public decimal Price { get; set; }

        [Range(typeof(decimal), "0", "5000")]
        public decimal DeliveryPrice { get; set; }

        public List<ProductOption> ProductOptions { get; set; }
    }
}
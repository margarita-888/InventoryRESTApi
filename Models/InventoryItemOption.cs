﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryRESTApi.Models
{
    public class InventoryItemOption
    {
        public Guid Id { get; set; }

        public Guid InventoryItemId { get; set; }

        [Column(TypeName = "nvarchar(35)")]
        [StringLength(35)]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100)]
        public string Description { get; set; }

        public InventoryItem InventoryItem { get; set; }
    }
}
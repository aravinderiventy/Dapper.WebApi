﻿using System.ComponentModel.DataAnnotations;

namespace Dapper.Core
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Barcode { get; set; }
        public string? Description { get; set; }
        public decimal Rate { get; set; }
        public DateTime AddedOn { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
    }
}
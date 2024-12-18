﻿namespace Backend.Application.DTOs
{
    public class ProductUpdate
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public List<int> SupplierIds { get; set; }
    }
}
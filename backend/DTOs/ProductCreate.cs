﻿namespace backend.DTOs
{
    public class ProductCreate
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
    }
}

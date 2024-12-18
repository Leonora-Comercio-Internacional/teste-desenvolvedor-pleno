namespace backend.DTOs
{
    public class ProductUpdate
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public List<int> SupplierId { get; set; }
    }
}

namespace Backend.Application.DTOs
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }
        public List<int> SupplierId { get; set; } = new List<int>();
        public DateTime DateAdded { get; set; }
    }
}

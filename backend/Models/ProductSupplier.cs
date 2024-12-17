namespace backend.Models;

public class ProductSupplier
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int SupplierId { get; set; }
    public DateTime DateAdded { get; set; }
}

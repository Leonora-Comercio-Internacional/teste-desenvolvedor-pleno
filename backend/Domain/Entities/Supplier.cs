namespace Backend.Models;

public class Supplier
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CNPJ { get; set; }
    public string? Telephone { get; set; }
    public string? Address { get; set; }
    public DateTime DateAdded { get; set; }
}

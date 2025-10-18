using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Required]
    [Range(0.0, 99999.99)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [StringLength(2048)]
    [DataType(DataType.ImageUrl)]
    public string? ImageUrl { get; set; }
}
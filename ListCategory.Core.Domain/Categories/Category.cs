using System.ComponentModel.DataAnnotations;

namespace ListCategory.Core.Domain.Categories
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "The name must be 20 characters")]
        public string Name { get; set; }
        [Range(0, 100, ErrorMessage = "The number must be between 0 to 100")]
        public int DisplayOrder { get; set; }
    }
}

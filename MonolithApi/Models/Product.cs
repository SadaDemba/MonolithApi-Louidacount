using System.ComponentModel.DataAnnotations;

namespace MonolithApi.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public int ShopId { get; set; }

        [Required]
        public int ProductTypeId { get; set; }

        [Required]
        public string Name { get; set; } = String.Empty;

        [Required]
        public int Stock {  get; set; }

        [Required]
        public double Price { get; set; }


        [MaxLength(100, ErrorMessage = "Product description's max length is 100!")]
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date of last update
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        public Product()
        {

        }


        public virtual ProductType? ProductType { get; set; }
        public virtual Shop? Shop { get; set; }
        public virtual ICollection<ProductReduction>? ProductReductions { get; set; }

    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MonolithApi.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class ProductType
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Name of the subscriptionType
        /// </summary>
        [Required(ErrorMessage = "Product type's name is required")]
        public string Name { get; set; } = String.Empty;


        [MaxLength(500, ErrorMessage ="Product type Description's max length is 700!")]
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date of last update
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        public ProductType()
        {}

        /// <summary>
        /// For the relation between product and this class
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }
    }
}

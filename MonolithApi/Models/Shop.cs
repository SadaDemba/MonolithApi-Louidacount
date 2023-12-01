using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace MonolithApi.Models
{
    [Index(nameof(ShopName), IsUnique = true)]
    public class Shop
    {
        [Key]
        public int ShopId { get; set; }

        [Required]
        public string ShopName { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Shop description's max length is 500!")]
        public string ShopDescription { get; set; } = String.Empty;

        [Required]
        public string OwnerId { get; set; } = String.Empty;

        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date of last update
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        public Shop()
        {

        }

        /// <summary>
        /// For the relation between product and this class
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }

        [JsonIgnore]
        public virtual ICollection<Reduction>? Reductions { get; set; }
    }
}

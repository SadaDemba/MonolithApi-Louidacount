using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MonolithApi.Models
{
    public class ProductReduction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int ReductionId { get; set; }
        [Required]
        public bool IsActivated { get; set; }


        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date of last update
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public virtual Product? Product { get; set; }

        
        public virtual Reduction? Reduction { get; set; }
    }
}

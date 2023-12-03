using MonolithApi.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;

namespace MonolithApi.Models
{
    public class Reduction
    {
        [Key]
        public int Id { get; set; }

        public int ShopId { get; set; }

        [Required]
        public string Title { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// The percentage of reduction
        /// </summary>
        [Range(0.0, 100.0)]
        public double Percentage { get; set; }

        /// <summary>
        /// Beginning of the reduction. Here we dont need the year of reduction and we will not use it.
        /// But I have'nt yet found a solution on which I will have a type that will only have day, month and time.
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// End of the the reduction. Same problem than BeginDate!
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date of last update
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Method which return true if the reduction is currently active and false otherwise
        /// </summary>
        [NotMapped]
        public bool Status
        {
            get
            {
                DateTime date = DateTime.UtcNow;

                return (date.Month >= BeginDate.Month && date.Month <= EndDate.Month) &&
                    (date.Day >= BeginDate.Day && date.Day <= EndDate.Day) &&
                    (date.TimeOfDay >= BeginDate.TimeOfDay && date.TimeOfDay <= EndDate.TimeOfDay);
            }
        }

        public Reduction()
        {
            
        }

        //__________________Relations______________________

        
        public virtual Shop? Shop { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductReduction>? ProductReductions { get; set; }
    }
}

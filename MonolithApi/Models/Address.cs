using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonolithApi.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        public int StreetNumber { get; set; }

        [Required]
        public String Street { get; set; } = string.Empty;

        [Required]
        public String City { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;
        public int PostalCode { get; set; }

        /// <summary>
        /// Method which return the full address well formatted
        /// </summary>
        [NotMapped]
        public string FullAddress
        {
            get { return $"{StreetNumber} {Street}, {PostalCode} {City}, {Country}"; }
        }

        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date of last update
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        public Address()
        {

        }

    }
}

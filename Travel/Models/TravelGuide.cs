using System.ComponentModel.DataAnnotations;

namespace Travel.Models
{
    public class TravelGuide
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DestinationName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Activities { get; set; } = string.Empty;

    }
}

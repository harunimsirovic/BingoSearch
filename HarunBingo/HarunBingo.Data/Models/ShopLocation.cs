using System.ComponentModel.DataAnnotations;

namespace HarunBingo.Models
{
    public class ShopLocation
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string DistanceInKm { get; set; }
        [MaxLength(50)]
        public string GeoCoordinates { get; set; }

    }
}

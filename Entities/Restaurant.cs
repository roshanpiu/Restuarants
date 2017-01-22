using System.ComponentModel.DataAnnotations;

namespace WebApplication.Entities
{
    public enum CuisineType
    {
        None,
        Italian,
        French,
        SriLankan,
        American
    }
    public class Restaurant
    {
        public int Id { get; set; }

        [Required, MaxLength(80)]
        [DataType(DataType.Text)]
        [Display(Name="Restaurant Name")]
        public string Name { get; set; }
        public CuisineType Cuisine { get; set; }
    }
}
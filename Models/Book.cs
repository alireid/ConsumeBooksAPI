using System.ComponentModel.DataAnnotations;

namespace ConsumeBooksAPI.Models {
    public class Book
    {

        [Display (Name = "Id")]
        [Key]
        public int id { get; set; }

        [Required]
        [Display (Name = "Title")]
        public string title { get; set; }

        [Required]
        [Display (Name = "Author")]
        public string author { get; set; }

        [Required]
        [Display (Name = "Description")]
        public string description { get; set; }
    }
}
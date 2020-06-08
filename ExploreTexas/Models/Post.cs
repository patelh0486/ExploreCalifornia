using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExploreTexas.Models
{
    public class Post
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [MinLength(1, ErrorMessage ="Error!")]
        private string _key;

        public string Key
        {
            get
            {    
                return _key;
            }
            set
            {
                if (_key == null)
                {
                    _key = Regex.Replace(Title.ToLower(), "[^a-z0-9]", "-");
                }
                _key = value;
            }
        }

        [Display(Name = "Post Title")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength =5, 
            ErrorMessage ="Title must be between 5 and 100 characters long")]
        public string Title { get; set; }
        public string Author { get; set; }

        [Required]
        [MinLength(5, ErrorMessage ="blog-posts should be atleast 100 characters long")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
        public DateTime Posted { get; set; }
       
        [DisplayName("Image Name")]
        public string ImageName { get; set; }
       
        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile ImageUpload { get; set; } 
    }
}

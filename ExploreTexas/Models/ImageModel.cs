using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreTexas.Models
{
    public class ImageModel
    {
        [Key]
        public int Imageid { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; }
        [Column(TypeName ="nvarchar(100)")]
        public string Imagename { get; set; }
    }
}

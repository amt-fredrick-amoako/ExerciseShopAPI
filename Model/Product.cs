using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exercise.Model
{
    public class Product
    {
        [Required]
        public string Name { get; set; }
        [Range(1, 10000)]
        public int Weight { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }
}

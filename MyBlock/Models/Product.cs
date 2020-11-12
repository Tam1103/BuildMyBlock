using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TechWebsite.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public bool Status { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public bool Featured { get; set; }
        public virtual Category Category { get; set; }

        public virtual IEnumerable<Photo> Photos { get; set; }

    }
}

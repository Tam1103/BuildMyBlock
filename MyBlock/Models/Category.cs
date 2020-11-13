using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlock.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Status { get; set; }

        public int? ParentId { get; set; }

        public virtual Category Parent { get; set; }

        public virtual IEnumerable<Category> InverseParents { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }
        
    }
}

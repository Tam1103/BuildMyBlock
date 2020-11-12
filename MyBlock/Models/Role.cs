using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TechWebsite.Models
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<RoleAccount> RoleAccounts { get; set; }
    }
}

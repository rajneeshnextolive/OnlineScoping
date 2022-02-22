using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class Permissions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PermissionsId { get; set; }
        public string Name { get; set; }
        public string Tyepe { get; set; }
        public bool Toggle { get; set; }
    }
}

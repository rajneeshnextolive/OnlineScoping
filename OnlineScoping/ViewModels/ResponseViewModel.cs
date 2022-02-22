using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class ResponseViewModel
    {
        public Guid ResponseId { get; set; }
        public Guid ResponseOptionId { get; set; }
        public DateTime ResponseDateTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class PermissionsViewModel
    {
        public Guid PermissionsId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Tyepe { get; set; }
        public bool Toggle { get; set; }
        public List<PermissionsViewModel> PermissionsList { get; set; }

    }
}

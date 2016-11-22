using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormUserRole.Models.ViewModels
{
    public class UserRoleViewModel
    {
        public User User { get; set; }
        public Role Role { get; set; }

        public bool isSelected { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoharSang.Models.vmModel
{
    public class vmlistuser
    {
        public List<ListUsers> listUsers { get; set; }
        public List<Role> listrole { get; set; }

    }

    public class ListUsers
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserRole userrole { get; set; }
    }


}
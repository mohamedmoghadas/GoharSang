using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoharSang.Models.vmModel
{
    public class vmlistuser
    {
        public List<Users> listUsers { get; set; }

        public Users user { get; set; }

        public List<Role> listrole { get; set; }
        public List<UserRole> userrole { get; set; }

        public List<Store> listStore { get; set; }
        public List<UserStoreRole> listUserStoreRole { get; set; }

    }




}
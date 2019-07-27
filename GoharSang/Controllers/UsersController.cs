using GoharSang.Models;
using GoharSang.Models.SelectedProps;
using GoharSang.Models.vmModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GoharSang.Controllers
{
    public class UsersController : Controller
    {
        GoharSangEntities db = new GoharSangEntities();

        public ActionResult Index()
        {
            var result = GetListUser();
            return View(result);
        }

        private object GetListUser()
        {
            List<Role> listrole = db.Role.ToList();

            var listuser = getUsers();

            vmlistuser _vmlistuser = new vmlistuser();
            _vmlistuser.listrole = listrole;
            _vmlistuser.listUsers = listuser;

            return _vmlistuser;
        }


        public List<Users> getUsers()
        {
            List<Users> listuser = db.Users.Where(p=>p.StateDelete==0).ToList();


            return listuser;
        }

        [HttpPost]
        public async Task<ActionResult> mgnUser(Users user, ItemPropSelect prop)
        {
            var pass = CreatHash.HashPass(user.Password);
            if (user.Id == 0)
            {
                user.StateDelete = 0;
                user.Password = pass;
                db.Users.Add(user);
                await db.SaveChangesAsync();

                List<UserRole> _listprops = new List<UserRole>();
                UserRole _p = null;

                foreach (var item in prop.ListProps)
                {
                    _p = new UserRole();
                    _p.IdUser = user.Id;
                    _p.IdRole = item.Id;

                    _listprops.Add(_p);
                }
                db.UserRole.AddRange(_listprops);
                await db.SaveChangesAsync();
                return Json("Ok", JsonRequestBehavior.AllowGet);

            }
            else
            {
                var deleteprops = db.UserRole.Where(p => p.IdUser == user.Id);
                db.UserRole.RemoveRange(deleteprops);
                await db.SaveChangesAsync();

                Users eUser = db.Users.Find(user.Id);

                eUser.StateDelete = 0;
                eUser.Password = pass;


                eUser.Email = eUser.Email;
                eUser.FullName = eUser.FullName;
                eUser.UserName = eUser.UserName;
               


                await db.SaveChangesAsync();

                List<UserRole> _listprops = new List<UserRole>();
                UserRole _p = null;

                foreach (var item in prop.ListProps)
                {
                    _p = new UserRole();
                    _p.IdUser =eUser.Id;
                    _p.IdRole = item.Id;

                    _listprops.Add(_p);
                }
                db.UserRole.AddRange(_listprops);
                await db.SaveChangesAsync();


                return Json("Ok", JsonRequestBehavior.AllowGet);

            }
        }


        [HttpPost]
        public async Task<ActionResult> DeleteUser(long id)
        {
            Users eUser = db.Users.Find(id);

            eUser.StateDelete = 1;


            var deleteprops = db.UserRole.Where(p => p.IdUser == eUser.Id);
            db.UserRole.RemoveRange(deleteprops);
            await db.SaveChangesAsync();


            return Json("Ok", JsonRequestBehavior.AllowGet);
        }
    }
}
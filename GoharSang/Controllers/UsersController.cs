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
            try
            {
                string UserIdcookie = "";
                if (Request.Cookies.AllKeys.Contains("UserId"))
                {
                    UserIdcookie = Request.Cookies["UserId"].Value;
                    string _Id = UserIdcookie;
                    long Id = Convert.ToInt16(CreatHash.Decrypt(_Id));
                    Users admin = db.Users.FirstOrDefault(p => p.Id == Id);
                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                        var result = GetListUser();
                        return View(result);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "LogIn");

                }
            }
            catch (Exception ee)
            {
                return RedirectToAction("Index", "LogIn");

            }

           
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


        [HttpPost]
        public ActionResult EditUser(long? id)
        {

            try
            {
                string UserIdcookie = "";
                if (Request.Cookies.AllKeys.Contains("UserId"))
                {
                    UserIdcookie = Request.Cookies["UserId"].Value;
                    string _Id = UserIdcookie;
                    long Id = Convert.ToInt16(CreatHash.Decrypt(_Id));
                    Users admin = db.Users.FirstOrDefault(p => p.Id == Id);
                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                        if (id!=null && id!=0)
                        {
                            Users user = db.Users.Find(id);
                            List<Role> listrole = db.Role.ToList();
                            List<UserRole> listuserrole = db.UserRole.ToList();
                            List<Store> listStore = db.Store.Where(p=>p.StateDelete==0).ToList();
                            List<UserStoreRole> listUserStoreRole = db.UserStoreRole.ToList();



                            vmlistuser _vmlistuser = new vmlistuser();
                            _vmlistuser.listrole = listrole;
                            _vmlistuser.user = user;
                            _vmlistuser.userrole = listuserrole;

                            _vmlistuser.listStore = listStore;
                            _vmlistuser.listUserStoreRole = listUserStoreRole;



                            return View(_vmlistuser);
                        }
                        else
                        {
                            return RedirectToAction("Index", "LogIn");

                        }

                    }
                }
                else
                {
                    return RedirectToAction("Index", "LogIn");

                }
            }
            catch (Exception ee)
            {
                return RedirectToAction("Index", "LogIn");

            }

          
        }

        public List<Users> getUsers()
        {
            List<Users> listuser = db.Users.Where(p=>p.StateDelete==0).ToList();


            return listuser;
        }

        [HttpPost]
        public async Task<ActionResult> mgnUser(Users user, ItemPropSelect prop,long? idstore)
        {
            var pass = CreatHash.HashPass(user.Password);
            if (user.Id == 0)
            {
                user.StateDelete = 0;
                user.StateAdmin = false;
                user.Password = pass;
                db.Users.Add(user);
                await db.SaveChangesAsync();

                List<UserRole> _listprops = new List<UserRole>();
                UserRole _p = null;
               


                if (idstore!=null && idstore!=0)
                {

                    UserStoreRole usst = new UserStoreRole();
                    Store store = db.Store.Find(idstore);

                    usst.IdStore = store.Id;

                    usst.IdUser = user.Id;

                    db.UserStoreRole.Add(usst);
                    await db.SaveChangesAsync();

                }

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
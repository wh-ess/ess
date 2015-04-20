using ESS.Domain.JXC;
using ESS.Domain.JXC.Models;
using ESS.Framework.Common;
using ESS.Framework.Common.Utilities;
using ESS.Framework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ESS_ERPContext db = new ESS_ERPContext();
        public ActionResult Index()
        {
            log.Info(ClientIP.GetClientIP());
            SystemConfig sc = new SystemConfig();      
            ViewBag.LoginTitle = sc.GetConfigValue("LoginTitle");
            ViewBag.CopyRight = sc.GetConfigValue("CopyRight");
            return View();
        }

        #region login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            bool ret = false;
            User user = db.Users.Include("Roles").FirstOrDefault(c =>  c.UserName == username);
            if (user != null)
            {
                if (user.Password == password)
                {
                    ret = true;
                    CurrentUserInfo.CurrentUserId = Convert.ToInt32(user.UserId);
                    CurrentUserInfo.CurrentUserName = user.UserName;
                    var role = user.Roles.First();
                    CurrentUserInfo.CurrentRoleId = role.RoleId;
                    CurrentUserInfo.CurrentRoleName = role.RoleName;
                    CurrentUserInfo.CurrentLastLoginTime = DateTime.Now;
                }
            }
            return new Result(ret);

        }
        #endregion

        [Log]
        public ActionResult Main()
        {
            //判定直接登录，未记录当前用户认为非法入侵，回到登录页面
            if (CurrentUserInfo.CurrentUserId == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                SystemConfig sc = new SystemConfig();
                ViewBag.ApplicationName = sc.GetConfigValue("ApplicationName");
                ViewBag.CopyRight = sc.GetConfigValue("CopyRight");
                return View();
            }
        }

        [Log]
        public ActionResult Logout()
        {
            CurrentUserInfo.CurrentUserId = 0;
            return RedirectToAction("Index", "Home");
        }

        [Log]
        public ActionResult GetMenu()
        {
            //test
            if (CurrentUserInfo.CurrentUserId == 0)
            {
                return new Result(db.Menus.OrderBy(c=>c.Seq));
            }


            var priv = db.Privileges.Where(c=>c.RoleId == CurrentUserInfo.CurrentRoleId).Select(c=>c.ModuleNo).Distinct().ToArray();
            var menus = db.Menus.Where(c=>priv.Contains(c.MenuNo));

            var ret = menus.Union(db.Menus.Where(c => menus.Select(d => d.MenuParentId).Contains(c.MenuId)));

            return new Result(ret.OrderBy(c => c.Seq));
        }


        public ActionResult Test()
        {
            return View();
        }

        [Log]
        public ActionResult Welcome()
        {
            return View();
        }

        #region 收藏

        /// <summary>
        /// 获取 收藏 
        /// </summary>
        [Log]
        public ActionResult GetMyFavorite()
        {
            return new Result(db.Favorites.Where(c => c.UserId == CurrentUserInfo.CurrentUserId));
        }

        [Log]
        [HttpPost]
        public ActionResult AddMyFavorite(int MenuId, string note)
        {
            Favorite f = new Favorite();
            var menu = db.Menus.First(c => c.MenuId == MenuId);

            if (db.Favorites.Any(c => c.Title == menu.Title && c.UserId == CurrentUserInfo.CurrentRoleId)) return Json(AjaxResult.Success());

            f.Title = menu.Title;
            f.Note = note;
            f.AddTime = DateTime.Now;
            f.Url = menu.Url;
            f.Icon = menu.Icon;
            //f.UserId = CurrentUserInfo.CurrentRoleId;
            f.UserId = CurrentUserInfo.CurrentUserId;
            db.Favorites.Add(f);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [Log]
        [HttpPost]
        public ActionResult RemoveMyFavorite(int Id)
        {
            db.Favorites.Remove(db.Favorites.First(c => c.Id == Id));
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion
    }
}

using QuanLyCuaHangBanDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QuanLyCuaHangBanDienThoai.Controllers
{
    public class UserController : Controller
    {
        DataClasses1DataContext data = new DataClasses1DataContext();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection loginform, Nhanvien nv)
        {
            var username = loginform["username"];
            var password = loginform["password"];
            var user = data.Nhanviens.FirstOrDefault(m => m.usernane == username);

            if (user != null)
            {
                if (user.password == password)
                {
                    ViewData["loginSuccess"] = "thanh cong";
                    Session["userId"] = user.id.ToString();
                    Session["tenNv"] = user.ten;
                    Session["username"] = user.usernane;
                    Session["password"] = user.usernane;
                    Session["role"] = user.quyen_han;
                    return RedirectToAction("Index", "Home");
                }
                ViewData["invalidPassword"] = "Sai mật khẩu!!!";
            } else
            {
                ViewData["invalidUsername"] = "Tên đăng nhập không đúng!";

            }
            return View();
        }

        public ActionResult logOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("login", "User");
        }
        
    }
}
using Microsoft.Ajax.Utilities;
using QuanLyCuaHangBanDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.WebPages;
using System.Xml.Linq;

namespace QuanLyCuaHangBanDienThoai.Controllers
{
    public class CustomerController : Controller
    {
       DataClasses1DataContext data = new DataClasses1DataContext();
        
        public bool checkName(String s)
        {
            for(int i=0;i<s.Length; i++)
            {
                if (s[i].ToString().All(char.IsDigit) == true)
                {
                    return false;
                }
            }
            return true;
        }

        public bool checkNumber(String s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].ToString().All(char.IsDigit) == false)
                {
                    return false;
                }
            }
            return true;
        }

        public ActionResult Index()
        {
            var All_KH = from tt in data.KhachHangs select tt;
            return View(All_KH);
        }

        [HttpPost]
        public ActionResult Search(FormCollection collection)
        {
            string searchString = collection["searchString"];
            if (searchString.IsEmpty() == true)
            {
                TempData["Loitk"] = "Vui lòng nhập từ khóa tìm kiếm";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("dsadas", searchString);
                var All_KH = data.KhachHangs.Where(m => m.ten.Contains(searchString));
                ViewData["s"] = searchString;
                return View(All_KH);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, KhachHang KH)
        {
            DateTime d;
            String ten = collection["inputName"];
            var sdt = collection["inputNumber"];
            String ngay = collection["inputDate"];
           
            KH.ten = ten;
            System.Diagnostics.Debug.WriteLine(ten.IsEmpty());
            if (checkName(ten) == true && checkNumber(sdt) == true && ngay.IsEmpty() == false)
            {

                d = DateTime.Parse(ngay, new CultureInfo("en-CA"));
                KH.sdt = sdt;        
                KH.ngay_sinh = d;

                data.KhachHangs.InsertOnSubmit(KH);
                data.SubmitChanges();               
            }
            else
            {               
                if (ten.IsEmpty() == true)
                {
                    TempData["Loiten"] = "Không được bỏ trống tên";
                }
                if(checkName(ten) == false)
                {
                    TempData["Loiten"] = "Tên không hợp lệ";
                }
                if(checkNumber(sdt) == false)
                {
                    TempData["Loisdt"] = "Số điện thoại không hợp lệ";
                }
                if (sdt.IsEmpty() == true)
                {
                    TempData["Loisdt"] = "Không được bỏ trống số điện thoại";
                }
                if (ngay.IsEmpty() == true)
                {
                    TempData["Loilich"] = "Không được bỏ trống ngày sinh";
                }
            }

            return RedirectToAction("Index");
        }


        public ActionResult Delete(int id)
        {
            var D_tin = data.KhachHangs.First(m => m.id ==id); 
            return View(D_tin);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_tin = data.KhachHangs.Where(m => m.id == id).First();
            data.KhachHangs.DeleteOnSubmit(D_tin);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            var tt = data.KhachHangs.First(m => m.id ==id);
            return View(tt);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
           
            DateTime d;
            var tt = data.KhachHangs.First(m => m.id ==id); 
            var ten = collection["fixName"];
            var sdt = collection["fixNumber"];
            String ngay = collection["fixDate"];
      
            if(checkName(ten) == true && checkNumber(sdt) == true && ngay.IsEmpty() == false)
            {
                tt.ten = ten;
                tt.sdt = sdt;
                d = DateTime.Parse(ngay, new CultureInfo("en-CA"));
                tt.ngay_sinh = d;
                UpdateModel(tt);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            else
            {
                if (ten.IsEmpty() == true)
                {
                    TempData["Loiten"] = "Không được bỏ trống tên";
                }
                if (checkName(ten) == false)
                {
                    TempData["Loiten"] = "Tên không hợp lệ";
                }
                if (checkNumber(sdt) == false)
                {
                    TempData["Loisdt"] = "Số điện thoại không hợp lệ";
                }
                if (sdt.IsEmpty() == true)
                {
                    TempData["Loisdt"] = "Không được bỏ trống số điện thoại";
                }
                if (ngay.IsEmpty() == true)
                {
                    TempData["Loilich"] = "Không được bỏ trống ngày sinh";
                }
            }
            return this.Edit(id);
        }
    }
}

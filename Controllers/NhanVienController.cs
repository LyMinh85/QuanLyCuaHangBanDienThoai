using QuanLyCuaHangBanDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace QuanLyCuaHangBanDienThoai.Controllers
{
    public class NhanVienController : Controller

    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        // GET: NhanVien
        public ActionResult Index()
        {
            return RedirectToAction("ShowAll");
        }

        public ActionResult ShowAll()
        {
            var nhanViens = from nv in db.Nhanviens select nv;
            return View("ShowAll", nhanViens);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection formnv )
        {
            var ten = formnv["ten"];
            var sdt = formnv["sdt"];
            var ngaysinh = formnv["ngay_sinh"];
            var quyenhan = formnv["quyen_han"];
            var hinhanh = formnv["hinh_anh"];
            var username = formnv["usernane"];
            var password = formnv["password"];

            var nv = new Nhanvien
            {
                ten = ten,
                sdt = sdt,
                ngay_sinh = DateTime.Parse(ngaysinh),
                quyen_han = quyenhan,
                hinh_anh = hinhanh,
                usernane = username,
                password = password
            };
            db.Nhanviens.InsertOnSubmit(nv);
            db.SubmitChanges();


            return RedirectToAction("ShowAll");
        }

        public ActionResult Delete(int id)
        {
            Nhanvien n = db.Nhanviens.SingleOrDefault(m => m.id == id);
            if (n != null)
            {
                db.Nhanviens.DeleteOnSubmit(n);
                db.SubmitChanges();
            }
            return RedirectToAction("ShowAll");
        }

        public ActionResult Edit(int id)
        {
            Nhanvien nv = db.Nhanviens.FirstOrDefault(q => q.id == id);
            if (nv != null) //tìm thấy
            {
                return View(nv);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
            public ActionResult Edit( int id ,FormCollection formnv)
        {
            var ten = formnv["ten"];
            var sdt = formnv["sdt"];
            var ngaysinh = formnv["ngay_sinh"];
            var quyenhan = formnv["quyen_han"];
            var hinhanh = formnv["hinh_anh"];
            var username = formnv["usernane"];
            var password = formnv["password"];

            Nhanvien nv = db.Nhanviens.FirstOrDefault(m => m.id == id);
            if (nv == null)
            {
                return HttpNotFound();
            }
            else
            {
                nv.ten = ten;
                nv.usernane = username;
                nv.password = password;
                nv.quyen_han = quyenhan;
                nv.hinh_anh = hinhanh;
                nv.ngay_sinh = DateTime.Parse(ngaysinh);
                db.SubmitChanges();
            }
            return RedirectToAction("ShowAll");
        }

    }   
}
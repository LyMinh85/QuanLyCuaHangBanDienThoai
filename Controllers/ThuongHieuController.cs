using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyCuaHangBanDienThoai.Models;

namespace QuanLyCuaHangBanDienThoai.Controllers
{
    public class ThuongHieuController : Controller
    {
        // GET: /ThuongHieu/
        DataClasses1DataContext data = new DataClasses1DataContext();
        public ActionResult Index()
        {
            var All_ThuongHieu = from tt in data.ThuongHieus select tt; 
            return View(All_ThuongHieu);
        }
        //thông tin
        public ActionResult Details(int id)
        {
            var Details_TH = data.ThuongHieus.Where(m => m.id ==
            id).First(); return View(Details_TH);
        }
        public ActionResult Create()
        {
            return View();
        }
        //tạo
        [HttpPost]
        public ActionResult Create(FormCollection collection, ThuongHieu ltt)
        {
           var CB_Thuonghieu = collection["ten"];
            if (string.IsNullOrEmpty(CB_Thuonghieu))
            {
                ViewData["Lỗi"] = " Tên Thương Hiệu không được để trống";
            }
            else
            {
                ltt.ten = CB_Thuonghieu;
                data.ThuongHieus.InsertOnSubmit(ltt);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }
        //sửa
        public ActionResult Edit(int id)
        {
            var EB_Thuonghieu = data.ThuongHieus.First(m => m.id ==
            id); return View(EB_Thuonghieu);
        }
        
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var Ltt = data.ThuongHieus.First(m => m.id == id);
            var E_Thuonghieu = collection["ten"];
            if (string.IsNullOrEmpty(E_Thuonghieu))
            {
                ViewData["Lỗi"] = "Tên Thương Hiệu không được để trống";
            }
            else
            {
                Ltt.ten = E_Thuonghieu;
                UpdateModel(Ltt);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }
        //Xóa
        public ActionResult Delete(int id)
        {
            var D_tt = data.ThuongHieus.First(m => m.id ==
            id); return View(D_tt);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_tt = data.ThuongHieus.Where(m => m.id == id).First();
            data.ThuongHieus.DeleteOnSubmit(D_tt);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }

    }
}
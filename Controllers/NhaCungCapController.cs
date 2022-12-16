using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyCuaHangBanDienThoai.Models;

namespace QuanLyCuaHangBanDienThoai.Controllers
{
    public class NhaCungCapController : Controller
    {
        // GET: /NhaCungCap/
        DataClasses1DataContext data = new DataClasses1DataContext();
        public ActionResult Index()
        {
            var All_NCC = from tt in data.NhaCungCaps select tt;
            return View(All_NCC);
        }
        //tạo
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, NhaCungCap lncc)
        {
            var CB_Nhacungcap = collection["ten"];
            if (string.IsNullOrEmpty(CB_Nhacungcap))
            {
                ViewData["Lỗi"] = "Không được để trống";
            }
            else
            {
                lncc.ten = CB_Nhacungcap;
                lncc.sdt = collection["sdt"];
                lncc.diachi = collection["diachi"];
                lncc.ma_thue = collection["ma_thue"];
                data.NhaCungCaps.InsertOnSubmit(lncc);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }
        //sửa
        public ActionResult Edit(int id)
        {
            var EB_Nhacungcap = data.NhaCungCaps.First(m => m.id ==
            id); return View(EB_Nhacungcap);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var Lncc = data.NhaCungCaps.First(m => m.id == id);
            var E_Nhacungcap = collection["ten"];
            Lncc.id = id;
            if (string.IsNullOrEmpty(E_Nhacungcap))
            {
                ViewData["Lỗi"] = "Tên Nhà Cung Cấp không được để trống";
            }
            else
            {
                Lncc.ten = E_Nhacungcap;
                Lncc.sdt = collection["sdt"];
                Lncc.diachi = collection["diachi"];
                Lncc.ma_thue = collection["ma_thue"];
                UpdateModel(Lncc);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }
        //Xóa
        public ActionResult Delete(int id)
        {
            var D_ncc = data.NhaCungCaps.First(m => m.id ==
            id); return View(D_ncc);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_ncc = data.NhaCungCaps.Where(m => m.id == id).First();
            data.NhaCungCaps.DeleteOnSubmit(D_ncc);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }

    }
}
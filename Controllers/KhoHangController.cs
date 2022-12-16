using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows;
using System.Web.Mvc;
using System.Xml.Linq;
using QuanLyCuaHangBanDienThoai.Models;


namespace QuanLyCuaHangBanDienThoai.Controllers
{
    public class KhoHangController : Controller
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        // GET: Kho
        public ActionResult Index()
        {
            var All_Kho = from k in data.Khos select k;
            return View(All_Kho);

        }
        [HttpPost]
        public ActionResult Index(String strSearch)
        {
      
            var obj = from k in data.Khos select k;
            if (!String.IsNullOrEmpty(strSearch))
            {
                obj = data.Khos.Where(m => m.ten.Contains(strSearch) || m.dia_chi.Contains(strSearch));
            }
            ViewBag.StrSearch = strSearch;
            return View(obj);
        }

        public ActionResult Details(int id)
        {
            var Details_kho = data.Khos.Where(m => m.id == id).First();

            
            return View(Details_kho);
        }
        public ActionResult Create()
        {
            ViewData["ScriptTenKho"] = "<script>\r\n                   var inp = document.getElementById(\"TenKho\").value;\r\n                                     if (inp == \"\") {\r\n\r\n                        document.getElementById(\"TenKho\").focus();\r\n                    }\r\n                </script> ";
            ViewData["ScriptDCKho"] = "<script>\r\n                    var inp = document.getElementById(\"dia_chi\").value;\r\n                                      if (inp == \"\") {\r\n\r\n                        document.getElementById(\"dia_chi\").focus();\r\n                    }\r\n                </script> ";

            return View();
        }
        [HttpPost]
        public ActionResult Create (FormCollection collection, Kho kho)
        {
            var CB_TenKho = collection["TenKho"];
            var CB_DiaChi = collection["dia_chi"];

            ViewData["StrDCKho"] = CB_DiaChi;
            ViewBag.StrTenKho = CB_TenKho;


            //Nếu CB_TenKho có giá trị == null ( để trống )
            if (string.IsNullOrEmpty(CB_TenKho))
            {
                ViewData["LoiTen"] = " Tên kho không được để trống ";
                ViewData["ScriptTenKho"] = "<script>\r\n                    var inp = document.getElementById(\"TenKho\").value;\r\n                    console.log(inp)\r\n                    if (inp == \"\") {\r\n\r\n                        document.getElementById(\"TenKho\").focus();\r\n                    }\r\n                </script> ";

            }

            if (string.IsNullOrEmpty(CB_DiaChi))
            {
                ViewData["LoiDC"] = " Địa chỉ kho không được để trống ";
                ViewData["ScriptDCKho"] = "<script>\r\n                    var inp = document.getElementById(\"dia_chi\").value;\r\n                    console.log(inp)\r\n                    if (inp == \"\") {\r\n\r\n                        document.getElementById(\"dia_chi\").focus();\r\n                    }\r\n                </script> ";

            }

            if (CB_TenKho != "" && CB_DiaChi != "")
            {
                kho.ten = CB_TenKho;
                kho.dia_chi = CB_DiaChi;
                data.Khos.InsertOnSubmit(kho);
                //Thực hiện tạo mới 
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }
        public ActionResult Edit(int id)
        {
            var EB_kho = data.Khos.First(m => m.id == id);
            return View(EB_kho);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var kho01 = data.Khos.First(m => m.id == id); 
            var E_Ten = collection["ten"];
            var E_DC = collection["dia_chi"];
            kho01.id = id;
            if (string.IsNullOrEmpty(E_Ten))
            {
                ViewData["LoiTen"] = " Tên kho không được để trống ";
            }else
            {
                ViewData["LoiTen"] = "";
            }

            if (string.IsNullOrEmpty(E_DC))
            {
                ViewData["LoiDC"] = " Địa chỉ kho không được để trống ";
            }
            else
            {
                ViewData["LoiDC"] = "";
            }
            if (E_Ten != "" && E_DC != "")
            {
                kho01.ten = E_Ten;
                kho01.dia_chi = E_DC;
                // Thực hiện updat
                UpdateModel(kho01);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }
        public ActionResult Delete(int id)
        {
            var D_kho = data.Khos.First(m => m.id == id); 
            return View(D_kho);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_kho = data.Khos.Where(m => m.id == id).First();

            var sanPhamTonKhos = data.SanPhamTonKhos.Where(tk => tk.id_kho == id).Select(tk => tk);

            foreach (var tk in sanPhamTonKhos)
            {
                data.SanPhamTonKhos.DeleteOnSubmit(tk);
            }
            data.SubmitChanges();

                //xóa
                data.Khos.DeleteOnSubmit(D_kho);
                data.SubmitChanges();
                return RedirectToAction("Index");

        }
    }
}
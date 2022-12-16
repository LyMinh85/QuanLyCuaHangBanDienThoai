using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using QuanLyCuaHangBanDienThoai.Models;

namespace QuanLyCuaHangBanDienThoai.Controllers
{
    public class PhieuNhapController : Controller
    {
        private readonly DataClasses1DataContext _db = new DataClasses1DataContext();
        private readonly int pageSize = 5;
        // GET: /phieunhap/
        public ActionResult Index()
        {
            // Ko có số trang
            var page = 1;
            if (!string.IsNullOrEmpty(Request.Params["page"]))
            {
                page = int.Parse(Request.Params["page"]);
            }
            var phieuNhaps 
                = _db.PhieuNhaps
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(pn => pn);
            
            var totalPhieuNhap = _db.PhieuNhaps.Count();
            var pageCount = Convert
                .ToInt32(Math.Ceiling(totalPhieuNhap / Convert.ToDouble(pageSize)));
            
            ViewBag.page = page;
            ViewBag.pageCount = pageCount;
            
            return View(phieuNhaps);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var nhaCungCaps = _db.NhaCungCaps.Select(ncc => ncc);
            var listKho = _db.Khos.Select(kho => kho);

            ViewBag.listKho = listKho;

            return View(nhaCungCaps);
        }

        public class Data
        {
            public int id { get; set; }
            public decimal gia_nhap { get; set; }
            public int so_luong { get; set; }
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var idNhaCungCap = collection["idNhaCungCap"];
            var idKho = int.Parse(collection["idKho"]);
            var listCtpnString = collection["listCtpn"];
            var listCtpn = JsonConvert.DeserializeObject<List<Data>>(listCtpnString);

            System.Diagnostics.Debug.WriteLine(listCtpnString);

            var phieuNhap = new PhieuNhap
            {
                id_nha_cung_cap = int.Parse(idNhaCungCap),
                ngay_tao_phieu = DateTime.Now
            };

            _db.PhieuNhaps.InsertOnSubmit(phieuNhap);
            _db.SubmitChanges();

            foreach (var ctpn in listCtpn)
            {
                var chiTietPhieuNhap = new ChiTietPhieuNhap
                {
                    id_phieu_nhap = phieuNhap.id,
                    id_bien_the_san_pham = ctpn.id,
                    so_luong = ctpn.so_luong
                };
                _db.ChiTietPhieuNhaps.InsertOnSubmit(chiTietPhieuNhap);

                var sanPhamTonKho = _db.SanPhamTonKhos.
                    FirstOrDefault(tk => tk.id_bien_the_san_pham == ctpn.id && tk.id_kho == idKho);
                
                if (sanPhamTonKho == null)
                {
                    sanPhamTonKho = new SanPhamTonKho
                    {
                        id_bien_the_san_pham = ctpn.id,
                        id_kho = idKho,
                        so_luong = ctpn.so_luong
                    };
                    _db.SanPhamTonKhos.InsertOnSubmit(sanPhamTonKho);
                } else
                {
                    sanPhamTonKho.so_luong += ctpn.so_luong;
                }
                _db.SubmitChanges();
                
            }
            _db.SubmitChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Detail(int id)
        {
            var phieuNhap = _db.PhieuNhaps.Where(pn => pn.id == id).FirstOrDefault();
            var ctpns = _db.ChiTietPhieuNhaps.Where(ctpn => ctpn.id_phieu_nhap == id).Select(ctpn => ctpn);

            ViewBag.ctpns = ctpns;

            return View(phieuNhap);
        }

        [HttpPost]
        public ActionResult Search()
        {
            return RedirectToAction("Index");
        }
    }
}
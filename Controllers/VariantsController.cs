using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyCuaHangBanDienThoai.Models;

namespace QuanLyCuaHangBanDienThoai.Controllers
{
    public class VariantsController : Controller
    {
        private DataClasses1DataContext _db = new DataClasses1DataContext();
        // GET: /variants/delete/id
        [Route("variants/delete/{id}")]
        public ActionResult Delete(int id = 1)
        {
            var productId = Request.Params["productId"];
            var variant = _db.BienTheSanPhams.FirstOrDefault(v => v.id == id);
            var thongSoSanPham = _db.ThongSoSanPhams.FirstOrDefault(ts => ts.id == variant.id_thong_so_san_pham);
            // If not found
            if (variant == null)
            {
                return new HttpNotFoundResult();
            }

            if (thongSoSanPham == null)
            {
                return new HttpNotFoundResult();
            }
            
            _db.BienTheSanPhams.DeleteOnSubmit(variant);
            _db.SubmitChanges();

            _db.ThongSoSanPhams.DeleteOnSubmit(thongSoSanPham);
            _db.SubmitChanges();
            return RedirectToAction("Detail", "SanPham", new {id = productId});
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var productId = collection["productId"];
            var giaBan = decimal.Parse(collection["inputGiaBan"].Replace(",", ""));
            var giaNhap = decimal.Parse(collection["inputGiaNhap"].Replace(",", ""));

            var variant = _db.BienTheSanPhams.FirstOrDefault(v => v.id == id);

            if (variant != null)
            {
                variant.gia_ban = giaBan;
                variant.gia_nhap = giaNhap;
                _db.SubmitChanges();
            }
            return RedirectToAction("Detail", "SanPham", new {id = productId});

        }

        public class TonKho
        {
            public BienTheSanPham variant { get; set; }
            public SanPhamTonKho sanPhamTonKho { get; set; }
            public Kho kho { get; set; }

            public TonKho(BienTheSanPham variant, SanPhamTonKho sanPhamTonKho, Kho kho)
            {
                this.variant = variant;
                this.sanPhamTonKho = sanPhamTonKho;
                this.kho = kho;
            }   
        }

        [HttpGet]
        public ActionResult Index()
        {
            var variants = from variant in _db.BienTheSanPhams
                           join tonKho in _db.SanPhamTonKhos on variant.id equals tonKho.id_bien_the_san_pham
                           join kho in _db.Khos on tonKho.id_kho equals kho.id
                           where kho.id == 1
                           select new TonKho(variant, tonKho, kho);

            var listKho = _db.Khos.Select(kho => kho);
            ViewBag.listKho = listKho;
            ViewBag.idkhoDuocChon = 1;

            return View(variants);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            var idKho = int.Parse(collection["idKho"]);
            var variants = from variant in _db.BienTheSanPhams
                           join tonKho in _db.SanPhamTonKhos on variant.id equals tonKho.id_bien_the_san_pham
                           join kho in _db.Khos on tonKho.id_kho equals kho.id
                           where kho.id == idKho
                           select new TonKho(variant, tonKho, kho);

            var listKho = _db.Khos.Select(kho => kho);
            ViewBag.listKho = listKho;
            ViewBag.idkhoDuocChon = idKho;

            return View(variants);
        }
    }
    
}
using Microsoft.Ajax.Utilities;
using QuanLyCuaHangBanDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.WebPages;

namespace QuanLyCuaHangBanDienThoai.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        private readonly DataClasses1DataContext _db = new DataClasses1DataContext();
        private readonly int pageSize = 5;
        // GET: /SanPham/
        [HttpGet]
        public ActionResult Index()
        {
            // Ko có số trang
            var page = 1;
            if (!string.IsNullOrEmpty(Request.Params["page"]))
            {
                page = int.Parse(Request.Params["page"]);
            }

            var SanPhams 
                = _db.SanPhams
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(sp => sp);
            
            var totalProduct = _db.SanPhams.Count();
            var pageCount = Convert.ToInt32(Math.Ceiling(totalProduct / Convert.ToDouble(pageSize)));
            
            ViewBag.page = page;
            ViewBag.pageCount = pageCount;

            return View(SanPhams);
        }

        // GET: /SanPham/create
        [HttpGet]
        public ActionResult Create()
        {
            var ThuongHieus = from th in _db.ThuongHieus select th;
            ViewBag.ThuongHieus = ThuongHieus;
            return View();
        }

        // POST: /sanpham/create
        [HttpPost]
        public ActionResult Create(FormCollection formCollection, HttpPostedFileBase file)
        {
            var ThuongHieus = from th in _db.ThuongHieus select th;
            ViewBag.ThuongHieus = ThuongHieus;
            try
            {
                var name = formCollection["name"];
                var description = formCollection["description"];
                var brandId = int.Parse(formCollection["brandId"]);
                var inputMauSac = formCollection["mauSac"];
                var inputDungLuong = formCollection["dungLuong"];
                var inputRam = formCollection["ram"];
                var giaBan = decimal.Parse(formCollection["inputGiaBan"].Replace(",", ""));
                var giaNhap = decimal.Parse(formCollection["inputGiaNhap"].Replace(",", ""));
                var isError = false;

                // Validate
                if (string.IsNullOrEmpty(name))
                {
                    isError = true;
                    ViewBag.inValidName = "Tên không được bỏ trống";
                }

                if (!(validInputVariant(inputMauSac) &&
                      validInputVariant(inputDungLuong) &&
                      validInputVariant(inputRam)))
                {
                    isError = true;
                    ViewBag.invalidThongSoSanPham = "Thông số sản phẩm không đúng định dạng";
                }

                if (giaBan <= 0)
                {
                    isError = true;
                    ViewBag.invalidGiaBan = "Giá phải lớn hơn 0";
                }

                if (giaNhap <= 0)
                {
                    isError = true;
                    ViewBag.invalidGiaNhap = "Giá phải lớn hơn 0";
                }

                if (isError)
                {
                    ViewBag.name = name;
                    ViewBag.description = description;
                    ViewBag.giaBan = giaBan;
                    ViewBag.giaNhap = giaNhap;
                    ViewBag.mauSac = inputMauSac;
                    ViewBag.dungLuong = inputDungLuong;
                    ViewBag.ram = inputRam;
                    ViewBag.isError = "novalidate";
                    return Create();
                }

                //Insert to db.SanPhams
                var sanPham = new SanPham
                {
                    ten = name,
                    mo_ta = description,
                    hinh_anh = null,
                };

                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(
                        Server.MapPath("~/Content/img/products"), pic);
                    string relativePath = "/Content/img/products/" + pic;
                    // file is uploaded
                    file.SaveAs(path);
                    sanPham.hinh_anh = relativePath;
                } else
                {
                    sanPham.hinh_anh = "/Content/img/default-thumbnail.png";
                }

                if (brandId > 0)
                {
                    sanPham.id_thuong_hieu = brandId;
                }

                _db.SanPhams.InsertOnSubmit(sanPham);
                _db.SubmitChanges();

                inputMauSac = checkInputVariant(inputMauSac);
                inputDungLuong = checkInputVariant(inputDungLuong);
                inputRam = checkInputVariant(inputRam);

                foreach (var mauSac in inputMauSac.Split(','))
                {
                    foreach (var dungLuong in inputDungLuong.Split(','))
                    {
                        foreach (var ram in inputRam.Split(','))
                        {
                            ThongSoSanPham thongSoSanPham = new ThongSoSanPham
                            {
                                mau_sac = string.IsNullOrEmpty(mauSac) ? null : mauSac,
                                dung_luong = string.IsNullOrEmpty(dungLuong) ? null : dungLuong,
                                ram = string.IsNullOrEmpty(ram) ? null : mauSac
                            };
                            _db.ThongSoSanPhams.InsertOnSubmit(thongSoSanPham);
                            _db.SubmitChanges();


                            BienTheSanPham bienTheSanPham = new BienTheSanPham
                            {
                                hinh_anh = sanPham.hinh_anh,
                                id_thong_so_san_pham = thongSoSanPham.id,
                                id_san_pham = sanPham.id,
                                gia_ban = giaBan,
                                gia_nhap = giaNhap
                            };
                            _db.BienTheSanPhams.InsertOnSubmit(bienTheSanPham);
                            _db.SubmitChanges();
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return View();
            }
        }

        // GET: /sanpham/detail/<id>
        public ActionResult Detail(int id = 1)
        {
            var sanPham = _db.SanPhams.FirstOrDefault(sp => sp.id == id);
            var variants = from v in _db.BienTheSanPhams where v.id_san_pham == id select v;
            ViewBag.variants = variants;
            return View(sanPham);
        }

        [Route("/SanPham/Delete/{id}")]
        public ActionResult Delete(int id)
        {
            var sanPham = _db.SanPhams.FirstOrDefault(sp => sp.id == id);
            if (sanPham == null)
            {
                return RedirectToAction("Index");
            }

            // Get all variant of product
            var variants = from v in _db.BienTheSanPhams where v.id_san_pham == id select v;
            foreach (var variant in variants)
            {
                var thongSoSanPham = _db.ThongSoSanPhams.First(ts => ts.id == variant.id_thong_so_san_pham);
                var sanPhamTonKhos = _db.SanPhamTonKhos.Where(tk => tk.id_bien_the_san_pham == variant.id).Select(tk => tk);
                // Delete variant
                _db.BienTheSanPhams.DeleteOnSubmit(variant);

                // SanPhamTonKho
                foreach (var tk in sanPhamTonKhos)
                {
                    _db.SanPhamTonKhos.DeleteOnSubmit(tk);
                }

                // Delete ThongSoSanPham
                _db.ThongSoSanPhams.DeleteOnSubmit(thongSoSanPham);
            }

            // Delete SanPham
            _db.SanPhams.DeleteOnSubmit(sanPham);
            _db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [Route("/SanPham/Edit/{id}")]
        public ActionResult Edit(int id)
        {
            var sanPham = _db.SanPhams.FirstOrDefault(sp => sp.id == id);
            var thuongHieus = _db.ThuongHieus.Select(h => h);

            ViewBag.thuongHieus = thuongHieus;

            return View(sanPham);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, HttpPostedFileBase file)
        {
            var sanPham = _db.SanPhams.FirstOrDefault(sp => sp.id == id);
            try
            {
                var isError = false;
                var name = collection["name"];
                var brandId = int.Parse(collection["brandId"]);

                // Validate
                if (string.IsNullOrEmpty(name))
                {
                    isError = true;
                    ViewBag.inValidName = "Tên không được bỏ trống";
                }

                if (isError)
                {
                    return RedirectToAction("Edit", new { id });
                }

                sanPham.ten = name;
                sanPham.id_thuong_hieu = brandId;

                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(
                        Server.MapPath("~/Content/img/products"), pic);
                    string relativePath = "/Content/img/products/" + pic;
                    // file is uploaded
                    file.SaveAs(path);
                    sanPham.hinh_anh = relativePath;
                }

                _db.SubmitChanges();

                return RedirectToAction("Detail", new {id});
            } catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return View();
            }

        }

        // Post: /sanpham/search
        [HttpPost]
        public ActionResult Search(FormCollection collection)
        {
            var query = collection["query"];
            var sanPhams 
                = _db.SanPhams
                    .Where(sp => sp.ten.Contains(query));

            return View("Index", sanPhams);
        }

        public string checkInputVariant(String inputVariant)
        {
            if (string.IsNullOrEmpty(inputVariant))
            {
                return "";
            }

            return inputVariant;
        }

        public bool validInputVariant(string inputVariant)
        {
            if (string.IsNullOrEmpty(inputVariant))
            {
                return true;
            }

            var options = inputVariant.Split(',');
            foreach (var option in options)
            {
                if (string.IsNullOrEmpty(option))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
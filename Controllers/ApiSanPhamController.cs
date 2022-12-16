using QuanLyCuaHangBanDienThoai.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace QuanLyCuaHangBanDienThoai.Controllers
{
    public class ApiSanPhamController : ApiController
    {
        private readonly DataClasses1DataContext _db = new DataClasses1DataContext();
        private readonly int pageSize = 5;

        [System.Web.Http.AcceptVerbs("GET")]
        public HttpResponseMessage Index()
        {
            // Ko có số trang
            var page = 1;
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["page"]))
            {
                page = int.Parse(HttpContext.Current.Request.QueryString["page"]);
            }

            var variants
                = _db.BienTheSanPhams
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(sp => new {
                        sp.id,
                        sp.gia_nhap,
                        sp.hinh_anh,
                        sp.SanPham.ten,
                        sp.ThongSoSanPham.mau_sac,
                        sp.ThongSoSanPham.dung_luong,
                        sp.ThongSoSanPham.ram 
                    });

            var totalProduct = _db.BienTheSanPhams.Count();
            var pageCount = Convert.ToInt32(Math.Ceiling(totalProduct / Convert.ToDouble(pageSize)));

            var response = new { page, pageCount, variants };
            
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }
        
    }
    
}
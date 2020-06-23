using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebsiteBanHang
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
           // // cấu hình đường dẫn cho trang home/index => trang-chu
           // routes.MapRoute(
           //    name: "trangchu",
           //    url: "trang-chu",
           //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            // cấu hình đường dẫn trang chitietsan pham cua controler san phẩm
           // routes.MapRoute(
           //    name: "ChiTietSanPham",
           //    url: "{tensp}-{id}",
           //    defaults: new { controller = "SanPham", action = "ChiTietSanPham", id = UrlParameter.Optional }
           //);

        }
    }
}

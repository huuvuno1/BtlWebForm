﻿using BtlWebForm.Entity;
using BtlWebForm.Repository;
using BtlWebForm.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BtlWebForm.Views.Common
{
    public partial class category : System.Web.UI.Page
    {
        
        ProductRepository productRepository = new ProductRepository();
        protected void Page_Load(object sender, EventArgs e)
        {

            // xử lí trường hợp run tại page này hoặc cố tình vào page /Views/Common/...
            if (Request.Url.AbsolutePath.Contains("Views/Common"))
                Response.Redirect("/may-tinh");

            string category = (string)RouteData.Values["category"];

            url_server.Attributes.Add("href", "/" + category);
            switch (category)
            {
                case "may-tinh":
                    name_page.InnerText = "Máy tính";
                    break;
                case "phu-kien":
                    name_page.InnerText = "Phụ kiện";
                    break;
                case "san-pham":
                    name_page.InnerText = "Tất cả sản phẩm";
                    break;
                case "san-pham-moi-nhat":
                    name_page.InnerText = "Sản phẩm mới nhất";
                    break;
                case "san-pham-khuyen-mai":
                    name_page.InnerText = "Sản phẩm khuyến mãi";
                    break;
                default:
                    Response.Redirect("/san-pham");
                    break;
            }

            string HTML = "";

            // value của query có dạng "duoi-10-trieu,tren-25-trieu"
            string filter = Request.QueryString.Get("filter");

            List<ProductEntity> result = productRepository.FindProductsByFilter(category, filter);

            // sort
            string sort = Request.QueryString.Get("sort");
            if (sort != null && !"".Equals(sort))
            {
                if (Constant.SORT_LATEST.Equals(sort))
                    result.Sort((p1, p2) => {
                        return ((p1.TimeAdd - p2.TimeAdd).TotalMilliseconds > 0) ? -1 : 1;
                    });
                else if (Constant.SORT_PRICE_1.Equals(sort))
                    result.Sort((p1, p2) =>
                    {
                        return (p1.Price - p2.Price) > 0 ? 1 : -1;
                    });
                else
                    result.Sort((p1, p2) =>
                    {
                        return (p1.Price - p2.Price) > 0 ? -1 : 1;
                    });

            }

            foreach (ProductEntity product in result)
            {
                string html = ProductUtil.MatchHtmlWithData(product);
                HTML += html;
            }

            quantity_result.InnerText = "(" + result.Count + " sản phẩm)";
            data_server.InnerHtml = HTML;
        }
    }
}
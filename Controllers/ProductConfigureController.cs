using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
namespace WitBird.XiaoChangHe.Controllers
{
    public class ProductConfigureController : Controller
    {
        //
        // GET: /ProductConfigure/

        public int save(string id,string LoveCount=null,string Count=null)
        {
            ProductConfigure info = new ProductConfigure();
            ProductConfigureModel model = new ProductConfigureModel();
            int i; string type = "";
            List<ProductConfigure> list = model.SelProductConfigureById(id);
            if (list != null && list.Count > 0)
            {
                info = list.First();
                type = "Update";
            }
            else {
                type = "Insert";
            }
            info.ProductId = new Guid(id);
            if (!string.IsNullOrEmpty(LoveCount))
            {
                info.LoveCount = Convert.ToInt32(LoveCount);
            }
            if (!string.IsNullOrEmpty(Count))
            {
                info.Count = Convert.ToInt32(Count);
            }
            i = model.SaveProductConfigure(type, info);
            return i;
        }

    }
}

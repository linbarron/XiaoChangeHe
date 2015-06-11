using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
namespace WitBird.XiaoChangHe.Controllers
{
    public class SubscribeController : Controller
    {
        //
        // GET: /Subscribe/

        public ActionResult Index(String id)
        {


            List<Extension> pt = null;

            ExtensionModel ptm = new ExtensionModel();
            pt = ptm.getSubscribeInfo(id);


            return View(pt);
        }


        public FileContentResult Images(string id)
        {
            ExtensionModel odb = new ExtensionModel();
            if (!string.IsNullOrEmpty(id))
            {

                List<ExtensionImg> list = odb.getExtensionImg(id);
                if (list != null && list.Count > 0)
                {
                    ExtensionImg info = list.First();
                    if (info != null)
                    {
                        if (info.Photo != null)
                        {
                            return File(info.Photo, "jpg", "image_" + id + ".jpg");
                        }


                    }

                }
              
            }
            return File(new Byte[] { }, "jpg", "image_" + id + ".jpg");
        }

    }
}

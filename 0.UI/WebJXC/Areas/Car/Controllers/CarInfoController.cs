using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESS.Domain.Car;
using ESS.Domain.Car.Models;
using ESS.Framework.UI;
using ESS.Framework.UI.Attribute;

namespace WebJXC.Areas.Car.Controllers
{
    public class CarInfoController : Controller
    {
        private DB db = new DB();
        #region CarInfo
        public ActionResult Index()
        {
            return RedirectToAction("GridView", "manage", new { Area = "", menuno = "CarInfo" });
        }

        [Module("CarInfo", "query")]
        public ActionResult GetCarInfo(string PlateNo)
        {
            string sql = @"SELECT * from Car.CarInfo  where ";
            sql += string.IsNullOrEmpty(PlateNo) ? "1=1" : "PlateNo like  '%" + PlateNo + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql,db));
        }

        [HttpPost]
        [Module("CarInfo", "add")]
        public ActionResult AddCarInfo(FormCollection form)
        {
            CarInfo m = new CarInfo();
            TryUpdateModel(m);
            db.CarInfos.Add(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("CarInfo", "edit")]
        public ActionResult EditCarInfo(FormCollection form)
        {
            CarInfo m = db.CarInfos.First(c => c.Id == Convert.ToInt32(form["Id"]));
            TryUpdateModel(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("CarInfo", "del")]
        public ActionResult DelCarInfo(int Id)
        {
            CarInfo m = db.CarInfos.First(c => c.Id == Id);
            db.CarInfos.Remove(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
#endregion
    }
}
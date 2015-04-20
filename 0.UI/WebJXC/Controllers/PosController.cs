using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESS.Domain.JXC.Models;
using Newtonsoft.Json;
using ESS.Framework.UI;
using ESS.Domain.JXC;

namespace WebUI.Controllers
{
    [Log]
    public class PosController : Controller
    {
        private ESS_ERPContext db = new ESS_ERPContext();

        public ActionResult PosSale()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SavePosSale(FormCollection form)
        {

            //出货
            var dm = new DeliveryMaster();
            TryUpdateModel(dm);

            if (dm.CustomerId == 0)
            {
                dm.CustomerId = db.Customers.First(c => c.CustomerAttribName.Contains("零售客户")).CustomerId;
            }

            //处理同时增加Id重复问题
            dm.DeliveryId = new OrderController().GenMaxDeliveryId();
            dm.DeliveryProperty = "1";
            dm.CreateDate = DateTime.Now;
            db.DeliveryMasters.Add(dm);

            if (!string.IsNullOrEmpty(form["DetailGrid"]))
            {
                var detail = JsonConvert.DeserializeObject<List<DeliveryDetail>>(form["DetailGrid"]);
                if (detail.Count <= 0)
                {
                    return new Result(AjaxResult.Error("无从单,无法保存!"));
                }
                foreach (var d in detail)
                {
                    dm.DeliveryDetails.Add(d);
                }

                //处理库存
                foreach (var d in detail)
                {
                    if(string.IsNullOrEmpty(d.BatchNo))
                    {
                        d.BatchNo = dm.DeliveryId;
                    }
                    new OrderController().ChangeProductStock(d.ProductId, d.BatchNo, -d.Quantity??0, d.UnitPrice, -d.Weight??0, dm.DeliveryProperty, "C", dm.DeliveryId);
                }
            }
            else
            {
                return new Result(AjaxResult.Error("无从单,无法保存!"));
            }

            //计算合计
            dm.SubTotal = dm.DeliveryDetails.Sum(c => c.Amount);
            if (dm.DeliveryProperty == "2")
            {
                dm.SubTotal = -dm.SubTotal;
            }
            dm.ValueAddTax = dm.SubTotal * SystemConfig.Tax;
            dm.Amount = dm.SubTotal + dm.ValueAddTax;
            dm.Receivable = dm.Amount;

            //收款
            var arm = new ReceiveMaster();
            TryUpdateModel(arm);

            var ReceiveId = db.ReceiveMasters.Max(c => c.ReceiveId);
            arm.ReceiveId = String.Format("{0:D8}", Convert.ToInt32(ReceiveId) + 1);

            arm.CreateDate = DateTime.Now;
            arm.ReceiveDate = DateTime.Now;
            arm.CustomerId = dm.CustomerId;
            arm.ReceiveCash = Convert.ToDecimal(form["pay"]);

            db.ReceiveMasters.Add(arm);

            ReceiveDetail ard = new ReceiveDetail();
            ard.ReceiveId = arm.ReceiveId;
            ard.DeliveryId = dm.DeliveryId;
            ard.Balance = arm.ReceiveCash;
            arm.ReceiveDetails.Add(ard);

            //计算合计
            arm.TotalBalance = arm.ReceiveDetails.Sum(c => c.Balance);
            arm.ReceiveAmount = arm.ReceiveCash + arm.ReceiveCheck + arm.AccountAmt - arm.Discount + arm.Remittance + arm.AdvancePay + arm.Others;

            //判断单据类型  预收款单给提示 增加 预收款支付不计入
            //（业务处理为发现预收款支付项目不为0，提示预收款该字段不能填写 必须为0，同时处理停止并回退）
            if (arm.ReceiveDetails.Count <= 0)
            {
                arm.Type = "预收款单";
            }
            else if (arm.ReceiveAmount > arm.TotalBalance)
            {
                arm.Type = "收款大于应收";
            }
            else if (arm.ReceiveAmount < arm.TotalBalance)
            {
                arm.Type = "收款小于应收";
            }
            else
            {
                arm.Type = "收款等于应收";
            }

            /*应收实收 差额 计入预付款（定金） 
             *预付款支付部分金额不可计入预付款 通过减扣退出
             */
            Customer ct = db.Customers.First(c => c.CustomerId == arm.CustomerId);
            ct.Advance += (arm.ReceiveAmount - arm.TotalBalance) - arm.AdvancePay;

            db.SaveChanges();

            return new Result(AjaxResult.Success());
        }
    }
}

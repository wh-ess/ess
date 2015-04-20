using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ESS.Domain.JXC.Models;
using ESS.Framework.UI.Attribute;
using Newtonsoft.Json;
using ESS.Framework.UI;

namespace WebUI.Controllers
{
    [Log]
    public class OrderController : Controller
    {
        private ESS_ERPContext db = new ESS_ERPContext();
        /// <summary>
        /// 更改库存
        /// </summary>
        public void ChangeProductStock(int productId, string batchNo, decimal quantity, decimal unitPrice, decimal weight,
            string property, string type, string orderId)
        {

            var stock = new ProductStock();
            stock.BatchNo = batchNo;
            stock.ProductId = productId;
            stock.UnitPrice = unitPrice;
            stock.Type = type;


            if (property == "2")
            {
                quantity = -quantity;
                weight = -weight;
            }
            stock.OrderId = orderId;
            stock.Quantity = quantity;
            stock.Weight = weight;
            stock.CreateDate = DateTime.Now;
            db.ProductStocks.Add(stock);
            db.SaveChanges();
        }

        #region 进货单
        public ActionResult Purchase()
        {
            return RedirectToAction("gridview", "manage", new { menuno = "PurchaseMaster" });
        }

        [Module("PurchaseMaster", "query")]
        public ActionResult GetPurchase(string SupplierAttribName, bool noPaid = false)
        {
            string sql = @"SELECT     a.PurchaseId, a.PurchaseDate, a.SupplierId, CASE a.PurchaseProperty WHEN 1 THEN '进货' ELSE '退货' END AS PurchaseProperty, a.InvoiceNo, a.SubTotal, 
                      a.ValueAddTax, a.Amount, a.AccountPayable, a.Paid, a.LimitDate, a.OpDate, b.SupplierAttribName
                        FROM         Order.PurchaseMaster AS a INNER JOIN
                      Order.Supplier AS b ON a.SupplierId = b.SupplierId ";
            sql += string.IsNullOrEmpty(SupplierAttribName) ? " and 1=1" : " and SupplierAttribName like  '%" + SupplierAttribName + "%'";
            sql += noPaid ? " and  Paid <=0" : " and 1=1";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        public ActionResult GetProductStock(string ProductName, string type)
        {
            string sql = "";
            if (SystemConfig.AllowNoInventory)
            {
                sql = @"SELECT a.ProductId,a.ProductName,b.BatchNo,Quantity,Weight,a.UnitPrice 
                        FROM Order.Product a LEFT JOIN Order.v_ProductStock b ON a.ProductId = b.ProductId where ";
                sql += string.IsNullOrEmpty(ProductName) ? "  1=1" : " ProductName like  '%" + ProductName + "%'";

            }
            else
            {
                sql = @"SELECT * from v_ProductStock  where  ";
                sql += string.IsNullOrEmpty(ProductName) ? "  1=1" : " ProductName like  '%" + ProductName + "%'";
            }

            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        public string GenMaxPurchaseId()
        {
            var PurchaseId = db.PurchaseMasters.Where(c => c.PurchaseDate == DateTime.Now).Max(c => c.PurchaseId);
            return String.Format("JHD{0:yyMMdd}{1:D3}", 
                DateTime.Now, 
                Convert.ToInt32(PurchaseId == null ? "0" : PurchaseId.Substring(9, 3)) + 1);
        }

        public ActionResult GetNewPurchaseId()
        {
            return new Result(new { PurchaseId = GenMaxPurchaseId(), PurchaseProperty = 1, PurchaseDate = DateTime.Now });
        }

        public ActionResult GetPurchaseById(string PurchaseId)
        {
            string sql = @"SELECT * from PurchaseMaster  where PurchaseId = '" + PurchaseId + "'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }
        
        public ActionResult GetPurchaseDetail(string PurchaseId)
        {
            string sql = @"SELECT a.*,ProductName from PurchaseDetail a,product b  where a.productid=b.productid and  PurchaseId = '" + PurchaseId + "'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        [HttpGet]
        public ActionResult AddPurchase()
        {
            return View("EditPurchase");
        }

        [HttpPost]
        [Module("PurchaseMaster", "add")]
        public ActionResult AddPurchase(FormCollection form)
        {
            PurchaseMaster pm = new PurchaseMaster();
            var ret = AddOrEditPurchase(form, true, pm);
            return Json(ret);
        }

        [HttpGet]
        public ActionResult EditPurchase(string PurchaseId)
        {
            ViewData["id"] = string.IsNullOrEmpty(PurchaseId) ? "" : PurchaseId;
            return View();
        }

        [HttpPost]
        [Module("PurchaseMaster", "edit")]
        public ActionResult EditPurchase(FormCollection form)
        {
            var PurchaseId = form["PurchaseId"];
            PurchaseMaster pm = db.PurchaseMasters.First(c => c.PurchaseId == PurchaseId);

            if (pm.PurchaseDate < DateTime.Now.AddMonths(-2))
            {
                return Json(AjaxResult.Error("不允许编辑或删除2个月前的单据!"));
            }

            if (pm.Paid != 0)
            {
                return Json(AjaxResult.Error("不允许编辑已付款的单据!"));
            }

            var ret = AddOrEditPurchase(form, false, pm);
            return Json(ret);
        }

        public AjaxResult AddOrEditPurchase(FormCollection form, bool isNew, PurchaseMaster pm)
        {
            TryUpdateModel(pm);
            pm.CreateDate = DateTime.Now;

            if (isNew)
            {
                db.PurchaseMasters.Add(pm);
            }

            if (!isNew)
            {
                db.PurchaseDetails.RemoveRange(db.PurchaseDetails.Where(c => c.PurchaseId == pm.PurchaseId));
                pm.PurchaseDetails.Clear();

            }
            if (!string.IsNullOrEmpty(form["DetailGrid"]))
            {

                var detail = JsonConvert.DeserializeObject<List<PurchaseDetail>>(form["DetailGrid"]);
                if (detail.Count <= 0)
                {
                    return AjaxResult.Error("无从单,无法保存!");
                }

                //处理库存
                foreach (var d in detail)
                {
                    if (string.IsNullOrEmpty(d.BatchNo))
                    {
                        d.BatchNo = pm.PurchaseId;
                    }
                    if (!isNew)
                    {
                        //清除该单据库存表 退货时batchno不等于单据的id
                        db.ProductStocks.RemoveRange(db.ProductStocks.Where(c => c.BatchNo == d.BatchNo && c.OrderId == pm.PurchaseId
                            && c.ProductId == d.ProductId && c.Type == "J"));
                    }
                    ChangeProductStock(d.ProductId, d.BatchNo, d.Quantity, d.UnitPrice, d.Weight, pm.PurchaseProperty, "J", pm.PurchaseId);
                    pm.PurchaseDetails.Add(d);
                }
                
            }
            else
            {
                return AjaxResult.Error("无从单,无法保存!");
            }

            //计算合计
            pm.SubTotal = pm.PurchaseDetails.Sum(c => c.Amount);
            if (pm.PurchaseProperty == "2")
            {
                pm.SubTotal = -pm.SubTotal;
            }
            pm.ValueAddTax = pm.SubTotal * SystemConfig.Tax;
            pm.Amount = pm.SubTotal + pm.ValueAddTax;
            pm.Payable = pm.Amount;

            db.SaveChanges();
            return AjaxResult.Success();
        }

        [HttpPost]
        [Module("PurchaseMaster", "del")]
        public ActionResult DelPurchase(string PurchaseId)
        {
            var pm = db.PurchaseMasters.First(c => c.PurchaseId == PurchaseId);
            if (pm.PurchaseDate < DateTime.Now.AddMonths(-2))
            {
                return Json(AjaxResult.Error("不允许编辑或删除2个月前的单据!"));
            }
            //处理库存
            var detail = db.PurchaseDetails.Where(c => c.PurchaseId == PurchaseId);
            foreach (var d in detail)
            {
                //清除该单据库存表 退货时batchno不等于单据的id
                db.ProductStocks.RemoveRange(db.ProductStocks.Where(c => c.BatchNo == d.BatchNo && c.OrderId == pm.PurchaseId
                    && c.ProductId == d.ProductId && c.Type == "J"));
            }

            db.PurchaseDetails.RemoveRange(detail);
            db.PurchaseMasters.Remove(db.PurchaseMasters.First(c => c.PurchaseId == PurchaseId));
            db.SaveChanges();
            return Json(AjaxResult.Success());
        }
        #endregion

        #region 出货单
        public ActionResult Delivery()
        {
            return RedirectToAction("gridview", "manage", new { menuno = "DeliveryMaster" });
        }

        [Module("DeliveryMaster", "query")]
        public ActionResult GetDelivery(string CustomerAttribName, bool noReceived = false)
        {
            string sql = @"SELECT     a.DeliveryId, a.DeliveryDate, a.CustomerId, a.SalesManId, CASE DeliveryProperty WHEN 1 THEN '进货' ELSE '退货' END AS DeliveryProperty, a.DeliveryAddress, 
                      a.InvoiceNo, a.CustomerOrderNo, a.SubTotal, a.ValueAddTax, a.Amount, a.AccountReceivable, a.Received, a.LimitDate, a.CarNo, a.Tel, a.OpDate, 
                      b.CustomerAttribName
                        FROM         Order.DeliveryMaster AS a INNER JOIN
                      Order.Customer AS b ON a.CustomerId = b.CustomerId ";
            sql += string.IsNullOrEmpty(CustomerAttribName) ? " and 1=1" : " and CustomerAttribName like  '%" + CustomerAttribName + "%'";
            sql += noReceived ? " and  Received <=0" : " and 1=1";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        public ActionResult GetDeliveryById(string DeliveryId)
        {
            string sql = @"SELECT * from DeliveryMaster  where DeliveryId = '" + DeliveryId + "'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        public ActionResult GetDeliveryDetail(string DeliveryId)
        {
            string sql = @"SELECT a.*,ProductName from DeliveryDetail a,product b  where a.productid=b.productid and  DeliveryId = '" + DeliveryId + "'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        public string GenMaxDeliveryId()
        {
            var DeliveryId = db.DeliveryMasters.Where(c => c.DeliveryDate == DateTime.Now).Max(c => c.DeliveryId);
            return String.Format("CHD{0:yyMMdd}{1:D3}",
                DateTime.Now,
                Convert.ToInt32(DeliveryId == null ? "0" : DeliveryId.Substring(9, 3)) + 1);
        }

        public ActionResult GetNewDeliveryId()
        {
            return new Result(new { DeliveryId = GenMaxDeliveryId(), DeliveryProperty = 1, DeliveryDate = DateTime.Now });
        }

        [Module("DeliveryMaster", "print")]
        public ActionResult PrintDelivery(string DeliveryId)
        {
            var master = db.DeliveryMasters.First(c => c.DeliveryId == DeliveryId);
            ViewData["master"] = master;
            ViewData["detail"] = db.DeliveryDetails.Where(c => c.DeliveryId == DeliveryId).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult AddDelivery()
        {
            return View("EditDelivery");
        }

        [HttpPost]
        [Module("DeliveryMaster", "add")]
        public ActionResult AddDelivery(FormCollection form)
        {
            var dm = new DeliveryMaster();
            var ret = AddOrEditDelivery(form, true, dm);

            return Json(ret);
        }

        [HttpGet]
        public ActionResult EditDelivery(string DeliveryId)
        {
            ViewData["id"] = string.IsNullOrEmpty(DeliveryId) ? "" : DeliveryId;
            return View();
        }

        [HttpPost]
        [Module("DeliveryMaster", "edit")]
        public ActionResult EditDelivery(FormCollection form)
        {
            var DeliveryId = form["DeliveryId"];
            var dm = db.DeliveryMasters.First(c => c.DeliveryId == DeliveryId);

            if (dm.DeliveryDate < DateTime.Now.AddMonths(-2))
            {
                return Json(AjaxResult.Error("不允许编辑或删除2个月前的单据!"));
            }

            if (dm.Received != 0)
            {
                return Json(AjaxResult.Error("不允许编辑已收款的单据!"));
            }

            var ret = AddOrEditDelivery(form, false, dm);
            return Json(ret);
        }

        public AjaxResult AddOrEditDelivery(FormCollection form, bool isNew, DeliveryMaster dm)
        {

            TryUpdateModel(dm);
            dm.CreateDate = DateTime.Now;

            if (isNew)
            {
                //处理同时增加id重复问题
                dm.DeliveryId = GenMaxDeliveryId();
                db.DeliveryMasters.Add(dm);
            }

            if (!isNew)
            {
                db.DeliveryDetails.RemoveRange(db.DeliveryDetails.Where(c => c.DeliveryId == dm.DeliveryId));
                dm.DeliveryDetails.Clear();
            }

            if (!string.IsNullOrEmpty(form["DetailGrid"]))
            {
                var detail = JsonConvert.DeserializeObject<List<DeliveryDetail>>(form["DetailGrid"]);
                if (detail.Count <= 0)
                {
                    return AjaxResult.Error("无从单,无法保存!");
                }

                //处理库存
                foreach (var d in detail)
                {
                    if (!isNew)
                    {
                        //清除该单据库存表 退货时batchno不等于单据的id
                        db.ProductStocks.RemoveRange(db.ProductStocks.Where(c => c.BatchNo == d.BatchNo && c.OrderId == dm.DeliveryId
                            && c.ProductId == d.ProductId && c.Type == "C"));
                    }
                    if (string.IsNullOrEmpty(d.BatchNo))
                    {
                        d.BatchNo = dm.DeliveryId;
                    }
                    ChangeProductStock(d.ProductId, d.BatchNo, -d.Quantity??0, d.UnitPrice, -d.Weight??0, dm.DeliveryProperty, "C", dm.DeliveryId);
                }
                dm.DeliveryDetails.ToList().AddRange(detail);
            }
            else
            {
                return AjaxResult.Error("无从单,无法保存!");
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

            db.SaveChanges();

            if (dm.Received >= dm.Receivable)
            {
                return AjaxResult.Success();
            }
            else
            {
                //用于直接付款
                return AjaxResult.Success(dm.DeliveryId);
            }
        }

        [HttpPost]
        [Module("DeliveryMaster", "del")]
        public ActionResult DelDelivery(string DeliveryId)
        {
            var dm = db.DeliveryMasters.First(c => c.DeliveryId == DeliveryId);
            if (dm.DeliveryDate < DateTime.Now.AddMonths(-2))
            {
                return Json(AjaxResult.Error("不允许编辑或删除2个月前的单据!"));
            }
            //处理库存
            var detail = db.DeliveryDetails.Where(c => c.DeliveryId == DeliveryId);
            foreach (var d in detail)
            {
                //清除该单据库存表 退货时batchno不等于单据的id
                db.ProductStocks.RemoveRange(db.ProductStocks.Where(c => c.BatchNo == d.BatchNo && c.OrderId == dm.DeliveryId
                    && c.ProductId == d.ProductId && c.Type == "C"));
            }

            db.DeliveryDetails.RemoveRange(detail);
            db.DeliveryMasters.Remove(db.DeliveryMasters.First(c => c.DeliveryId == DeliveryId));
            db.SaveChanges();
            return Json(AjaxResult.Success());
        }

        #endregion

        #region 收款单
        public ActionResult Receive()
        {
            return RedirectToAction("gridview", "manage", new { menuno = "ReceiveMasters" });
        }

        [Module("ReceiveMasters", "query")]
        public ActionResult GetReceive(string CustomerAttribName)
        {
            string sql = @"SELECT a.*,CustomerAttribName from Order.ReceiveMasters a,Customer b  where a.CustomerId=b.CustomerId ";
            sql += string.IsNullOrEmpty(CustomerAttribName) ? " and 1=1" : " and CustomerAttribName like  '%" + CustomerAttribName + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        public ActionResult GetReceiveById(string ReceiveId)
        {
            string sql = @"SELECT * from Order.ReceiveMasters  where ReceiveId = '" + ReceiveId + "'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        public ActionResult GetReceiveDetail(string ReceiveId)
        {
            string sql = @"SELECT * from Order.ReceiveDetails   where   ReceiveId = '" + ReceiveId + "'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        public ActionResult GetNewReceiveId()
        {
            var ReceiveId = db.ReceiveMasters.Max(c => c.ReceiveId);
            ReceiveId = String.Format("{0:D8}", Convert.ToInt32(ReceiveId) + 1);
            return new Result(new { ReceiveId = ReceiveId, ReceiveDate = DateTime.Now });
        }

        [HttpGet]
        public ActionResult AddReceive()
        {
            return View("EditReceive");
        }

        [HttpPost]
        [Module("ReceiveMaster", "add")]
        public ActionResult AddReceive(FormCollection form)
        {
            var arm = new ReceiveMaster();
            AddOrEditReceive(form, true, arm);

            return Json(AjaxResult.Success(arm.Type));
        }

        [HttpGet]
        public ActionResult EditReceive(string ReceiveId)
        {
            ViewData["id"] = string.IsNullOrEmpty(ReceiveId) ? "" : ReceiveId;
            return View();
        }

        [HttpPost]
        //[Module("ReceiveMasters", "edit")]
        public ActionResult EditReceive(FormCollection form)
        {
            var ReceiveId = form["ReceiveId"];
            var arm = db.ReceiveMasters.First(c => c.ReceiveId == ReceiveId);

            if (arm.ReceiveDate < DateTime.Now.AddMonths(-2))
            {
                return Json(AjaxResult.Error("不允许编辑或删除2个月前的单据!"));
            }

            AddOrEditReceive(form, false, arm);
            return Json(AjaxResult.Success(arm.Type));
        }

        public void AddOrEditReceive(FormCollection form, bool isNew, ReceiveMaster arm)
        {
            TryUpdateModel(arm); ;

            if (isNew)
            {
                db.ReceiveMasters.Add(arm);
            }
            if (!isNew)
            {
                db.ReceiveDetails.RemoveRange(db.ReceiveDetails.Where(c => c.ReceiveId == arm.ReceiveId));
                arm.ReceiveDetails.Clear();
            }

            var detail = JsonConvert.DeserializeObject<List<ReceiveDetail>>(form["DetailGrid"]);

            CalcReceive(ref arm, ref detail, arm.CustomerId);
            db.SaveChanges();
        }

        [HttpPost]
        public ActionResult AddReceiveInDelivery(FormCollection form)
        {
            var dm = db.DeliveryMasters.First(c => c.DeliveryId == form["DeliveryId"]);
            var CustomerId = dm.CustomerId;
            var arm = new ReceiveMaster();

            arm.ReceiveDate = DateTime.Now;
            var ReceiveId = db.ReceiveMasters.Max(c => c.ReceiveId);
            ReceiveId = String.Format("{0:D8}", Convert.ToInt32(ReceiveId) + 1);
            arm.ReceiveId = ReceiveId;
            arm.CustomerId = CustomerId;

            TryUpdateModel(arm);
            db.ReceiveMasters.Add(arm);

            var ard = new ReceiveDetail();
            ard.DeliveryId = dm.DeliveryId;
            ard.ReceiveId = arm.ReceiveId;
            ard.Balance = dm.Receivable;

            var detail = new List<ReceiveDetail>();
            detail.Add(ard);

            CalcReceive(ref arm, ref detail, CustomerId);
            db.SaveChanges();
            return Json(AjaxResult.Success(arm.Type));
        }

        public void CalcReceive(ref ReceiveMaster arm, ref List<ReceiveDetail> detail, int CustomerId)
        {
            arm.CreateDate = DateTime.Now;
            arm.ReceiveDetails.ToList().AddRange(detail);
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
            Customer ct = db.Customers.First(c => c.CustomerId == CustomerId);
            ct.Advance += (arm.ReceiveAmount - arm.TotalBalance) - arm.AdvancePay;

        }

        [HttpPost]
        [Module("ReceiveMasters", "del")]
        //考虑下收款单删除 账务回退反算
        public ActionResult DelReceive(string ReceiveId)
        {
            var arm = db.ReceiveMasters.First(c => c.ReceiveId == ReceiveId);
            if (arm.ReceiveDate < DateTime.Now.AddMonths(-2))
            {
                return Json(AjaxResult.Error("不允许编辑或删除2个月前的单据!"));
            }

            Customer ct = db.Customers.Where(c => c.CustomerId == arm.CustomerId).First();
            ct.Advance -= (arm.ReceiveAmount - arm.TotalBalance) - arm.AdvancePay;

            db.ReceiveDetails.RemoveRange(db.ReceiveDetails.Where(c => c.ReceiveId == ReceiveId));
            db.ReceiveMasters.Remove(db.ReceiveMasters.First(c => c.ReceiveId == ReceiveId));
            db.SaveChanges();
            return Json(AjaxResult.Success());
        }

        #endregion

        #region 付款单
        public ActionResult Payment()
        {
            return RedirectToAction("gridview", "manage", new { menuno = "PaymentMaster" });
        }

        [Module("PaymentMaster", "query")]
        public ActionResult GetPayment(string SupplierAttribName, string SupplierName)
        {
            string sql = @"SELECT a.*,SupplierAttribName from Order.PaymentMaster a,supplier b  where a.SupplierId= b.SupplierId ";
            sql += string.IsNullOrEmpty(SupplierAttribName) ? " and 1=1" : " and SupplierAttribName like  '%" + SupplierAttribName + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        public ActionResult GetPaymentById(string PaymentId)
        {
            string sql = @"SELECT * from Order.PaymentMaster  where PaymentId = '" + PaymentId + "'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        public ActionResult GetPaymentDetail(string PaymentId)
        {
            string sql = @"SELECT * from Order.PaymentDetails  where   PaymentId = '" + PaymentId + "'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        public ActionResult GetNewPaymentId()
        {
            var PaymentId = db.PaymentMasters.Max(c => c.PaymentId);
            PaymentId = String.Format("{0:D8}", Convert.ToInt32(PaymentId) + 1);
            return new Result(new { PaymentId = PaymentId, PayDate = DateTime.Now });
        }

        [HttpGet]
        public ActionResult AddPayment()
        {
            return View("EditPayment");
        }

        [HttpPost]
        [Module("PaymentMaster", "add")]
        public ActionResult AddPayment(FormCollection form)
        {
            var apm = new PaymentMaster();
            AddOrEditPayment(form, true, apm);

            return Json(AjaxResult.Success(apm.Type));
        }

        [HttpGet]
        public ActionResult EditPayment(string PaymentId)
        {
            ViewData["id"] = string.IsNullOrEmpty(PaymentId) ? "" : PaymentId;
            return View();
        }

        [HttpPost]
        //[Module("PaymentMaster", "edit")]
        public ActionResult EditPayment(FormCollection form)
        {
            var PaymentId = form["PaymentId"];
            var apm = db.PaymentMasters.First(c => c.PaymentId == PaymentId);

            if (apm.PayDate < DateTime.Now.AddMonths(-2))
            {
                return Json(AjaxResult.Error("不允许编辑或删除2个月前的单据!"));
            }

            AddOrEditPayment(form, false, apm);
            return Json(AjaxResult.Success(apm.Type));
        }

        public void AddOrEditPayment(FormCollection form, bool isNew, PaymentMaster apm)
        {

            TryUpdateModel(apm);
            apm.CreateDate = DateTime.Now;

            if (isNew)
            {
                db.PaymentMasters.Add(apm);
            }
            if (!isNew)
            {
                db.PaymentDetails.RemoveRange(db.PaymentDetails.Where(c => c.PaymentId == apm.PaymentId));
                apm.PaymentDetails.Clear();
            }

            if (!string.IsNullOrEmpty(form["DetailGrid"]))
            {
                var detail = JsonConvert.DeserializeObject<List<PaymentDetail>>(form["DetailGrid"]);
                apm.PaymentDetails.ToList().AddRange(detail);
            }

            //计算合计
            apm.TotalBalance = apm.PaymentDetails.Sum(c => c.Balance);
            apm.PayAmount = apm.PayCash + apm.PayCheck + apm.AccountAmt - apm.Discount + apm.Remittance + apm.Prepayment + apm.Others;

            //判断单据类型

            if (apm.PaymentDetails.Count <= 0)
            {
                apm.Type = "预收款单";
            }
            else if (apm.PayAmount > apm.TotalBalance)
            {
                apm.Type = "收款大于应收";
            }
            else if (apm.PayAmount < apm.TotalBalance)
            {
                apm.Type = "收款小于应收";
            }
            else
            {
                apm.Type = "付款等于应付";
            }
            db.SaveChanges();
        }

        [HttpPost]
        [Module("PaymentMaster", "del")]
        public ActionResult DelPayment(string PaymentId)
        {
            var apm = db.PaymentMasters.First(c => c.PaymentId == PaymentId);
            if (apm.PayDate < DateTime.Now.AddMonths(-2))
            {
                return Json(AjaxResult.Error("不允许编辑或删除2个月前的单据!"));
            }

            db.PaymentDetails.RemoveRange(db.PaymentDetails.Where(c => c.PaymentId == PaymentId));
            db.PaymentMasters.Remove(db.PaymentMasters.First(c => c.PaymentId == PaymentId));
            db.SaveChanges();
            return Json(AjaxResult.Success());
        }

        #endregion

    }
}

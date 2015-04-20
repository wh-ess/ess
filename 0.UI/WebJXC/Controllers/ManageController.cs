using ESS.Domain.JXC.Models;
//using ESS.Domain.POP.Models;
using ESS.Framework.UI;
using ESS.Framework.UI.Attribute;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebJXC.Controllers
{
    [Log]
    public class ManageController : Controller
    {
        private ESS_ERPContext db = new ESS_ERPContext();
        //private ESS.Domain.POP.ESS_POPDB popdb = new ESS_POPDB();
        //private ESS.Domain.POP.Models.ESS_POSDataContext popdb = new ESS.Domain.POP.Models.ESS_POSDataContext();

        public ActionResult GridView(string menuno)
        {
            var menu = db.Menus.FirstOrDefault(c => c.MenuNo == menuno);
            if (menu != null)
            {
                ViewData["menuId"] = menu.MenuId;
                ViewData["t"] = menu.Title;
                ViewData["moduleNo"] = menuno;
            }
            return View();
        }

        #region query
        public ActionResult QueryView(string Id)
        {
            ViewData["p"] = Id;
            var menu = db.Menus.FirstOrDefault(c => c.MenuNo == Id);
            ViewData["title"] = menu != null ? menu.Title : "";
            return View();
        }

        [HttpPost]
        public ActionResult QueryView(FormCollection form)
        {
            return new Result(new SqlHelper().GetSqlResult(form));
        }

        [HttpGet]
        public ActionResult GetExcel()
        {
            NameValueCollection nv = new NameValueCollection(HttpContext.Request.QueryString);
            return new Result(new SqlHelper().GetSqlResult(nv), ResultType.Excel);
        }

        #endregion

        #region DdlControl
        public ActionResult DdlControl()
        {
            return RedirectToAction("GridView", "manage", new { menuno = "DdlControl" });
        }

        [Module("DdlControl", "query")]
        public ActionResult GetDdlControl(string Name)
        {
            string sql = @"SELECT * from Foundation.DdlControl  where ";
            sql += string.IsNullOrEmpty(Name) ? "1=1" : "Name like  '%" + Name + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        [HttpPost]
        [Module("DdlControl", "add")]
        public ActionResult AddDdlControl(FormCollection form)
        {
            DdlControl m = new DdlControl();
            TryUpdateModel(m);
            db.DdlControls.Add(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("DdlControl", "edit")]
        public ActionResult EditDdlControl(FormCollection form)
        {
            var id = (Convert.ToInt32(form["Id"]));
            DdlControl m = db.DdlControls.First(c => c.Id == id);
            TryUpdateModel(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("DdlControl", "del")]
        public ActionResult DelDdlControl(int Id)
        {
            DdlControl m = db.DdlControls.First(c => c.Id == Id);
            db.DdlControls.Remove(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion

        #region Menu
        public ActionResult Menu()
        {
            return RedirectToAction("GridView", "manage", new { menuno = "Menu" });
        }

        [Module("Menu", "query")]
        public ActionResult GetMenu(string menuno, string title)
        {
            string sql = @"SELECT * from Foundation.Menu  where ";
            sql += string.IsNullOrEmpty(menuno) ? "1=1" : "menuno like '%" + menuno + "%'";
            sql += string.IsNullOrEmpty(title) ? " and 1=1" : " and title like '%" + title + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        [HttpPost]
        [Module("Menu", "add")]
        public ActionResult AddMenu(FormCollection form)
        {
            Menu m = new Menu();
            TryUpdateModel(m);
            db.Menus.Add(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Menu", "edit")]
        public ActionResult EditMenu(FormCollection form)
        {
            var menuid = Convert.ToInt32(form["MenuId"]);
            Menu m = db.Menus.First(c => c.MenuId == menuid);
            TryUpdateModel(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Menu", "del")]
        public ActionResult DelMenu(int MenuId)
        {
            Menu m = db.Menus.First(c => c.MenuId == MenuId);
            db.Menus.Remove(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion

        #region User
        public new ActionResult User()
        {
            return RedirectToAction("GridView", "manage", new { menuno = "User" });
        }

        [Module("User", "query")]
        public ActionResult GetUser(string UserId, string UserName)
        {
            string sql = @"SELECT * from foundation.[User]  where ";
            sql += string.IsNullOrEmpty(UserId) ? "1=1" : "UserId = '" + UserId + "'";
            sql += string.IsNullOrEmpty(UserName) ? " and 1=1" : " and UserName like '%" + UserName + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        [HttpPost]
        [Module("User", "add")]
        public ActionResult AddUser(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                User m = new User();
                TryUpdateModel(m);
                db.Users.Add(m);
                db.SaveChanges();
                return new Result(AjaxResult.Success());
            }
            return new Result(AjaxResult.Error());
        }

        [HttpPost]
        [Module("User", "edit")]
        public ActionResult EditUser(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var usrid=Convert.ToInt32(form["UserId"]);
                User m = db.Users.First(c => c.UserId == usrid);
                TryUpdateModel(m);
                db.SaveChanges();
                return new Result(AjaxResult.Success());
            }
            return new Result(AjaxResult.Error());
        }

        [HttpPost]
        [Module("User", "del")]
        public ActionResult DelUser(int UserId)
        {
            User m = db.Users.First(c => c.UserId == UserId);
            db.Users.Remove(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion

        #region Role
        public ActionResult Role()
        {
            return RedirectToAction("GridView", "manage", new { menuno = "Role" });
        }

        [Module("Role", "query")]
        public ActionResult GetRole(string RoleName)
        {
            string sql = @"SELECT * from Foundation.Role  where ";
            sql += string.IsNullOrEmpty(RoleName) ? "  1=1" : "  UserName like '%" + RoleName + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        [HttpPost]
        [Module("Role", "add")]
        public ActionResult AddRole(FormCollection form)
        {
            Role m = new Role();
            TryUpdateModel(m);
            db.Roles.Add(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Role", "edit")]
        public ActionResult EditRole(FormCollection form)
        {
            var roleid = Convert.ToInt32(form["RoleId"]);
            Role m = db.Roles.First(c => c.RoleId == roleid);
            TryUpdateModel(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Role", "del")]
        public ActionResult DelRole(int RoleId)
        {
            Role m = db.Roles.First(c => c.RoleId == RoleId);
            db.Roles.Remove(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion

        #region Privilege
        public ActionResult Privilege()
        {
            return View();
        }

        public ActionResult GetRoleUsers(int RoleId)
        {
            var sql = "SELECT * from Foundation.[User] where userId in (select distinct userId from Foundation.User_Role where roleId=" + RoleId + ")";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        public ActionResult GetRoleRights(int RoleId)
        {
            var sql = "SELECT * from Foundation.Privilege where roleId=" + RoleId;
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        [HttpPost]
        public ActionResult SaveRoleUser(int roleId, string data)
        {
            IList<User> list = JsonConvert.DeserializeObject<List<User>>(data);
            var role = db.Roles.FirstOrDefault(c => c.RoleId == roleId);
            if (role != null) role.Users = list;
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        public ActionResult SaveRoleRights(int roleId, string data)
        {
            IList<ModuleAttribute> list = JsonConvert.DeserializeObject<List<ModuleAttribute>>(data);
            db.Privileges.RemoveRange(db.Privileges.Where(c => c.RoleId == roleId));

            foreach (var m in list)
            {
                var priv = new Privilege();
                priv.RoleId = roleId;
                priv.ModuleNo = m.ModuleNo;
                priv.ActionName = m.Action;
                db.Privileges.Add(priv);
            }
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion

        #region Customer
        public ActionResult Customer()
        {
            return RedirectToAction("GridView", "manage", new { menuno = "Customer" });
        }

        [Module("Customer", "query")]
        public ActionResult GetCustomer(string CustomerName)
        {
            string sql = @"SELECT * from Common.Customer  where ";
            sql += string.IsNullOrEmpty(CustomerName) ? "1=1" : "CustomerName like  '%" + CustomerName + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        [HttpPost]
        [Module("Customer", "add")]
        public ActionResult AddCustomer(FormCollection form)
        {
            Customer m = new Customer();
            TryUpdateModel(m);
            m.Advance = 0;
            db.Customers.Add(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Customer", "edit")]
        public ActionResult EditCustomer(FormCollection form)
        {
            Customer m = db.Customers.First(c => c.CustomerId == Convert.ToInt32(form["CustomerId"]));
            TryUpdateModel(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Customer", "del")]
        public ActionResult DelCustomer(int CustomerId)
        {
            Customer m = db.Customers.First(c => c.CustomerId == CustomerId);
            db.Customers.Remove(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion

        #region SalesMan
        public ActionResult SalesMan()
        {
            return RedirectToAction("GridView", "manage", new { menuno = "SalesMan" });
        }

        [Module("SalesMan", "query")]
        public ActionResult GetSalesMan(string SalesManName)
        {
            string sql = @"SELECT * from Common.SalesMan  where ";
            sql += string.IsNullOrEmpty(SalesManName) ? "1=1" : "ChineseName like  '%" + SalesManName + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        [HttpPost]
        [Module("SalesMan", "add")]
        public ActionResult AddSalesMan(FormCollection form)
        {
            SalesMan m = new SalesMan();
            TryUpdateModel(m);
            db.SalesMen.Add(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("SalesMan", "edit")]
        public ActionResult EditSalesMan(FormCollection form)
        {
            SalesMan m = db.SalesMen.First(c => c.SalesManId == Convert.ToInt32(form["SalesManId"]));
            TryUpdateModel(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("SalesMan", "del")]
        public ActionResult DelSalesMan(int SalesManId)
        {
            SalesMan m = db.SalesMen.First(c => c.SalesManId == SalesManId);
            db.SalesMen.Remove(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion

        #region Bank
        public ActionResult Bank()
        {
            return RedirectToAction("GridView", "manage", new { menuno = "Bank" });
        }

        [Module("Bank", "query")]
        public ActionResult GetBank(string BankName)
        {
            string sql = @"SELECT * from Common.Bank  where ";
            sql += string.IsNullOrEmpty(BankName) ? "1=1" : "BankName like  '%" + BankName + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        [HttpPost]
        [Module("Bank", "add")]
        public ActionResult AddBank(FormCollection form)
        {
            Bank m = new Bank();
            TryUpdateModel(m);
            db.Banks.Add(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Bank", "edit")]
        public ActionResult EditBank(FormCollection form)
        {
            Bank m = db.Banks.First(c => c.BankId == Convert.ToInt32(form["BankId"]));
            TryUpdateModel(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Bank", "del")]
        public ActionResult DelBank(int BankId)
        {
            Bank m = db.Banks.First(c => c.BankId == BankId);
            db.Banks.Remove(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion

        #region Product
        public ActionResult Product()
        {
            return RedirectToAction("GridView", "manage", new { menuno = "Product" });
        }

        [Module("Product", "query")]
        public ActionResult GetProduct(string ProductName)
        {
            string sql = @"SELECT * from Common.Product  where ";
            sql += string.IsNullOrEmpty(ProductName) ? "1=1" : "ProductName like  '%" + ProductName + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }


        [HttpPost]
        [Module("Product", "add")]
        public ActionResult AddProduct(FormCollection form)
        {
            Product m = new Product();
            TryUpdateModel(m);
            db.Products.Add(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Product", "edit")]
        public ActionResult EditProduct(FormCollection form)
        {
            Product m = db.Products.First(c => c.ProductId == Convert.ToInt32(form["ProductId"]));
            TryUpdateModel(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Product", "del")]
        public ActionResult DelProduct(int ProductId)
        {
            Product m = db.Products.First(c => c.ProductId == ProductId);
            db.Products.Remove(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion

        #region ProductClass
        public ActionResult ProductClass()
        {
            return RedirectToAction("GridView", "manage", new { menuno = "ProductClass" });
        }

        [Module("ProductClass", "query")]
        public ActionResult GetProductClass(string ClassName)
        {
            string sql = @"SELECT * from Common.ProductClass  where ";
            sql += string.IsNullOrEmpty(ClassName) ? "1=1" : "ClassName like  '%" + ClassName + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }


        [HttpPost]
        [Module("ProductClass", "add")]
        public ActionResult AddProductClass(FormCollection form)
        {
            ProductClass m = new ProductClass();
            TryUpdateModel(m);
            db.ProductClasses.Add(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("ProductClass", "edit")]
        public ActionResult EditProductClass(FormCollection form)
        {
            ProductClass m = db.ProductClasses.First(c => c.ClassId == Convert.ToInt32(form["ClassId"]));
            TryUpdateModel(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("ProductClass", "del")]
        public ActionResult DelProductClass(int ClassId)
        {
            ProductClass m = db.ProductClasses.First(c => c.ClassId == ClassId);
            db.ProductClasses.Remove(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion

        #region Brand
        public ActionResult Brand()
        {
            return RedirectToAction("GridView", "manage", new { menuno = "Brand" });
        }

        [Module("Brand", "query")]
        public ActionResult GetBrand(string BrandName)
        {
            string sql = @"SELECT * from Common.Brand  where ";
            sql += string.IsNullOrEmpty(BrandName) ? "1=1" : "BrandName like  '%" + BrandName + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }


        [HttpPost]
        [Module("Brand", "add")]
        public ActionResult AddBrand(FormCollection form)
        {
            Brand m = new Brand();
            TryUpdateModel(m);
            db.Brands.Add(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Brand", "edit")]
        public ActionResult EditBrand(FormCollection form)
        {
            Brand m = db.Brands.First(c => c.BrandId == Convert.ToInt32(form["BrandId"]));
            TryUpdateModel(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Brand", "del")]
        public ActionResult DelBrand(int BrandId)
        {
            Brand m = db.Brands.First(c => c.BrandId == BrandId);
            db.Brands.Remove(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion

        #region Supplier
        public ActionResult Supplier()
        {
            return RedirectToAction("GridView", "manage", new { menuno = "Supplier" });
        }

        [Module("Supplier", "query")]
        public ActionResult GetSupplier(string SupplierAttribName, string SupplierName)
        {
            string sql = @"SELECT * from Common.Supplier  where ";
            sql += string.IsNullOrEmpty(SupplierName) ? "1=1" : "SupplierName like  '%" + SupplierName + "%'";
            sql += string.IsNullOrEmpty(SupplierAttribName) ? " and 1=1" : " and SupplierAttribName like  '%" + SupplierAttribName + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        [HttpPost]
        [Module("Supplier", "add")]
        public ActionResult AddSupplier(FormCollection form)
        {
            Supplier m = new Supplier();
            TryUpdateModel(m);
            m.Prepaid = 0;
            db.Suppliers.Add(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Supplier", "edit")]
        public ActionResult EditSupplier(FormCollection form)
        {
            Supplier m = db.Suppliers.First(c => c.SupplierId == Convert.ToInt32(form["SupplierId"]));
            TryUpdateModel(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Supplier", "del")]
        public ActionResult DelSupplier(int SupplierId)
        {
            Supplier m = db.Suppliers.First(c => c.SupplierId == SupplierId);
            db.Suppliers.Remove(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion

        //#region TemplateHead 模板头信息主要定义模板的主信息
        //public ActionResult POP_TemplateHead()
        //{
        //    return RedirectToAction("GridView", "manage", new { menuno = "TemplateHead" });
        //}

        //[Module("TemplateHead", "query")]
        //public ActionResult GetTemplateHead(string TemplateHeadName)
        //{
        //    string sql = @"SELECT * from Pop.TemplateHead  where ";
        //    sql += string.IsNullOrEmpty(TemplateHeadName) ? "1=1" : "Name like  '%" + TemplateHeadName + "%'";
        //    return new Result(new SqlHelper().GetSqlResult(sql));
        //}


        //[HttpPost]
        //[Module("TemplateHead", "add")]
        //public ActionResult AddTemplateHead(FormCollection form)
        //{
        //    TemplateHead m = new TemplateHead();
        //    m.Guid = Guid.NewGuid();
        //    TryUpdateModel(m);
        //    //popdb.TemplateHead.Add(m);

        //    popdb.TemplateHead.InsertOnSubmit(m);
        //    popdb.SubmitChanges();
        //    //popdb.SaveChanges();
        //    return new Result(AjaxResult.Success());
        //}

        //[HttpPost]
        //[Module("TemplateHead", "edit")]
        //public ActionResult EditTemplateHead(FormCollection form)
        //{
        //    TemplateHead m = popdb.TemplateHead.First(c => c.Guid == Guid.Parse(form["guid"]));
        //    TryUpdateModel(m);
        //    //popdb.SaveChanges();
        //    popdb.SubmitChanges();
        //    return new Result(AjaxResult.Success());
        //}

        //[HttpPost]
        //[Module("TemplateHead", "del")]
        //public ActionResult DelTemplateHead(Guid guid)
        //{
        //    TemplateHead m = popdb.TemplateHead.First(c => c.Guid == guid);
        //    //popdb.TemplateHead.Remove(m);
        //    popdb.TemplateHead.DeleteOnSubmit(m);
        //    popdb.SubmitChanges();
        //    //popdb.SaveChanges();
        //    return new Result(AjaxResult.Success());
        //}
        //#endregion

        //#region TemplateBody 模板体信息主要定义具体模板的版体设计和水印信息
        //public ActionResult POP_TemplateBody()
        //{
        //    return RedirectToAction("GridView", "manage", new { menuno = "TemplateBody" });
        //}

        //[Module("TemplateBody", "query")]
        //public ActionResult GetTemplateBody(string TemplateHeadName)
        //{
        //    string sql = @"SELECT * from Pop.TemplateBody  where ";
        //    sql += string.IsNullOrEmpty(TemplateHeadName) ? "1=1" : "Name like  '%" + TemplateHeadName + "%'";
        //    return new Result(new SqlHelper().GetSqlResult(sql));
        //}


        //[HttpPost]
        //[Module("TemplateBody", "add")]
        //public ActionResult AddTemplateBody(FormCollection form)
        //{
        //    TemplateBody m = new TemplateBody();
        //    m.Guid = Guid.NewGuid();
        //    TryUpdateModel(m);
        //    //popdb.TemplateHead.Add(m);

        //    popdb.TemplateBody.InsertOnSubmit(m);
        //    popdb.SubmitChanges();
        //    //popdb.SaveChanges();
        //    return new Result(AjaxResult.Success());
        //}

        //[HttpPost]
        //[Module("TemplateBody", "edit")]
        //public ActionResult EditTemplateBody(FormCollection form)
        //{
        //    TemplateBody m = popdb.TemplateBody.First(c => c.Guid == Guid.Parse(form["guid"]));
        //    TryUpdateModel(m);
        //    //popdb.SaveChanges();
        //    popdb.SubmitChanges();
        //    return new Result(AjaxResult.Success());
        //}

        //[HttpPost]
        //[Module("TemplateBody", "del")]
        //public ActionResult DelTemplateBody(Guid guid)
        //{
        //    TemplateBody m = popdb.TemplateBody.First(c => c.Guid == guid);
        //    //popdb.TemplateHead.Remove(m);
        //    popdb.TemplateBody.DeleteOnSubmit(m);
        //    popdb.SubmitChanges();
        //    //popdb.SaveChanges();
        //    return new Result(AjaxResult.Success());
        //}
        //#endregion

        //#region 定义促销档期空白模版信息
        //public ActionResult POP_EmptyTemplate()
        //{
        //    return RedirectToAction("GridView", "manage", new { menuno = "EmptyTemplate" });
        //}

        //[Module("EmptyTemplate", "query")]
        //public ActionResult GetEmptyTemplate(string TemplateHeadName)
        //{
        //    string sql = @"SELECT * from Pop.EmptyTemplate  where ";
        //    sql += string.IsNullOrEmpty(TemplateHeadName) ? "1=1" : "Name like  '%" + TemplateHeadName + "%'";
        //    return new Result(new SqlHelper().GetSqlResult(sql));
        //}


        //[HttpPost]
        //[Module("EmptyTemplate", "add")]
        //public ActionResult AddEmptyTemplate(FormCollection form)
        //{
        //    EmptyTemplate m = new EmptyTemplate();
        //    m.Guid = Guid.NewGuid();
        //    TryUpdateModel(m);
        //    //popdb.TemplateHead.Add(m);

        //    popdb.EmptyTemplate.InsertOnSubmit(m);
        //    popdb.SubmitChanges();
        //    //popdb.SaveChanges();
        //    return new Result(AjaxResult.Success());
        //}

        //[HttpPost]
        //[Module("TemplateBody", "edit")]
        //public ActionResult EditEmptyTemplate(FormCollection form)
        //{
        //    EmptyTemplate m = popdb.EmptyTemplate.First(c => c.Guid == Guid.Parse(form["guid"]));
        //    TryUpdateModel(m);
        //    //popdb.SaveChanges();
        //    popdb.SubmitChanges();
        //    return new Result(AjaxResult.Success());
        //}

        //[HttpPost]
        //[Module("EmptyTemplate", "del")]
        //public ActionResult DelEmptyTemplate(Guid guid)
        //{
        //    EmptyTemplate m = popdb.EmptyTemplate.First(c => c.Guid == guid);
        //    //popdb.TemplateHead.Remove(m);
        //    popdb.EmptyTemplate.DeleteOnSubmit(m);
        //    popdb.SubmitChanges();
        //    //popdb.SaveChanges();
        //    return new Result(AjaxResult.Success());
        //}
        //#endregion

    }
}

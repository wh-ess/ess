using ESS.Domain.Foundation;
using ESS.Domain.JXC;
using ESS.Domain.JXC.Models;
using ESS.Domain.POP;
using ESS.Domain.POP.Models;
using ESS.Framework.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESS.Framework.UI.Attribute;
using WebJXC.Controllers;

namespace WebJXC.Areas.POP.Controllers
{
    [Log]
    public class POPManageController : Controller
    {
        private ESS_POSDataContext popdb = new ESS_POSDataContext();

        #region TemplateHead 模板头信息主要定义模板的主信息
        public ActionResult POP_TemplateHead()
        {
            return RedirectToAction("GridView", "manage", new { Area = "", menuno = "TemplateHead" });
        }

        [Module("TemplateHead", "query")]
        public ActionResult GetTemplateHead(string Name)
        {
            string sql = @"SELECT * from Pop.TemplateHead  where ";
            sql += string.IsNullOrEmpty(Name) ? "1=1" : "Name like  '%" + Name + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        [HttpPost]
        [Module("TemplateHead", "add")]
        public ActionResult AddTemplateHead(FormCollection form)
        {
            TemplateHead m = new TemplateHead();
            m.Guid = Guid.NewGuid();
            TryUpdateModel(m);
            //popdb.TemplateHead.Add(m);

            popdb.TemplateHead.InsertOnSubmit(m);
            popdb.SubmitChanges();
            //popdb.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("TemplateHead", "edit")]
        public ActionResult EditTemplateHead(FormCollection form)
        {
            TemplateHead m = popdb.TemplateHead.First(c => c.Guid == Guid.Parse(form["guid"]));
            TryUpdateModel(m);
            //popdb.SaveChanges();
            popdb.SubmitChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("TemplateHead", "del")]
        public ActionResult DelTemplateHead(Guid guid)
        {
            TemplateHead m = popdb.TemplateHead.First(c => c.Guid == guid);
            //popdb.TemplateHead.Remove(m);
            popdb.TemplateHead.DeleteOnSubmit(m);
            popdb.SubmitChanges();
            //popdb.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        /// <summary>
        /// view 
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadTemplateHead()
        {
            return View();
        }

        /// <summary>
        /// 处理上传动作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadTemplateHead(HttpPostedFileBase Filedata)
        {
            // 如果没有上传文件
            if (Filedata == null ||
                string.IsNullOrEmpty(Filedata.FileName) ||
                Filedata.ContentLength == 0)
            {
                return this.HttpNotFound();
            }
            // 保存到 ~/Areas/POP/POPImages/PopTemplates 文件夹中，名称不变
            string filename = System.IO.Path.GetFileName(Filedata.FileName);
            string virtualPath = string.Format("/Areas/POP/POPImages/PopTemplates/{0}", filename);
            // 文件系统不能使用虚拟路径
            string path = this.Server.MapPath(virtualPath);

            Filedata.SaveAs(path);
            return this.Json(new { }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region TemplateBody 模板体信息主要定义具体模板的版体设计和水印信息
        public ActionResult POP_TemplateBody()
        {
            return RedirectToAction("GridView", "manage", new { Area = "", menuno = "TemplateBody" });
        }

        [Module("TemplateBody", "query")]
        public ActionResult GetTemplateBody(string Name, string HeadGuid)
        {
            string sql = @"SELECT * from Pop.TemplateBody  where ";
            sql += string.IsNullOrEmpty(Name) ? "1=1" : "Name like  '%" + Name + "%'";
            sql += " and ";
            sql += string.IsNullOrEmpty(HeadGuid) ? "1=1" : "HeadGuid='" + Guid.Parse(HeadGuid) + "'";
            sql += "order by headguid,seq";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        [HttpPost]
        [Module("TemplateBody", "add")]
        public ActionResult AddTemplateBody(FormCollection form)
        {
            TemplateBody m = new TemplateBody();
            m.Guid = Guid.NewGuid();
            TryUpdateModel(m);
            //popdb.TemplateHead.Add(m);

            popdb.TemplateBody.InsertOnSubmit(m);
            popdb.SubmitChanges();
            //popdb.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("TemplateBody", "edit")]
        public ActionResult EditTemplateBody(FormCollection form)
        {
            TemplateBody m = popdb.TemplateBody.First(c => c.Guid == Guid.Parse(form["guid"]));
            TryUpdateModel(m);
            //popdb.SaveChanges();
            popdb.SubmitChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("TemplateBody", "del")]
        public ActionResult DelTemplateBody(Guid guid)
        {
            TemplateBody m = popdb.TemplateBody.First(c => c.Guid == guid);
            //popdb.TemplateHead.Remove(m);
            popdb.TemplateBody.DeleteOnSubmit(m);
            popdb.SubmitChanges();
            //popdb.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion

        #region 定义促销档期空白模版信息
        public ActionResult POP_EmptyTemplate()
        {
            return RedirectToAction("GridView", "manage", new { Area = "", menuno = "EmptyTemplate" });
        }

        [Module("EmptyTemplate", "query")]
        public ActionResult GetEmptyTemplate(string TemplateHeadName)
        {
            string sql = @"SELECT * from Pop.EmptyTemplate  where ";
            sql += string.IsNullOrEmpty(TemplateHeadName) ? "1=1" : "Name like  '%" + TemplateHeadName + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }


        [HttpPost]
        [Module("EmptyTemplate", "add")]
        public ActionResult AddEmptyTemplate(FormCollection form)
        {
            EmptyTemplate m = new EmptyTemplate();
            m.Guid = Guid.NewGuid();
            TryUpdateModel(m);
            //popdb.TemplateHead.Add(m);

            popdb.EmptyTemplate.InsertOnSubmit(m);
            popdb.SubmitChanges();
            //popdb.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("EmptyTemplate", "edit")]
        public ActionResult EditEmptyTemplate(FormCollection form)
        {
            EmptyTemplate m = popdb.EmptyTemplate.First(c => c.Guid == Guid.Parse(form["Guid"]));
            TryUpdateModel(m);
            //popdb.SaveChanges();
            popdb.SubmitChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("EmptyTemplate", "del")]
        public ActionResult DelEmptyTemplate(Guid guid)
        {
            EmptyTemplate m = popdb.EmptyTemplate.First(c => c.Guid == guid);
            //popdb.TemplateHead.Remove(m);
            popdb.EmptyTemplate.DeleteOnSubmit(m);
            popdb.SubmitChanges();
            //popdb.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        public ActionResult UploadEmptyTemplate()
        {
            return View();
        }

        /// <summary>
        /// 处理上传动作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadEmptyTemplate(HttpPostedFileBase Filedata)
        {
            // 如果没有上传文件
            if (Filedata == null ||
                string.IsNullOrEmpty(Filedata.FileName) ||
                Filedata.ContentLength == 0)
            {
                return this.HttpNotFound();
            }
            // 保存到 ~/Areas/POP/POPImages/PopTemplates 文件夹中，名称不变
            string filename = System.IO.Path.GetFileName(Filedata.FileName);
            string virtualPath = string.Format("/Areas/POP/POPImages/EmptyTemplates/{0}", filename);
            // 文件系统不能使用虚拟路径
            string path = this.Server.MapPath(virtualPath);

            Filedata.SaveAs(path);
            return this.Json(new { }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 模版配置 TemplateConfig
        public ActionResult TemplateConfig()
        {
            return View();
        }

        public ActionResult GetTemplateBodyByGuid(string HeadGuid)
        {
            HeadGuid = HeadGuid.Substring(1, HeadGuid.Length - 2);
            string sql = @"SELECT * from Pop.TemplateBody  where ";
            sql += string.IsNullOrEmpty(HeadGuid) ? "1=1" : "HeadGuid='" + Guid.Parse(HeadGuid) + "'";
            sql += "order by headguid,seq";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase Filedata)
        {
            // 如果没有上传文件
            if (Filedata == null ||
                string.IsNullOrEmpty(Filedata.FileName) ||
                Filedata.ContentLength == 0)
            {
                return this.HttpNotFound();
            }
            // 保存到 ~/Areas/POP/POPImages/PopTemplates 文件夹中，名称不变
            string filename = System.IO.Path.GetFileName(Filedata.FileName);
            string virtualPath = string.Format("/Areas/POP/POPImages/PopTemplates/{0}", filename);
            // 文件系统不能使用虚拟路径
            string path = this.Server.MapPath(virtualPath);

            Filedata.SaveAs(path);
            return this.Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTemplateImagePath()
        {
            string imagePath = this.Server.MapPath("~/Areas/POP/POPImages/PopTemplates/");
            DirectoryInfo imgDir = new DirectoryInfo(imagePath);
            List<DdlControl> imageList = new List<DdlControl>();
            foreach (FileSystemInfo fsi in imgDir.GetFileSystemInfos())
            {
                DdlControl ddlc = new DdlControl();
                ddlc.Id = 1;
                ddlc.Name = "imagepath";
                ddlc.FieldText = fsi.Name.Replace('.', '_');
                //ddlc.FieldValue = fsi.FullName.Replace('.','_');
                ddlc.FieldValue = ddlc.FieldText;

                imageList.Add(ddlc);
                //imageList.Add(fsi.Name,fsi.FullName);
            }
            return Json(imageList, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region GenPOPByStepForm 向导模式套打辅助设计
        public ActionResult GenPOPByStepForm()
        {
            //有效可以控制
            List<TemplateHead> thlist = popdb.TemplateHead.Where(c => c.IsEnable == 1).OrderBy(c => c.Seq).ToList();
            List<DropdownList> ddllist = new List<DropdownList>();

            //配置读取路径端口 上线改参数即可
            SystemConfig sc = new SystemConfig();
            string serverPath = sc.GetConfigValue("ServerPath");

            foreach (var th in thlist)
            {
                DropdownList ddl = new DropdownList();
                ddl.id = th.Guid.ToString();
                ddl.text = serverPath + th.ImageFileDir + th.ImageFileName.Replace('_', '.');
                ddllist.Add(ddl);
            }
            ViewBag.ddllist = ddllist;

            List<EmptyTemplate> etlist = popdb.EmptyTemplate.Where(c => c.IsEnable == 1).OrderBy(c => c.Seq).ToList();
            List<DropdownList> emptytemplatelist = new List<DropdownList>();
            foreach (var et in etlist)
            {
                DropdownList ddl = new DropdownList();
                ddl.id = et.Guid.ToString();
                ddl.text = serverPath + et.ImageFileDir+ et.ImageFileName.Replace('_', '.');
                emptytemplatelist.Add(ddl);
            }

            ViewBag.emptytemplatelist = emptytemplatelist;
            return View();
        }


        /// <summary> 填表单生成POP 考虑用局部视图实现
        /// 集中判断，根据传入的模板GUID获取相应的处理HTML视图
        /// 返回局部视图至STEMFORM处理
        /// </summary>
        /// <param name="form">templatehead_guid</param>
        /// <param name="form">emptytemplate_guid</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult GetPOPView(FormCollection form)
        {
            TemplateHead th = popdb.TemplateHead.Where(c => c.Guid == Guid.Parse(form["currentTemplate"])).SingleOrDefault();
            //todo 判断模板的名称类型，调取相应的后台处理 视图，返回给前台生成
            //if (th.Name == "满额减现模板")
            //{
            //    //return HtmlHelper.
            //    //return PartialView("~/Views/Home/ViewUserControl.cshtml");
            //    return PartialView("/Areas/POP/Views/pv_GenPOPViews/partial_Template1.cshtml");
            //}
            if (th != null)
            {
                return PartialView(th.ParialViewDir + th.ParialViewName);
            }

            return PartialView();
        }

        /// <summary>
        /// GenPOP 使用FORM上传，不定参数，
        /// 后台规则:
        /// 第一个参数为templatehead guid,
        /// 第二参数为templateheaddir 以此来获取templatehead对象
        /// 其他为KEY VALUE的调用水印方法打印
        /// </summary>
        /// <returns>返回JSON 如果T表示成功，正常F失败</returns>
        [HttpPost]
        public JsonResult GenPOP()
        {
            int paraCount = this.Request.Form.Keys.Count;
            List<string> paraValues = new List<string>();
            //Guid templateHeadGuid = Guid.Parse(this.Request.Form.GetValues(0)[0]);
            Guid templateHeadGuid = Guid.Parse(this.Request.Form["templateHeadGuid"]);

            //Guid emptytemplateGuid = Guid.Parse(this.Request.Form.GetValues(1)[0]);
            Guid emptytemplateGuid = Guid.Parse(this.Request.Form["emptyTemplateGuid"]);

            EmptyTemplate et = popdb.EmptyTemplate.Where(c => c.Guid == emptytemplateGuid).SingleOrDefault();
            string emptyTemplateDir = et.ImageFileDir + et.ImageFileName.Replace('_', '.');

            for (int i = 2; i < paraCount; i++)
            {
                paraValues.Add(this.Request.Form.GetValues(i)[0]);
            }

            //第一参数为传入POP类型
            //第二参数为传入空白样式模板地址
            //均不到后台处理
            List<string> paraList = new List<string>();
            paraList = this.Request.Form.AllKeys.ToList();

            paraList.Remove("templateHeadGuid");
            paraList.Remove("emptyTemplateGuid");

            //paraList.RemoveAt(0);
            //paraList.RemoveAt(0);

            string brandName = paraValues[0].ToString();

            //for 循环拿到参数和值参照
            for (int i = 0; i < paraList.Count(); i++)
            {
                WaterMark wm = new WaterMark(paraList[i], paraValues[i], templateHeadGuid, brandName, emptyTemplateDir);
                wm.addWaterMark();
            }
            //return Json(emptyTemplateDir, JsonRequestBehavior.AllowGet);

            return Json('t', JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region GenPOPBySilerlight 自定义设计SLIVERLIGHT
        public ActionResult GenPOPBySilerlight()
        {
            return View();
        }
        #endregion


        [HttpPost]
        public ActionResult index(FormCollection form)
        {

            return View();
        }



        /// <summary>
        /// 获取POP方法
        /// </summary>
        /// <returns>FILE类型，前台页面保存下载</returns>
        [HttpGet]
        public ActionResult DownloadPOP(string POPName)
        {
            string[] fileNameList = POPName.Split('/');
            var fileName = fileNameList.Last();
            fileName = fileName.Substring(0, fileName.Length - 1);

            var path = Server.MapPath("/Areas/POP/POPImages/OutPOPImages/" + fileName);
            return File(path, "application/x-zip-commpressed", DateTime.Now.ToString() + fileName);
        }
    }
}

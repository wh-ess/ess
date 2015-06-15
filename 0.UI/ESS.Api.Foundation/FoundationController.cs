#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ESS.Framework.Common.Utilities;
using ESS.Framework.UI.Attribute;
using Path = System.IO.Path;

#endregion

namespace ESS.Api.Foundation
{
    /// <summary>
    /// for menu
    /// </summary>

    [Module(parentModuleNo: "", moduleNo: "Foundation")]
    public class FoundationController : ApiController
    {
        public void Init()
        {
            new InitData();
        }
    }

    public class AccessControlController : ApiController
    {
    }
    public class EntityConfigController : ApiController
    {
    }
    public class SystemConfigController : ApiController
    {
    }

    #region upload

    public class UploadController : ApiController
    {
        public async Task<List<string>> PostFormData()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/upload");
            var provider = new MultipartFormDataStreamProvider(root);

            var result = new List<string>();
            // Read the form data and return an async task.
            await Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }

                    // This illustrates how to get the file names.
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        string fileType = file.Headers.ContentType.MediaType.Replace("image/","");
                        var fileName = ObjectId.GetNextGuid() + "." + fileType;
                        File.Move(file.LocalFileName, Path.Combine(root, fileName));
                        result.Add(fileName);
                    }
                    return result;
                });
            return result;
        }
    }
    #endregion
}
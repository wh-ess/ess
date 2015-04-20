#region

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#endregion

namespace ESS.Framework.UI.ActionResultExtentions
{
    public class JsonNetResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null)
                return;

            IList<JsonConverter> converters = new List<JsonConverter>();
            var dateTimeConverter = new IsoDateTimeConverter();
            dateTimeConverter.DateTimeFormat = "yyyy-MM-dd";
            converters.Add(dateTimeConverter);

            var serializedObject = JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = converters,
                NullValueHandling = NullValueHandling.Ignore
                //PreserveReferencesHandling  = PreserveReferencesHandling.All
            });
            response.Write(serializedObject);
        }
    }
}
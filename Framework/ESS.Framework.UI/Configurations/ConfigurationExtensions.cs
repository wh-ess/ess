#region

using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Autofac;
using Autofac.Integration.WebApi;
using ESS.Framework.Common.Autofac;
using ESS.Framework.Common.Components;
using ESS.Framework.Common.Configurations;
using ESS.Framework.UI.Attribute;
using ESS.Framework.UI.Formatting.Jsonp;
using ESS.Framework.UI.Formatting.Xlsx;
using ESS.Framework.UI.Tracing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OfficeOpenXml.Style;
using Owin;

#endregion

namespace ESS.Framework.UI.Configurations
{
    /// <summary>
    ///     Configuration class for framework.
    /// </summary>
    public static class ConfigurationExtensions
    {
        public static Configuration RegisterController(this Configuration configuration, HttpConfiguration config, Assembly[] ass)
        {
            var objectContainer = ObjectContainer.Current as AutofacObjectContainer;
            if (objectContainer != null)
            {
                var container = objectContainer.Container;
                var builder = new ContainerBuilder();
                builder.RegisterApiControllers(ass);
                builder.Update(container);
                config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            }

            ModuleBuilder.Build(ass);

            return configuration;
        }

        public static Configuration UseWebApi(this Configuration configuration, IAppBuilder app, HttpConfiguration config)
        {
            //filter 
            //全局model验证
            config.Filters.Add(new ValidateModelAttribute());

            //formatting
            // Set up the XlsxMediaTypeFormatter
            var xlsxFormatter = new XlsxMediaTypeFormatter(autoFilter: true, freezeHeader: true, headerHeight: 25f,
                cellStyle: (ExcelStyle s) => s.Font.SetFromFont(new Font("Segoe UI", 13f, FontStyle.Regular)), headerStyle: (ExcelStyle s) =>
                {
                    s.Fill.PatternType = ExcelFillStyle.Solid;
                    s.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 114, 51));
                    s.Font.Color.SetColor(Color.White);
                    s.Font.Size = 15f;
                });

            // Add XlsxMediaTypeFormatter to the collection
            config.Formatters.Add(xlsxFormatter);
            config.AddJsonpFormatter();

            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new IsoDateTimeConverter(), new StringEnumConverter { CamelCaseText = true } }
            };

            //Trace
            //config.Services.Replace(typeof(ITraceWriter), new Log4NetTraceWriter());

            //ExceptionLogger
            config.Services.Add(typeof(IExceptionLogger), new Log4NetExceptionLogger());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            app.UseWebApi(config);

            return configuration;
        }
    }
}
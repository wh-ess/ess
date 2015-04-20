using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;

namespace ESS.Framework.UI.Link
{

    /// <summary>
    /// http://chimera.labs.oreilly.com/books/1234000001708/ch07.html#_retrieving_open_and_closed_issues
    /// </summary>
    public abstract class LinkFactory
    {
        private readonly UrlHelper _urlHelper;
        private readonly string _controllerName;
        private const string DefaultApi = "DefaultApi";

        protected LinkFactory(HttpRequestMessage request, Type controllerType) // <1>
        {
            _urlHelper = new UrlHelper(request); // <2>
            _controllerName = GetControllerName(controllerType);
        }

        protected Link GetLink<TController>(string rel, object id, string action,
            string route = DefaultApi) // <3>
        {
            var uri = GetUri(new
            {
                controller = GetControllerName(
                    typeof(TController)),
                id,
                action
            }, route);
            return new Link { Action = action, Href = uri, Rel = rel };
        }

        private string GetControllerName(Type controllerType) // <4>
        {
            var name = controllerType.Name;
            return name.Substring(0, name.Length - "controller".Length).ToLower();
        }

        protected Uri GetUri(object routeValues, string route = DefaultApi) // <5>
        {
            return new Uri(_urlHelper.Link(route, routeValues));
        }

        public Link Self(string id, string route = DefaultApi) // <6>
        {
            return new Link
            {
                Rel = Rels.Self,
                Href = GetUri(
                    new { controller = _controllerName, id = id }, route)
            };
        }

        public class Rels
        {
            public const string Self = "self";
        }
    }

    public abstract class LinkFactory<TController> : LinkFactory // <7>
    {
        protected LinkFactory(HttpRequestMessage request) :
            base(request, typeof(TController)) { }
    }

    
}

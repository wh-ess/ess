using System.Web.Mvc;

namespace WebJXC.Areas.POP
{
    public class POPAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "POP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "POP_default",
                "POP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

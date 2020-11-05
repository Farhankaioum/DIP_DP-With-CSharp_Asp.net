using System.Web.Mvc;

namespace AutofacAspNetMvcDemos
{
  // attribute itself does not need to be registered in the container
  public class LoggingActionFilter : ActionFilterAttribute
  {
    public ILogger Logger { get; set; }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      Logger.Log("Executing action!");
    }
  } // same goes for e.g. inheritors of AuthorizeAttribute
}
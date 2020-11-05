using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using AutofacAspNetMvcDemos.Controllers;
using RegistrationExtensions = Autofac.Builder.RegistrationExtensions;

namespace AutofacAspNetMvcDemos
{
  public class MvcApplication : System.Web.HttpApplication
  {
    public static void RegisterBundles(BundleCollection bundles)
    {
      bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css"));
    }

    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }

    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute(
        name: "Default",
        url: "{controller}/{action}/{id}",
        defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
      );
    }

    protected void Application_Start()
    {
      var builder = new ContainerBuilder();

      builder.RegisterType<TextFileLogger>().As<ILogger>();

      //builder.RegisterControllers(GetType().Assembly);
      builder.RegisterType<HomeController>()
        .InstancePerRequest();

      builder.RegisterModelBinders(GetType().Assembly);
      builder.RegisterModelBinderProvider();

      builder.RegisterModule<AutofacWebTypesModule>();

      builder.RegisterSource(new ViewRegistrationSource());

      builder.RegisterFilterProvider();

      var container = builder.Build();
      DependencyResolver.SetResolver(
        new AutofacDependencyResolver(container));

      AreaRegistration.RegisterAllAreas();
      RegisterGlobalFilters(GlobalFilters.Filters);
      RegisterRoutes(RouteTable.Routes);
      RegisterBundles(BundleTable.Bundles);
    }
  }
}

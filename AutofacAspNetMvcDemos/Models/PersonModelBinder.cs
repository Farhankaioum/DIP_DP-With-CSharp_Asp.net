using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac.Integration.Mvc;

namespace AutofacAspNetMvcDemos.Models
{
  [ModelBinderType(typeof(Person))] // type the binder is to be registered for
  public class PersonModelBinder
  {
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      var request = controllerContext.HttpContext.Request;

      string firstName = request.Form.Get("firstName");
      string middleName = request.Form.Get("middleName");
      string lastName = request.Form.Get("lastName");
      int Age = Convert.ToInt32(request.Form.Get("age"));
      return new Person { Name = $"{firstName} {middleName} {lastName}", Age = Age };
    }
  }
}
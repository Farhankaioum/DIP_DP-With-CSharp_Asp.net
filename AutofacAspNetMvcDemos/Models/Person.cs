using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutofacAspNetMvcDemos.Models
{
  public class Person
  {
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please specify age")]
    [Range(1,150)]
    public int Age { get; set; }
    
  }
}
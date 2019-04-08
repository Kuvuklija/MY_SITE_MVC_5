using MyFirstSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFirstSite.WebUI.Models{

    public class CartViewModel{
        public Cart Cart{ get; set; }
        public string ReturnUrl { get; set; }
    }
}
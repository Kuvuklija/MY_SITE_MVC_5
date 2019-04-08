using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSite.Domain.Entities{

    public class ShippingDetail{

        [Required(ErrorMessage="Please enter a name")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Please enter the last name")]
        [Display(Name="Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Please enter your email address")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage ="Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please enter your phone number")]
        [RegularExpression(@"(\+7|8|\b)[\(\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[)\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)",ErrorMessage ="Please enter a valid phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage ="Please enter your address")]
        [Display(Name = "Home address")]
        public string Address { get; set; }

        public string Zip { get; set; }

        public bool GiftWrap { get; set; }
    }
}

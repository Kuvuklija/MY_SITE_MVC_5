using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSite.Domain.YandexMoney{

    public class ClientOrder{
        
        public string Id { get; set; } //id order
        public DateTime? Date { get; set; }//date
        public decimal Sum { get; set; }//order sum
        public string Sender { get; set; }//sender-purse in YD
        public decimal? Amount { get; set; }//sum that has payed with commission
        public decimal? WithdrawAmount { get; set; }//sum that has payed without commission
        public int? UserId { get; set; }//userId (shopper) in our system
    }
}

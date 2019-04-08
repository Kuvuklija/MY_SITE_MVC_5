using MyFirstSite.Domain.Entities;

namespace MyFirstSite.Domain.Abstract{

    public interface IOrderProcessor{
        void ProcessorOrder(Cart cart, ShippingDetail shippingDetail);
    }
}

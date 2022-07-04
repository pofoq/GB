using MediatR;
using RazorApp1.Models;

namespace RazorApp1.Domain.Events
{
    public class ProductAdded : INotification
    {
        public Product Product { get; set; }

        public ProductAdded(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);

            Product = product;
        }
    }
}

using MediatR;

namespace RazorApp1.Domain.Events
{
    public class ProductDeleted : INotification
    {
        public int Id { get; set; }

        public ProductDeleted(int id)
        {
            Id = id;
        }
    }
}

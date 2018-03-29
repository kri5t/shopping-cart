using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Shopping.Core.Commands.ShoppingCart
{
    public class AddItemToShoppingCartCommand : IRequest<string>
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public AddItemToShoppingCartCommand()
        {
            
        }
    }
    
    public class AddItemToShoppingCartCommandHandler : IRequestHandler<AddItemToShoppingCartCommand, string> {
        public Task<string> Handle(AddItemToShoppingCartCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
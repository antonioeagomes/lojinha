using MediatR;
using Store.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Vendas.Application.Commands
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, bool>
    {
        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarCommand(message)) return false;
            return true;
        }

        private bool ValidarCommand(Command message)
        {
            if (message.IsValido())
            {
                return true;
            }

            foreach (var error in message.ValidationResult.Errors)
            {
                // Lançar evento de erro
            }

            return false;
        }
    }
}

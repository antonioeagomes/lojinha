using Store.Vendas.Application.Queries.DTO;
using Store.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Vendas.Application.Queries
{
    public class PedidoQueries : IPedidoQueries
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoQueries(IPedidoRepository pedidoRepository)
        {
            this._pedidoRepository = pedidoRepository;
        }
        public async Task<CarrinhoDTO> ObterCarrinhoCliente(Guid clienteId)
        {
            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(clienteId);

            if (pedido == null) return null;

            var carrinho = new CarrinhoDTO
            {
                ClienteId = pedido.ClienteId,
                PedidoId = pedido.Id,
                ValorTotal = pedido.ValorTotal,
                SubTotal = pedido.Desconto + pedido.ValorTotal,
                ValorDesconto = pedido.Desconto,
                VoucherCodigo = pedido.Voucher?.Codigo
            };

            foreach (var item in pedido.PedidoItems)
            {
                carrinho.Itens.Add(new CarrinhoItemDTO
                {
                    ProdutoId = item.ProdutoId,
                    ProdutoNome = item.ProdutoNome,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario,
                    ValorTotal = item.ValorUnitario * item.Quantidade
                });
            }

            return carrinho;
        }

        public async Task<IEnumerable<PedidoDTO>> ObterPedidosCliente(Guid clienteId)
        {
            var pedidos = await _pedidoRepository.ObterPedidosPorClienteId(clienteId);

            pedidos = pedidos.Where(p => p.PedidoStatus == PedidoStatus.Pago || p.PedidoStatus == PedidoStatus.Cancelado)
                .OrderByDescending(p => p.Codigo);

            var pedidosDto = new List<PedidoDTO>();

            foreach (var pedido in pedidos)
            {
                pedidosDto.Add(new PedidoDTO
                {
                    Codigo = pedido.Codigo,
                    ValorTotal = pedido.ValorTotal,
                    DataCadastro = pedido.DataCadastro,
                    PedidoStatus = (int)pedido.PedidoStatus
                });
            }
            return pedidosDto;
        }
    }
}

using Trabalho1.Enums;
using Trabalho1.Models;

namespace Trabalho1.Services
{
    public interface IAgendamentoService
    {
        List<Pedido> GetAllPedidos(eStatusPedido? status, eOrigemPedido? origem);
        Pedido RealizarPedido(string origem);
        Pedido AlterarPedido(int senha);
        Pedido PrepararPedido();
        Pedido FinalizarPedido();
        List<Pedido> EntregarPedido();
        Pedido RetirarPedido(int senha);

    }
}

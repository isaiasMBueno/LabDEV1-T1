using Trabalho1.Models;

namespace Trabalho1.Services
{
    public interface IPedidosService
    {
        List<Pedido> GetAllPedidos(string? status, string? origem);
        Pedido RealizarPedido(string origem);
        Pedido AlterarPedido(int senha);
        Pedido PrepararPedido();
        Pedido FinalizarPedido();
        List<Pedido> EntregarPedido();
        Pedido RetirarPedido(int senha);

    }
}

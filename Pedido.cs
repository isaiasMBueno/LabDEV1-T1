namespace Trabalho1;

public class Pedido
{
    public int Senha { get; set; }
    public  eOrigemPedido Origem { get; set; } = eOrigemPedido.qualquer;
    public eStatusPedido Status { get; set; } = eStatusPedido.aguardando;

    public Pedido()
    {
    }

}
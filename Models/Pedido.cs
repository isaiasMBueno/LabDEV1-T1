using System.Text.Json.Serialization;
using Trabalho1.Enums;

namespace Trabalho1.Models;

public class Pedido
{
    public int Senha { get; set; }

    [JsonIgnore]
    public eOrigemPedido Origem { get; set; } = eOrigemPedido.qualquer;

    public string OrigemPedido { get; set; }

    [JsonIgnore]
    public eStatusPedido Status { get; set; } = eStatusPedido.aguardando;

    public string StatusPedido { get; set; }

    public Pedido()
    {
        StatusPedido = char.ToUpper(Status.ToString()[0]) + Status.ToString().Substring(1);
    }

}

using System.Runtime.Serialization;
namespace Trabalho1.Enums;

public enum eStatusPedido
{
    [EnumMember(Value = "Balcão")]
    aguardando = 0,
    [EnumMember(Value = "Delivery")]
    fazendo = 1,
    [EnumMember(Value = "DriveThru")]
    pronto = 2
}
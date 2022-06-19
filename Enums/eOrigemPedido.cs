namespace Trabalho1.Enums;
using System.Runtime.Serialization;

public enum eOrigemPedido
{
    [EnumMember(Value = "Balcão")]
    balcao = 0,
    [EnumMember(Value = "Delivery")]
    delivery = 1,
    [EnumMember(Value = "DriveThru")]
    drivethru = 2,

    qualquer = 100
}
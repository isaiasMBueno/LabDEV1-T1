namespace Trabalho1;
using System;
using System.Runtime.Serialization;

public enum eStatusPedido
{
    [EnumMember(Value = "Balcão")]
    aguardando = 0,
    [EnumMember(Value = "Delivery")]
    fazendo = 1,
    [EnumMember(Value = "DriveThru")]
    pronto = 2
}
using Microsoft.AspNetCore.Mvc;
using Trabalho1.Models;
using Trabalho1.Services;

namespace Trabalho1.Controllers;

[ApiController]
[Route("api/pedidos")]
public class PedidosController : ControllerBase
{


    private readonly IPedidosService _pedidosService;

    public PedidosController(IPedidosService pedidosService)
    {
        _pedidosService = pedidosService;
    }

    [HttpGet]
    public ActionResult<List<Pedido>> GetAll(string? status, string? origem)
    {
        try
        {
            return Ok(_pedidosService.GetAllPedidos(status, origem));
        }
        catch (NullReferenceException)
        {
            return StatusCode(404, "Nenhum pedido encontrado");
        }
        catch (ArgumentException)
        {
            return StatusCode(400, "Os dados informados para busca são inválidos");
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar requisição");
        }
    }

    //Cliente faz o pedido //------->Realizar ====> 1º endpoint
    [HttpPost]
    [Route("{origem}")]
    public ActionResult<Pedido> RealizarPedido(string origem)
    {
        try
        {

            if (origem == null || origem == "")
            {
                return StatusCode(400, "Origem do pedido não informada no corpo da requisição.");
            }
            return Ok(_pedidosService.RealizarPedido(origem));
        }
        catch (ArgumentException)
        {
            return StatusCode(400, "A Origem informada é invalida");
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar requisição");
        }
    }

    //Alterar o pedido //------->alterarPedido ====> 2º endpoint
    [HttpPut]
    [Route("{senha}")]
    public ActionResult<Pedido> AlterarPedido(int senha)
    {
        try
        {
            if (senha <= 0)
            {
                return StatusCode(400, "Senha informada com valor inválido, igual ou menor a zero. Informe uma senha maior que zero na URL para alteração do pedido.");
            }

            return Ok(_pedidosService.AlterarPedido(senha));
        }
        catch (NullReferenceException)
        {
            return StatusCode(404, "Pedido não encontrado para alteraçao");
        }
        catch (Exception)
        {
            return StatusCode(500, "Falha ao alterar pedido (cliente).");
        }
    }

    //Cozinha faz o pedido //------->FazerPedido ====> 3º endpoint
    [HttpPatch]
    [Route("preparar")]
    public ActionResult<Pedido> PrepararPedido()
    {
        try
        {
            return Ok(_pedidosService.PrepararPedido());
        }
        catch (NullReferenceException)
        {
            return StatusCode(404, "Nenhum pedido com status AGUARDANDO localizado.");
        }
        catch (InvalidOperationException)
        {
            return StatusCode(400, "A cozinha está cheia (3), espere a finalização de algum pedido.");
        }
        catch (Exception)
        {
            return StatusCode(500, "Falha ao fazer pedido (cozinha).");
        }
    }



    //Finaliza o pedido //------->finalizarPedido ====> Endpoint Extra conforme alinhamento com professor ==> não está no pdf dos temas
    [HttpGet]
    [Route("finalizar")]
    public ActionResult<Pedido> FinalizarPedido()
    {
        try
        {
            return Ok(_pedidosService.FinalizarPedido());
        }
        catch (NullReferenceException)
        {
            return StatusCode(404, "Nenhum pedido com status PREPARANDO localizado");
        }
        catch (Exception)
        {
            return StatusCode(500, "Falha ao finalizar pedido");
        }
    }

    //Entrega o pedido //------->EntregarPedido ====> 4º endpoint //Delivery
    [HttpGet]
    [Route("entrega")]
    public ActionResult<List<Pedido>> EntregarPedido()
    {
        try
        {
            return Ok(_pedidosService.EntregarPedido());
        }
        catch (NullReferenceException)
        {
            return StatusCode(404, "Nenhum pedido com status PRONTO e origem DELIVERY encontrado.");
        }
        catch (InvalidOperationException)
        {
            return StatusCode(400, "Aguarde o acúmulo de 3 pedidos para fazer a entrega via delivery");
        }
        catch (Exception)
        {
            return StatusCode(500, "Falha ao entregar pedido (Entregador delivery).");
        }
    }


    //Cliente retira o pedido //------->RetirarPedido ====> 5º endpoint
    [HttpDelete]
    [Route("{senha}")]
    public ActionResult<Pedido> RetirarPedido(int senha)
    {
        try
        {
            if (senha <= 0)
            {
                return StatusCode(400, "Senha informada com valor inválido, igual ou menor a zero. Informe uma senha maior que zero na URL para retirada do pedido.");
            }
            return Ok(_pedidosService.RetirarPedido(senha));
        }
        catch (NullReferenceException)
        {
            return StatusCode(404, "Nenhum pedido com Status PRONTO disponivel para retirada");
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao retirar pedido.");
        }

    }

}

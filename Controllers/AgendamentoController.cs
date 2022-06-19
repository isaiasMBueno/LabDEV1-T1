using Microsoft.AspNetCore.Mvc;
using Trabalho1.Enums;
using Trabalho1.Models;
using Trabalho1.Services;

namespace Trabalho1.Controllers;

[ApiController]
[Route("api/agendamentos")]
public class AgendamentoController : ControllerBase
{


    private readonly IAgendamentoService _agendamentoService;

    public AgendamentoController(IAgendamentoService agendamentoService)
    {
        _agendamentoService = agendamentoService;
    }

    [HttpGet]
    public ActionResult<List<Pedido>> GetAll(eStatusPedido? status, eOrigemPedido? origem)
    {
        return Ok(_agendamentoService.GetAllPedidos(status, origem));
    }

    //Cliente faz o pedido //------->Realizar ====> 1º endpoint
    [HttpPost]
    [Route("{origem}")]
    public ActionResult<Pedido> RealizarPedido(string origem)
    {
        if (origem == null || origem == "")
        {
            return StatusCode(400, "Origem do pedido não informada no corpo da requisição.");
        }

        return Ok(_agendamentoService.RealizarPedido(origem));
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

            return Ok(_agendamentoService.AlterarPedido(senha));
        }
        catch (System.Exception)
        {
            return Problem("Falha ao alterar pedido (cliente).");
        }
    }

    //Cozinha faz o pedido //------->FazerPedido ====> 3º endpoint
    [HttpPatch]
    [Route("preparar")]
    public ActionResult<Pedido> PrepararPedido()
    {
        try
        {
            return (Ok(_agendamentoService.PrepararPedido()));
        }
        catch (System.Exception)
        {
            return Problem("Falha ao fazer pedido (cozinha).");
        }
    }



    //Finaliza o pedido //------->finalizarPedido ====> Endpoint Extra conforme alinhamento com professor ==> não está no pdf dos temas
    [HttpGet]
    [Route("finalizapedido")]
    public ActionResult<Pedido> FinalizarPedido()
    {
        try
        {
            return Ok(_agendamentoService.FinalizarPedido());
        }
        catch (System.Exception)
        {
            return Problem("Falha ao finalizar pedido");
        }
    }

    //Entrega o pedido //------->EntregarPedido ====> 4º endpoint //Delivery
    [HttpGet]
    [Route("entrega")]
    public ActionResult<List<Pedido>> EntregarPedido()
    {
        try
        {
            return Ok(_agendamentoService.EntregarPedido());
        }
        catch (System.Exception)
        {
            return Problem("Falha ao entregar pedido (Cozinha).");
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
            return Ok(_agendamentoService.RetirarPedido(senha));
        }
        catch (System.Exception)
        {
            return Problem("Erro ao retirar pedido.");
        }

    }

}

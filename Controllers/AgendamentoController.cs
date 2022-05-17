using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Trabalho1.Controllers;

[ApiController]
[Route("api/agendamentos")]
public class AgendamentoController : ControllerBase
{
    /*Lista que representa onde os pedidos se encontram*/
    private static List<Pedido> _listPedidosAguardando = new List<Pedido>(); //Pedidos com status AGUARDANDO
    private static List<Pedido> _listPedidosFazendo = new List<Pedido>();   //Pedidos com status FAZENDO
    private static List<Pedido> _listPedidosPronto = new List<Pedido>();    //Pedidos com status PRONTO
    private static List<Pedido> _listPedidosEntregues = new List<Pedido>(); //Pedidos que já foram ENTREGUES através do EndPoint "RetirarPedido"

    /*Listas para controle do histórico da fila de pedidos feitos*/
    private static List<Pedido> _listPedidosFazendoAuxOld = new List<Pedido>();    //Lista de pedidos que passaram para o Status FAZENDO até o penultimo
    private static List<Pedido> _listPedidosFazendoAux = new List<Pedido>();       //Lista de pedidos que passaram para o Status FAZENDO até o ultimo


    private static int _senha = 1;

    private readonly ILogger<AgendamentoController> _logger;
    public AgendamentoController(ILogger<AgendamentoController> logger)
    {
        _logger = logger;
    }

    /*-------------Apenas para teste de desenvolvimento*/
    [HttpGet]
    [Route("listpedidos/{status}")]
    public ActionResult<List<Pedido>> GetListPedidos(eStatusPedido status)
    {
        try
        {
            if (status.ToString() == null)
            {
                return StatusCode(400, "Status não informado na URL para consulta");
            }

            switch (status)
            {
                case eStatusPedido.aguardando:
                    return Ok(_listPedidosAguardando);

                case eStatusPedido.fazendo:
                    return Ok(_listPedidosFazendo);

                case eStatusPedido.pronto:
                    return Ok(_listPedidosPronto);
            }

            return Problem("Erro ao listar pedidos");
        }
        catch (System.Exception ex)
        {
            return Problem(ex.ToString());
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

            Pedido pedido = new Pedido();
            pedido.Senha = _senha++;
            pedido.Origem = eOrigemPedido.Parse<eOrigemPedido>(origem.Replace("ã", "a").Replace("Ã", "A").ToLower());

            _listPedidosAguardando.Add(pedido);

            return Ok(pedido);
        }
        catch (System.Exception)
        {
            return Problem("Erro ao inserir pedido");
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

            var pedido = _listPedidosAguardando.Where(x => x.Senha == senha).FirstOrDefault();

            if (pedido == null)
            {
                return StatusCode(404, "Pedido não encontrado na lista de pedidos com status AGUARDANDO");
            }

            _listPedidosAguardando.Remove(pedido); //Remove e adiciona novamente, movendo o pedido para o final da fila
            _listPedidosAguardando.Add(pedido);

            return Ok(pedido);
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
            if (!_listPedidosAguardando.Where(a => a.Status == eStatusPedido.aguardando).Any())
            {
                return NotFound("Não há pedidos com status AGUARDANDO.");
            }

            if (_listPedidosFazendo != null && _listPedidosFazendo.Count() == 3)
            {
                return StatusCode(400, "Cozinha atingiu o limite máximo de produção, espere a finalização de um pedido e tente novamente.");
            }

            var tipoProximoPedido = GetTipoProximoPedido();
            var proximoPedido = new Pedido();
            if (tipoProximoPedido == eOrigemPedido.qualquer) //Não tem nenhum fazendo ou Já rodou todas as regras -> pegar o primeiro da fila
            {
                proximoPedido = _listPedidosAguardando.FirstOrDefault();
            }
            else
            {
                proximoPedido = _listPedidosAguardando.Where(a => a.Origem == tipoProximoPedido).FirstOrDefault();
                if (proximoPedido == null) //Não encontrou o tipo esperado para o próximo pedido, então seguir a fila normalmente
                {
                    proximoPedido = _listPedidosAguardando.FirstOrDefault();
                }
            }


            _listPedidosAguardando.Remove(proximoPedido);

            _listPedidosFazendoAuxOld = _listPedidosFazendoAux;
            _listPedidosFazendoAux.Add(proximoPedido);

            proximoPedido.Status = eStatusPedido.fazendo;
            _listPedidosFazendo.Add(proximoPedido);

            return Ok(proximoPedido);

        }
        catch (System.Exception)
        {
            return Problem("Falha ao fazer pedido (cozinha).");
        }
    }

    private eOrigemPedido GetTipoProximoPedido()
    {

        if (!_listPedidosFazendoAux.ToList().Any()) //Primeiro pedido
        {
            return eOrigemPedido.qualquer;
        }

        var ultimosPedidosBalcaoAux = _listPedidosFazendoAuxOld.Where(a => a.Origem == eOrigemPedido.balcao).ToList();
        var ultimosPedidosDriveAux = _listPedidosFazendoAuxOld.Where(a => a.Origem == eOrigemPedido.drivethru).ToList();
        var ultimosPedidosDeliveryAux = _listPedidosFazendoAuxOld.Where(a => a.Origem == eOrigemPedido.delivery).ToList();

        var ultimosPedidosBalcao = _listPedidosFazendoAux.Where(a => a.Origem == eOrigemPedido.balcao).ToList();
        var ultimosPedidosDrive = _listPedidosFazendoAux.Where(a => a.Origem == eOrigemPedido.drivethru).ToList();
        var ultimosPedidosDelivery = _listPedidosFazendoAux.Where(a => a.Origem == eOrigemPedido.delivery).ToList();

        if (ultimosPedidosDrive.Count() % 2 == 0)//A cada dois do drive um do balcao deve sair
        {
            if (ultimosPedidosBalcao.Count() > ultimosPedidosBalcaoAux.Count())  //Nesse caso o ultimo pedido ja foi do balcao, partir para a próxima regra
            {
                if (ultimosPedidosBalcao.Count() % 2 == 0) //Saiu 2 do balcão, logo pode atender um do delivery
                {
                    if (ultimosPedidosDelivery.Count() > ultimosPedidosDelivery.Count()) //O ultimo foi delivery? se sim, partir pra proxima regra
                    {
                        return eOrigemPedido.qualquer;
                    }
                    else   //Significa que pode atender um delivery
                    {
                        return eOrigemPedido.delivery;
                    }
                }
            }
            else   //Significa que pode atender um do balcao
            {
                return eOrigemPedido.balcao;
            }
        }
        else
        {
            if ((ultimosPedidosDrive.Count() % 3 == 0) || (ultimosPedidosBalcao.Count() % 2 == 0))
            {
                if (ultimosPedidosDelivery.Count() > ultimosPedidosDelivery.Count()) //O ultimo foi delivery? se sim, partir pra proxima regra
                {
                    return eOrigemPedido.qualquer;
                }
                else   //Significa que pode atender um delivery
                {
                    return eOrigemPedido.delivery;
                }
            }
        }
        //No caso que não se encaixou em nenhuma regra, buscar o próximo da fila
        return eOrigemPedido.qualquer;
    }

    //Finaliza o pedido //------->finalizarPedido ====> Endpoint Extra conforme alinhamento com professor ==> não está no pdf dos temas
    [HttpGet]
    [Route("finalizapedido")]
    public ActionResult<Pedido> FinalizarPedido()
    {
        try
        {
            var pedidoFinalizar = _listPedidosFazendo.FirstOrDefault();
            if(pedidoFinalizar == null)
            {
                return StatusCode(404, "Nenhum pedido com status FAZENDO para ser finalizado");
            }

            _listPedidosFazendo.Remove(pedidoFinalizar);
            pedidoFinalizar.Status = eStatusPedido.pronto;
            _listPedidosPronto.Add(pedidoFinalizar);

            return Ok(pedidoFinalizar);
        }
        catch(System.Exception)
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
            var listPedidosEntregar = _listPedidosPronto.Where(a => a.Origem == eOrigemPedido.delivery).ToList();

            if (!listPedidosEntregar.Any())   //Não há pedidos prontos para serem entregues pelo delivery
            {
                return StatusCode(404, "Nenhum pedido com status PRONTO encontrado.");
            }
            if (listPedidosEntregar.Count() == 3) //Somente se acumular 3 delivery é para fazer a entrega
            {
                foreach (var pedido in listPedidosEntregar) //Fazer a entrega dos 3
                {
                    _listPedidosPronto.Remove(pedido);
                    _listPedidosEntregues.Add(pedido);
                }
                return Ok(listPedidosEntregar);
            }
            return Problem("Aguarde o acúmulo de 3 pedidos para realizar entrega delivery");
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

            var pedido = _listPedidosPronto.Where(a => a.Senha == senha).FirstOrDefault();
            
            if (pedido == null)
            {
                return StatusCode(404, "Não foi encontrado pedido PRONTO com a senha informada.");
            }

            _listPedidosPronto.Remove(pedido);
            _listPedidosEntregues.Add(pedido);

            return Ok(pedido);
        }
        catch (System.Exception)
        {
            return Problem("Erro ao retirar pedido.");
        }

    }

}

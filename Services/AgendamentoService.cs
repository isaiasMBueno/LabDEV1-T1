using Trabalho1.Enums;
using Trabalho1.Models;

namespace Trabalho1.Services
{
    public class AgendamentoService : IAgendamentoService
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


        public List<Pedido> GetAllPedidos(eStatusPedido? status, eOrigemPedido? origem)
        {
            List<Pedido> listAllPedidos = new List<Pedido>();
            listAllPedidos.AddRange(_listPedidosAguardando);
            listAllPedidos.AddRange(_listPedidosFazendo);
            listAllPedidos.AddRange(_listPedidosPronto);
            listAllPedidos.AddRange(_listPedidosEntregues);

            if (status != null)
            {
                if (origem != null)
                {
                    return listAllPedidos.Where(x => x.Status == status && x.Origem == origem).ToList();
                }
                return listAllPedidos.Where(x => x.Status == status).ToList();
            }

            return listAllPedidos;
        }

        public Pedido RealizarPedido(string origem)
        {
            Pedido pedido = new Pedido();
            pedido.Senha = _senha++;
            pedido.Origem = eOrigemPedido.Parse<eOrigemPedido>(origem.Replace("ã", "a").Replace("Ã", "A").ToLower());

            _listPedidosAguardando.Add(pedido);

            return pedido;
        }

        public Pedido AlterarPedido(int senha)
        {
            var pedido = _listPedidosAguardando.Where(x => x.Senha == senha).FirstOrDefault();
            _listPedidosAguardando.Remove(pedido); //Remove e adiciona novamente, movendo o pedido para o final da fila
            _listPedidosAguardando.Add(pedido);
            return pedido;
        }

        public Pedido PrepararPedido()
        {
            if (!_listPedidosAguardando.Where(a => a.Status == eStatusPedido.aguardando).Any())
            {
                return null;
            }

            if (_listPedidosFazendo != null && _listPedidosFazendo.Count() == 3)
            {
                return null;
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

            return proximoPedido;
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

        public Pedido FinalizarPedido()
        {
            var pedidoFinalizar = _listPedidosFazendo.FirstOrDefault();

            _listPedidosFazendo.Remove(pedidoFinalizar);
            pedidoFinalizar.Status = eStatusPedido.pronto;
            _listPedidosPronto.Add(pedidoFinalizar);

            return pedidoFinalizar;
        }

        public List<Pedido> EntregarPedido()
        {
            var listPedidosEntregar = _listPedidosPronto.Where(a => a.Origem == eOrigemPedido.delivery).ToList();

            if (!listPedidosEntregar.Any())   //Não há pedidos prontos para serem entregues pelo delivery
            {
                //return StatusCode(404, "Nenhum pedido com status PRONTO encontrado.");
                return null;
            }
            if (listPedidosEntregar.Count() == 3) //Somente se acumular 3 delivery é para fazer a entrega
            {
                foreach (var pedido in listPedidosEntregar) //Fazer a entrega dos 3
                {
                    _listPedidosPronto.Remove(pedido);
                    _listPedidosEntregues.Add(pedido);
                }
                return listPedidosEntregar;
            }
            return null;
        }

        public Pedido RetirarPedido(int senha)
        {
            var pedido = _listPedidosPronto.Where(a => a.Senha == senha).FirstOrDefault();
            _listPedidosPronto.Remove(pedido);
            _listPedidosEntregues.Add(pedido);

            return pedido;
        }
        //
    }
}

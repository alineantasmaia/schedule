using Blue.Agenda.Domain.Dtos;
using Blue.Agenda.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Blue.Agenda.API.Controladores
{
    [ApiVersion("1")]
    [ApiController]
    public class InterfaceController : Controller
    {
        private readonly IServicoEvento _servicoEvento;
        private readonly IServicoTipoErro<Evento> _errosEvento;
        private readonly IServicoAgenda _servicoAgenda;

        public InterfaceController(IServicoTipoErro<Evento> errosEvento, IServicoEvento servicoEvento, IServicoAgenda servicoAgenda)
        {
            _errosEvento = errosEvento;
            _servicoEvento = servicoEvento;
            _servicoAgenda = servicoAgenda;
        }

        [HttpGet("Validar")]
        public IActionResult Get()
        {
            return Ok("Get Interface controller");
        }

        [HttpPost("/Erros")]
        public IActionResult Post(Evento evento)
        {
            _errosEvento.AddErro("ERR_N", evento: evento);
            _errosEvento.AddErroInconsistencia("ERR_STATUS", "A", "T");
            _errosEvento.AddErroRegraNegocio(string.Empty, "Informacao não cadastrada");

            return BadRequest(_errosEvento.Notificacoes);
        }

        [HttpPost("/Executar")]
        public async Task<IActionResult> Executar(Evento evento)
        {
            try
            {
                if (!evento.Valido()) return BadRequest(evento.Notificacoes);
                await _servicoEvento.Executar(evento);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ConsultarAgenda")]
        public IActionResult ConsultarAgenda()
        {
            try
            {
                var result = _servicoAgenda.ConsultarAgenda();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}

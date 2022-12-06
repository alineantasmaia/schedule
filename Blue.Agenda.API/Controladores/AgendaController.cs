using Blue.Agenda.API.Configuracoes;
using Blue.Agenda.Domain.Dtos;
using Blue.Agenda.Domain.Entidades;
using Blue.Agenda.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blue.Agenda.API.Controladores
{
    [Route("[controller]")]
    [ApiController]
    public class AgendaController : Controller
    {
        private readonly IServicoAgenda _servicoAgenda;

        public AgendaController(IServicoAgenda servicoAgenda)
        {
            _servicoAgenda = servicoAgenda;
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

        /*[HttpPost("IncluirAgenda")]
        public IActionResult IncluirAgenda(Usuario usuario)
        {
            try
            {
                _servicoAgenda  .InserirScore(scoreDto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("AtualizarAgenda")]
        public IActionResult AtualizarAgenda(ScoreDto scoreDto)
        {
            try
            {
                _servicoAgenda .AtualizarScore(scoreDto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }*/

    }
}

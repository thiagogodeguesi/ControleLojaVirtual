using ControleLojaVirtual.Context;
using ControleLojaVirtual.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleLojaVirtual.Controllers
{
    [Produces("application/json")]
    [Route("api/Lojas")]
    [Authorize()]
    public class LojasController : Controller
    {
        private readonly LvContext _context;

        public LojasController(LvContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Get()
        {
            var lojas = await _context.Lojas.ToArrayAsync();

            var resposta = lojas.Select(l => new
            {
                Id = l.Id,
                Nome = l.Nome,
                Site = l.Site,
                Endereco = l.Endereco
            });

            return Ok(resposta);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var loja = await _context.Lojas.FindAsync(id);

            if (loja.Nome != "" && loja.Nome != null)
            {
                var resposta = (new
                {
                    Id = loja.Id,
                    Nome = loja.Nome,
                    Site = loja.Site,
                    Endereco = loja.Endereco
                });

                return Ok(resposta);
            }
            else
                return Ok("Nenhuma loja encontrada");
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Loja loja)
        {
            if (loja.Nome != "" && loja.Nome != null)
            {
                await _context.Lojas.AddAsync(loja);
                _context.SaveChanges();
                return Ok(loja.Nome + " - Inserido com Sucesso");
            }
            return null;
        }

        [HttpPut]
        public async Task<IActionResult> update([FromBody] Loja loja)
        {
            if (loja.Nome != "" && loja.Nome != null)
            {
                _context.Lojas.Update(loja); 
                _context.SaveChanges();
                return Ok(loja.Nome + " - Atualizado com Sucesso");
            }
            return null;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vloja = await _context.Lojas.FindAsync(id);

            if (vloja.Nome != "" && vloja.Nome != null)
            {
                _context.Lojas.Remove(vloja);
                _context.SaveChanges();
                return Ok(vloja.Nome + " - Excluido com Sucesso");
            }
            return Ok(" Loja não encontrada"); ;
        }
    }
}

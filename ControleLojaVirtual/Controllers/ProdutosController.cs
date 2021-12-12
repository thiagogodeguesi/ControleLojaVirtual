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
    [Route("api/Produtos")]
    [Authorize()]
    public class ProdutosController : Controller
    {
        private readonly LvContext _context;

        public ProdutosController(LvContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Get()
        {
            var produtos = await _context.Produtos.ToArrayAsync();

            var resposta = produtos.Select(p => new
            {
                Id = p.Id,
                Nome = p.Nome,
                Custo = p.Custo,
                Valor = p.Valor
            });

            return Ok(resposta);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto.Nome != "" && produto.Nome != null)
            {
                var resposta = (new
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Custo = produto.Custo,
                    Valor = produto.Valor
                });

                return Ok(resposta);
            }
            else
                return Ok("Nenhum produto encontrado");
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Produto produto)
        {
            if (produto.Nome != "" && produto.Nome != null)
            {
                await _context.Produtos.AddAsync(produto);
                _context.SaveChanges();
                return Ok(produto.Nome + " - Inserido com Sucesso");
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> update([FromBody] Produto produto)
        {
            if (produto.Nome != "" && produto.Nome != null)
            {
                _context.Produtos.Update(produto);
                _context.SaveChanges();
                return Ok(produto.Nome + " - Atualizado com Sucesso");
            }
            return null;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto.Nome != "" && produto.Nome != null)
            {
                _context.Produtos.Remove(produto);
                _context.SaveChanges();
                return Ok(produto.Nome + " - Excluido com Sucesso");
            }
            return Ok("Produto não encontrada"); ;
        }
    }
}

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
    [Route("api/Estoque")]
    [Authorize()]
    public class ItemEstoquesController : Controller
    {
        private readonly LvContext _context;

        public ItemEstoquesController(LvContext context)
        {
            _context = context;
        }

        //LISTA TODOS PRODUTOS
        public async Task<IActionResult> Get()
        {
            var estoque = await _context.ItemEstoques.ToArrayAsync();

            var resposta = estoque.Select(e => new
            {
                IdIE = e.IdIE,
                IdLoja = e.IdLoja,
                IdProduto = e.IdProduto,
                Estoque = e.Estoque,
                EstoqueCompra = e.EstoqueCompra,
                EstoqueVenda = e.EstoqueVenda,
                Endereco = e.Endereco
            });

            return Ok(resposta);
        }

        //LISTA PRODUTOS POR LOJA
        [HttpGet("{idloja}")]
        public async Task<IActionResult> GetEstoqueLoja(int idloja)
        {
            var estoque = await _context.ItemEstoques.Where(e => e.IdLoja == idloja).ToArrayAsync();

            if (estoque.Count() > 0)
            {
                var resposta = estoque.Select(e => new
                {
                    IdIE = e.IdIE,
                    IdLoja = e.IdLoja,
                    IdProduto = e.IdProduto,
                    Estoque = e.Estoque,
                    EstoqueCompra = e.EstoqueCompra,
                    EstoqueVenda = e.EstoqueVenda,
                    Endereco = e.Endereco
                });

                return Ok(resposta);
            }
            else
                return Ok("Nenhum produto encontrado nesta loja");
        }

        public class MovEstoque
        {
            public int idloja { get; set; }
            public int idproduto { get; set; }
            public double qtde { get; set; }
        }

        //ADICIONA QUANTIDADE AO ESTOQUE
        [HttpPost]
        [Route("Compra")]
        public async Task<IActionResult> Compra([FromBody] MovEstoque moveestoque) // int idloja, int idproduto, double qtde) //[FromBody]
        {
            var estoque = await _context.ItemEstoques.Where(l => l.IdLoja == moveestoque.idloja).Where(p => p.IdProduto == moveestoque.idproduto).ToArrayAsync();
            var maxid = await _context.ItemEstoques.MaxAsync(i => i.IdIE) + 1 ;

            if (estoque.Count() == 1)
            {
                estoque[0].Estoque = estoque[0].Estoque + moveestoque.qtde;
                estoque[0].EstoqueCompra = estoque[0].EstoqueCompra + moveestoque.qtde;

                _context.ItemEstoques.Update(estoque[0]);
                _context.SaveChanges();
                return Ok("Produto " + estoque[0].IdProduto + " - Atualizado com Sucesso para loja " + estoque[0].IdLoja);
            }
            if (estoque.Count() == 0 && moveestoque.idproduto > 0)
            {
                var nestoque = (new ItemEstoque { IdIE = maxid, IdLoja = moveestoque.idloja, IdProduto = moveestoque.idproduto, Estoque = moveestoque.qtde, EstoqueCompra = moveestoque.qtde, Endereco = "NOVO REGISTO "+maxid });
                    
                _context.ItemEstoques.Add(nestoque);
                _context.SaveChanges();
                return Ok("Produto " + nestoque.IdProduto + " - Atualizado com Sucesso para loja " + nestoque.IdLoja);
            }
            if (estoque.Count() > 1 )
            {
                return Ok("Produto " + moveestoque.idproduto + " duplicado para loja " + moveestoque.idloja);
            }
           
            return BadRequest();
        }

        //SUBTRAI QUANTIDADE DO ESTOQUE
        [HttpPost]
        [Route("Venda")]
        public async Task<IActionResult> Venda([FromBody] MovEstoque moveestoque)
        {
            var estoque = await _context.ItemEstoques.Where(l => l.IdLoja == moveestoque.idloja).Where(p => p.IdProduto == moveestoque.idproduto).ToArrayAsync();
            var maxid = await _context.ItemEstoques.MaxAsync(i => i.IdIE) + 1;

            if (estoque.Count() == 1)
            {
                if (estoque[0].Estoque < moveestoque.qtde) 
                {
                    return Ok("Verificar estoque do Produto " + estoque[0].IdProduto + " para loja " + estoque[0].IdLoja 
                        + " Quantidade em estoque  " + estoque[0].Estoque + " quantidade de venda  " + moveestoque.qtde);
                }
                else
                {
                    estoque[0].Estoque = estoque[0].Estoque - moveestoque.qtde;
                    estoque[0].EstoqueVenda = estoque[0].EstoqueVenda + moveestoque.qtde;
                    _context.ItemEstoques.Update(estoque[0]);
                    _context.SaveChanges();
                    return Ok("Venda de " + moveestoque.qtde + " do Produto " + estoque[0].IdProduto + " para loja " + estoque[0].IdLoja);
                }
            }
            if (estoque.Count() == 0)
            {
                return Ok("Produto " + moveestoque.idproduto + " NÂO existe para loja " + moveestoque.idloja);
            }
            if (estoque.Count() > 1)
            {
             return Ok("Produto " + moveestoque.idproduto + " duplicado para loja " + moveestoque.idloja);
            }
            return BadRequest();
        }

        // ADICIONA ESTOQUE SEM NECESSIDADE DE COMPRA
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]  ItemEstoque estoque)
        {
            if (estoque.IdLoja >0 && estoque.IdProduto > 0)
            {
                await _context.ItemEstoques.AddAsync(estoque);
                _context.SaveChanges();
                return Ok("Produto " + estoque.IdProduto +" - Inserido com Sucesso para loja "+estoque.IdLoja);
            }
            return BadRequest();
        }

        // ATUALIZA ESTOQUE DA LOJA
        [HttpPut]
        public async Task<IActionResult> update([FromBody] ItemEstoque estoque)
        {
            if (estoque.IdLoja > 0 && estoque.IdProduto > 0)
            {
                _context.ItemEstoques.Update(estoque);
                _context.SaveChanges();
                return Ok("Produto " + estoque.IdProduto + " - Atualizado com Sucesso para loja " + estoque.IdLoja);
            }
            return null;
        }

        // EXCLUI ESTOQUE PARA A LOJA
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var estoque = await _context.ItemEstoques.FindAsync(id);

            if (estoque.IdLoja > 0 && estoque.IdProduto > 0)
            {
                _context.ItemEstoques.Remove(estoque);
                _context.SaveChanges();
                return Ok("Produto " + estoque.IdProduto + " - Deletado com Sucesso da loja " + estoque.IdLoja);
            }
            return Ok(" Estoque nao encontrado "); ;
        }
    }
}

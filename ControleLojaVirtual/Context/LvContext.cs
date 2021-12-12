using ControleLojaVirtual.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleLojaVirtual.Context
{
    public class LvContext : DbContext
    {
        public LvContext(DbContextOptions<LvContext> options)
            : base(options)
        { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Loja> Lojas { get; set; }
        public DbSet<ItemEstoque> ItemEstoques { get; set; }
    }
}

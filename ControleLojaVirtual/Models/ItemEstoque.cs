using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ControleLojaVirtual.Models
{
    public class ItemEstoque
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IdIE { get; set; }
        [ForeignKey("Produto")]
        public int IdLoja { get; set; }
        [ForeignKey("Loja")]
        public int IdProduto { get; set; }
        
        public double Estoque { get; set; }
        public double EstoqueCompra { get; set; }
        public double EstoqueVenda { get; set; }
        public string Endereco { get; set; }

    }
}

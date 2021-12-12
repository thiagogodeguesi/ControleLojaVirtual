using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ControleLojaVirtual.Models
{
    public class Usuario
    {
        [Key]
        public int UserId { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        
    }
}

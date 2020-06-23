using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciamentoAniversarioAspNet.Models
{
    public class Aniversariante
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public String Nome { get; set; }

        [Required]
        [Display(Name = "Sobrenome")]
        public String Sobrenome { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "Dias para o aniversário (int)")]
        public int DiasInt { get; }

        [Display(Name = "Dias para o aniversário")]
        public String Dias
        {
            get
            {
                if (DiasInt == 0)
                {
                    return "Feliz aniversário!";
                }
                else
                {
                    return DiasInt.ToString();
                }

            }
        }

        public int ExibirAniversariantesDoDia()
        {
            var proximoNiver = this.DataNascimento.AddYears(DateTime.Now.Year - this.DataNascimento.Year);
            proximoNiver = (proximoNiver.Date < DateTime.Now.Date) ? proximoNiver.AddYears(1) : proximoNiver;
            var teste = (proximoNiver.Date - DateTime.Now.Date).Days;
            return (proximoNiver - DateTime.Now.Date).Days;
        }
    }
}

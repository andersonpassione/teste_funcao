using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.DML
{
    /// <summary>
    /// Classe de beneficiario que representa o registo na tabela Beneficiario do Banco de Dados
    /// </summary>
    public class Beneficiario
    {
        public long Id { get; set; }

        public long IdCliente { get; set; }

        public string Nome { get; set; }

        public string Cpf { get; set; }
    }
}

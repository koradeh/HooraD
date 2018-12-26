using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SQLite;

namespace Nancy_Standalone
{

    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string NumeroCartao { get; set; }
    }
    public class Estabelecimento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CNPJ { get; set; }
        public string NaturezaJuridica { get; set; }
        public string Situacao { get; set; }
    }
    public class Pagamento
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdEstabelecimento { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
    }
    public class ReceitaWs
    {
        public string nome { get; set; }
        public string cnpj { get; set; }
        public string natureza_juridica { get; set; }
        public string situacao { get; set; }
    }
    public class Versao
    {
        public string Version { get; set; }
    }
}
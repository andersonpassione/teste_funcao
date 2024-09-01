using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using FI.WebAtividadeEntrevista.Models;
using FI.WebAtividadeEntrevista.Models.Validacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace FI.WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        // GET: Beneficiario
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(BeneficiarioModel model)
        {
            BoBeneficiario bo = new BoBeneficiario();


            if ((!this.ModelState.IsValid) || (!new Cpf().Valida(model.Cpf)))
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                if (erros.Count == 0)
                {
                    return Json("CPF inválido");
                }
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                if (!bo.VerificarExistencia(model.Cpf))
                {
                    model.Id = bo.Incluir(new Beneficiario()
                    {                        
                        Nome = model.Nome,                        
                        Cpf = model.Cpf,
                        IdCliente = model.IdCliente
                    });
                    return Json("Cadastro efetuado com sucesso");
                }
                Response.StatusCode = 400;
                return Json("CPF já cadastrado");
            }
        }

        [HttpPost]
        public JsonResult Alterar(BeneficiarioModel model)
        {
            BoBeneficiario bo = new BoBeneficiario();

            if ((!this.ModelState.IsValid) || (!new Cpf().Valida(model.Cpf)))
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                if (erros.Count == 0)
                {
                    return Json("CPF inválido");
                }
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                bo.Alterar(new Beneficiario()
                {
                    Nome = model.Nome,
                    Cpf = model.Cpf,
                    IdCliente = model.IdCliente
                });
                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoBeneficiario bo = new BoBeneficiario();
            Beneficiario beneficiario = bo.Consultar(id);
            Models.BeneficiarioModel model = null;

            if (beneficiario != null)
            {
                model = new BeneficiarioModel()
                {
                    Id = beneficiario.Id,
                    IdCliente = beneficiario.IdCliente,
                    Nome = beneficiario.Nome,
                    Cpf = Regex.Replace(beneficiario.Cpf, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4")                    
                };
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Excluir(long id)
        {
            BoBeneficiario bo = new BoBeneficiario();

            try
            {
                bo.Excluir(id);
                return Json("Beneficiário excluído com sucesso");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json("Erro ao excluir beneficiário: " + ex.Message);
            }
        }

        [HttpPost]
        public JsonResult BeneficiarioList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Beneficiario> beneficiarios = new BoBeneficiario().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = beneficiarios, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}
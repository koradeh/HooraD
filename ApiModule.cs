using Nancy;
using Newtonsoft.Json;
//using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyStandalone
{
    public class ApiModule : NancyModule
    {
        public ApiModule() : base("/api")
        {
            // Versao
            Get("/version", parameters =>
            {
                var model = new Versao() { Version = "1.0.0.0" };
                return Response.AsJson(model, HttpStatusCode.OK);
            });
            
            #region Cliente

            // 1 - Listar todos os clientes:
            Get("/clients", parameters =>
            {
                var model = DbManager.GetAllClients();
                return Response.AsJson(model);
            });

            // 2 - Obter os dados de um cliente específico:
            Get("/clients/{id:int}", parameters =>
            {
                var model = DbManager.GetClientById(parameters.id);
                return model;
            });

            // 3 - Cadastrar um novo cliente:
            Post("/clients", parameters =>
            {
                var model = ReadBodyObject<Cliente>();
                var success = DbManager.SaveClient(model);
                if (success)
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            });

            #endregion

            #region Estabelecimento

            // 4 - Listar todos os estabelecimentos:
            Get("/establishments", parameters =>
            {
                var model = DbManager.GetAllEstablishments();
                return Response.AsJson(model);
            });

            // 5 - Obter os dados de um cliente específico:
            Get("/establishments/{id:int}", parameters =>
            {
                var model = DbManager.GetEstablishmentById(parameters.id);
                return model;
            });

            // 6 - Cadastrar um novo estabelecimento:
            Post("/establishments", parameters =>
            {
                var estabelecimento = ReadBodyObject<Estabelecimento>();
                var model = BuscaEstabelecimento(estabelecimento.CNPJ);
                var success = DbManager.SaveEstablishment(model);
                if (success)
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            });

            #endregion

            #region Pagamento

            // 7 - Fazer um pagamento:
            Post("/payments", parameters =>
            {
                var model = ReadBodyObject<Pagamento>();
                var success = DbManager.SavePayment(model);
                if (success)
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            });

            // 8 - Cancelar um pagamento:
            Delete("/payments/{id:int}", parameters =>
            {
                var success = DbManager.DeletePayment(parameters.id);
                if (success)
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            });

            // 9 - Listar todos os pagamentos de um estabelecimento:
            Get("/payments/{id:int}", parameters =>
            {
                var model = DbManager.GetPaymentByIdEstablishment(parameters.id);
                return model;
            });

            #endregion
        }

        private T ReadBodyObject<T>()
        {
            var body = this.Request.Body;
            var data = new byte[body.Length];
            body.Read(data, 0, (int)body.Length);
            var json = System.Text.Encoding.UTF8.GetString(data);
            var model = JsonConvert.DeserializeObject<T>(json);
            return model;
        }

        private Estabelecimento BuscaEstabelecimento(string cnpj)
        {
            var client = new RestClient("https://www.receitaws.com.br");
            var request = new RestRequest("/v1/cnpj/{cnpj}", Method.GET);
            request.AddUrlSegment("cnpj", cnpj);
            var response = client.Execute<ReceitaWs>(request);

            var estabelecimento = new Estabelecimento()
            {
                Nome = response.Data.nome,
                CNPJ = response.Data.cnpj,
                NaturezaJuridica = response.Data.natureza_juridica,
                Situacao = response.Data.situacao,
            };

            return estabelecimento;
        }
    }
}
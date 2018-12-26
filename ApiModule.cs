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
                var model = new Ver() { Version = "1.0.0.0" };
                return Response.AsJson(model, HttpStatusCode.OK);
            });
            
            #region stock

            // 1 - Listar todos os clientes:
            Get("/stocks", parameters =>
            {
                var model = DbManager.GetAllStocks();
                return Response.AsJson(model);
            });

            // 2 - Obter os dados de um cliente específico:
            Get("/stocks/{id:int}", parameters =>
            {
                var model = DbManager.GetStockById(parameters.id);
                return model;
            });

            // 3 - Cadastrar um novo cliente:
            Post("/stocks", parameters =>
            {
                var model = ReadBodyObject<Stock>();
                var success = DbManager.SaveStock(model);
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

            #region Valuation

            // 4 - Listar todos os estabelecimentos:
            Get("/valuations", parameters =>
            {
                var model = DbManager.GetAllValuations();
                return Response.AsJson(model);
            });

            // 5 - Obter os dados de um cliente específico:
            Get("/valuations/{id:int}", parameters =>
            {
                var model = DbManager.GetValuationById(parameters.id);
                return model;
            });

            // 6 - Cadastrar um novo estabelecimento:
            Post("/valuations", parameters =>
            {
                var estabelecimento = ReadBodyObject<Valuation>();
                var model = BuscaEstabelecimento("");
                var success = DbManager.SaveValuation(model);
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

            // #region Pagamento

            // // 7 - Fazer um pagamento:
            // Post("/payments", parameters =>
            // {
            //     var model = ReadBodyObject<Pagamento>();
            //     var success = DbManager.SavePayment(model);
            //     if (success)
            //     {
            //         return HttpStatusCode.OK;
            //     }
            //     else
            //     {
            //         return HttpStatusCode.BadRequest;
            //     }
            // });

            // // 8 - Cancelar um pagamento:
            // Delete("/payments/{id:int}", parameters =>
            // {
            //     var success = DbManager.DeletePayment(parameters.id);
            //     if (success)
            //     {
            //         return HttpStatusCode.OK;
            //     }
            //     else
            //     {
            //         return HttpStatusCode.BadRequest;
            //     }
            // });

            // // 9 - Listar todos os pagamentos de um estabelecimento:
            // Get("/payments/{id:int}", parameters =>
            // {
            //     var model = DbManager.GetPaymentByIdEstablishment(parameters.id);
            //     return model;
            // });

            // #endregion
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

        private Valuation BuscaEstabelecimento(string cnpj)
        {
            // var client = new RestClient("https://www.receitaws.com.br");
            // var request = new RestRequest("/v1/cnpj/{cnpj}", Method.GET);
            // request.AddUrlSegment("cnpj", cnpj);
            // var response = client.Execute<ReceitaWs>(request);

            var estabelecimento = new Valuation()
            {
                Time = DateTime.Now,
                Price = 5.6M
            };

            return estabelecimento;
        }
    }
}
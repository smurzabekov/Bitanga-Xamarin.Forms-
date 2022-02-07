using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Bitango_.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bitango_.API
{
    public static class ApiAccess
    {
        static string baseURL = "https://iiko.biz:9900/api/0/";
        static string user_id = "Bitanga";
        static string user_secret = "Bitanga2020";
        static public Menu BaseMenu = new Menu();

        /// <summary>
        /// Текущий авторизованный пользователь
        /// </summary>
        static public CurrentUser current_user;

        /// <summary>
        /// Актульные контактные данные, данные со страницы "О нас"
        /// </summary>
        static public AboutContainer aboutContainer;

        /// <summary>
        /// Актуальный список акций ресторана
        /// </summary>
        static public List<Promotion> promotions;

        /// <summary>
        /// Получение токена доступа к API
        /// </summary>
        /// <returns>Токен доступа</returns>
        static public string GetToken()
        {
            string exceptionMessage = string.Empty;
            string responseContent = string.Empty;
            try
            {
                RestRequest restRequest = new RestRequest(Method.GET);
                restRequest.AddHeader("content-type", "application/json");
                RestClient restClient = new RestClient(baseURL + $"auth/access_token?user_id={user_id}&user_secret={user_secret}");
                IRestResponse iRestResponse = restClient.Execute(restRequest);
                string errorMessage = iRestResponse.ErrorMessage;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    responseContent = iRestResponse.Content;
                }
                else
                {
                    responseContent = errorMessage;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Обнаружена ошибка. {ex.Message}");
            }
            return responseContent;
        }

        /// <summary>
        /// Получение ID организации
        /// </summary>
        /// <returns>ID организации</returns>
        static public string GetGuID()
        {
            string exceptionMessage = string.Empty;
            string responseContent = string.Empty;
            try
            {
                RestRequest restRequest = new RestRequest(Method.GET);
                restRequest.AddHeader("content-type", "application/json");
                RestClient restClient = new RestClient(baseURL + $"organization/list?access_token={GetToken().Replace("\"", "")}");
                IRestResponse iRestResponse = restClient.Execute(restRequest);
                string errorMessage = iRestResponse.ErrorMessage;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    responseContent = iRestResponse.Content;
                    JArray array = JArray.Parse(responseContent);
                    JObject child = array.First.ToObject<JObject>();
                    responseContent = (string)child["id"];
                }
                else
                {
                    responseContent = errorMessage;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Обнаружена ошибка. {ex.Message}");
            }
            return responseContent;
        }

        /// <summary>
        /// Получение меню ресторана
        /// </summary>
        static public void GetMenu()
        {
            string exceptionMessage = string.Empty;
            string responseContent = string.Empty;
            Menu menu = new Menu();
            try
            {
                RestRequest restRequest = new RestRequest(Method.GET);
                restRequest.AddHeader("content-type", "application/json");
                RestClient restClient = new RestClient(baseURL + $"nomenclature/{GetGuID().Replace("\"", "")}?access_token={GetToken().Replace("\"", "")}");
                IRestResponse iRestResponse = restClient.Execute(restRequest);
                string errorMessage = iRestResponse.ErrorMessage;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    responseContent = iRestResponse.Content;
                    JObject child = JObject.Parse(responseContent);
                    //responseContent = (JArray)child["groups"];
                    JArray groups = (JArray)child["groups"];
                    JArray products = (JArray)child["products"];

                    foreach (var item in groups.Where(x => x.Count() != 0))
                    {
                        Category category = new Category((string)item["name"], (string)item["description"], "", (string)item["id"], 0, (string)item["parentGroup"]);
                        foreach (var it in products.Where(x => ((string)x["parentGroup"]).Equals((string)item["id"])))
                        {
                            category.Add(new Dish((string)it["name"], (string)it["description"], "", (string)it["id"], int.Parse((string)it["price"]), (string)it["parentGroup"]));
                        }
                        if (category.Dishes.Count != 0)
                        {
                            menu.Add(category);
                        }
                    }
                }
                else
                {
                    responseContent = errorMessage;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Обнаружена ошибка. {ex.Message}");
            }
            BaseMenu = menu;
        }

        static string UserJSON(string name, string phone, string email, string birthday)
        {
            JObject jObject = new JObject(new JProperty("customer",
                new JObject(new JProperty("name", name), new JProperty("phone", phone), new JProperty("email", email), new JProperty("birthday", birthday), new JProperty("consentStatus", "1"))));
            return jObject.ToString();
        }
        static string UserJSON(string name, string phone, string email, string id, string birthday)
        {
            JObject jObject = new JObject(new JProperty("customer",
                new JObject(new JProperty("name", name), new JProperty("phone", phone), new JProperty("email", email), new JProperty("id", id), new JProperty("birthday", birthday), new JProperty("consentStatus", "1"))));
            return jObject.ToString();
        }
        public static string CreateUser(string name, string phone, string email, string birthday)
        {
            string responseContent = string.Empty;
            try
            {
                RestRequest restRequest = new RestRequest(Method.POST);
                restRequest.AddHeader("content-type", "application/json");
                restRequest.AddParameter("application/json", UserJSON(name, phone, email, birthday), ParameterType.RequestBody);
                RestClient restClient = new RestClient(baseURL + $"customers/create_or_update?access_token={GetToken().Replace("\"", "")}&organization={GetGuID().Replace("\"", "")}");
                IRestResponse iRestResponse = restClient.Execute(restRequest);
                string errorMessage = iRestResponse.ErrorMessage;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    responseContent = iRestResponse.Content;
                }
                else
                {
                    responseContent = errorMessage;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Обнаружена ошибка. {ex.Message}");
            }
            return responseContent;
        }
        public static string UpdateUser(string name, string phone, string email, string id, string birthday)
        {
            string responseContent = string.Empty;
            try
            {
                RestRequest restRequest = new RestRequest(Method.POST);
                restRequest.AddHeader("content-type", "application/json");
                restRequest.AddParameter("application/json", UserJSON(name, phone, email, id, birthday), ParameterType.RequestBody);
                RestClient restClient = new RestClient(baseURL + $"customers/create_or_update?access_token={GetToken().Replace("\"", "")}&organization={GetGuID().Replace("\"", "")}");
                IRestResponse iRestResponse = restClient.Execute(restRequest);
                string errorMessage = iRestResponse.ErrorMessage;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    responseContent = iRestResponse.Content;
                }
                else
                {
                    responseContent = errorMessage;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Обнаружена ошибка. {ex.Message}");
            }
            return responseContent;
        }
        static public void GetUserByID(string id)
        {
            string exceptionMessage = string.Empty;
            string responseContent = string.Empty;
            CurrentUser user = new CurrentUser();
            try
            {
                RestRequest restRequest = new RestRequest(Method.GET);
                restRequest.AddHeader("content-type", "application/json");
                RestClient restClient = new RestClient(baseURL + $"customers/get_customer_by_id?access_token={GetToken().Replace("\"", "")}&organization={GetGuID().Replace("\"", "")}&id={id}");
                IRestResponse iRestResponse = restClient.Execute(restRequest);
                string errorMessage = iRestResponse.ErrorMessage;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    responseContent = iRestResponse.Content;
                    JObject child = JObject.Parse(responseContent);
                    var task = Task.Run(() => DBConnect.GetRole((string)child["email"]));
                    string role = task.Result;
                    JArray wallets = (JArray)child["walletBalances"];
                    Wallet wallet = new Wallet(int.Parse((string)((wallets.First())["balance"])), (string)(wallets.FirstOrDefault()["wallet"]["id"]));
                    user = new CurrentUser((string)child["name"], ((string)child["phone"]), (string)child["id"], (string)child["email"], (string)child["birthday"], (string)child["social"], wallet, role);
                    Debug.WriteLine(user);
                }
                else
                {
                    responseContent = errorMessage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Обнаружена ошибка. {ex.Message}");
            }
            current_user = user;
        }
        static string orderJson(CurrentUser user, List<Dish> dishes, Menu menu)
        {

            JArray items = new JArray();
            for (int i = 0; i < dishes.Count; i++)
            {
                items.Add(new JObject(
                    new JProperty("id", dishes[i].ID),
                    new JProperty("code", i + 10),
                    new JProperty("name", dishes[i].Name),
                    new JProperty("amount", 1),
                    new JProperty("sum", dishes[i].Price),
                    new JProperty("category", menu.Where(x => x.ID.Equals(dishes[i].ParentGroup)).FirstOrDefault().Name)
                    ));
            }
            JObject addresses = new JObject(
                new JProperty("city", "Almaty"),
                new JProperty("street", "Kolotushkino"),
                new JProperty("home", "25 house")
                );
            JObject order = new JObject(
                new JProperty("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new JProperty("phone", user.Phone),
                new JProperty("address", addresses),
                new JProperty("items", items)
                );
            JObject jObject = new JObject(
                new JProperty("organization", GetGuID()),
                new JProperty("customer", new JObject(new JProperty("id", user.ID), new JProperty("name", user.Name), new JProperty("phone", user.Phone))),
                new JProperty("deliveryTerminal", "b7c26638-d47a-4818-8597-75a27aae3ba4"),
                new JProperty("order", order)
                );
            return jObject.ToString();
        }
        /// <summary>
        /// JSON-тело для смены баланса
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="cash">Сумма, на которую будет произведено изменение.</param>
        /// <returns>JSON-тело для смены баланса</returns>
        static string BalanceJson(CurrentUser user, int cash)
        {
            JObject jObject = new JObject(new JProperty("customerId", user.ID), new JProperty("organizationId", GetGuID()),
                new JProperty("walletId", user.wallet.ID), new JProperty("sum", cash));
            return jObject.ToString();
        }
        /// <summary>
        /// Уменьшает баланс пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="cash">Количество баллов</param>
        public static string WithdrawBalance(CurrentUser user, int cash)
        {
            string obj = string.Empty;
            string JsonString = string.Empty;
            string exceptionMessage = string.Empty;
            string responseContent = string.Empty;
            try
            {
                RestRequest restRequest = new RestRequest(Method.POST);
                restRequest.AddHeader("content-type", "application/json");
                restRequest.AddParameter("application/json", BalanceJson(user, cash), ParameterType.RequestBody);
                RestClient restClient = new RestClient(baseURL + $"customers/withdraw_balance?access_token={GetToken().Replace("\"", "")}");
                IRestResponse iRestResponse = restClient.Execute(restRequest);
                string errorMessage = iRestResponse.ErrorMessage;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    responseContent = iRestResponse.Content;
                }
                else
                {
                    responseContent = errorMessage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Обнаружена ошибка. {ex.Message}");
            }
            return responseContent;
        }
        /// <summary>
        /// Увеличивает баланс пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="cash">Количество баллов</param>
        public static string FillBalance(CurrentUser user, int cash)
        {
            string obj = string.Empty;
            string JsonString = string.Empty;
            string exceptionMessage = string.Empty;
            string responseContent = string.Empty;
            try
            {
                RestRequest restRequest = new RestRequest(Method.POST);
                restRequest.AddHeader("content-type", "application/json");
                restRequest.AddParameter("application/json", BalanceJson(user, cash), ParameterType.RequestBody);
                RestClient restClient = new RestClient(baseURL + $"customers/refill_balance?access_token={GetToken().Replace("\"", "")}");
                IRestResponse iRestResponse = restClient.Execute(restRequest);
                string errorMessage = iRestResponse.ErrorMessage;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    responseContent = iRestResponse.Content;
                }
                else
                {
                    responseContent = errorMessage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Обнаружена ошибка. {ex.Message}");
            }
            return responseContent;
        }
        public static string CreateOrder(CurrentUser user, List<Dish> dishes, Menu menu)
        {
            string obj = string.Empty;
            string JsonString = string.Empty;
            string exceptionMessage = string.Empty;
            string responseContent = string.Empty;
            try
            {
                RestRequest restRequest = new RestRequest(Method.POST);
                restRequest.AddHeader("content-type", "application/json");
                restRequest.AddParameter("application/json", orderJson(user, dishes, menu), ParameterType.RequestBody);
                RestClient restClient = new RestClient(baseURL + $"orders/add?access_token={GetToken().Replace("\"", "")}");
                IRestResponse iRestResponse = restClient.Execute(restRequest);
                string errorMessage = iRestResponse.ErrorMessage;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    responseContent = iRestResponse.Content;
                }
                else
                {
                    responseContent = errorMessage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Обнаружена ошибка. {ex.Message}");
            }
            return responseContent;
        }
    }
}

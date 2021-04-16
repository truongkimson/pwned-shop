using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using RazorEngine;
using RazorEngine.Templating;


namespace pwned_shop.Utils
{
    class EmailReceipt
    {
        const string DOMAIN = "sandboxdeb1de62d19e452eb9100de85568a874.mailgun.org";
        const string API_KEY = "344631cfe5180a4e8b35f23409c68980-a09d6718-29580a08";
        static void Main(string[] args)
        {
            string template = File.ReadAllText("Utils/Receipt_template.html", System.Text.Encoding.UTF8);
            const string key = "templateKey";


            var one = Engine.Razor
                            .RunCompile(template,
                                key,
                                null,
                                new
                                {
                                    List = new List<Receipt>
                                    {
                                        new Receipt
                                        {
                                            ProductName = "Valheim",
                                            ActivationCodes = new List<string> { "1234dsdf45", "231984u", "3209ufsldifj"},
                                            UnitPrice = 18,
                                            Qty = 3
                                        },

                                        new Receipt
                                        {
                                            ProductName = "Three Kingdom",
                                            ActivationCodes = new List<string> { "1dfdsf45", "32ds546di65fj"},
                                            UnitPrice = 38,
                                            Qty = 2
                                        }
                                    },

                                    TotalPrice = 130
                                });

            Console.WriteLine(one);
            Console.WriteLine(SendSimpleMessage(one, "throwawaygarbage6969@outlook.com"));
            Console.WriteLine(SendSimpleMessage(one, "pwnedshop.tester@gmail.com"));
        }

        public static IRestResponse SendSimpleMessage(string html, string email)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");

            client.Authenticator =
                new HttpBasicAuthenticator("api", API_KEY);
            RestRequest request = new RestRequest();
            request.AddParameter("domain", DOMAIN, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", $"PwnedShop-NoReply <receipt-no-reply@{DOMAIN}>");
            request.AddParameter("to", email);
            //request.AddParameter("to", "YOU@YOUR_DOMAIN_NAME");
            request.AddParameter("subject", "Pwned Shop - Purchase Receipt");
            request.AddParameter("text", "Gotcha, bitch!");
            request.AddParameter("html", html);
            request.Method = Method.POST;
            return client.Execute(request);
        }
    }

    class Receipt
    {
        public string ProductName { get; set; }
        public List<string> ActivationCodes { get; set; }
        public int UnitPrice { get; set; }
        public int Qty { get; set; }
    }
}


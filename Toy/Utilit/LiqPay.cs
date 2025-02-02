using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Web;
using Toy.Models;
using Azure.Core;
namespace Toy.Utilit
{
    public class LiqPay
    {
        private const string PrivateKey = "sandbox_WxqKOcuP8brNI6Mfz45vB8CQA47yWgCMljrNOmot";
        private const string PublicKey = "sandbox_i86637046955";

        public Dictionary<string, string> CreatePayment(string amount, string login, bool cookie)
        {
            string orderId = Guid.NewGuid().ToString();
           
            var paymentData = new
            {
                public_key = PublicKey,
                private_key = PrivateKey,
                version = "3",
                action = "pay",
                amount,
                currency = "UAH",
                description = "Оплата товару",
                order_id = orderId,
                sandbox = 1,
                result_url = $"https://lamprey-tidy-blindly.ngrok-free.app/Product/Result?login={login}&cookie={cookie}"
            };

            string dataJson = JsonConvert.SerializeObject(paymentData);
            string encodedData = Convert.ToBase64String(Encoding.UTF8.GetBytes(dataJson));

            string signature = GetLiqPaySignature(encodedData);

            Dictionary<string, string> result = new();
            result.Add("encodedData", encodedData);
            result.Add("signature", signature);
            return result;
        }

        public string GetLiqPaySignature(string encodedData)
        {
            using var sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(PrivateKey + encodedData + PrivateKey));
            return Convert.ToBase64String(hash);
        }
    }
}

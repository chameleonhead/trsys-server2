using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Trsys.CopyTrading.Abstractions
{
    public class PublishedOrders
    {
        public static readonly PublishedOrders Empty = new PublishedOrders("");

        private PublishedOrders(string text)
        {
            Text = text;
            Hash = CalculateHash(text);
        }

        public string Text { get; }
        public string Hash { get; }

        private List<PublishedOrder> _orders;
        public List<PublishedOrder> Orders
        {
            get
            {
                if (_orders == null)
                {
                    _orders = new List<PublishedOrder>();
                    if (!string.IsNullOrEmpty(Text))
                    {
                        foreach (var item in Text.Split("@"))
                        {
                            _orders.Add(PublishedOrder.Parse(item));
                        }
                    }
                }
                return _orders;
            }
        }

        public static PublishedOrders Parse(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return PublishedOrders.Empty;
            }
            foreach (var order in text.Split("@"))
            {
                if (!Regex.IsMatch(order, @"^\d+:[A-Z]+:[01]:\d+(\.\d+)?:\d+(\.\d+)?:\d+"))
                {
                    throw new PublishOrderFormatException();
                }
            }
            return new PublishedOrders(text);
        }

        private string CalculateHash(string text)
        {
            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));
            var str = BitConverter.ToString(hash);
            return str.Replace("-", string.Empty);
        }
    }
}

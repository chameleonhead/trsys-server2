using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Trsys.CopyTrading.Abstractions
{
    public class OrderText
    {
        public static readonly OrderText Empty = new OrderText("");

        private OrderText(string text)
        {
            Text = text;
            Hash = CalculateHash(text);
        }

        public string Text { get; }
        public string Hash { get; }

        private List<OrderTextItem> _orders;

        public List<OrderTextItem> Orders
        {
            get
            {
                if (_orders == null)
                {
                    _orders = new List<OrderTextItem>();
                    if (!string.IsNullOrEmpty(Text))
                    {
                        foreach (var item in Text.Split("@"))
                        {
                            _orders.Add(OrderTextItem.Parse(item));
                        }
                    }
                }
                return _orders;
            }
        }

        public static OrderText Parse(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Empty;
            }
            foreach (var order in text.Split("@"))
            {
                if (!Regex.IsMatch(order, @"^\d+:[A-Z]+:[01]:\d+(\.\d+)?:\d+(\.\d+)?:\d+"))
                {
                    throw new OrderTextFormatException();
                }
            }
            return new OrderText(text);
        }

        private string CalculateHash(string text)
        {
            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));
            var str = BitConverter.ToString(hash);
            return str.Replace("-", string.Empty);
        }

        public override bool Equals(object other)
        {
            if (other is OrderText ot)
            {
                return Text == ot.Text;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }

        public static bool operator ==(OrderText left, OrderText right)
        {
            if (left is null && right is null)
            {
                return true;
            }
            else if (left is null)
            {
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(OrderText left, OrderText right)
        {
            if (left is null && right is null)
            {
                return false;
            }
            else if (left is null)
            {
                return true;
            }
            return !left.Equals(right);
        }
    }
}

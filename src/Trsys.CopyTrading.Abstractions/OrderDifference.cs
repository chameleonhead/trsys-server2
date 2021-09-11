using System;
using System.Collections.Generic;
using System.Linq;

namespace Trsys.CopyTrading.Abstractions
{
    public class OrderDifference
    {
        public static readonly OrderDifference NoDifference = new(Array.Empty<OrderTextItem>(), Array.Empty<OrderTextItem>());

        public OrderDifference(IEnumerable<OrderTextItem> opened, IEnumerable<OrderTextItem> closed)
        {
            Opened = opened;
            Closed = closed;
            HasDifference = opened.Any() || closed.Any();
        }

        public IEnumerable<OrderTextItem> Opened { get; }
        public IEnumerable<OrderTextItem> Closed { get; }
        public bool HasDifference { get; }

        public static OrderDifference CalculateDifference(OrderText before, OrderText after)
        {
            if (before.Orders.Any())
            {
                if (after.Orders.Any())
                {
                    var closed = new List<OrderTextItem>();
                    var remainingAfter = after.Orders.ToList();
                    var same = new List<OrderTextItem>();
                    foreach (var beforeOrder in before.Orders)
                    {
                        bool found = false;
                        foreach (var afterOrder in remainingAfter.ToArray())
                        {
                            if (beforeOrder.Text == afterOrder.Text)
                            {
                                found = true;
                                same.Add(beforeOrder);
                                remainingAfter.Remove(afterOrder);
                                break;
                            }
                        }
                        if (!found)
                        {
                            closed.Add(beforeOrder);
                        }
                    }
                    return new OrderDifference(remainingAfter.ToList(), closed.ToArray());
                }
                else
                {
                    return new OrderDifference(Array.Empty<OrderTextItem>(), before.Orders);
                }
            }
            else
            {
                if (after.Orders.Any())
                {
                    return new OrderDifference(after.Orders.ToArray(), Array.Empty<OrderTextItem>());
                }
                else
                {
                    return OrderDifference.NoDifference;
                }
            }
        }
    }
}

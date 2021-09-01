using System;
using System.Collections.Generic;
using System.Linq;

namespace Trsys.CopyTrading.Abstractions
{
    public class OrderDifference<T>
    {
        public static readonly OrderDifference<T> NoDifference = new(Array.Empty<T>(), Array.Empty<T>());

        public OrderDifference(IEnumerable<T> opened, IEnumerable<T> closed)
        {
            Opened = opened;
            Closed = closed;
            HasDifference = opened.Any() || closed.Any();
        }

        public IEnumerable<T> Opened { get; }
        public IEnumerable<T> Closed { get; }
        public bool HasDifference { get; }

        public static OrderDifference<T> CalculateDifference<TOrder>(IEnumerable<T> before, IEnumerable<TOrder> after, Func<T, TOrder, int> comparer, Func<TOrder, T> targetFactory)
        {
            if (before.Any())
            {
                if (after.Any())
                {
                    var closed = new List<T>();
                    var remainingAfter = after.ToList();
                    var same = new List<T>();
                    foreach (var beforeOrder in before)
                    {
                        bool found = false;
                        foreach (var afterOrder in remainingAfter.ToArray())
                        {
                            if (comparer(beforeOrder, afterOrder) == 0)
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
                    return new OrderDifference<T>(remainingAfter.Select(targetFactory).ToList(), closed.ToArray());
                }
                else
                {
                    return new OrderDifference<T>(Array.Empty<T>(), before.ToArray());
                }
            }
            else
            {
                if (after.Any())
                {
                    return new OrderDifference<T>(after.Select(targetFactory).ToArray(), Array.Empty<T>());
                }
                else
                {
                    return OrderDifference<T>.NoDifference;
                }
            }
        }
    }
}

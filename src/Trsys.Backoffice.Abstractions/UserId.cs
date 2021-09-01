using EventFlow.Core;

namespace Trsys.Backoffice.Abstractions
{
    public class UserId : Identity<UserId>
    {
        public UserId(string value) : base(value)
        {
        }
    }
}
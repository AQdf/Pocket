using System.Net;

namespace Sho.Pocket.Application.Exceptions
{
    public class UserHasNoAssetsException : BasePocketException
    {
        private const string _message = "You should have at least one asset to execute the operation.";

        public UserHasNoAssetsException() : base(nameof(UserHasNoAssetsException), _message, HttpStatusCode.BadRequest)
        {
        }
    }
}

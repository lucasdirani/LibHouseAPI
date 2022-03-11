using KissLog;
using LibHouse.API.Extensions.Notifications;
using LibHouse.Business.Notifiers;
using LibHouse.Infrastructure.Authentication.Token;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LibHouse.API.BaseControllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotifier _notifier;

        public ILoggedUser LoggedUser { get; }
        public IKLogger Logger { get; }

        protected MainController(
            INotifier notifier,
            ILoggedUser loggedUser,
            IKLogger logger)
        {
            _notifier = notifier;
            LoggedUser = loggedUser;
            Logger = logger;
        }

        protected bool EndpointOperationWasSuccessful => !_notifier.HasNotification();
        protected bool EndpointOperationFailed => !EndpointOperationWasSuccessful;

        protected void NotifyError(string errorTitle, string errorMessage, string errorCode) =>
            _notifier.Handle(new Notification(errorMessage, errorCode, errorTitle));

        protected ActionResult CustomResponseForGetEndpoint(object response = null)
        {
            if (response is null)
            {
                return NotFound(_notifier.GetNotifications().ToProblemDetails(Request, HttpStatusCode.NotFound));
            }

            return CustomResponse(response);
        }

        protected ActionResult CustomResponseForPostEndpoint(
            object response,
            object resourceIdentifier = null,
            string resourceCreatedAt = null)
        {
            if (EndpointOperationFailed)
            {
                return BadRequest(_notifier.GetNotifications().ToProblemDetails(Request, HttpStatusCode.BadRequest));
            }

            return CreatedAtAction(resourceCreatedAt, new { id = resourceIdentifier }, response);
        }

        private ActionResult CustomResponse(object response)
        {
            if (EndpointOperationWasSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(_notifier.GetNotifications().ToProblemDetails(Request, HttpStatusCode.BadRequest));
        }
    }
}
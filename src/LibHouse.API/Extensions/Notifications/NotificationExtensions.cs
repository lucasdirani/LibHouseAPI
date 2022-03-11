using LibHouse.Business.Notifiers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LibHouse.API.Extensions.Notifications
{
    public static class NotificationExtensions
    {
        public static IEnumerable<ProblemDetails> ToProblemDetails(
            this IReadOnlyList<Notification> notifications,
            HttpRequest httpRequest,
            HttpStatusCode statusCode)
            => notifications.Select(notification => new ProblemDetails()
            {
                Instance = httpRequest.Path.Value,
                Title = notification.Title,
                Detail = notification.Message,
                Status = (int) statusCode
            });
    }
}
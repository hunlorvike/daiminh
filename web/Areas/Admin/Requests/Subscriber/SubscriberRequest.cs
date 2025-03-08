using System.ComponentModel;
using core.Common.Enums;
using FluentValidation;

namespace web.Areas.Admin.Requests.Subscriber;

public class SubscriberDeleteRequest
{
    public int Id { get; set; }
}

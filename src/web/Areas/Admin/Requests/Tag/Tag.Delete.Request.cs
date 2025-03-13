using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Tag;

public class TagDeleteRequest
{
    public int Id { get; set; }
}
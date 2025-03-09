using System.ComponentModel;

namespace web.Areas.Client.Models.Account;

public class UserViewModel
{
    public int Id { get; set; }
    [DisplayName("Tài khoản")] public string? Username { get; set; }
    [DisplayName("Địa chỉ email")] public string? Email { get; set; }
    [DisplayName("Vai trò")] public string? RoleName { get; set; }
    [DisplayName("Ngày tạo")] public DateTime? CreatedAt { get; set; }
}
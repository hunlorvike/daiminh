namespace shared.Enums;

using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Defines a set of permissions that can be granted to users or roles within the application.
/// This enum uses bitwise flags, allowing multiple permissions to be combined into a single value.
/// </summary>
[Flags] // Important: Add the [Flags] attribute
public enum Permission
{
    /// <summary>
    /// No permissions granted.
    /// </summary>
    [Display(Name = "Không có quyền")] // Vietnamese: No permissions
    None = 0,

    /// <summary>
    /// Permission to view resources.
    /// </summary>
    [Display(Name = "Xem")] // Vietnamese: View
    View = 1 << 0,

    /// <summary>
    /// Permission to create new resources.
    /// </summary>
    [Display(Name = "Tạo")] // Vietnamese: Create
    Create = 1 << 1,

    /// <summary>
    /// Permission to edit existing resources.
    /// </summary>
    [Display(Name = "Sửa")] // Vietnamese: Edit
    Edit = 1 << 2,

    /// <summary>
    /// Permission to delete resources.
    /// </summary>
    [Display(Name = "Xóa")] // Vietnamese: Delete
    Delete = 1 << 3,

    /// <summary>
    /// Permission to publish content.
    /// </summary>
    [Display(Name = "Xuất bản")] // Vietnamese: Publish
    Publish = 1 << 4,

    /// <summary>
    /// Permission to manage users.
    /// </summary>
    [Display(Name = "Quản lý người dùng")] // Vietnamese: Manage Users
    ManageUsers = 1 << 5,

    /// <summary>
    /// Permission to manage roles.
    /// </summary>
    [Display(Name = "Quản lý vai trò")] // Vietnamese: Manage Roles
    ManageRoles = 1 << 6,

    /// <summary>
    /// Permission to manage application settings.
    /// </summary>
    [Display(Name = "Quản lý cài đặt")] // Vietnamese: Manage Settings
    ManageSettings = 1 << 7,

    /// <summary>
    /// Permission to manage categories.
    /// </summary>
    [Display(Name = "Quản lý danh mục")] // Vietnamese: Manage Categories
    ManageCategories = 1 << 8,

    /// <summary>
    /// Permission to manage tags.
    /// </summary>
    [Display(Name = "Quản lý thẻ")] // Vietnamese: Manage Tags
    ManageTags = 1 << 9,

    /// <summary>
    /// Permission to manage comments.
    /// </summary>
    [Display(Name = "Quản lý bình luận")] // Vietnamese: Manage Comments
    ManageComments = 1 << 10,

    /// <summary>
    /// Permission to manage products.
    /// </summary>
    [Display(Name = "Quản lý sản phẩm")] // Vietnamese: Manage Products
    ManageProducts = 1 << 11,

    /// <summary>
    /// Permission to manage orders.
    /// </summary>
    [Display(Name = "Quản lý đơn hàng")] // Vietnamese: Manage Orders
    ManageOrders = 1 << 12,

    /// <summary>
    ///  Represents full access, granting all available permissions. Calculated as the bitwise NOT of None.
    /// </summary>
    [Display(Name = "Toàn quyền")] // Vietnamese: Full Access
    FullAccess = ~None // Bitwise NOT of None (all bits set to 1)
}
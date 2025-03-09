namespace shared.Enums;

[Flags]
public enum Permission
{
    None = 0,
    View = 1 << 0,
    Create = 1 << 1,
    Edit = 1 << 2,
    Delete = 1 << 3,
    Publish = 1 << 4,
    ManageUsers = 1 << 5,
    ManageRoles = 1 << 6,
    ManageSettings = 1 << 7,
    ManageCategories = 1 << 8,
    ManageTags = 1 << 9,
    ManageComments = 1 << 10,
    ManageProducts = 1 << 11,
    ManageOrders = 1 << 12,
    FullAccess = ~None
}
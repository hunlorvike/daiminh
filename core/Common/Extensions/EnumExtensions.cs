using core.Common.Enums;

namespace core.Common.Extensions;

public static class EnumExtensions
{
    public static string ToStringValue(this Status status)
    {
        return status.ToString().ToLowerInvariant();
    }

    public static Status ToStatusEnum(this string status)
    {
        return Enum.Parse<Status>(status, true);
    }

    public static string ToStringValue(this PublishStatus status)
    {
        return status.ToString().ToLowerInvariant();
    }

    public static PublishStatus ToPublishStatusEnum(this string status)
    {
        return Enum.Parse<PublishStatus>(status, true);
    }

    public static string ToStringValue(this CommentStatus status)
    {
        return status.ToString().ToLowerInvariant();
    }

    public static CommentStatus ToCommentStatusEnum(this string status)
    {
        return Enum.Parse<CommentStatus>(status, true);
    }

    public static string ToStringValue(this ContactStatus status)
    {
        return status.ToString().ToLowerInvariant();
    }

    public static ContactStatus ToContactStatusEnum(this string status)
    {
        return Enum.Parse<ContactStatus>(status, true);
    }

    public static string ToStringValue(this SubscriberStatus status)
    {
        return status.ToString().ToLowerInvariant();
    }

    public static SubscriberStatus ToSubscriberStatusEnum(this string status)
    {
        return Enum.Parse<SubscriberStatus>(status, true);
    }

    public static string ToStringValue(this ReviewStatus status)
    {
        return status.ToString().ToLowerInvariant();
    }

    public static ReviewStatus ToReviewStatusEnum(this string status)
    {
        return Enum.Parse<ReviewStatus>(status, true);
    }

    public static string ToStringValue(this FieldType fieldType)
    {
        return fieldType.ToString().ToLowerInvariant();
    }

    public static FieldType ToFieldTypeEnum(this string fieldType)
    {
        return Enum.Parse<FieldType>(fieldType, true);
    }

    public static string[] ToStringArray(this Permission permissions)
    {
        return permissions.ToString()
            .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.ToLowerInvariant())
            .ToArray();
    }

    public static Permission ToPermissionEnum(this string[] permissions)
    {
        return permissions
            .Select(p => Enum.Parse<Permission>(p, true))
            .Aggregate(Permission.None, (current, permission) => current | permission);
    }
}
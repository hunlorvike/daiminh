using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class version1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AdminNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    AltText = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    MediaType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Newsletters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UnsubscribedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Newsletters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanonicalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoIndex = table.Column<bool>(type: "bit", nullable: false),
                    NoFollow = table.Column<bool>(type: "bit", nullable: false),
                    OgTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OgDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OgImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OgType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchemaMarkup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BreadcrumbJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SitemapPriority = table.Column<double>(type: "float", nullable: true),
                    SitemapChangeFrequency = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PopupModals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    LinkUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PopupModals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Slide",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CtaText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CtaLink = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Target = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "_self"),
                    OrderIndex = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slide", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClientTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClientCompany = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClientAvatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttributeValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FeaturedImage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ThumbnailImage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AuthorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AuthorAvatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EstimatedReadingMinutes = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    meta_title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    meta_description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    meta_keywords = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    canonical_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    no_index = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    no_follow = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    og_title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    og_description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    og_image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    og_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "website"),
                    twitter_title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    twitter_description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    twitter_image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    twitter_card = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "summary_large_image"),
                    schema_markup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    breadcrumb_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sitemap_priority = table.Column<double>(type: "float", nullable: true, defaultValue: 0.5),
                    sitemap_change_frequency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "monthly")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FAQs_Categories_category_id",
                        column: x => x.category_id,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Origin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Specifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Usage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BrandId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    meta_title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    meta_description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    meta_keywords = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    canonical_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    no_index = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    no_follow = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    og_title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    og_description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    og_image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    og_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "website"),
                    twitter_title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    twitter_description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    twitter_image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    twitter_card = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "summary_large_image"),
                    schema_markup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    breadcrumb_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sitemap_priority = table.Column<double>(type: "float", nullable: true, defaultValue: 0.5),
                    sitemap_change_frequency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "monthly")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTags",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTags", x => new { x.ArticleId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ArticleTags_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleProducts",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleProducts", x => new { x.ArticleId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ArticleProducts_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributes",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    AttributeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributes", x => new { x.ProductId, x.AttributeId });
                    table.ForeignKey(
                        name: "FK_ProductAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAttributes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AltText = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsMain = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductReviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductReviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => new { x.ProductId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ProductTags_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StockQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariationAttributeValues",
                columns: table => new
                {
                    ProductVariationId = table.Column<int>(type: "int", nullable: false),
                    AttributeValueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariationAttributeValues", x => new { x.ProductVariationId, x.AttributeValueId });
                    table.ForeignKey(
                        name: "FK_ProductVariationAttributeValues_AttributeValues_AttributeValueId",
                        column: x => x.AttributeValueId,
                        principalTable: "AttributeValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariationAttributeValues_ProductVariations_ProductVariationId",
                        column: x => x.ProductVariationId,
                        principalTable: "ProductVariations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Attributes",
                columns: new[] { "Id", "created_at", "created_by", "Name", "Slug", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 17, 11, 25, 27, 150, DateTimeKind.Utc).AddTicks(5256), null, "Màu sắc", "mau-sac", null, null },
                    { 2, new DateTime(2025, 5, 17, 11, 25, 27, 150, DateTimeKind.Utc).AddTicks(5258), null, "Dung tích", "dung-tich", null, null },
                    { 3, new DateTime(2025, 5, 17, 11, 25, 27, 150, DateTimeKind.Utc).AddTicks(5259), null, "Độ bóng", "do-bong", null, null },
                    { 4, new DateTime(2025, 5, 17, 11, 25, 27, 150, DateTimeKind.Utc).AddTicks(5260), null, "Bề mặt áp dụng", "be-mat-ap-dung", null, null }
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "created_at", "created_by", "Description", "IsActive", "LogoUrl", "Name", "Slug", "updated_at", "updated_by", "Website" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 17, 11, 25, 27, 150, DateTimeKind.Utc).AddTicks(2649), null, null, true, null, "Dulux", "dulux", null, null, null },
                    { 2, new DateTime(2025, 5, 17, 11, 25, 27, 150, DateTimeKind.Utc).AddTicks(2652), null, null, true, null, "Jotun", "jotun", null, null, null },
                    { 3, new DateTime(2025, 5, 17, 11, 25, 27, 150, DateTimeKind.Utc).AddTicks(2749), null, null, true, null, "Kova", "kova", null, null, null },
                    { 4, new DateTime(2025, 5, 17, 11, 25, 27, 150, DateTimeKind.Utc).AddTicks(2751), null, null, true, null, "Sika", "sika", null, null, null },
                    { 5, new DateTime(2025, 5, 17, 11, 25, 27, 150, DateTimeKind.Utc).AddTicks(2752), null, null, true, null, "Nippon Paint", "nippon-paint", null, null, null },
                    { 6, new DateTime(2025, 5, 17, 11, 25, 27, 150, DateTimeKind.Utc).AddTicks(2754), null, null, true, null, "My Kolor", "my-kolor", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "created_at", "created_by", "Description", "Icon", "IsActive", "Name", "ParentId", "Slug", "updated_at", "updated_by" },
                values: new object[] { 1, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(1918), null, null, null, true, "Sơn", null, "son", null, null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "created_at", "created_by", "Description", "Icon", "IsActive", "Name", "OrderIndex", "ParentId", "Slug", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 2, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(1919), null, null, null, true, "Chống Thấm", 1, null, "chong-tham", null, null },
                    { 3, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(1921), null, null, null, true, "Vật Liệu Xây Dựng", 2, null, "vat-lieu-xay-dung", null, null }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "created_at", "created_by", "Description", "Icon", "IsActive", "Name", "OrderIndex", "ParentId", "Slug", "Type", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 4, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(1923), null, null, null, true, "Tin Tức & Sự Kiện", 3, null, "tin-tuc-su-kien", 1, null, null },
                    { 5, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(1924), null, null, null, true, "Câu Hỏi Thường Gặp", 4, null, "cau-hoi-thuong-gap", 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "Pages",
                columns: new[] { "Id", "BreadcrumbJson", "CanonicalUrl", "Content", "created_at", "created_by", "MetaDescription", "MetaKeywords", "MetaTitle", "NoFollow", "NoIndex", "OgDescription", "OgImage", "OgTitle", "OgType", "PublishedAt", "SchemaMarkup", "SitemapChangeFrequency", "SitemapPriority", "Slug", "Title", "TwitterCard", "TwitterDescription", "TwitterImage", "TwitterTitle", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1, null, null, "This is the About Us page content.", new DateTime(2025, 5, 17, 11, 25, 27, 155, DateTimeKind.Utc).AddTicks(1760), null, null, null, null, false, false, null, null, null, "website", new DateTime(2025, 5, 17, 11, 25, 27, 155, DateTimeKind.Utc).AddTicks(1757), null, "monthly", 0.5, "about-us", "About Us", "summary_large_image", null, null, null, null, null },
                    { 2, null, null, "This is the Privacy Policy page content.", new DateTime(2025, 5, 17, 11, 25, 27, 155, DateTimeKind.Utc).AddTicks(1764), null, null, null, null, false, false, null, null, null, "website", new DateTime(2025, 5, 17, 11, 25, 27, 155, DateTimeKind.Utc).AddTicks(1763), null, "monthly", 0.5, "privacy-policy", "Privacy Policy", "summary_large_image", null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "updated_at", "updated_by", "Value" },
                values: new object[] { 1, "General", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8668), null, "Sơn Đại Minh Việt Nam", "Tên website hiển thị trên trang và tiêu đề trình duyệt.", true, "SiteName", null, null, "Sơn Đại Minh Việt Nam" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "Type", "updated_at", "updated_by", "Value" },
                values: new object[,]
                {
                    { 2, "General", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8670), null, "https://localhost:7001", "Địa chỉ URL chính của website (ví dụ: https://www.example.com).", true, "SiteUrl", 7, null, null, "https://localhost:7001" },
                    { 3, "General", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8672), null, "sondaiminh@gmail.com", "Địa chỉ email quản trị viên để nhận thông báo hệ thống (đơn hàng, liên hệ...).", true, "AdminEmail", 6, null, null, "sondaiminh@gmail.com" },
                    { 4, "General", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8750), null, "/images/logo.png", "Đường dẫn đến file logo website.", true, "LogoUrl", 3, null, null, "/images/logo.png" },
                    { 5, "General", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8752), null, "/images/favicon.ico", "Đường dẫn đến file favicon.ico hoặc ảnh favicon.", true, "FaviconUrl", 3, null, null, "/images/favicon.ico" },
                    { 6, "General", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8753), null, "12", "Số lượng sản phẩm/bài viết hiển thị trên mỗi trang danh sách mặc định.", true, "ItemsPerPage", 8, null, null, "12" }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "updated_at", "updated_by", "Value" },
                values: new object[] { 11, "Contact", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8755), null, "Sơn Đại Minh", "Tên công ty hoặc tổ chức sở hữu website.", true, "CompanyName", null, null, "Sơn Đại Minh" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "Type", "updated_at", "updated_by", "Value" },
                values: new object[,]
                {
                    { 12, "Contact", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8757), null, "Tiên Phương, Chương Mỹ, Hà Nội", "Địa chỉ liên hệ đầy đủ của công ty.", true, "ContactAddress", 1, null, null, "Tiên Phương, Chương Mỹ, Hà Nội" },
                    { 13, "Contact", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8758), null, "0979758340", "Số điện thoại liên hệ chung.", true, "ContactPhone", 4, null, null, "0979758340" },
                    { 14, "Contact", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8760), null, "sondaiminh@gmail.com", "Địa chỉ email hiển thị công khai để liên hệ.", true, "ContactEmail", 6, null, null, "sondaiminh@gmail.com" },
                    { 15, "Contact", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8761), null, "<iframe src=\"https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5152.719628304902!2d105.68369562421606!3d20.94205043073292!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3134539b465b1be5%3A0x279c032890c390c5!2zxJDhuqFpIE1pbmggVmnhu4d0IE5hbQ!5e1!3m2!1svi!2s!4v1745895737603!5m2!1svi!2s\" width=\"600\" height=\"450\" style=\"border:0;\" allowfullscreen=\"\" loading=\"lazy\" referrerpolicy=\"no-referrer-when-downgrade\"></iframe>", "Mã nhúng HTML của bản đồ (ví dụ: Google Maps iframe) hiển thị trên trang Liên hệ.", true, "ContactMapEmbed", 2, null, null, "<iframe src=\"https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5152.719628304902!2d105.68369562421606!3d20.94205043073292!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3134539b465b1be5%3A0x279c032890c390c5!2zxJDhuqFpIE1pbmggVmnhu4d0IE5hbQ!5e1!3m2!1svi!2s!4v1745895737603!5m2!1svi!2s\" width=\"600\" height=\"450\" style=\"border:0;\" allowfullscreen=\"\" loading=\"lazy\" referrerpolicy=\"no-referrer-when-downgrade\"></iframe>" },
                    { 16, "Contact", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8763), null, "0979758340", "Số điện thoại Hotline hỗ trợ nhanh.", true, "HotlinePhone", 4, null, null, "0979758340" }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "updated_at", "updated_by", "Value" },
                values: new object[,]
                {
                    { 17, "Contact", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8764), null, "Thứ 2 - Thứ 7: 8h00 - 17h00", "Giờ làm việc của công ty.", true, "ContactWorkingHours", null, null, "Thứ 2 - Thứ 7: 8h00 - 17h00" },
                    { 18, "Contact", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8766), null, null, "Mã số thuế của công ty.", true, "TaxId", null, null, null },
                    { 21, "SEO", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8768), null, "Sơn Chống Thấm, Vật Liệu Sơn Chính Hãng | Đại Minh Việt Nam", "Tiêu đề meta mặc định cho các trang không có tiêu đề riêng.", true, "DefaultMetaTitle", null, null, "Sơn Chống Thấm, Vật Liệu Sơn Chính Hãng | Đại Minh Việt Nam" }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "Type", "updated_at", "updated_by", "Value" },
                values: new object[] { 22, "SEO", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8770), null, "Đại Minh Việt Nam chuyên cung cấp sơn, vật liệu chống thấm, phụ gia bê tông chính hãng từ các thương hiệu hàng đầu. Tư vấn giải pháp thi công hiệu quả. Liên hệ 0979758340.", "Mô tả meta mặc định (dưới 160 ký tự) cho các trang không có mô tả riêng.", true, "DefaultMetaDescription", 1, null, null, "Đại Minh Việt Nam chuyên cung cấp sơn, vật liệu chống thấm, phụ gia bê tông chính hãng từ các thương hiệu hàng đầu. Tư vấn giải pháp thi công hiệu quả. Liên hệ 0979758340." });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "updated_at", "updated_by", "Value" },
                values: new object[] { 23, "SEO", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8772), null, null, "Mã ID Google Analytics (ví dụ: UA-XXXXXXX-Y hoặc G-XXXXXXXXXX).", true, "GoogleAnalyticsId", null, null, null });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "Type", "updated_at", "updated_by", "Value" },
                values: new object[,]
                {
                    { 24, "SEO", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8773), null, null, "Các meta tag xác minh website (Google, Bing, ...).", true, "VerificationMetaTags", 1, null, null, null },
                    { 31, "Social Media", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8775), null, "https://www.facebook.com/LienDaiMinh", "URL trang Facebook của công ty.", true, "SocialFacebookUrl", 7, null, null, "https://www.facebook.com/LienDaiMinh" },
                    { 32, "Social Media", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8776), null, null, "URL trang Twitter (X) của công ty.", true, "SocialTwitterUrl", 7, null, null, null },
                    { 33, "Social Media", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8778), null, null, "URL trang Instagram của công ty.", true, "SocialInstagramUrl", 7, null, null, null },
                    { 34, "Social Media", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8779), null, null, "URL trang LinkedIn của công ty.", true, "SocialLinkedInUrl", 7, null, null, null },
                    { 35, "Social Media", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8781), null, null, "URL kênh Youtube của công ty.", true, "SocialYoutubeUrl", 7, null, null, null },
                    { 36, "Social Media", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8783), null, "https://www.tiktok.com/@hung.daiminh", "URL kênh Tiktok của công ty.", true, "SocialTiktokUrl", 7, null, null, "https://www.tiktok.com/@hung.daiminh" },
                    { 37, "Social Media", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8784), null, "0979758340", "Số điện thoại Zalo để liên hệ nhanh (có thể khác Hotline).", true, "SocialZaloPhone", 4, null, null, "0979758340" }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "updated_at", "updated_by", "Value" },
                values: new object[] { 41, "Email", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8786), null, "smtp.example.com", "Địa chỉ máy chủ SMTP để gửi email.", true, "SmtpHost", null, null, "smtp.example.com" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "Type", "updated_at", "updated_by", "Value" },
                values: new object[] { 42, "Email", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8787), null, "587", "Cổng SMTP (ví dụ: 587, 465, 25).", true, "SmtpPort", 8, null, null, "587" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "updated_at", "updated_by", "Value" },
                values: new object[,]
                {
                    { 43, "Email", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8789), null, "user@example.com", "Tên đăng nhập SMTP.", true, "SmtpUsername", null, null, "user@example.com" },
                    { 44, "Email", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8791), null, null, "**QUAN TRỌNG**: Mật khẩu SMTP. Nên cấu hình qua UI, không seed giá trị thật.", true, "SmtpPassword", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "Type", "updated_at", "updated_by", "Value" },
                values: new object[] { 45, "Email", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8792), null, "true", "Sử dụng mã hóa SSL/TLS khi gửi email.", true, "SmtpUseSsl", 9, null, null, "true" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "updated_at", "updated_by", "Value" },
                values: new object[] { 46, "Email", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8794), null, "Đại Minh Việt Nam", "Tên hiển thị trong ô 'From' của email gửi đi.", true, "EmailFromName", null, null, "Đại Minh Việt Nam" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "Type", "updated_at", "updated_by", "Value" },
                values: new object[] { 47, "Email", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8795), null, "noreply@daiminhvietnam.com", "Địa chỉ email hiển thị trong ô 'From' của email gửi đi.", true, "EmailFromAddress", 6, null, null, "noreply@daiminhvietnam.com" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "updated_at", "updated_by", "Value" },
                values: new object[,]
                {
                    { 48, "Email", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8798), null, "ContactReply", "Mã/tên template email phản hồi tự động khi nhận form liên hệ.", true, "EmailTemplateContactFormReply", null, null, "ContactReply" },
                    { 49, "Email", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8800), null, "NewsletterSubscribe", "Mã/tên template email xác nhận đăng ký nhận tin.", true, "EmailTemplateNewsletterSubscribe", null, null, "NewsletterSubscribe" }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "Type", "updated_at", "updated_by", "Value" },
                values: new object[,]
                {
                    { 51, "Appearance", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8801), null, "/images/banners/banner1.jpg", "URL ảnh banner chính trang chủ.", true, "HomepageBanner1Url", 3, null, null, "/images/banners/banner1.jpg" },
                    { 52, "Appearance", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8803), null, "/", "URL liên kết khi click banner chính trang chủ.", true, "HomepageBanner1Link", 7, null, null, "/" }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "updated_at", "updated_by", "Value" },
                values: new object[] { 53, "Appearance", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8850), null, "© 2025 Sơn Đại Minh Việt Nam. All rights reserved.", "Nội dung text copyright hiển thị ở chân trang.", true, "CopyrightText", null, null, "© 2025 Sơn Đại Minh Việt Nam. All rights reserved." });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "Type", "updated_at", "updated_by", "Value" },
                values: new object[] { 61, "Integration", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8851), null, null, "Mã script nhúng Live Chat (Zalo, Tawk.to, subiz...).", true, "LiveChatScript", 2, null, null, null });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "updated_at", "updated_by", "Value" },
                values: new object[,]
                {
                    { 71, "E-commerce", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8853), null, "VND", "Mã tiền tệ chính được sử dụng (ví dụ: VND, USD).", true, "CurrencyCode", null, null, "VND" },
                    { 72, "E-commerce", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8854), null, "đ", "Ký hiệu tiền tệ hiển thị (ví dụ: đ, $).", true, "CurrencySymbol", null, null, "đ" }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Category", "created_at", "created_by", "DefaultValue", "Description", "IsActive", "Key", "Type", "updated_at", "updated_by", "Value" },
                values: new object[] { 73, "E-commerce", new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(8856), null, "/images/product-placeholder.png", "URL ảnh mặc định hiển thị khi sản phẩm không có ảnh.", true, "DefaultProductImageUrl", 3, null, null, "/images/product-placeholder.png" });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "created_at", "created_by", "Description", "Name", "Slug", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(4689), null, null, "Chống thấm", "chong-tham", null, null },
                    { 2, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(4691), null, null, "Sơn nội thất", "son-noi-that", null, null },
                    { 3, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(4692), null, null, "Sơn ngoại thất", "son-ngoai-that", null, null },
                    { 4, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(4693), null, null, "Phụ gia bê tông", "phu-gia-be-tong", null, null },
                    { 5, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(4695), null, null, "Keo chít mạch", "keo-chit-mach", null, null },
                    { 6, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(4696), null, null, "Vật liệu xây dựng", "vat-lieu-xay-dung", null, null }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "created_at", "created_by", "Description", "Name", "Slug", "Type", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 7, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(4697), null, null, "Tư vấn chọn sơn", "tu-van-chon-son", 1, null, null },
                    { 8, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(4698), null, null, "Hướng dẫn thi công", "huong-dan-thi-cong", 1, null, null },
                    { 9, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(4700), null, null, "Kinh nghiệm chống thấm", "kinh-nghiem-chong-tham", 1, null, null },
                    { 10, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(4701), null, null, "Bảo trì nhà cửa", "bao-tri-nha-cua", 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "Testimonials",
                columns: new[] { "Id", "ClientAvatar", "ClientCompany", "ClientName", "ClientTitle", "Content", "created_at", "created_by", "IsActive", "Rating", "updated_at", "updated_by" },
                values: new object[] { 1, null, null, "Nguyễn Văn A", "Chủ nhà", "Sơn của Đại Minh rất bền màu và dễ thi công. Tôi rất hài lòng!", new DateTime(2025, 5, 17, 11, 25, 27, 153, DateTimeKind.Utc).AddTicks(1562), null, true, 5, null, null });

            migrationBuilder.InsertData(
                table: "Testimonials",
                columns: new[] { "Id", "ClientAvatar", "ClientCompany", "ClientName", "ClientTitle", "Content", "created_at", "created_by", "IsActive", "OrderIndex", "Rating", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 2, null, "Công ty Xây dựng B&B", "Trần Thị B", "Nhà thầu", "Vật liệu chống thấm Sika từ Đại Minh luôn đảm bảo chất lượng cho công trình của chúng tôi.", new DateTime(2025, 5, 17, 11, 25, 27, 153, DateTimeKind.Utc).AddTicks(1565), null, true, 1, 5, null, null },
                    { 3, null, null, "Lê Văn C", "Khách hàng cá nhân", "Được tư vấn rất nhiệt tình để chọn đúng loại sơn cho ngôi nhà cũ. Dịch vụ tuyệt vời!", new DateTime(2025, 5, 17, 11, 25, 27, 153, DateTimeKind.Utc).AddTicks(1566), null, true, 2, 4, null, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "created_at", "created_by", "Email", "FullName", "IsActive", "PasswordHash", "updated_at", "updated_by", "Username" },
                values: new object[] { 1, new DateTime(2025, 5, 17, 11, 25, 27, 87, DateTimeKind.Utc).AddTicks(266), null, "admin@admin.com", "Quản trị viên", true, "AQAAAAIAAYagAAAAEOrfCmWPBSqoU/V7uVPeQXDquWEX/FkmWzgi/r1VwWDV9S90GF/sqlGjOcFaMtJe4g==", null, null, "admin" });

            migrationBuilder.InsertData(
                table: "AttributeValues",
                columns: new[] { "Id", "AttributeId", "created_at", "created_by", "Slug", "updated_at", "updated_by", "Value" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5489), null, "trang", null, null, "Trắng" },
                    { 2, 1, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5491), null, "do", null, null, "Đỏ" },
                    { 3, 1, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5492), null, "xanh-duong", null, null, "Xanh dương" },
                    { 4, 1, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5494), null, "vang", null, null, "Vàng" },
                    { 5, 2, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5495), null, "1-lit", null, null, "1 Lít" },
                    { 6, 2, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5496), null, "5-lit", null, null, "5 Lít" },
                    { 7, 2, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5497), null, "18-lit", null, null, "18 Lít" },
                    { 8, 2, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5499), null, "20-kg", null, null, "20 Kg" },
                    { 9, 3, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5500), null, "bong", null, null, "Bóng" },
                    { 10, 3, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5501), null, "mo", null, null, "Mờ" },
                    { 11, 3, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5502), null, "ban-bong", null, null, "Bán bóng" },
                    { 12, 4, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5504), null, "tuong-noi-that", null, null, "Tường nội thất" },
                    { 13, 4, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5505), null, "tuong-ngoai-that", null, null, "Tường ngoại thất" },
                    { 14, 4, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5506), null, "san-be-tong", null, null, "Sàn bê tông" },
                    { 15, 4, new DateTime(2025, 5, 17, 11, 25, 27, 158, DateTimeKind.Utc).AddTicks(5507), null, "san-thuong", null, null, "Sân thượng" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "created_at", "created_by", "Description", "Icon", "IsActive", "Name", "ParentId", "Slug", "updated_at", "updated_by" },
                values: new object[] { 6, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(1926), null, null, null, true, "Sơn Nội Thất", 1, "son-noi-that", null, null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "created_at", "created_by", "Description", "Icon", "IsActive", "Name", "OrderIndex", "ParentId", "Slug", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 7, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(1928), null, null, null, true, "Sơn Ngoại Thất", 1, 1, "son-ngoai-that", null, null },
                    { 8, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(1929), null, null, null, true, "Sơn Lót", 2, 1, "son-lot", null, null },
                    { 9, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(1931), null, null, null, true, "Sơn Chống Kiềm", 3, 1, "son-chong-kiem", null, null }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "created_at", "created_by", "Description", "Icon", "IsActive", "Name", "ParentId", "Slug", "updated_at", "updated_by" },
                values: new object[] { 10, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(1932), null, null, null, true, "Chống Thấm Sàn Mái", 2, "chong-tham-san-mai", null, null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "created_at", "created_by", "Description", "Icon", "IsActive", "Name", "OrderIndex", "ParentId", "Slug", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 11, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(1934), null, null, null, true, "Chống Thấm Tường", 1, 2, "chong-tham-tuong", null, null },
                    { 12, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(1935), null, null, null, true, "Chống Thấm Nhà Vệ Sinh", 2, 2, "chong-tham-nha-ve-sinh", null, null }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "created_at", "created_by", "Description", "Icon", "IsActive", "Name", "ParentId", "Slug", "updated_at", "updated_by" },
                values: new object[] { 13, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(1937), null, null, null, true, "Phụ Gia Bê Tông", 3, "phu-gia-be-tong", null, null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "created_at", "created_by", "Description", "Icon", "IsActive", "Name", "OrderIndex", "ParentId", "Slug", "updated_at", "updated_by" },
                values: new object[] { 14, new DateTime(2025, 5, 17, 11, 25, 27, 151, DateTimeKind.Utc).AddTicks(1939), null, null, null, true, "Keo Chít Mạch", 1, 3, "keo-chit-mach", null, null });

            migrationBuilder.InsertData(
                table: "FAQs",
                columns: new[] { "Id", "Answer", "category_id", "created_at", "created_by", "IsActive", "Question", "updated_at", "updated_by" },
                values: new object[] { 1, "Sơn nội thất và ngoại thất khác nhau về thành phần hóa học để phù hợp với điều kiện môi trường. Sơn ngoại thất chứa các chất chống tia UV, chống thấm tốt hơn để chịu được nắng, mưa, ẩm ướt. Sơn nội thất an toàn hơn cho sức khỏe, ít mùi và có độ bền màu trong nhà tốt.", 5, new DateTime(2025, 5, 17, 11, 25, 27, 161, DateTimeKind.Utc).AddTicks(7691), null, true, "Sơn nội thất và sơn ngoại thất khác nhau như thế nào?", null, null });

            migrationBuilder.InsertData(
                table: "FAQs",
                columns: new[] { "Id", "Answer", "category_id", "created_at", "created_by", "IsActive", "OrderIndex", "Question", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 2, "Lượng sơn cần dùng phụ thuộc vào diện tích cần sơn, loại sơn, và bề mặt. Trung bình 1 lít sơn có thể phủ được 8-10m2 cho 2 lớp. Bạn cần đo diện tích tường, trần nhà và tham khảo hướng dẫn của nhà sản xuất sơn.", 5, new DateTime(2025, 5, 17, 11, 25, 27, 161, DateTimeKind.Utc).AddTicks(7693), null, true, 1, "Làm thế nào để tính toán lượng sơn cần dùng?", null, null },
                    { 3, "Sơn lót chống kiềm cần được sử dụng trên các bề mặt mới xây (vữa, bê tông) hoặc các bề mặt cũ có dấu hiệu bị kiềm hóa (ố vàng, phấn trắng) để ngăn chặn kiềm từ xi măng ăn mòn lớp sơn phủ màu.", 5, new DateTime(2025, 5, 17, 11, 25, 27, 161, DateTimeKind.Utc).AddTicks(7696), null, true, 2, "Khi nào cần sử dụng sơn lót chống kiềm?", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleProducts_ProductId",
                table: "ArticleProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_PublishedAt",
                table: "Articles",
                column: "PublishedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Slug",
                table: "Articles",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTags_TagId",
                table: "ArticleTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_Slug",
                table: "Attributes",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttributeValues_AttributeId_Slug",
                table: "AttributeValues",
                columns: new[] { "AttributeId", "Slug" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Slug",
                table: "Brands",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Email",
                table: "Contacts",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_FAQs_category_id",
                table: "FAQs",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_FAQs_OrderIndex",
                table: "FAQs",
                column: "OrderIndex");

            migrationBuilder.CreateIndex(
                name: "IX_Newsletters_Email",
                table: "Newsletters",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_Slug",
                table: "Pages",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributes_AttributeId",
                table: "ProductAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_OrderIndex",
                table: "ProductImages",
                column: "OrderIndex");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ProductId",
                table: "ProductReviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_Rating",
                table: "ProductReviews",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_UserId",
                table: "ProductReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Slug",
                table: "Products",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_TagId",
                table: "ProductTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariationAttributeValues_AttributeValueId",
                table: "ProductVariationAttributeValues",
                column: "AttributeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariations_IsDefault",
                table: "ProductVariations",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariations_ProductId",
                table: "ProductVariations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_Key_Category",
                table: "Settings",
                columns: new[] { "Key", "Category" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Slide_IsActive",
                table: "Slide",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Slide_OrderIndex",
                table: "Slide",
                column: "OrderIndex");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Slug",
                table: "Tags",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_OrderIndex",
                table: "Testimonials",
                column: "OrderIndex");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_Rating",
                table: "Testimonials",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleProducts");

            migrationBuilder.DropTable(
                name: "ArticleTags");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "FAQs");

            migrationBuilder.DropTable(
                name: "MediaFiles");

            migrationBuilder.DropTable(
                name: "Newsletters");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "PopupModals");

            migrationBuilder.DropTable(
                name: "ProductAttributes");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductReviews");

            migrationBuilder.DropTable(
                name: "ProductTags");

            migrationBuilder.DropTable(
                name: "ProductVariationAttributeValues");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Slide");

            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "AttributeValues");

            migrationBuilder.DropTable(
                name: "ProductVariations");

            migrationBuilder.DropTable(
                name: "Attributes");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}

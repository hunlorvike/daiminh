using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v1_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "attributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    logo_url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    website = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    order_index = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    type = table.Column<int>(type: "int", maxLength: 20, nullable: false, defaultValue: 0),
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
                    table.PrimaryKey("PK_categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_categories_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    subject = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    admin_notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ip_address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "media_files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    file_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    original_file_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    mime_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    file_extension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    file_path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    alt_text = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    duration = table.Column<int>(type: "int", nullable: true),
                    media_type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "newsletters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ip_address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    confirmed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    unsubscribed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_newsletters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    default_value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "testimonials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    client_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    client_title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    client_company = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    client_avatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    order_index = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_testimonials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attribute_values",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    attribute_id = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attribute_values", x => x.Id);
                    table.ForeignKey(
                        name: "FK_attribute_values_attributes_attribute_id",
                        column: x => x.attribute_id,
                        principalTable: "attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    summary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    featured_image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    thumbnail_image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    view_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    is_featured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    published_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    author_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    author_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    author_avatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    estimated_reading_minutes = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    status = table.Column<int>(type: "int", maxLength: 20, nullable: false, defaultValue: 0),
                    category_id = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_articles_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "faqs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    question = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    order_index = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_faqs_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    short_description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    manufacturer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    origin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    specifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    usage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    view_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    is_featured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    brand_id = table.Column<int>(type: "int", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_products_brands_brand_id",
                        column: x => x.brand_id,
                        principalTable: "brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_products_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "article_tags",
                columns: table => new
                {
                    article_id = table.Column<int>(type: "int", nullable: false),
                    tag_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article_tags", x => new { x.article_id, x.tag_id });
                    table.ForeignKey(
                        name: "FK_article_tags_articles_article_id",
                        column: x => x.article_id,
                        principalTable: "articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_article_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "article_products",
                columns: table => new
                {
                    article_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article_products", x => new { x.article_id, x.product_id });
                    table.ForeignKey(
                        name: "FK_article_products_articles_article_id",
                        column: x => x.article_id,
                        principalTable: "articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_article_products_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_attributes",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false),
                    attribute_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_attributes", x => new { x.product_id, x.attribute_id });
                    table.ForeignKey(
                        name: "FK_product_attributes_attributes_attribute_id",
                        column: x => x.attribute_id,
                        principalTable: "attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_attributes_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    thumbnail_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    alt_text = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    order_index = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    is_main = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_product_images_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    user_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    user_email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    rating = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_product_reviews_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_reviews_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_tags",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false),
                    tag_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_tags", x => new { x.product_id, x.tag_id });
                    table.ForeignKey(
                        name: "FK_product_tags_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_variations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sale_price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    stock_quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    image_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    is_default = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_variations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_product_variations_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_variation_attribute_values",
                columns: table => new
                {
                    product_variation_id = table.Column<int>(type: "int", nullable: false),
                    attribute_value_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_variation_attribute_values", x => new { x.product_variation_id, x.attribute_value_id });
                    table.ForeignKey(
                        name: "FK_product_variation_attribute_values_attribute_values_attribute_value_id",
                        column: x => x.attribute_value_id,
                        principalTable: "attribute_values",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_variation_attribute_values_product_variations_product_variation_id",
                        column: x => x.product_variation_id,
                        principalTable: "product_variations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
                values: new object[] { 1, "General", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3559), null, "Đại Minh Việt Nam", "Tên website hiển thị trên trang và tiêu đề trình duyệt.", true, "SiteName", null, null, "Đại Minh Việt Nam" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
                values: new object[,]
                {
                    { 2, "General", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3569), null, "https://localhost:7001", "Địa chỉ URL chính của website (ví dụ: https://www.example.com).", true, "SiteUrl", 7, null, null, "https://localhost:7001" },
                    { 3, "General", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3571), null, "sondaiminh@gmail.com", "Địa chỉ email quản trị viên để nhận thông báo hệ thống.", true, "AdminEmail", 6, null, null, "sondaiminh@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
                values: new object[] { 5, "Contact", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3574), null, "Đại Minh Việt Nam", "Tên công ty hoặc tổ chức sở hữu website.", true, "CompanyName", null, null, "Đại Minh Việt Nam" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
                values: new object[,]
                {
                    { 6, "Contact", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3575), null, "123 Main Street, Anytown, CA 91234", "Địa chỉ liên hệ đầy đủ.", true, "ContactAddress", 1, null, null, "123 Main Street, Anytown, CA 91234" },
                    { 7, "Contact", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3577), null, "(123) 456-7890", "Số điện thoại liên hệ chính.", true, "ContactPhone", 4, null, null, "(123) 456-7890" },
                    { 8, "Contact", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3578), null, "contact@example.com", "Địa chỉ email hiển thị công khai để liên hệ.", true, "ContactEmail", 6, null, null, "contact@example.com" },
                    { 9, "Contact", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3580), null, null, "Mã nhúng HTML của bản đồ (ví dụ: Google Maps iframe).", true, "ContactMapEmbed", 2, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
                values: new object[] { 10, "SEO", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3582), null, "Welcome to My Application", "Tiêu đề meta mặc định cho các trang không có tiêu đề riêng.", true, "DefaultMetaTitle", null, null, "Welcome to My Application" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
                values: new object[,]
                {
                    { 11, "SEO", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3583), null, "This is the default description for My Application.", "Mô tả meta mặc định (dưới 160 ký tự).", true, "DefaultMetaDescription", 1, null, null, "This is the default description for My Application." },
                    { 12, "SEO", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3585), null, "/image/icon.jpg", "Đường dẫn đến file favicon.ico hoặc ảnh favicon.", true, "FaviconUrl", 3, null, null, "/image/icon.jpg" },
                    { 13, "Social Media", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3586), null, null, "URL trang Facebook.", true, "SocialFacebookUrl", 7, null, null, null },
                    { 14, "Social Media", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3588), null, null, "URL trang Twitter (X).", true, "SocialTwitterUrl", 7, null, null, null },
                    { 15, "Social Media", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3589), null, null, "URL trang Instagram.", true, "SocialInstagramUrl", 7, null, null, null },
                    { 16, "Social Media", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3591), null, null, "URL trang LinkedIn.", true, "SocialLinkedInUrl", 7, null, null, null },
                    { 17, "Social Media", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3592), null, null, "URL kênh Youtube.", true, "SocialYoutubeUrl", 7, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
                values: new object[] { 18, "Email", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3594), null, "smtp.example.com", "Địa chỉ máy chủ SMTP.", true, "SmtpHost", null, null, "smtp.example.com" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
                values: new object[] { 19, "Email", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3596), null, "587", "Cổng SMTP (ví dụ: 587, 465, 25).", true, "SmtpPort", 8, null, null, "587" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
                values: new object[,]
                {
                    { 20, "Email", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3597), null, "user@example.com", "Tên đăng nhập SMTP.", true, "SmtpUsername", null, null, "user@example.com" },
                    { 21, "Email", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3599), null, null, "**QUAN TRỌNG**: Mật khẩu SMTP. Nên cấu hình qua UI, không seed giá trị thật.", true, "SmtpPassword", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
                values: new object[] { 22, "Email", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3601), null, "true", "Sử dụng mã hóa SSL/TLS khi gửi email.", true, "SmtpUseSsl", 9, null, null, "true" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
                values: new object[] { 23, "Email", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3602), null, "My Application Support", "Tên hiển thị trong ô 'From' của email gửi đi.", true, "EmailFromName", null, null, "My Application Support" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
                values: new object[] { 24, "Email", new DateTime(2025, 4, 25, 19, 3, 20, 844, DateTimeKind.Utc).AddTicks(3604), null, "noreply@example.com", "Địa chỉ email hiển thị trong ô 'From' của email gửi đi.", true, "EmailFromAddress", 6, null, null, "noreply@example.com" });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "created_at", "created_by", "email", "full_name", "is_active", "password_hash", "updated_at", "updated_by", "username" },
                values: new object[] { 1, new DateTime(2025, 4, 25, 19, 3, 20, 845, DateTimeKind.Utc).AddTicks(6583), null, "admin@admin.com", "Quản trị viên", true, "AQAAAAIAAYagAAAAEPZaAbH4xn31ZNOcHHtmF0GN+x19pVhTaPTuoEAbsIQW/30lIaCjaj3YCI6WwrhDag==", null, null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_article_products_product_id",
                table: "article_products",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_article_tags_tag_id",
                table: "article_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_articles_category_id",
                table: "articles",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_attribute_values_attribute_id",
                table: "attribute_values",
                column: "attribute_id");

            migrationBuilder.CreateIndex(
                name: "IX_categories_parent_id",
                table: "categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_faqs_category_id",
                table: "faqs",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_attributes_attribute_id",
                table: "product_attributes",
                column: "attribute_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_images_product_id",
                table: "product_images",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_reviews_product_id",
                table: "product_reviews",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_reviews_user_id",
                table: "product_reviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_tags_tag_id",
                table: "product_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_variation_attribute_values_attribute_value_id",
                table: "product_variation_attribute_values",
                column: "attribute_value_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_variations_product_id",
                table: "product_variations",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_brand_id",
                table: "products",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_category_id",
                table: "products",
                column: "category_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "article_products");

            migrationBuilder.DropTable(
                name: "article_tags");

            migrationBuilder.DropTable(
                name: "contacts");

            migrationBuilder.DropTable(
                name: "faqs");

            migrationBuilder.DropTable(
                name: "media_files");

            migrationBuilder.DropTable(
                name: "newsletters");

            migrationBuilder.DropTable(
                name: "product_attributes");

            migrationBuilder.DropTable(
                name: "product_images");

            migrationBuilder.DropTable(
                name: "product_reviews");

            migrationBuilder.DropTable(
                name: "product_tags");

            migrationBuilder.DropTable(
                name: "product_variation_attribute_values");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "testimonials");

            migrationBuilder.DropTable(
                name: "articles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "attribute_values");

            migrationBuilder.DropTable(
                name: "product_variations");

            migrationBuilder.DropTable(
                name: "attributes");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}

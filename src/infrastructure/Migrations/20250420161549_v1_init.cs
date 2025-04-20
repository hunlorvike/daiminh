using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                name: "brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    logo_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    meta_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    meta_description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    meta_keywords = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    canonical_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    no_index = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    no_follow = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    og_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    og_description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    og_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    og_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValue: "website"),
                    twitter_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    twitter_description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    twitter_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    twitter_card = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValue: "summary_large_image"),
                    schema_markup = table.Column<string>(type: "text", nullable: true),
                    breadcrumb_json = table.Column<string>(type: "text", nullable: true),
                    sitemap_priority = table.Column<double>(type: "double precision", nullable: true, defaultValue: 0.5),
                    sitemap_change_frequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "monthly")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    icon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    order_index = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    type = table.Column<int>(type: "integer", maxLength: 20, nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    meta_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    meta_description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    meta_keywords = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    canonical_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    no_index = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    no_follow = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    og_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    og_description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    og_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    og_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValue: "website"),
                    twitter_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    twitter_description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    twitter_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    twitter_card = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValue: "summary_large_image"),
                    schema_markup = table.Column<string>(type: "text", nullable: true),
                    breadcrumb_json = table.Column<string>(type: "text", nullable: true),
                    sitemap_priority = table.Column<double>(type: "double precision", nullable: true, defaultValue: 0.5),
                    sitemap_change_frequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "monthly")
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    subject = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    company_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    project_details = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", maxLength: 20, nullable: false, defaultValue: 0),
                    admin_notes = table.Column<string>(type: "text", nullable: true),
                    ip_address = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    user_agent = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "media_folders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_media_folders_media_folders_parent_id",
                        column: x => x.parent_id,
                        principalTable: "media_folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "newsletters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ip_address = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    user_agent = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    confirmed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    unsubscribed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_newsletters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    type = table.Column<int>(type: "integer", maxLength: 20, nullable: false, defaultValue: 0),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    default_value = table.Column<string>(type: "text", nullable: true),
                    value = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    slug = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    type = table.Column<int>(type: "integer", maxLength: 20, nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "testimonials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    client_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    client_company = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    client_avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    content = table.Column<string>(type: "text", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false, defaultValue: 5),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    order_index = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_testimonials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    featured_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    thumbnail_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    view_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_featured = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    author_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    author_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    author_avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    estimated_reading_minutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    status = table.Column<int>(type: "integer", maxLength: 20, nullable: false, defaultValue: 0),
                    category_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    meta_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    meta_description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    meta_keywords = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    canonical_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    no_index = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    no_follow = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    og_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    og_description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    og_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    og_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValue: "website"),
                    twitter_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    twitter_description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    twitter_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    twitter_card = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValue: "summary_large_image"),
                    schema_markup = table.Column<string>(type: "text", nullable: true),
                    breadcrumb_json = table.Column<string>(type: "text", nullable: true),
                    sitemap_priority = table.Column<double>(type: "double precision", nullable: true, defaultValue: 0.5),
                    sitemap_change_frequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "monthly")
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    answer = table.Column<string>(type: "text", nullable: false),
                    order_index = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    category_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    short_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    manufacturer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    origin = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    specifications = table.Column<string>(type: "text", nullable: true),
                    usage = table.Column<string>(type: "text", nullable: true),
                    features = table.Column<string>(type: "text", nullable: true),
                    packaging_info = table.Column<string>(type: "text", nullable: true),
                    storage_instructions = table.Column<string>(type: "text", nullable: true),
                    safety_info = table.Column<string>(type: "text", nullable: true),
                    application_areas = table.Column<string>(type: "text", nullable: true),
                    technical_documents = table.Column<string>(type: "json", nullable: true),
                    view_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_featured = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    status = table.Column<int>(type: "integer", maxLength: 20, nullable: false, defaultValue: 0),
                    brand_id = table.Column<int>(type: "integer", nullable: true),
                    category_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    meta_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    meta_description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    meta_keywords = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    canonical_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    no_index = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    no_follow = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    og_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    og_description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    og_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    og_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValue: "website"),
                    twitter_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    twitter_description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    twitter_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    twitter_card = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValue: "summary_large_image"),
                    schema_markup = table.Column<string>(type: "text", nullable: true),
                    breadcrumb_json = table.Column<string>(type: "text", nullable: true),
                    sitemap_priority = table.Column<double>(type: "double precision", nullable: true, defaultValue: 0.5),
                    sitemap_change_frequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "monthly")
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
                name: "media_files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    original_file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    mime_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    file_extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    file_path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    alt_text = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    width = table.Column<int>(type: "integer", nullable: true),
                    height = table.Column<int>(type: "integer", nullable: true),
                    duration = table.Column<int>(type: "integer", nullable: true),
                    media_type = table.Column<int>(type: "integer", maxLength: 20, nullable: false, defaultValue: 0),
                    folder_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_media_files_media_folders_folder_id",
                        column: x => x.folder_id,
                        principalTable: "media_folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "article_tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    article_id = table.Column<int>(type: "integer", nullable: false),
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article_tags", x => x.Id);
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    article_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    order_index = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article_products", x => x.Id);
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
                name: "product_images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    image_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    thumbnail_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    alt_text = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    order_index = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_main = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
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
                name: "product_tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_tags", x => x.Id);
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
                name: "product_variants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    stock_quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    size = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    packaging = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    image_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_variants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_product_variants_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "CreatedAt", "CreatedBy", "default_value", "description", "is_active", "key", "UpdatedAt", "UpdatedBy", "value" },
                values: new object[] { 1, "General", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(220), null, "Đại Minh Việt Nam", "Tên website hiển thị trên trang và tiêu đề trình duyệt.", true, "SiteName", null, null, "Đại Minh Việt Nam" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "CreatedAt", "CreatedBy", "default_value", "description", "is_active", "key", "type", "UpdatedAt", "UpdatedBy", "value" },
                values: new object[,]
                {
                    { 2, "General", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(229), null, "https://localhost:7001", "Địa chỉ URL chính của website (ví dụ: https://www.example.com).", true, "SiteUrl", 7, null, null, "https://localhost:7001" },
                    { 3, "General", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(233), null, "sondaiminh@gmail.com", "Địa chỉ email quản trị viên để nhận thông báo hệ thống.", true, "AdminEmail", 6, null, null, "sondaiminh@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "CreatedAt", "CreatedBy", "default_value", "description", "is_active", "key", "UpdatedAt", "UpdatedBy", "value" },
                values: new object[] { 5, "Contact", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(235), null, "Đại Minh Việt Nam", "Tên công ty hoặc tổ chức sở hữu website.", true, "CompanyName", null, null, "Đại Minh Việt Nam" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "CreatedAt", "CreatedBy", "default_value", "description", "is_active", "key", "type", "UpdatedAt", "UpdatedBy", "value" },
                values: new object[,]
                {
                    { 6, "Contact", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(238), null, "123 Main Street, Anytown, CA 91234", "Địa chỉ liên hệ đầy đủ.", true, "ContactAddress", 1, null, null, "123 Main Street, Anytown, CA 91234" },
                    { 7, "Contact", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(239), null, "(123) 456-7890", "Số điện thoại liên hệ chính.", true, "ContactPhone", 4, null, null, "(123) 456-7890" },
                    { 8, "Contact", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(241), null, "contact@example.com", "Địa chỉ email hiển thị công khai để liên hệ.", true, "ContactEmail", 6, null, null, "contact@example.com" },
                    { 9, "Contact", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(242), null, null, "Mã nhúng HTML của bản đồ (ví dụ: Google Maps iframe).", true, "ContactMapEmbed", 2, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "CreatedAt", "CreatedBy", "default_value", "description", "is_active", "key", "UpdatedAt", "UpdatedBy", "value" },
                values: new object[] { 10, "SEO", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(244), null, "Welcome to My Application", "Tiêu đề meta mặc định cho các trang không có tiêu đề riêng.", true, "DefaultMetaTitle", null, null, "Welcome to My Application" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "CreatedAt", "CreatedBy", "default_value", "description", "is_active", "key", "type", "UpdatedAt", "UpdatedBy", "value" },
                values: new object[,]
                {
                    { 11, "SEO", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(246), null, "This is the default description for My Application.", "Mô tả meta mặc định (dưới 160 ký tự).", true, "DefaultMetaDescription", 1, null, null, "This is the default description for My Application." },
                    { 12, "SEO", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(247), null, "/image/icon.jpg", "Đường dẫn đến file favicon.ico hoặc ảnh favicon.", true, "FaviconUrl", 3, null, null, "/image/icon.jpg" },
                    { 13, "Social Media", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(249), null, null, "URL trang Facebook.", true, "SocialFacebookUrl", 7, null, null, null },
                    { 14, "Social Media", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(250), null, null, "URL trang Twitter (X).", true, "SocialTwitterUrl", 7, null, null, null },
                    { 15, "Social Media", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(252), null, null, "URL trang Instagram.", true, "SocialInstagramUrl", 7, null, null, null },
                    { 16, "Social Media", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(254), null, null, "URL trang LinkedIn.", true, "SocialLinkedInUrl", 7, null, null, null },
                    { 17, "Social Media", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(255), null, null, "URL kênh Youtube.", true, "SocialYoutubeUrl", 7, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "CreatedAt", "CreatedBy", "default_value", "description", "is_active", "key", "UpdatedAt", "UpdatedBy", "value" },
                values: new object[] { 18, "Email", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(257), null, "smtp.example.com", "Địa chỉ máy chủ SMTP.", true, "SmtpHost", null, null, "smtp.example.com" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "CreatedAt", "CreatedBy", "default_value", "description", "is_active", "key", "type", "UpdatedAt", "UpdatedBy", "value" },
                values: new object[] { 19, "Email", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(258), null, "587", "Cổng SMTP (ví dụ: 587, 465, 25).", true, "SmtpPort", 8, null, null, "587" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "CreatedAt", "CreatedBy", "default_value", "description", "is_active", "key", "UpdatedAt", "UpdatedBy", "value" },
                values: new object[,]
                {
                    { 20, "Email", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(260), null, "user@example.com", "Tên đăng nhập SMTP.", true, "SmtpUsername", null, null, "user@example.com" },
                    { 21, "Email", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(261), null, null, "**QUAN TRỌNG**: Mật khẩu SMTP. Nên cấu hình qua UI, không seed giá trị thật.", true, "SmtpPassword", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "CreatedAt", "CreatedBy", "default_value", "description", "is_active", "key", "type", "UpdatedAt", "UpdatedBy", "value" },
                values: new object[] { 22, "Email", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(263), null, "true", "Sử dụng mã hóa SSL/TLS khi gửi email.", true, "SmtpUseSsl", 9, null, null, "true" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "CreatedAt", "CreatedBy", "default_value", "description", "is_active", "key", "UpdatedAt", "UpdatedBy", "value" },
                values: new object[] { 23, "Email", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(265), null, "My Application Support", "Tên hiển thị trong ô 'From' của email gửi đi.", true, "EmailFromName", null, null, "My Application Support" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "CreatedAt", "CreatedBy", "default_value", "description", "is_active", "key", "type", "UpdatedAt", "UpdatedBy", "value" },
                values: new object[] { 24, "Email", new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(267), null, "noreply@example.com", "Địa chỉ email hiển thị trong ô 'From' của email gửi đi.", true, "EmailFromAddress", 6, null, null, "noreply@example.com" });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "created_at", "created_by", "email", "full_name", "is_active", "password_hash", "updated_at", "updated_by", "username" },
                values: new object[] { 1, new DateTime(2025, 4, 20, 16, 15, 46, 808, DateTimeKind.Utc).AddTicks(3734), null, "admin@admin.com", "Quản trị viên", true, "AQAAAAIAAYagAAAAEEU0ahmFyarn4e1nycB+Uj7e5jaL905jDRlqF825e+PkF3CmT9VlYXbJTwc3tPNr6A==", null, null, "admin" });

            migrationBuilder.CreateIndex(
                name: "idx_article_products_article_id",
                table: "article_products",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "idx_article_products_article_product",
                table: "article_products",
                columns: new[] { "article_id", "product_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_article_products_order_index",
                table: "article_products",
                column: "order_index");

            migrationBuilder.CreateIndex(
                name: "idx_article_products_product_id",
                table: "article_products",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "idx_article_tags_article_id",
                table: "article_tags",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "idx_article_tags_article_tag",
                table: "article_tags",
                columns: new[] { "article_id", "tag_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_article_tags_tag_id",
                table: "article_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "idx_articles_author_id",
                table: "articles",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "idx_articles_category_id",
                table: "articles",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "idx_articles_is_featured",
                table: "articles",
                column: "is_featured");

            migrationBuilder.CreateIndex(
                name: "idx_articles_published_at",
                table: "articles",
                column: "published_at");

            migrationBuilder.CreateIndex(
                name: "idx_articles_slug",
                table: "articles",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_articles_status",
                table: "articles",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_articles_view_count",
                table: "articles",
                column: "view_count");

            migrationBuilder.CreateIndex(
                name: "idx_brands_is_active",
                table: "brands",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_brands_slug",
                table: "brands",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_categories_is_active",
                table: "categories",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_categories_order_index",
                table: "categories",
                column: "order_index");

            migrationBuilder.CreateIndex(
                name: "idx_categories_parent_id",
                table: "categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "idx_categories_slug_type",
                table: "categories",
                columns: new[] { "slug", "type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_categories_type",
                table: "categories",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "idx_contacts_created_at",
                table: "contacts",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "idx_contacts_email",
                table: "contacts",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "idx_contacts_status",
                table: "contacts",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_faqs_is_active",
                table: "faqs",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_faqs_order_index",
                table: "faqs",
                column: "order_index");

            migrationBuilder.CreateIndex(
                name: "IX_faqs_category_id",
                table: "faqs",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_media_files_folder_id",
                table: "media_files",
                column: "folder_id");

            migrationBuilder.CreateIndex(
                name: "IX_media_folders_parent_id",
                table: "media_folders",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "idx_newsletters_email",
                table: "newsletters",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_newsletters_is_active",
                table: "newsletters",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_product_images_order_index",
                table: "product_images",
                column: "order_index");

            migrationBuilder.CreateIndex(
                name: "idx_product_images_product_id",
                table: "product_images",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "idx_product_images_product_main",
                table: "product_images",
                columns: new[] { "product_id", "is_main" });

            migrationBuilder.CreateIndex(
                name: "idx_product_tags_product_id",
                table: "product_tags",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "idx_product_tags_product_tag",
                table: "product_tags",
                columns: new[] { "product_id", "tag_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_product_tags_tag_id",
                table: "product_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "idx_product_variants_is_active",
                table: "product_variants",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_product_variants_product_id",
                table: "product_variants",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "idx_product_variants_sku",
                table: "product_variants",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_products_brand_id",
                table: "products",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "idx_products_is_featured",
                table: "products",
                column: "is_featured");

            migrationBuilder.CreateIndex(
                name: "idx_products_slug",
                table: "products",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_products_status",
                table: "products",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_products_view_count",
                table: "products",
                column: "view_count");

            migrationBuilder.CreateIndex(
                name: "IX_products_category_id",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "idx_settings_category",
                table: "settings",
                column: "category");

            migrationBuilder.CreateIndex(
                name: "idx_settings_is_active",
                table: "settings",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_settings_key",
                table: "settings",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_settings_type",
                table: "settings",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "idx_tags_slug_type",
                table: "tags",
                columns: new[] { "slug", "type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_tags_type",
                table: "tags",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "idx_testimonials_is_active",
                table: "testimonials",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_testimonials_order_index",
                table: "testimonials",
                column: "order_index");

            migrationBuilder.CreateIndex(
                name: "idx_testimonials_rating",
                table: "testimonials",
                column: "rating");

            migrationBuilder.CreateIndex(
                name: "idx_users_email",
                table: "users",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "idx_users_username",
                table: "users",
                column: "username");
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
                name: "product_images");

            migrationBuilder.DropTable(
                name: "product_tags");

            migrationBuilder.DropTable(
                name: "product_variants");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "testimonials");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "articles");

            migrationBuilder.DropTable(
                name: "media_folders");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}

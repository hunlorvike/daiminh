using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace infrastructure.Migrations;

/// <inheritdoc />
public partial class DaiMinhVersion3 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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
                type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "knowledge"),
                status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "draft"),
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
                sitemap_priority = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.5),
                sitemap_change_frequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "monthly")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_articles", x => x.Id);
            });

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
                sitemap_priority = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.5),
                sitemap_change_frequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "monthly")
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
                image_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                parent_id = table.Column<int>(type: "integer", nullable: true),
                order_index = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "product"),
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
                sitemap_priority = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.5),
                sitemap_change_frequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "monthly")
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
                status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "new"),
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
            name: "faq_categories",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                order_index = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_faq_categories", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "galleries",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                description = table.Column<string>(type: "text", nullable: true),
                cover_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                view_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                is_featured = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "draft"),
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
                sitemap_priority = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.5),
                sitemap_change_frequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "monthly")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_galleries", x => x.Id);
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
            name: "product_types",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                icon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_product_types", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "projects",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                description = table.Column<string>(type: "text", nullable: false),
                short_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                client = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                area = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                completion_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                featured_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                thumbnail_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                view_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                is_featured = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "inprogress"),
                publish_status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "draft"),
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
                sitemap_priority = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.5),
                sitemap_change_frequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "monthly")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_projects", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "settings",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "product"),
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
                project_reference = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
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
            name: "comments",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                content = table.Column<string>(type: "text", nullable: false),
                author_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                author_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                author_email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                author_avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                author_website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                is_approved = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                parent_id = table.Column<int>(type: "integer", nullable: true),
                article_id = table.Column<int>(type: "integer", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_comments", x => x.Id);
                table.ForeignKey(
                    name: "FK_comments_articles_article_id",
                    column: x => x.article_id,
                    principalTable: "articles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_comments_comments_parent_id",
                    column: x => x.parent_id,
                    principalTable: "comments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "article_categories",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                article_id = table.Column<int>(type: "integer", nullable: false),
                category_id = table.Column<int>(type: "integer", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_article_categories", x => x.Id);
                table.ForeignKey(
                    name: "FK_article_categories_articles_article_id",
                    column: x => x.article_id,
                    principalTable: "articles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_article_categories_categories_category_id",
                    column: x => x.category_id,
                    principalTable: "categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "faqs",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                question = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                answer = table.Column<string>(type: "text", nullable: false),
                category_id = table.Column<int>(type: "integer", nullable: false),
                order_index = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_faqs", x => x.Id);
                table.ForeignKey(
                    name: "FK_faqs_faq_categories_category_id",
                    column: x => x.category_id,
                    principalTable: "faq_categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "gallery_categories",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                gallery_id = table.Column<int>(type: "integer", nullable: false),
                category_id = table.Column<int>(type: "integer", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_gallery_categories", x => x.Id);
                table.ForeignKey(
                    name: "FK_gallery_categories_categories_category_id",
                    column: x => x.category_id,
                    principalTable: "categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_gallery_categories_galleries_gallery_id",
                    column: x => x.gallery_id,
                    principalTable: "galleries",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "gallery_images",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                gallery_id = table.Column<int>(type: "integer", nullable: false),
                image_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                thumbnail_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                alt_text = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                description = table.Column<string>(type: "text", nullable: true),
                order_index = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_gallery_images", x => x.Id);
                table.ForeignKey(
                    name: "FK_gallery_images_galleries_gallery_id",
                    column: x => x.gallery_id,
                    principalTable: "galleries",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
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
                thumbnail_path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                medium_size_path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                large_size_path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                file_size = table.Column<long>(type: "bigint", nullable: false),
                alt_text = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                width = table.Column<int>(type: "integer", nullable: true),
                height = table.Column<int>(type: "integer", nullable: true),
                duration = table.Column<int>(type: "integer", nullable: true),
                media_type = table.Column<int>(type: "integer", nullable: false),
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
                product_type_id = table.Column<int>(type: "integer", nullable: false),
                status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "draft"),
                brand_id = table.Column<int>(type: "integer", nullable: true),
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
                sitemap_priority = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.5),
                sitemap_change_frequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "monthly")
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
                    name: "FK_products_product_types_product_type_id",
                    column: x => x.product_type_id,
                    principalTable: "product_types",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "project_categories",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                project_id = table.Column<int>(type: "integer", nullable: false),
                category_id = table.Column<int>(type: "integer", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_project_categories", x => x.Id);
                table.ForeignKey(
                    name: "FK_project_categories_categories_category_id",
                    column: x => x.category_id,
                    principalTable: "categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_project_categories_projects_project_id",
                    column: x => x.project_id,
                    principalTable: "projects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "project_images",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                project_id = table.Column<int>(type: "integer", nullable: false),
                image_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                thumbnail_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                alt_text = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                description = table.Column<string>(type: "text", nullable: true),
                order_index = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                is_main = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_project_images", x => x.Id);
                table.ForeignKey(
                    name: "FK_project_images_projects_project_id",
                    column: x => x.project_id,
                    principalTable: "projects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
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
            name: "gallery_tags",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                gallery_id = table.Column<int>(type: "integer", nullable: false),
                tag_id = table.Column<int>(type: "integer", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_gallery_tags", x => x.Id);
                table.ForeignKey(
                    name: "FK_gallery_tags_galleries_gallery_id",
                    column: x => x.gallery_id,
                    principalTable: "galleries",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_gallery_tags_tags_tag_id",
                    column: x => x.tag_id,
                    principalTable: "tags",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "project_tags",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                project_id = table.Column<int>(type: "integer", nullable: false),
                tag_id = table.Column<int>(type: "integer", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_project_tags", x => x.Id);
                table.ForeignKey(
                    name: "FK_project_tags_projects_project_id",
                    column: x => x.project_id,
                    principalTable: "projects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_project_tags_tags_tag_id",
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
            name: "product_categories",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                product_id = table.Column<int>(type: "integer", nullable: false),
                category_id = table.Column<int>(type: "integer", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_product_categories", x => x.Id);
                table.ForeignKey(
                    name: "FK_product_categories_categories_category_id",
                    column: x => x.category_id,
                    principalTable: "categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_product_categories_products_product_id",
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

        migrationBuilder.CreateTable(
            name: "project_products",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                project_id = table.Column<int>(type: "integer", nullable: false),
                product_id = table.Column<int>(type: "integer", nullable: false),
                usage = table.Column<string>(type: "text", nullable: true),
                order_index = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_project_products", x => x.Id);
                table.ForeignKey(
                    name: "FK_project_products_products_product_id",
                    column: x => x.product_id,
                    principalTable: "products",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_project_products_projects_project_id",
                    column: x => x.project_id,
                    principalTable: "projects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "idx_article_categories_article_category",
            table: "article_categories",
            columns: new[] { "article_id", "category_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_article_categories_article_id",
            table: "article_categories",
            column: "article_id");

        migrationBuilder.CreateIndex(
            name: "idx_article_categories_category_id",
            table: "article_categories",
            column: "category_id");

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
            name: "idx_articles_type",
            table: "articles",
            column: "type");

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
            name: "idx_comments_article_id",
            table: "comments",
            column: "article_id");

        migrationBuilder.CreateIndex(
            name: "idx_comments_author_id",
            table: "comments",
            column: "author_id");

        migrationBuilder.CreateIndex(
            name: "idx_comments_is_approved",
            table: "comments",
            column: "is_approved");

        migrationBuilder.CreateIndex(
            name: "idx_comments_parent_id",
            table: "comments",
            column: "parent_id");

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
            name: "idx_faq_categories_is_active",
            table: "faq_categories",
            column: "is_active");

        migrationBuilder.CreateIndex(
            name: "idx_faq_categories_order_index",
            table: "faq_categories",
            column: "order_index");

        migrationBuilder.CreateIndex(
            name: "idx_faq_categories_slug",
            table: "faq_categories",
            column: "slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_faqs_category_id",
            table: "faqs",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "idx_faqs_is_active",
            table: "faqs",
            column: "is_active");

        migrationBuilder.CreateIndex(
            name: "idx_faqs_order_index",
            table: "faqs",
            column: "order_index");

        migrationBuilder.CreateIndex(
            name: "idx_galleries_is_featured",
            table: "galleries",
            column: "is_featured");

        migrationBuilder.CreateIndex(
            name: "idx_galleries_slug",
            table: "galleries",
            column: "slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_galleries_status",
            table: "galleries",
            column: "status");

        migrationBuilder.CreateIndex(
            name: "idx_galleries_view_count",
            table: "galleries",
            column: "view_count");

        migrationBuilder.CreateIndex(
            name: "idx_gallery_categories_category_id",
            table: "gallery_categories",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "idx_gallery_categories_gallery_category",
            table: "gallery_categories",
            columns: new[] { "gallery_id", "category_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_gallery_categories_gallery_id",
            table: "gallery_categories",
            column: "gallery_id");

        migrationBuilder.CreateIndex(
            name: "idx_gallery_images_gallery_id",
            table: "gallery_images",
            column: "gallery_id");

        migrationBuilder.CreateIndex(
            name: "idx_gallery_images_order_index",
            table: "gallery_images",
            column: "order_index");

        migrationBuilder.CreateIndex(
            name: "idx_gallery_tags_gallery_id",
            table: "gallery_tags",
            column: "gallery_id");

        migrationBuilder.CreateIndex(
            name: "idx_gallery_tags_gallery_tag",
            table: "gallery_tags",
            columns: new[] { "gallery_id", "tag_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_gallery_tags_tag_id",
            table: "gallery_tags",
            column: "tag_id");

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
            name: "idx_product_categories_category_id",
            table: "product_categories",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "idx_product_categories_product_category",
            table: "product_categories",
            columns: new[] { "product_id", "category_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_product_categories_product_id",
            table: "product_categories",
            column: "product_id");

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
            name: "idx_product_types_is_active",
            table: "product_types",
            column: "is_active");

        migrationBuilder.CreateIndex(
            name: "idx_product_types_slug",
            table: "product_types",
            column: "slug",
            unique: true);

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
            name: "idx_products_product_type_id",
            table: "products",
            column: "product_type_id");

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
            name: "idx_project_categories_category_id",
            table: "project_categories",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "idx_project_categories_project_category",
            table: "project_categories",
            columns: new[] { "project_id", "category_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_project_categories_project_id",
            table: "project_categories",
            column: "project_id");

        migrationBuilder.CreateIndex(
            name: "idx_project_images_order_index",
            table: "project_images",
            column: "order_index");

        migrationBuilder.CreateIndex(
            name: "idx_project_images_project_id",
            table: "project_images",
            column: "project_id");

        migrationBuilder.CreateIndex(
            name: "idx_project_images_project_main",
            table: "project_images",
            columns: new[] { "project_id", "is_main" });

        migrationBuilder.CreateIndex(
            name: "idx_project_products_order_index",
            table: "project_products",
            column: "order_index");

        migrationBuilder.CreateIndex(
            name: "idx_project_products_product_id",
            table: "project_products",
            column: "product_id");

        migrationBuilder.CreateIndex(
            name: "idx_project_products_project_id",
            table: "project_products",
            column: "project_id");

        migrationBuilder.CreateIndex(
            name: "idx_project_products_project_product",
            table: "project_products",
            columns: new[] { "project_id", "product_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_project_tags_project_id",
            table: "project_tags",
            column: "project_id");

        migrationBuilder.CreateIndex(
            name: "idx_project_tags_project_tag",
            table: "project_tags",
            columns: new[] { "project_id", "tag_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_project_tags_tag_id",
            table: "project_tags",
            column: "tag_id");

        migrationBuilder.CreateIndex(
            name: "idx_projects_completion_date",
            table: "projects",
            column: "completion_date");

        migrationBuilder.CreateIndex(
            name: "idx_projects_is_featured",
            table: "projects",
            column: "is_featured");

        migrationBuilder.CreateIndex(
            name: "idx_projects_publish_status",
            table: "projects",
            column: "publish_status");

        migrationBuilder.CreateIndex(
            name: "idx_projects_slug",
            table: "projects",
            column: "slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_projects_start_date",
            table: "projects",
            column: "start_date");

        migrationBuilder.CreateIndex(
            name: "idx_projects_status",
            table: "projects",
            column: "status");

        migrationBuilder.CreateIndex(
            name: "idx_projects_view_count",
            table: "projects",
            column: "view_count");

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
            name: "article_categories");

        migrationBuilder.DropTable(
            name: "article_products");

        migrationBuilder.DropTable(
            name: "article_tags");

        migrationBuilder.DropTable(
            name: "comments");

        migrationBuilder.DropTable(
            name: "contacts");

        migrationBuilder.DropTable(
            name: "faqs");

        migrationBuilder.DropTable(
            name: "gallery_categories");

        migrationBuilder.DropTable(
            name: "gallery_images");

        migrationBuilder.DropTable(
            name: "gallery_tags");

        migrationBuilder.DropTable(
            name: "media_files");

        migrationBuilder.DropTable(
            name: "newsletters");

        migrationBuilder.DropTable(
            name: "product_categories");

        migrationBuilder.DropTable(
            name: "product_images");

        migrationBuilder.DropTable(
            name: "product_tags");

        migrationBuilder.DropTable(
            name: "product_variants");

        migrationBuilder.DropTable(
            name: "project_categories");

        migrationBuilder.DropTable(
            name: "project_images");

        migrationBuilder.DropTable(
            name: "project_products");

        migrationBuilder.DropTable(
            name: "project_tags");

        migrationBuilder.DropTable(
            name: "settings");

        migrationBuilder.DropTable(
            name: "testimonials");

        migrationBuilder.DropTable(
            name: "users");

        migrationBuilder.DropTable(
            name: "articles");

        migrationBuilder.DropTable(
            name: "faq_categories");

        migrationBuilder.DropTable(
            name: "galleries");

        migrationBuilder.DropTable(
            name: "media_folders");

        migrationBuilder.DropTable(
            name: "categories");

        migrationBuilder.DropTable(
            name: "products");

        migrationBuilder.DropTable(
            name: "projects");

        migrationBuilder.DropTable(
            name: "tags");

        migrationBuilder.DropTable(
            name: "brands");

        migrationBuilder.DropTable(
            name: "product_types");
    }
}

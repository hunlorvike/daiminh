using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations;

/// <inheritdoc />
public partial class version1 : Migration
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
            name: "banners",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                image_url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                link_url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                type = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                order_index = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_banners", x => x.Id);
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
                description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                parent_id = table.Column<int>(type: "int", nullable: true),
                order_index = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
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
                file_path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                file_size = table.Column<long>(type: "bigint", nullable: false),
                alt_text = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
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
            name: "pages",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                published_at = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                table.PrimaryKey("PK_pages", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "popup_modals",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                image_url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                link_url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                start_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                end_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_popup_modals", x => x.Id);
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
            name: "slides",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                subtitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                image_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                cta_text = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                cta_link = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                target = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "_self"),
                order_index = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                start_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                end_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_slides", x => x.Id);
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
                status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
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
            table: "attributes",
            columns: new[] { "Id", "created_at", "created_by", "name", "slug", "updated_at", "updated_by" },
            values: new object[,]
            {
                { 1, new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(7065), null, "Màu sắc", "mau-sac", null, null },
                { 2, new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(7066), null, "Dung tích", "dung-tich", null, null },
                { 3, new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(7068), null, "Độ bóng", "do-bong", null, null },
                { 4, new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(7069), null, "Bề mặt áp dụng", "be-mat-ap-dung", null, null }
            });

        migrationBuilder.InsertData(
            table: "brands",
            columns: new[] { "Id", "created_at", "created_by", "description", "is_active", "logo_url", "name", "slug", "updated_at", "updated_by", "website" },
            values: new object[,]
            {
                { 1, new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(4839), null, null, true, null, "Dulux", "dulux", null, null, null },
                { 2, new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(4842), null, null, true, null, "Jotun", "jotun", null, null, null },
                { 3, new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(4843), null, null, true, null, "Kova", "kova", null, null, null },
                { 4, new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(4844), null, null, true, null, "Sika", "sika", null, null, null },
                { 5, new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(4845), null, null, true, null, "Nippon Paint", "nippon-paint", null, null, null },
                { 6, new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(4847), null, null, true, null, "My Kolor", "my-kolor", null, null, null }
            });

        migrationBuilder.InsertData(
            table: "categories",
            columns: new[] { "Id", "created_at", "created_by", "description", "icon", "is_active", "name", "parent_id", "slug", "updated_at", "updated_by" },
            values: new object[] { 1, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3407), null, null, null, true, "Sơn", null, "son", null, null });

        migrationBuilder.InsertData(
            table: "categories",
            columns: new[] { "Id", "created_at", "created_by", "description", "icon", "is_active", "name", "order_index", "parent_id", "slug", "updated_at", "updated_by" },
            values: new object[,]
            {
                { 2, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3464), null, null, null, true, "Chống Thấm", 1, null, "chong-tham", null, null },
                { 3, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3465), null, null, null, true, "Vật Liệu Xây Dựng", 2, null, "vat-lieu-xay-dung", null, null }
            });

        migrationBuilder.InsertData(
            table: "categories",
            columns: new[] { "Id", "created_at", "created_by", "description", "icon", "is_active", "name", "order_index", "parent_id", "slug", "type", "updated_at", "updated_by" },
            values: new object[,]
            {
                { 4, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3467), null, null, null, true, "Tin Tức & Sự Kiện", 3, null, "tin-tuc-su-kien", 1, null, null },
                { 5, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3468), null, null, null, true, "Câu Hỏi Thường Gặp", 4, null, "cau-hoi-thuong-gap", 2, null, null }
            });

        migrationBuilder.InsertData(
            table: "pages",
            columns: new[] { "Id", "BreadcrumbJson", "CanonicalUrl", "content", "created_at", "created_by", "MetaDescription", "MetaKeywords", "MetaTitle", "NoFollow", "NoIndex", "OgDescription", "OgImage", "OgTitle", "OgType", "published_at", "SchemaMarkup", "SitemapChangeFrequency", "SitemapPriority", "slug", "title", "TwitterCard", "TwitterDescription", "TwitterImage", "TwitterTitle", "updated_at", "updated_by" },
            values: new object[,]
            {
                { 1, null, null, "This is the About Us page content.", new DateTime(2025, 5, 4, 17, 45, 6, 910, DateTimeKind.Utc).AddTicks(9826), null, null, null, null, false, false, null, null, null, "website", new DateTime(2025, 5, 4, 17, 45, 6, 910, DateTimeKind.Utc).AddTicks(9823), null, "monthly", 0.5, "about-us", "About Us", "summary_large_image", null, null, null, null, null },
                { 2, null, null, "This is the Privacy Policy page content.", new DateTime(2025, 5, 4, 17, 45, 6, 910, DateTimeKind.Utc).AddTicks(9828), null, null, null, null, false, false, null, null, null, "website", new DateTime(2025, 5, 4, 17, 45, 6, 910, DateTimeKind.Utc).AddTicks(9827), null, "monthly", 0.5, "privacy-policy", "Privacy Policy", "summary_large_image", null, null, null, null, null }
            });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
            values: new object[] { 1, "General", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(203), null, "Sơn Đại Minh Việt Nam", "Tên website hiển thị trên trang và tiêu đề trình duyệt.", true, "SiteName", null, null, "Sơn Đại Minh Việt Nam" });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
            values: new object[,]
            {
                { 2, "General", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(205), null, "https://localhost:7001", "Địa chỉ URL chính của website (ví dụ: https://www.example.com).", true, "SiteUrl", 7, null, null, "https://localhost:7001" },
                { 3, "General", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(207), null, "sondaiminh@gmail.com", "Địa chỉ email quản trị viên để nhận thông báo hệ thống (đơn hàng, liên hệ...).", true, "AdminEmail", 6, null, null, "sondaiminh@gmail.com" },
                { 4, "General", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(256), null, "/images/logo.png", "Đường dẫn đến file logo website.", true, "LogoUrl", 3, null, null, "/images/logo.png" },
                { 5, "General", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(258), null, "/images/favicon.ico", "Đường dẫn đến file favicon.ico hoặc ảnh favicon.", true, "FaviconUrl", 3, null, null, "/images/favicon.ico" },
                { 6, "General", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(259), null, "12", "Số lượng sản phẩm/bài viết hiển thị trên mỗi trang danh sách mặc định.", true, "ItemsPerPage", 8, null, null, "12" }
            });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
            values: new object[] { 11, "Contact", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(261), null, "Sơn Đại Minh", "Tên công ty hoặc tổ chức sở hữu website.", true, "CompanyName", null, null, "Sơn Đại Minh" });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
            values: new object[,]
            {
                { 12, "Contact", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(263), null, "Tiên Phương, Chương Mỹ, Hà Nội", "Địa chỉ liên hệ đầy đủ của công ty.", true, "ContactAddress", 1, null, null, "Tiên Phương, Chương Mỹ, Hà Nội" },
                { 13, "Contact", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(264), null, "0979758340", "Số điện thoại liên hệ chung.", true, "ContactPhone", 4, null, null, "0979758340" },
                { 14, "Contact", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(266), null, "sondaiminh@gmail.com", "Địa chỉ email hiển thị công khai để liên hệ.", true, "ContactEmail", 6, null, null, "sondaiminh@gmail.com" },
                { 15, "Contact", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(268), null, "<iframe src=\"https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5152.719628304902!2d105.68369562421606!3d20.94205043073292!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3134539b465b1be5%3A0x279c032890c390c5!2zxJDhuqFpIE1pbmggVmnhu4d0IE5hbQ!5e1!3m2!1svi!2s!4v1745895737603!5m2!1svi!2s\" width=\"600\" height=\"450\" style=\"border:0;\" allowfullscreen=\"\" loading=\"lazy\" referrerpolicy=\"no-referrer-when-downgrade\"></iframe>", "Mã nhúng HTML của bản đồ (ví dụ: Google Maps iframe) hiển thị trên trang Liên hệ.", true, "ContactMapEmbed", 2, null, null, "<iframe src=\"https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5152.719628304902!2d105.68369562421606!3d20.94205043073292!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3134539b465b1be5%3A0x279c032890c390c5!2zxJDhuqFpIE1pbmggVmnhu4d0IE5hbQ!5e1!3m2!1svi!2s!4v1745895737603!5m2!1svi!2s\" width=\"600\" height=\"450\" style=\"border:0;\" allowfullscreen=\"\" loading=\"lazy\" referrerpolicy=\"no-referrer-when-downgrade\"></iframe>" },
                { 16, "Contact", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(270), null, "0979758340", "Số điện thoại Hotline hỗ trợ nhanh.", true, "HotlinePhone", 4, null, null, "0979758340" }
            });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
            values: new object[,]
            {
                { 17, "Contact", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(271), null, "Thứ 2 - Thứ 7: 8h00 - 17h00", "Giờ làm việc của công ty.", true, "ContactWorkingHours", null, null, "Thứ 2 - Thứ 7: 8h00 - 17h00" },
                { 18, "Contact", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(273), null, null, "Mã số thuế của công ty.", true, "TaxId", null, null, null },
                { 21, "SEO", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(274), null, "Sơn Chống Thấm, Vật Liệu Sơn Chính Hãng | Đại Minh Việt Nam", "Tiêu đề meta mặc định cho các trang không có tiêu đề riêng.", true, "DefaultMetaTitle", null, null, "Sơn Chống Thấm, Vật Liệu Sơn Chính Hãng | Đại Minh Việt Nam" }
            });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
            values: new object[] { 22, "SEO", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(276), null, "Đại Minh Việt Nam chuyên cung cấp sơn, vật liệu chống thấm, phụ gia bê tông chính hãng từ các thương hiệu hàng đầu. Tư vấn giải pháp thi công hiệu quả. Liên hệ 0979758340.", "Mô tả meta mặc định (dưới 160 ký tự) cho các trang không có mô tả riêng.", true, "DefaultMetaDescription", 1, null, null, "Đại Minh Việt Nam chuyên cung cấp sơn, vật liệu chống thấm, phụ gia bê tông chính hãng từ các thương hiệu hàng đầu. Tư vấn giải pháp thi công hiệu quả. Liên hệ 0979758340." });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
            values: new object[] { 23, "SEO", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(278), null, null, "Mã ID Google Analytics (ví dụ: UA-XXXXXXX-Y hoặc G-XXXXXXXXXX).", true, "GoogleAnalyticsId", null, null, null });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
            values: new object[,]
            {
                { 24, "SEO", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(279), null, null, "Các meta tag xác minh website (Google, Bing, ...).", true, "VerificationMetaTags", 1, null, null, null },
                { 31, "Social Media", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(281), null, "https://www.facebook.com/LienDaiMinh", "URL trang Facebook của công ty.", true, "SocialFacebookUrl", 7, null, null, "https://www.facebook.com/LienDaiMinh" },
                { 32, "Social Media", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(282), null, null, "URL trang Twitter (X) của công ty.", true, "SocialTwitterUrl", 7, null, null, null },
                { 33, "Social Media", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(284), null, null, "URL trang Instagram của công ty.", true, "SocialInstagramUrl", 7, null, null, null },
                { 34, "Social Media", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(285), null, null, "URL trang LinkedIn của công ty.", true, "SocialLinkedInUrl", 7, null, null, null },
                { 35, "Social Media", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(287), null, null, "URL kênh Youtube của công ty.", true, "SocialYoutubeUrl", 7, null, null, null },
                { 36, "Social Media", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(288), null, "https://www.tiktok.com/@hung.daiminh", "URL kênh Tiktok của công ty.", true, "SocialTiktokUrl", 7, null, null, "https://www.tiktok.com/@hung.daiminh" },
                { 37, "Social Media", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(291), null, "0979758340", "Số điện thoại Zalo để liên hệ nhanh (có thể khác Hotline).", true, "SocialZaloPhone", 4, null, null, "0979758340" }
            });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
            values: new object[] { 41, "Email", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(292), null, "smtp.example.com", "Địa chỉ máy chủ SMTP để gửi email.", true, "SmtpHost", null, null, "smtp.example.com" });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
            values: new object[] { 42, "Email", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(294), null, "587", "Cổng SMTP (ví dụ: 587, 465, 25).", true, "SmtpPort", 8, null, null, "587" });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
            values: new object[,]
            {
                { 43, "Email", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(296), null, "user@example.com", "Tên đăng nhập SMTP.", true, "SmtpUsername", null, null, "user@example.com" },
                { 44, "Email", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(298), null, null, "**QUAN TRỌNG**: Mật khẩu SMTP. Nên cấu hình qua UI, không seed giá trị thật.", true, "SmtpPassword", null, null, null }
            });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
            values: new object[] { 45, "Email", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(299), null, "true", "Sử dụng mã hóa SSL/TLS khi gửi email.", true, "SmtpUseSsl", 9, null, null, "true" });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
            values: new object[] { 46, "Email", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(301), null, "Đại Minh Việt Nam", "Tên hiển thị trong ô 'From' của email gửi đi.", true, "EmailFromName", null, null, "Đại Minh Việt Nam" });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
            values: new object[] { 47, "Email", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(303), null, "noreply@daiminhvietnam.com", "Địa chỉ email hiển thị trong ô 'From' của email gửi đi.", true, "EmailFromAddress", 6, null, null, "noreply@daiminhvietnam.com" });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
            values: new object[,]
            {
                { 48, "Email", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(304), null, "ContactReply", "Mã/tên template email phản hồi tự động khi nhận form liên hệ.", true, "EmailTemplateContactFormReply", null, null, "ContactReply" },
                { 49, "Email", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(306), null, "NewsletterSubscribe", "Mã/tên template email xác nhận đăng ký nhận tin.", true, "EmailTemplateNewsletterSubscribe", null, null, "NewsletterSubscribe" }
            });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
            values: new object[,]
            {
                { 51, "Appearance", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(308), null, "/images/banners/banner1.jpg", "URL ảnh banner chính trang chủ.", true, "HomepageBanner1Url", 3, null, null, "/images/banners/banner1.jpg" },
                { 52, "Appearance", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(309), null, "/", "URL liên kết khi click banner chính trang chủ.", true, "HomepageBanner1Link", 7, null, null, "/" }
            });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
            values: new object[] { 53, "Appearance", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(353), null, "© 2025 Sơn Đại Minh Việt Nam. All rights reserved.", "Nội dung text copyright hiển thị ở chân trang.", true, "CopyrightText", null, null, "© 2025 Sơn Đại Minh Việt Nam. All rights reserved." });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
            values: new object[] { 61, "Integration", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(355), null, null, "Mã script nhúng Live Chat (Zalo, Tawk.to, subiz...).", true, "LiveChatScript", 2, null, null, null });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "updated_at", "updated_by", "value" },
            values: new object[,]
            {
                { 71, "E-commerce", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(357), null, "VND", "Mã tiền tệ chính được sử dụng (ví dụ: VND, USD).", true, "CurrencyCode", null, null, "VND" },
                { 72, "E-commerce", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(358), null, "đ", "Ký hiệu tiền tệ hiển thị (ví dụ: đ, $).", true, "CurrencySymbol", null, null, "đ" }
            });

        migrationBuilder.InsertData(
            table: "settings",
            columns: new[] { "Id", "category", "created_at", "created_by", "default_value", "description", "is_active", "key", "type", "updated_at", "updated_by", "value" },
            values: new object[] { 73, "E-commerce", new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(360), null, "/images/product-placeholder.png", "URL ảnh mặc định hiển thị khi sản phẩm không có ảnh.", true, "DefaultProductImageUrl", 3, null, null, "/images/product-placeholder.png" });

        migrationBuilder.InsertData(
            table: "tags",
            columns: new[] { "Id", "created_at", "created_by", "description", "name", "slug", "updated_at", "updated_by" },
            values: new object[,]
            {
                { 1, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6313), null, null, "Chống thấm", "chong-tham", null, null },
                { 2, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6314), null, null, "Sơn nội thất", "son-noi-that", null, null },
                { 3, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6316), null, null, "Sơn ngoại thất", "son-ngoai-that", null, null },
                { 4, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6317), null, null, "Phụ gia bê tông", "phu-gia-be-tong", null, null },
                { 5, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6318), null, null, "Keo chít mạch", "keo-chit-mach", null, null },
                { 6, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6319), null, null, "Vật liệu xây dựng", "vat-lieu-xay-dung", null, null }
            });

        migrationBuilder.InsertData(
            table: "tags",
            columns: new[] { "Id", "created_at", "created_by", "description", "name", "slug", "type", "updated_at", "updated_by" },
            values: new object[,]
            {
                { 7, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6321), null, null, "Tư vấn chọn sơn", "tu-van-chon-son", 1, null, null },
                { 8, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6322), null, null, "Hướng dẫn thi công", "huong-dan-thi-cong", 1, null, null },
                { 9, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6323), null, null, "Kinh nghiệm chống thấm", "kinh-nghiem-chong-tham", 1, null, null },
                { 10, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6324), null, null, "Bảo trì nhà cửa", "bao-tri-nha-cua", 1, null, null }
            });

        migrationBuilder.InsertData(
            table: "testimonials",
            columns: new[] { "Id", "client_avatar", "client_company", "client_name", "client_title", "content", "created_at", "created_by", "is_active", "rating", "updated_at", "updated_by" },
            values: new object[] { 1, null, null, "Nguyễn Văn A", "Chủ nhà", "Sơn của Đại Minh rất bền màu và dễ thi công. Tôi rất hài lòng!", new DateTime(2025, 5, 4, 17, 45, 6, 910, DateTimeKind.Utc).AddTicks(3717), null, true, 5, null, null });

        migrationBuilder.InsertData(
            table: "testimonials",
            columns: new[] { "Id", "client_avatar", "client_company", "client_name", "client_title", "content", "created_at", "created_by", "is_active", "order_index", "rating", "updated_at", "updated_by" },
            values: new object[,]
            {
                { 2, null, "Công ty Xây dựng B&B", "Trần Thị B", "Nhà thầu", "Vật liệu chống thấm Sika từ Đại Minh luôn đảm bảo chất lượng cho công trình của chúng tôi.", new DateTime(2025, 5, 4, 17, 45, 6, 910, DateTimeKind.Utc).AddTicks(3719), null, true, 1, 5, null, null },
                { 3, null, null, "Lê Văn C", "Khách hàng cá nhân", "Được tư vấn rất nhiệt tình để chọn đúng loại sơn cho ngôi nhà cũ. Dịch vụ tuyệt vời!", new DateTime(2025, 5, 4, 17, 45, 6, 910, DateTimeKind.Utc).AddTicks(3721), null, true, 2, 4, null, null }
            });

        migrationBuilder.InsertData(
            table: "users",
            columns: new[] { "Id", "created_at", "created_by", "email", "full_name", "is_active", "password_hash", "updated_at", "updated_by", "username" },
            values: new object[] { 1, new DateTime(2025, 5, 4, 17, 45, 6, 846, DateTimeKind.Utc).AddTicks(9424), null, "admin@admin.com", "Quản trị viên", true, "AQAAAAIAAYagAAAAEG3pA2H6U75R+y88fVvNbKk4fXzsSLz7Jz537rOFNfCpMqrjUVVKJcgQdiol+lrJSg==", null, null, "admin" });

        migrationBuilder.InsertData(
            table: "attribute_values",
            columns: new[] { "Id", "attribute_id", "created_at", "created_by", "slug", "updated_at", "updated_by", "value" },
            values: new object[,]
            {
                { 1, 1, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1096), null, "trang", null, null, "Trắng" },
                { 2, 1, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1148), null, "do", null, null, "Đỏ" },
                { 3, 1, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1149), null, "xanh-duong", null, null, "Xanh dương" },
                { 4, 1, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1151), null, "vang", null, null, "Vàng" },
                { 5, 2, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1152), null, "1-lit", null, null, "1 Lít" },
                { 6, 2, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1153), null, "5-lit", null, null, "5 Lít" },
                { 7, 2, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1154), null, "18-lit", null, null, "18 Lít" },
                { 8, 2, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1155), null, "20-kg", null, null, "20 Kg" },
                { 9, 3, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1156), null, "bong", null, null, "Bóng" },
                { 10, 3, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1158), null, "mo", null, null, "Mờ" },
                { 11, 3, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1159), null, "ban-bong", null, null, "Bán bóng" },
                { 12, 4, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1160), null, "tuong-noi-that", null, null, "Tường nội thất" },
                { 13, 4, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1162), null, "tuong-ngoai-that", null, null, "Tường ngoại thất" },
                { 14, 4, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1163), null, "san-be-tong", null, null, "Sàn bê tông" },
                { 15, 4, new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1164), null, "san-thuong", null, null, "Sân thượng" }
            });

        migrationBuilder.InsertData(
            table: "categories",
            columns: new[] { "Id", "created_at", "created_by", "description", "icon", "is_active", "name", "parent_id", "slug", "updated_at", "updated_by" },
            values: new object[] { 6, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3470), null, null, null, true, "Sơn Nội Thất", 1, "son-noi-that", null, null });

        migrationBuilder.InsertData(
            table: "categories",
            columns: new[] { "Id", "created_at", "created_by", "description", "icon", "is_active", "name", "order_index", "parent_id", "slug", "updated_at", "updated_by" },
            values: new object[,]
            {
                { 7, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3472), null, null, null, true, "Sơn Ngoại Thất", 1, 1, "son-ngoai-that", null, null },
                { 8, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3473), null, null, null, true, "Sơn Lót", 2, 1, "son-lot", null, null },
                { 9, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3475), null, null, null, true, "Sơn Chống Kiềm", 3, 1, "son-chong-kiem", null, null }
            });

        migrationBuilder.InsertData(
            table: "categories",
            columns: new[] { "Id", "created_at", "created_by", "description", "icon", "is_active", "name", "parent_id", "slug", "updated_at", "updated_by" },
            values: new object[] { 10, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3476), null, null, null, true, "Chống Thấm Sàn Mái", 2, "chong-tham-san-mai", null, null });

        migrationBuilder.InsertData(
            table: "categories",
            columns: new[] { "Id", "created_at", "created_by", "description", "icon", "is_active", "name", "order_index", "parent_id", "slug", "updated_at", "updated_by" },
            values: new object[,]
            {
                { 11, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3478), null, null, null, true, "Chống Thấm Tường", 1, 2, "chong-tham-tuong", null, null },
                { 12, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3479), null, null, null, true, "Chống Thấm Nhà Vệ Sinh", 2, 2, "chong-tham-nha-ve-sinh", null, null }
            });

        migrationBuilder.InsertData(
            table: "categories",
            columns: new[] { "Id", "created_at", "created_by", "description", "icon", "is_active", "name", "parent_id", "slug", "updated_at", "updated_by" },
            values: new object[] { 13, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3481), null, null, null, true, "Phụ Gia Bê Tông", 3, "phu-gia-be-tong", null, null });

        migrationBuilder.InsertData(
            table: "categories",
            columns: new[] { "Id", "created_at", "created_by", "description", "icon", "is_active", "name", "order_index", "parent_id", "slug", "updated_at", "updated_by" },
            values: new object[] { 14, new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3482), null, null, null, true, "Keo Chít Mạch", 1, 3, "keo-chit-mach", null, null });

        migrationBuilder.InsertData(
            table: "faqs",
            columns: new[] { "Id", "answer", "category_id", "created_at", "created_by", "is_active", "question", "updated_at", "updated_by" },
            values: new object[] { 1, "Sơn nội thất và ngoại thất khác nhau về thành phần hóa học để phù hợp với điều kiện môi trường. Sơn ngoại thất chứa các chất chống tia UV, chống thấm tốt hơn để chịu được nắng, mưa, ẩm ướt. Sơn nội thất an toàn hơn cho sức khỏe, ít mùi và có độ bền màu trong nhà tốt.", 5, new DateTime(2025, 5, 4, 17, 45, 6, 917, DateTimeKind.Utc).AddTicks(1827), null, true, "Sơn nội thất và sơn ngoại thất khác nhau như thế nào?", null, null });

        migrationBuilder.InsertData(
            table: "faqs",
            columns: new[] { "Id", "answer", "category_id", "created_at", "created_by", "is_active", "order_index", "question", "updated_at", "updated_by" },
            values: new object[,]
            {
                { 2, "Lượng sơn cần dùng phụ thuộc vào diện tích cần sơn, loại sơn, và bề mặt. Trung bình 1 lít sơn có thể phủ được 8-10m2 cho 2 lớp. Bạn cần đo diện tích tường, trần nhà và tham khảo hướng dẫn của nhà sản xuất sơn.", 5, new DateTime(2025, 5, 4, 17, 45, 6, 917, DateTimeKind.Utc).AddTicks(1830), null, true, 1, "Làm thế nào để tính toán lượng sơn cần dùng?", null, null },
                { 3, "Sơn lót chống kiềm cần được sử dụng trên các bề mặt mới xây (vữa, bê tông) hoặc các bề mặt cũ có dấu hiệu bị kiềm hóa (ố vàng, phấn trắng) để ngăn chặn kiềm từ xi măng ăn mòn lớp sơn phủ màu.", 5, new DateTime(2025, 5, 4, 17, 45, 6, 917, DateTimeKind.Utc).AddTicks(1832), null, true, 2, "Khi nào cần sử dụng sơn lót chống kiềm?", null, null }
            });

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
            name: "IX_articles_published_at",
            table: "articles",
            column: "published_at");

        migrationBuilder.CreateIndex(
            name: "IX_articles_slug",
            table: "articles",
            column: "slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_attribute_values_attribute_id_slug",
            table: "attribute_values",
            columns: new[] { "attribute_id", "slug" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_attributes_slug",
            table: "attributes",
            column: "slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_brands_slug",
            table: "brands",
            column: "slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_categories_parent_id",
            table: "categories",
            column: "parent_id");

        migrationBuilder.CreateIndex(
            name: "IX_categories_slug",
            table: "categories",
            column: "slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_contacts_email",
            table: "contacts",
            column: "email");

        migrationBuilder.CreateIndex(
            name: "IX_faqs_category_id",
            table: "faqs",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "IX_faqs_order_index",
            table: "faqs",
            column: "order_index");

        migrationBuilder.CreateIndex(
            name: "IX_newsletters_email",
            table: "newsletters",
            column: "email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_pages_slug",
            table: "pages",
            column: "slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_product_attributes_attribute_id",
            table: "product_attributes",
            column: "attribute_id");

        migrationBuilder.CreateIndex(
            name: "IX_product_images_order_index",
            table: "product_images",
            column: "order_index");

        migrationBuilder.CreateIndex(
            name: "IX_product_images_product_id",
            table: "product_images",
            column: "product_id");

        migrationBuilder.CreateIndex(
            name: "IX_product_reviews_product_id",
            table: "product_reviews",
            column: "product_id");

        migrationBuilder.CreateIndex(
            name: "IX_product_reviews_rating",
            table: "product_reviews",
            column: "rating");

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
            name: "IX_product_variations_is_default",
            table: "product_variations",
            column: "is_default");

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

        migrationBuilder.CreateIndex(
            name: "IX_products_slug",
            table: "products",
            column: "slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_settings_key_category",
            table: "settings",
            columns: new[] { "key", "category" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_slides_is_active",
            table: "slides",
            column: "is_active");

        migrationBuilder.CreateIndex(
            name: "IX_slides_order_index",
            table: "slides",
            column: "order_index");

        migrationBuilder.CreateIndex(
            name: "IX_tags_slug",
            table: "tags",
            column: "slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_testimonials_order_index",
            table: "testimonials",
            column: "order_index");

        migrationBuilder.CreateIndex(
            name: "IX_testimonials_rating",
            table: "testimonials",
            column: "rating");

        migrationBuilder.CreateIndex(
            name: "IX_users_email",
            table: "users",
            column: "email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_users_username",
            table: "users",
            column: "username",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "article_products");

        migrationBuilder.DropTable(
            name: "article_tags");

        migrationBuilder.DropTable(
            name: "banners");

        migrationBuilder.DropTable(
            name: "contacts");

        migrationBuilder.DropTable(
            name: "faqs");

        migrationBuilder.DropTable(
            name: "media_files");

        migrationBuilder.DropTable(
            name: "newsletters");

        migrationBuilder.DropTable(
            name: "pages");

        migrationBuilder.DropTable(
            name: "popup_modals");

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
            name: "slides");

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

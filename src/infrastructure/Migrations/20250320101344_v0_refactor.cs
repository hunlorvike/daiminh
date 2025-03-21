using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations;

/// <inheritdoc />
public partial class v0_refactor : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "categories",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                parent_category_id = table.Column<int>(type: "integer", nullable: true),
                entity_type = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_categories", x => x.id);
                table.ForeignKey(
                    name: "FK_categories_categories_parent_category_id",
                    column: x => x.parent_category_id,
                    principalTable: "categories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "contacts",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                message = table.Column<string>(type: "text", nullable: false),
                status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_contacts", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "content_types",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                slug = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_content_types", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "folders",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                parent_id = table.Column<int>(type: "integer", nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_folders", x => x.id);
                table.ForeignKey(
                    name: "fk_folders_parent_id",
                    column: x => x.parent_id,
                    principalTable: "folders",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "product_types",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                slug = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_product_types", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "roles",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                permissions = table.Column<string>(type: "text", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_roles", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "settings",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                group = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                key = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                value = table.Column<string>(type: "text", nullable: false),
                description = table.Column<string>(type: "text", nullable: false),
                order_number = table.Column<int>(type: "integer", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_settings", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "slider",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                title = table.Column<string>(type: "text", nullable: false),
                image_url = table.Column<string>(type: "text", nullable: false),
                link_url = table.Column<string>(type: "text", nullable: true),
                order_number = table.Column<int>(type: "integer", nullable: false),
                overlay_html = table.Column<string>(type: "text", nullable: true),
                overlay_position = table.Column<string>(type: "text", nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_slider", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "subscriber",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_subscriber", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "tags",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                slug = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                entity_type = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tags", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "content_field_definitions",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                content_type_id = table.Column<int>(type: "integer", nullable: false),
                field_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                field_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "text"),
                is_required = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                field_options = table.Column<string>(type: "text", nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_content_field_definitions", x => x.id);
                table.ForeignKey(
                    name: "FK_content_field_definitions_content_types_content_type_id",
                    column: x => x.content_type_id,
                    principalTable: "content_types",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "media_files",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                mime_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                size = table.Column<long>(type: "bigint", nullable: false),
                extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                folder_id = table.Column<int>(type: "integer", nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_media_files", x => x.id);
                table.ForeignKey(
                    name: "fk_media_files_folder_id",
                    column: x => x.folder_id,
                    principalTable: "folders",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "product_field_definitions",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                product_type_id = table.Column<int>(type: "integer", nullable: false),
                field_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                field_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "text"),
                is_required = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                field_options = table.Column<string>(type: "text", nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_product_field_definitions", x => x.id);
                table.ForeignKey(
                    name: "FK_product_field_definitions_product_types_product_type_id",
                    column: x => x.product_type_id,
                    principalTable: "product_types",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "products",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                description = table.Column<string>(type: "text", nullable: false),
                base_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                sku = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "draft"),
                product_type_id = table.Column<int>(type: "integer", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                meta_title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                meta_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                canonical_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                og_title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                og_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                og_image = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                structured_data = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_products", x => x.id);
                table.ForeignKey(
                    name: "FK_products_product_types_product_type_id",
                    column: x => x.product_type_id,
                    principalTable: "product_types",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "users",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                password_hash = table.Column<string>(type: "text", nullable: false),
                role_id = table.Column<int>(type: "integer", nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_users", x => x.id);
                table.ForeignKey(
                    name: "FK_users_roles_role_id",
                    column: x => x.role_id,
                    principalTable: "roles",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "product_categories",
            columns: table => new
            {
                product_id = table.Column<int>(type: "integer", nullable: false),
                category_id = table.Column<int>(type: "integer", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_product_categories", x => new { x.product_id, x.category_id });
                table.ForeignKey(
                    name: "FK_product_categories_categories_category_id",
                    column: x => x.category_id,
                    principalTable: "categories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_product_categories_products_product_id",
                    column: x => x.product_id,
                    principalTable: "products",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "product_field_values",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                product_id = table.Column<int>(type: "integer", nullable: false),
                field_id = table.Column<int>(type: "integer", nullable: false),
                value = table.Column<string>(type: "text", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_product_field_values", x => x.id);
                table.ForeignKey(
                    name: "FK_product_field_values_product_field_definitions_field_id",
                    column: x => x.field_id,
                    principalTable: "product_field_definitions",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_product_field_values_products_product_id",
                    column: x => x.product_id,
                    principalTable: "products",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "product_image",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                product_id = table.Column<int>(type: "integer", nullable: false),
                image_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                alt_text = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                is_primary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                display_order = table.Column<short>(type: "smallint", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_product_image", x => x.id);
                table.ForeignKey(
                    name: "FK_product_image_products_product_id",
                    column: x => x.product_id,
                    principalTable: "products",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "product_tags",
            columns: table => new
            {
                product_id = table.Column<int>(type: "integer", nullable: false),
                tag_id = table.Column<int>(type: "integer", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_product_tags", x => new { x.product_id, x.tag_id });
                table.ForeignKey(
                    name: "FK_product_tags_products_product_id",
                    column: x => x.product_id,
                    principalTable: "products",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_product_tags_tags_tag_id",
                    column: x => x.tag_id,
                    principalTable: "tags",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "contents",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                content_type_id = table.Column<int>(type: "integer", nullable: false),
                author_id = table.Column<int>(type: "integer", nullable: true),
                title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                content_body = table.Column<string>(type: "text", nullable: false),
                cover_image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "draft"),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                meta_title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                meta_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                canonical_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                og_title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                og_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                og_image = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                structured_data = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_contents", x => x.id);
                table.ForeignKey(
                    name: "FK_contents_content_types_content_type_id",
                    column: x => x.content_type_id,
                    principalTable: "content_types",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_contents_users_author_id",
                    column: x => x.author_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "reviews",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                product_id = table.Column<int>(type: "integer", nullable: false),
                user_id = table.Column<int>(type: "integer", nullable: false),
                rating = table.Column<short>(type: "smallint", nullable: false),
                comment = table.Column<string>(type: "text", nullable: false),
                status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_reviews", x => x.id);
                table.CheckConstraint("CK_Review_Rating", "rating BETWEEN 1 AND 5");
                table.ForeignKey(
                    name: "FK_reviews_products_product_id",
                    column: x => x.product_id,
                    principalTable: "products",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_reviews_users_user_id",
                    column: x => x.user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "comments",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                content_id = table.Column<int>(type: "integer", nullable: false),
                user_id = table.Column<int>(type: "integer", nullable: true),
                parent_comment_id = table.Column<int>(type: "integer", nullable: true),
                subject = table.Column<string>(type: "text", nullable: false),
                status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "approved"),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_comments", x => x.id);
                table.ForeignKey(
                    name: "FK_comments_comments_parent_comment_id",
                    column: x => x.parent_comment_id,
                    principalTable: "comments",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_comments_contents_content_id",
                    column: x => x.content_id,
                    principalTable: "contents",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_comments_users_user_id",
                    column: x => x.user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "content_categories",
            columns: table => new
            {
                content_id = table.Column<int>(type: "integer", nullable: false),
                category_id = table.Column<int>(type: "integer", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_content_categories", x => new { x.content_id, x.category_id });
                table.ForeignKey(
                    name: "FK_content_categories_categories_category_id",
                    column: x => x.category_id,
                    principalTable: "categories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_content_categories_contents_content_id",
                    column: x => x.content_id,
                    principalTable: "contents",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "content_field_values",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                content_id = table.Column<int>(type: "integer", nullable: false),
                field_id = table.Column<int>(type: "integer", nullable: false),
                value = table.Column<string>(type: "text", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_content_field_values", x => x.id);
                table.ForeignKey(
                    name: "FK_content_field_values_content_field_definitions_field_id",
                    column: x => x.field_id,
                    principalTable: "content_field_definitions",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_content_field_values_contents_content_id",
                    column: x => x.content_id,
                    principalTable: "contents",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "content_tags",
            columns: table => new
            {
                content_id = table.Column<int>(type: "integer", nullable: false),
                tag_id = table.Column<int>(type: "integer", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_content_tags", x => new { x.content_id, x.tag_id });
                table.ForeignKey(
                    name: "FK_content_tags_contents_content_id",
                    column: x => x.content_id,
                    principalTable: "contents",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_content_tags_tags_tag_id",
                    column: x => x.tag_id,
                    principalTable: "tags",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "categories",
            columns: new[] { "id", "created_at", "deleted_at", "is_active", "name", "parent_category_id", "slug", "updated_at" },
            values: new object[,]
            {
                { 1, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5876), null, true, "Sơn nội thất", null, "son-noi-that", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5876) },
                { 2, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5878), null, true, "Sơn ngoại thất", null, "son-ngoai-that", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5878) },
                { 3, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5879), null, true, "Sơn chống thấm", null, "son-chong-tham", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5879) },
                { 4, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5880), null, true, "Sơn lót", null, "son-lot", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5880) },
                { 5, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5881), null, true, "Sơn gỗ", null, "son-go", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5881) },
                { 6, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5882), null, true, "Sơn kim loại", null, "son-kim-loai", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5882) },
                { 7, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5883), null, true, "Dụng cụ sơn", null, "dung-cu-son", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5884) }
            });

        migrationBuilder.InsertData(
            table: "categories",
            columns: new[] { "id", "created_at", "deleted_at", "entity_type", "is_active", "name", "parent_category_id", "slug", "updated_at" },
            values: new object[,]
            {
                { 101, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5896), null, 1, true, "Dịch vụ thi công sơn trọn gói", null, "dich-vu-thi-cong-son-tron-goi", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5896) },
                { 102, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5897), null, 1, true, "Tư vấn phối màu sơn", null, "tu-van-phoi-mau-son", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5897) },
                { 103, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5898), null, 1, true, "Tư vấn kỹ thuật sơn", null, "tu-van-ky-thuat-son", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5898) }
            });

        migrationBuilder.InsertData(
            table: "content_types",
            columns: new[] { "id", "created_at", "deleted_at", "is_active", "name", "slug", "updated_at" },
            values: new object[,]
            {
                { 1, new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8892), null, true, "Bài viết", "bai-viet", new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8893) },
                { 2, new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8902), null, true, "Trang tĩnh", "trang-tinh", new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8902) },
                { 3, new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8903), null, true, "Dịch vụ", "dich-vu", new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8904) },
                { 4, new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8905), null, true, "Tư vấn", "tu-van", new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8905) },
                { 5, new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8906), null, true, "Chính sách", "chinh-sach", new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8906) }
            });

        migrationBuilder.InsertData(
            table: "product_types",
            columns: new[] { "id", "created_at", "deleted_at", "is_active", "name", "slug", "updated_at" },
            values: new object[,]
            {
                { 1, new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7886), null, true, "Sơn Nước", "son-nuoc", new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7888) },
                { 2, new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7889), null, true, "Sơn Dầu", "son-dau", new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7890) },
                { 3, new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7891), null, true, "Sơn Acrylic", "son-acrylic", new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7891) },
                { 4, new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7892), null, true, "Sơn Epoxy", "son-epoxy", new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7892) },
                { 5, new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7893), null, true, "Sơn Alkyd", "son-alkyd", new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7893) },
                { 6, new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7894), null, true, "Sơn Lót", "son-lot", new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7894) },
                { 7, new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7895), null, true, "Sơn Chống Thấm", "son-chong-tham", new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7895) },
                { 8, new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7896), null, true, "Sơn Gỗ", "son-go", new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7896) },
                { 9, new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7897), null, true, "Sơn Kim Loại", "son-kim-loai", new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7897) }
            });

        migrationBuilder.InsertData(
            table: "roles",
            columns: new[] { "id", "created_at", "deleted_at", "is_active", "name", "permissions", "updated_at" },
            values: new object[,]
            {
                { 1, new DateTime(2025, 3, 20, 10, 13, 43, 458, DateTimeKind.Utc).AddTicks(7040), null, true, "Admin", "", new DateTime(2025, 3, 20, 10, 13, 43, 458, DateTimeKind.Utc).AddTicks(7041) },
                { 2, new DateTime(2025, 3, 20, 10, 13, 43, 458, DateTimeKind.Utc).AddTicks(7042), null, true, "User", "", new DateTime(2025, 3, 20, 10, 13, 43, 458, DateTimeKind.Utc).AddTicks(7042) },
                { 3, new DateTime(2025, 3, 20, 10, 13, 43, 458, DateTimeKind.Utc).AddTicks(7043), null, true, "Manager", "", new DateTime(2025, 3, 20, 10, 13, 43, 458, DateTimeKind.Utc).AddTicks(7043) }
            });

        migrationBuilder.InsertData(
            table: "tags",
            columns: new[] { "id", "created_at", "deleted_at", "is_active", "name", "slug", "updated_at" },
            values: new object[,]
            {
                { 1, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9849), null, true, "Chống thấm", "chong-tham", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9850) },
                { 2, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9852), null, true, "Bền màu", "ben-mau", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9852) },
                { 3, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9853), null, true, "Dễ lau chùi", "de-lau-chui", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9853) },
                { 4, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9854), null, true, "Kháng khuẩn", "khang-khuan", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9855) },
                { 5, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9855), null, true, "Chống nấm mốc", "chong-nam-moc", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9856) },
                { 6, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9856), null, true, "Giá rẻ", "gia-re", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9857) },
                { 7, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9857), null, true, "Chất lượng", "chat-luong", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9858) },
                { 8, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9858), null, true, "Cao cấp", "cao-cap", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9859) },
                { 9, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9860), null, true, "Màu sắc bền đẹp", "mau-sac-ben-dep", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9860) },
                { 10, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9861), null, true, "Dễ thi công", "de-thi-cong", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9861) },
                { 11, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9862), null, true, "An toàn", "an-toan", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9862) },
                { 12, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9863), null, true, "Thương hiệu nổi tiếng", "thuong-hieu-noi-tieng", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9863) }
            });

        migrationBuilder.InsertData(
            table: "tags",
            columns: new[] { "id", "created_at", "deleted_at", "entity_type", "is_active", "name", "slug", "updated_at" },
            values: new object[,]
            {
                { 101, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9864), null, 1, true, "Thi công nhanh chóng", "thi-cong-nhanh-chong", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9864) },
                { 102, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9865), null, 1, true, "Thợ sơn tay nghề cao", "tho-son-tay-nghe-cao", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9865) },
                { 103, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9866), null, 1, true, "Bảo hành dài hạn", "bao-hanh-dai-han", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9866) },
                { 104, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9867), null, 1, true, "Tư vấn màu sắc miễn phí", "tu-van-mau-sac-mien-phi", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9867) },
                { 105, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9868), null, 1, true, "Báo giá cạnh tranh", "bao-gia-canh-tranh", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9868) },
                { 106, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9869), null, 1, true, "Thi công sơn nội thất", "thi-cong-son-noi-that", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9869) },
                { 107, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9870), null, 1, true, "Thi công sơn ngoại thất", "thi-cong-son-ngoai-that", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9870) },
                { 108, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9871), null, 1, true, "Tư vấn kỹ thuật chuyên nghiệp", "tu-van-ky-thuat-chuyen-nghiep", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9871) },
                { 109, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9872), null, 1, true, "Thi công sơn trọn gói", "thi-cong-son-tron-goi", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9872) },
                { 110, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9873), null, 1, true, "Tư vấn lựa chọn sơn", "tu-van-lua-chon-son", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9873) },
                { 111, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9874), null, 1, true, "Đảm bảo chất lượng thi công", "dam-bao-chat-luong-thi-cong", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9874) },
                { 112, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9875), null, 1, true, "Hỗ trợ tận tâm", "ho-tro-tan-tam", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9875) }
            });

        migrationBuilder.InsertData(
            table: "categories",
            columns: new[] { "id", "created_at", "deleted_at", "is_active", "name", "parent_category_id", "slug", "updated_at" },
            values: new object[,]
            {
                { 8, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5884), null, true, "Sơn bóng nội thất", 1, "son-bong-noi-that", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5885) },
                { 9, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5886), null, true, "Sơn mờ nội thất", 1, "son-mo-noi-that", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5886) },
                { 10, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5887), null, true, "Sơn bóng ngoại thất", 2, "son-bong-ngoai-that", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5888) },
                { 11, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5889), null, true, "Sơn mờ ngoại thất", 2, "son-mo-ngoai-that", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5889) },
                { 12, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5890), null, true, "Sơn chống thấm gốc nước", 3, "son-chong-tham-goc-nuoc", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5890) },
                { 13, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5891), null, true, "Sơn chống thấm gốc dầu", 3, "son-chong-tham-goc-dau", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5891) },
                { 14, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5892), null, true, "Cọ sơn", 7, "co-son", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5893) },
                { 15, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5893), null, true, "Con lăn sơn", 7, "con-lan-son", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5894) },
                { 16, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5895), null, true, "Băng keo dán sơn", 7, "bang-keo-dan-son", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5895) }
            });

        migrationBuilder.InsertData(
            table: "categories",
            columns: new[] { "id", "created_at", "deleted_at", "entity_type", "is_active", "name", "parent_category_id", "slug", "updated_at" },
            values: new object[,]
            {
                { 104, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5899), null, 1, true, "Thi công sơn nội thất trọn gói", 101, "thi-cong-son-noi-that-tron-goi", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5899) },
                { 105, new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5900), null, 1, true, "Thi công sơn ngoại thất trọn gói", 101, "thi-cong-son-ngoai-that-tron-goi", new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5901) }
            });

        migrationBuilder.InsertData(
            table: "content_field_definitions",
            columns: new[] { "id", "content_type_id", "created_at", "deleted_at", "field_name", "field_options", "is_active", "is_required", "updated_at" },
            values: new object[] { 1, 3, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5389), null, "Mô tả ngắn", null, true, true, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5389) });

        migrationBuilder.InsertData(
            table: "content_field_definitions",
            columns: new[] { "id", "content_type_id", "created_at", "deleted_at", "field_name", "field_options", "is_active", "updated_at" },
            values: new object[,]
            {
                { 2, 3, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5391), null, "Quy trình chi tiết", null, true, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5391) },
                { 3, 3, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5392), null, "Bảng giá tham khảo", null, true, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5392) }
            });

        migrationBuilder.InsertData(
            table: "content_field_definitions",
            columns: new[] { "id", "content_type_id", "created_at", "deleted_at", "field_name", "field_options", "is_active", "is_required", "updated_at" },
            values: new object[] { 4, 4, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5393), null, "Mô tả ngắn", null, true, true, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5393) });

        migrationBuilder.InsertData(
            table: "content_field_definitions",
            columns: new[] { "id", "content_type_id", "created_at", "deleted_at", "field_name", "field_options", "is_active", "updated_at" },
            values: new object[] { 5, 4, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5394), null, "Nội dung chi tiết", null, true, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5394) });

        migrationBuilder.InsertData(
            table: "content_field_definitions",
            columns: new[] { "id", "content_type_id", "created_at", "deleted_at", "field_name", "field_options", "field_type", "is_active", "is_required", "updated_at" },
            values: new object[,]
            {
                { 6, 4, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5435), null, "Hình thức tư vấn", "[{\"value\":\"online\",\"label\":\"Tr\\u1EF1c tuy\\u1EBFn\"},{\"value\":\"offline\",\"label\":\"Tr\\u1EF1c ti\\u1EBFp\"}]", "select", true, true, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5435) },
                { 7, 4, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(6019), null, "Thời lượng tư vấn (phút)", null, "number", true, true, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(6019) }
            });

        migrationBuilder.InsertData(
            table: "content_field_definitions",
            columns: new[] { "id", "content_type_id", "created_at", "deleted_at", "field_name", "field_options", "field_type", "is_active", "updated_at" },
            values: new object[] { 8, 4, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(6021), null, "Chi phí", null, "number", true, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(6021) });

        migrationBuilder.InsertData(
            table: "content_field_definitions",
            columns: new[] { "id", "content_type_id", "created_at", "deleted_at", "field_name", "field_options", "is_active", "updated_at" },
            values: new object[] { 9, 4, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(6022), null, "Ảnh minh họa", null, true, new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(6023) });

        migrationBuilder.InsertData(
            table: "product_field_definitions",
            columns: new[] { "id", "created_at", "deleted_at", "field_name", "field_options", "field_type", "is_active", "is_required", "product_type_id", "updated_at" },
            values: new object[,]
            {
                { 1, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(4989), null, "Độ bóng", "[{\"value\":\"bong-mo\",\"label\":\"B\\u00F3ng m\\u1EDD\"},{\"value\":\"bong-nhe\",\"label\":\"B\\u00F3ng nh\\u1EB9\"},{\"value\":\"bong-cao\",\"label\":\"B\\u00F3ng cao\"},{\"value\":\"sieu-bong\",\"label\":\"Si\\u00EAu b\\u00F3ng\"}]", "select", true, true, 1, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(4989) },
                { 2, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5666), null, "Dung tích", "[{\"value\":\"1-lit\",\"label\":\"1 L\\u00EDt\"},{\"value\":\"5-lit\",\"label\":\"5 L\\u00EDt\"},{\"value\":\"18-lit\",\"label\":\"18 L\\u00EDt\"}]", "select", true, true, 1, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5667) }
            });

        migrationBuilder.InsertData(
            table: "product_field_definitions",
            columns: new[] { "id", "created_at", "deleted_at", "field_name", "field_options", "is_active", "is_required", "product_type_id", "updated_at" },
            values: new object[] { 3, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5801), null, "Màu sắc", null, true, true, 1, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5802) });

        migrationBuilder.InsertData(
            table: "product_field_definitions",
            columns: new[] { "id", "created_at", "deleted_at", "field_name", "field_options", "field_type", "is_active", "is_required", "product_type_id", "updated_at" },
            values: new object[,]
            {
                { 4, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5804), null, "Độ bóng", "[{\"value\":\"bong\",\"label\":\"B\\u00F3ng\"},{\"value\":\"mo\",\"label\":\"M\\u1EDD\"}]", "select", true, true, 2, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5804) },
                { 5, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5888), null, "Dung tích", "[{\"value\":\"0-5-lit\",\"label\":\"0.5 L\\u00EDt\"},{\"value\":\"1-lit\",\"label\":\"1 L\\u00EDt\"},{\"value\":\"4-lit\",\"label\":\"4 L\\u00EDt\"}]", "select", true, true, 2, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5889) },
                { 6, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5986), null, "Loại bề mặt", "[{\"value\":\"go\",\"label\":\"G\\u1ED7\"},{\"value\":\"kim-loai\",\"label\":\"Kim lo\\u1EA1i\"}]", "select", true, true, 2, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5986) }
            });

        migrationBuilder.InsertData(
            table: "product_field_definitions",
            columns: new[] { "id", "created_at", "deleted_at", "field_name", "field_options", "is_active", "is_required", "product_type_id", "updated_at" },
            values: new object[] { 7, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6065), null, "Màu sắc", null, true, true, 2, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6066) });

        migrationBuilder.InsertData(
            table: "product_field_definitions",
            columns: new[] { "id", "created_at", "deleted_at", "field_name", "field_options", "field_type", "is_active", "is_required", "product_type_id", "updated_at" },
            values: new object[,]
            {
                { 8, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6067), null, "Loại chống thấm", "[{\"value\":\"tuong-dung\",\"label\":\"T\\u01B0\\u1EDDng \\u0111\\u1EE9ng\"},{\"value\":\"san-mai\",\"label\":\"S\\u00E0n m\\u00E1i\"},{\"value\":\"nha-ve-sinh\",\"label\":\"Nh\\u00E0 v\\u1EC7 sinh\"}]", "select", true, true, 7, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6068) },
                { 9, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6171), null, "Dung tích", "[{\"value\":\"1-kg\",\"label\":\"1 Kg\"},{\"value\":\"5-kg\",\"label\":\"5 Kg\"},{\"value\":\"20-kg\",\"label\":\"20 Kg\"}]", "select", true, true, 7, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6171) }
            });

        migrationBuilder.InsertData(
            table: "product_field_definitions",
            columns: new[] { "id", "created_at", "deleted_at", "field_name", "field_options", "is_active", "product_type_id", "updated_at" },
            values: new object[] { 10, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6242), null, "Màu sắc", null, true, 7, new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6242) });

        migrationBuilder.InsertData(
            table: "users",
            columns: new[] { "id", "created_at", "deleted_at", "email", "is_active", "password_hash", "role_id", "updated_at", "username" },
            values: new object[,]
            {
                { 1, new DateTime(2025, 3, 20, 10, 13, 43, 59, DateTimeKind.Utc).AddTicks(9884), null, "admin@admin.com", true, "$2a$11$Z4lGO7u7lXj7PmulY2393u3QodblRNxqbPZ/UW0JiJAZQ59OgJbpO", 1, new DateTime(2025, 3, 20, 10, 13, 43, 59, DateTimeKind.Utc).AddTicks(9886), "admin" },
                { 2, new DateTime(2025, 3, 20, 10, 13, 43, 178, DateTimeKind.Utc).AddTicks(3133), null, "user@user.com", true, "$2a$11$F/ZE0HaFXlSvfGTsguS3d.2iaTfKjfEDG4y/xxBalhYWTJK9Mzdoy", 2, new DateTime(2025, 3, 20, 10, 13, 43, 178, DateTimeKind.Utc).AddTicks(3136), "user" },
                { 3, new DateTime(2025, 3, 20, 10, 13, 43, 327, DateTimeKind.Utc).AddTicks(9359), null, "manager@manager.com", true, "$2a$11$QvcQcI3zebftm./bAIJLPuVVs6823.MDSmRfND0k0HPqmyyAp9SV6", 3, new DateTime(2025, 3, 20, 10, 13, 43, 327, DateTimeKind.Utc).AddTicks(9363), "manager" }
            });

        migrationBuilder.CreateIndex(
            name: "idx_categories_name",
            table: "categories",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_categories_parent_category_id",
            table: "categories",
            column: "parent_category_id");

        migrationBuilder.CreateIndex(
            name: "idx_comments_content_id",
            table: "comments",
            column: "content_id");

        migrationBuilder.CreateIndex(
            name: "idx_comments_parent_comment_id",
            table: "comments",
            column: "parent_comment_id");

        migrationBuilder.CreateIndex(
            name: "idx_comments_user_id",
            table: "comments",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "idx_contacts_email",
            table: "contacts",
            column: "email");

        migrationBuilder.CreateIndex(
            name: "idx_content_categories_category_id",
            table: "content_categories",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "idx_content_categories_content_id",
            table: "content_categories",
            column: "content_id");

        migrationBuilder.CreateIndex(
            name: "idx_content_field_definitions_content_type_id",
            table: "content_field_definitions",
            column: "content_type_id");

        migrationBuilder.CreateIndex(
            name: "idx_content_field_values_content_id",
            table: "content_field_values",
            column: "content_id");

        migrationBuilder.CreateIndex(
            name: "idx_content_field_values_field_id",
            table: "content_field_values",
            column: "field_id");

        migrationBuilder.CreateIndex(
            name: "idx_content_tags_content_id",
            table: "content_tags",
            column: "content_id");

        migrationBuilder.CreateIndex(
            name: "idx_content_tags_tag_id",
            table: "content_tags",
            column: "tag_id");

        migrationBuilder.CreateIndex(
            name: "idx_content_types_slug",
            table: "content_types",
            column: "slug");

        migrationBuilder.CreateIndex(
            name: "idx_contents_author_id",
            table: "contents",
            column: "author_id");

        migrationBuilder.CreateIndex(
            name: "idx_contents_content_type_id",
            table: "contents",
            column: "content_type_id");

        migrationBuilder.CreateIndex(
            name: "idx_contents_slug",
            table: "contents",
            column: "slug");

        migrationBuilder.CreateIndex(
            name: "idx_folders_path",
            table: "folders",
            column: "path");

        migrationBuilder.CreateIndex(
            name: "IX_folders_parent_id",
            table: "folders",
            column: "parent_id");

        migrationBuilder.CreateIndex(
            name: "idx_media_files_path",
            table: "media_files",
            column: "path");

        migrationBuilder.CreateIndex(
            name: "IX_media_files_folder_id",
            table: "media_files",
            column: "folder_id");

        migrationBuilder.CreateIndex(
            name: "idx_product_categories_category_id",
            table: "product_categories",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "idx_product_categories_product_id",
            table: "product_categories",
            column: "product_id");

        migrationBuilder.CreateIndex(
            name: "idx_product_field_definitions_product_type_id",
            table: "product_field_definitions",
            column: "product_type_id");

        migrationBuilder.CreateIndex(
            name: "idx_product_field_values_field_id",
            table: "product_field_values",
            column: "field_id");

        migrationBuilder.CreateIndex(
            name: "idx_product_field_values_product_id",
            table: "product_field_values",
            column: "product_id");

        migrationBuilder.CreateIndex(
            name: "idx_product_images_product_id",
            table: "product_image",
            column: "product_id");

        migrationBuilder.CreateIndex(
            name: "idx_product_tags_product_id",
            table: "product_tags",
            column: "product_id");

        migrationBuilder.CreateIndex(
            name: "idx_product_tags_tag_id",
            table: "product_tags",
            column: "tag_id");

        migrationBuilder.CreateIndex(
            name: "idx_product_types_name",
            table: "product_types",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_product_types_slug",
            table: "product_types",
            column: "slug",
            unique: true);

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
            name: "IX_products_sku",
            table: "products",
            column: "sku",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_reviews_product_id",
            table: "reviews",
            column: "product_id");

        migrationBuilder.CreateIndex(
            name: "idx_reviews_user_id",
            table: "reviews",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "idx_roles_name",
            table: "roles",
            column: "name");

        migrationBuilder.CreateIndex(
            name: "idx_settings_key",
            table: "settings",
            column: "key",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_subscriber_email",
            table: "subscriber",
            column: "email");

        migrationBuilder.CreateIndex(
            name: "idx_tags_slug",
            table: "tags",
            column: "slug");

        migrationBuilder.CreateIndex(
            name: "idx_users_email",
            table: "users",
            column: "email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "idx_users_username",
            table: "users",
            column: "username",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_users_role_id",
            table: "users",
            column: "role_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "comments");

        migrationBuilder.DropTable(
            name: "contacts");

        migrationBuilder.DropTable(
            name: "content_categories");

        migrationBuilder.DropTable(
            name: "content_field_values");

        migrationBuilder.DropTable(
            name: "content_tags");

        migrationBuilder.DropTable(
            name: "media_files");

        migrationBuilder.DropTable(
            name: "product_categories");

        migrationBuilder.DropTable(
            name: "product_field_values");

        migrationBuilder.DropTable(
            name: "product_image");

        migrationBuilder.DropTable(
            name: "product_tags");

        migrationBuilder.DropTable(
            name: "reviews");

        migrationBuilder.DropTable(
            name: "settings");

        migrationBuilder.DropTable(
            name: "slider");

        migrationBuilder.DropTable(
            name: "subscriber");

        migrationBuilder.DropTable(
            name: "content_field_definitions");

        migrationBuilder.DropTable(
            name: "contents");

        migrationBuilder.DropTable(
            name: "folders");

        migrationBuilder.DropTable(
            name: "categories");

        migrationBuilder.DropTable(
            name: "product_field_definitions");

        migrationBuilder.DropTable(
            name: "tags");

        migrationBuilder.DropTable(
            name: "products");

        migrationBuilder.DropTable(
            name: "content_types");

        migrationBuilder.DropTable(
            name: "users");

        migrationBuilder.DropTable(
            name: "product_types");

        migrationBuilder.DropTable(
            name: "roles");
    }
}

/*
 Navicat Premium Dump SQL

 Source Server         : daiminh
 Source Server Type    : PostgreSQL
 Source Server Version : 160008 (160008)
 Source Host           : localhost:5432
 Source Catalog        : daiminh
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 160008 (160008)
 File Encoding         : 65001

 Date: 24/03/2025 00:33:14
*/


-- ----------------------------
-- Table structure for products
-- ----------------------------
DROP TABLE IF EXISTS "public"."products";
CREATE TABLE "public"."products" (
  "id" int4 NOT NULL GENERATED ALWAYS AS IDENTITY (
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1
),
  "name" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "slug" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "description" text COLLATE "pg_catalog"."default" NOT NULL,
  "base_price" numeric(10,2) NOT NULL,
  "sku" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "status" varchar(20) COLLATE "pg_catalog"."default" NOT NULL DEFAULT 'draft'::character varying,
  "product_type_id" int4 NOT NULL,
  "is_active" bool NOT NULL DEFAULT true,
  "created_at" timestamptz(6) NOT NULL,
  "updated_at" timestamptz(6) NOT NULL,
  "deleted_at" timestamptz(6),
  "meta_title" varchar(255) COLLATE "pg_catalog"."default",
  "meta_description" varchar(500) COLLATE "pg_catalog"."default",
  "canonical_url" varchar(500) COLLATE "pg_catalog"."default",
  "og_title" varchar(255) COLLATE "pg_catalog"."default",
  "og_description" varchar(500) COLLATE "pg_catalog"."default",
  "og_image" varchar(500) COLLATE "pg_catalog"."default",
  "structured_data" text COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Indexes structure for table products
-- ----------------------------
CREATE INDEX "idx_products_product_type_id" ON "public"."products" USING btree (
  "product_type_id" "pg_catalog"."int4_ops" ASC NULLS LAST
);
CREATE INDEX "idx_products_sku" ON "public"."products" USING btree (
  "sku" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE INDEX "idx_products_slug" ON "public"."products" USING btree (
  "slug" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table products
-- ----------------------------
ALTER TABLE "public"."products" ADD CONSTRAINT "PK_products" PRIMARY KEY ("id");

-- ----------------------------
-- Foreign Keys structure for table products
-- ----------------------------
ALTER TABLE "public"."products" ADD CONSTRAINT "FK_products_product_types_product_type_id" FOREIGN KEY ("product_type_id") REFERENCES "public"."product_types" ("id") ON DELETE RESTRICT ON UPDATE NO ACTION;

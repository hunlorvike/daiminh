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

 Date: 24/03/2025 00:32:43
*/


-- ----------------------------
-- Table structure for product_field_definitions
-- ----------------------------
DROP TABLE IF EXISTS "public"."product_field_definitions";
CREATE TABLE "public"."product_field_definitions" (
  "id" int4 NOT NULL GENERATED ALWAYS AS IDENTITY (
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1
),
  "product_type_id" int4 NOT NULL,
  "field_name" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "field_type" varchar(20) COLLATE "pg_catalog"."default" NOT NULL DEFAULT 'text'::character varying,
  "is_required" bool NOT NULL DEFAULT false,
  "field_options" text COLLATE "pg_catalog"."default",
  "is_active" bool NOT NULL DEFAULT true,
  "created_at" timestamptz(6) NOT NULL,
  "updated_at" timestamptz(6) NOT NULL,
  "deleted_at" timestamptz(6)
)
;

-- ----------------------------
-- Indexes structure for table product_field_definitions
-- ----------------------------
CREATE INDEX "idx_product_field_definitions_product_type_id" ON "public"."product_field_definitions" USING btree (
  "product_type_id" "pg_catalog"."int4_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table product_field_definitions
-- ----------------------------
ALTER TABLE "public"."product_field_definitions" ADD CONSTRAINT "PK_product_field_definitions" PRIMARY KEY ("id");

-- ----------------------------
-- Foreign Keys structure for table product_field_definitions
-- ----------------------------
ALTER TABLE "public"."product_field_definitions" ADD CONSTRAINT "FK_product_field_definitions_product_types_product_type_id" FOREIGN KEY ("product_type_id") REFERENCES "public"."product_types" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;

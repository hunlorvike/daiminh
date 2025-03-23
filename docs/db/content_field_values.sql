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

 Date: 24/03/2025 00:31:58
*/


-- ----------------------------
-- Table structure for content_field_values
-- ----------------------------
DROP TABLE IF EXISTS "public"."content_field_values";
CREATE TABLE "public"."content_field_values" (
  "id" int4 NOT NULL GENERATED ALWAYS AS IDENTITY (
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1
),
  "content_id" int4 NOT NULL,
  "field_id" int4 NOT NULL,
  "value" text COLLATE "pg_catalog"."default" NOT NULL,
  "is_active" bool NOT NULL DEFAULT true,
  "created_at" timestamptz(6) NOT NULL,
  "updated_at" timestamptz(6) NOT NULL,
  "deleted_at" timestamptz(6)
)
;

-- ----------------------------
-- Indexes structure for table content_field_values
-- ----------------------------
CREATE INDEX "idx_content_field_values_content_id" ON "public"."content_field_values" USING btree (
  "content_id" "pg_catalog"."int4_ops" ASC NULLS LAST
);
CREATE INDEX "idx_content_field_values_field_id" ON "public"."content_field_values" USING btree (
  "field_id" "pg_catalog"."int4_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table content_field_values
-- ----------------------------
ALTER TABLE "public"."content_field_values" ADD CONSTRAINT "PK_content_field_values" PRIMARY KEY ("id");

-- ----------------------------
-- Foreign Keys structure for table content_field_values
-- ----------------------------
ALTER TABLE "public"."content_field_values" ADD CONSTRAINT "FK_content_field_values_content_field_definitions_field_id" FOREIGN KEY ("field_id") REFERENCES "public"."content_field_definitions" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;
ALTER TABLE "public"."content_field_values" ADD CONSTRAINT "FK_content_field_values_contents_content_id" FOREIGN KEY ("content_id") REFERENCES "public"."contents" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;

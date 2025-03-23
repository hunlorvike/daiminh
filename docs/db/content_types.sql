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

 Date: 24/03/2025 00:32:14
*/


-- ----------------------------
-- Table structure for content_types
-- ----------------------------
DROP TABLE IF EXISTS "public"."content_types";
CREATE TABLE "public"."content_types" (
  "id" int4 NOT NULL GENERATED ALWAYS AS IDENTITY (
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1
),
  "name" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "slug" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "is_active" bool NOT NULL DEFAULT true,
  "created_at" timestamptz(6) NOT NULL,
  "updated_at" timestamptz(6) NOT NULL,
  "deleted_at" timestamptz(6)
)
;

-- ----------------------------
-- Indexes structure for table content_types
-- ----------------------------
CREATE INDEX "idx_content_types_slug" ON "public"."content_types" USING btree (
  "slug" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table content_types
-- ----------------------------
ALTER TABLE "public"."content_types" ADD CONSTRAINT "PK_content_types" PRIMARY KEY ("id");

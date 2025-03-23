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

 Date: 24/03/2025 00:31:43
*/


-- ----------------------------
-- Table structure for content_categories
-- ----------------------------
DROP TABLE IF EXISTS "public"."content_categories";
CREATE TABLE "public"."content_categories" (
  "content_id" int4 NOT NULL,
  "category_id" int4 NOT NULL,
  "is_active" bool NOT NULL DEFAULT true,
  "created_at" timestamptz(6) NOT NULL,
  "updated_at" timestamptz(6) NOT NULL,
  "deleted_at" timestamptz(6)
)
;

-- ----------------------------
-- Indexes structure for table content_categories
-- ----------------------------
CREATE INDEX "idx_content_categories_category_id" ON "public"."content_categories" USING btree (
  "category_id" "pg_catalog"."int4_ops" ASC NULLS LAST
);
CREATE INDEX "idx_content_categories_content_id" ON "public"."content_categories" USING btree (
  "content_id" "pg_catalog"."int4_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table content_categories
-- ----------------------------
ALTER TABLE "public"."content_categories" ADD CONSTRAINT "PK_content_categories" PRIMARY KEY ("content_id", "category_id");

-- ----------------------------
-- Foreign Keys structure for table content_categories
-- ----------------------------
ALTER TABLE "public"."content_categories" ADD CONSTRAINT "FK_content_categories_categories_category_id" FOREIGN KEY ("category_id") REFERENCES "public"."categories" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;
ALTER TABLE "public"."content_categories" ADD CONSTRAINT "FK_content_categories_contents_content_id" FOREIGN KEY ("content_id") REFERENCES "public"."contents" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;

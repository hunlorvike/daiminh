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

 Date: 24/03/2025 00:32:05
*/


-- ----------------------------
-- Table structure for content_tags
-- ----------------------------
DROP TABLE IF EXISTS "public"."content_tags";
CREATE TABLE "public"."content_tags" (
  "content_id" int4 NOT NULL,
  "tag_id" int4 NOT NULL,
  "is_active" bool NOT NULL DEFAULT true,
  "created_at" timestamptz(6) NOT NULL,
  "updated_at" timestamptz(6) NOT NULL,
  "deleted_at" timestamptz(6)
)
;

-- ----------------------------
-- Indexes structure for table content_tags
-- ----------------------------
CREATE INDEX "idx_content_tags_content_id" ON "public"."content_tags" USING btree (
  "content_id" "pg_catalog"."int4_ops" ASC NULLS LAST
);
CREATE INDEX "idx_content_tags_tag_id" ON "public"."content_tags" USING btree (
  "tag_id" "pg_catalog"."int4_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table content_tags
-- ----------------------------
ALTER TABLE "public"."content_tags" ADD CONSTRAINT "PK_content_tags" PRIMARY KEY ("content_id", "tag_id");

-- ----------------------------
-- Foreign Keys structure for table content_tags
-- ----------------------------
ALTER TABLE "public"."content_tags" ADD CONSTRAINT "FK_content_tags_contents_content_id" FOREIGN KEY ("content_id") REFERENCES "public"."contents" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;
ALTER TABLE "public"."content_tags" ADD CONSTRAINT "FK_content_tags_tags_tag_id" FOREIGN KEY ("tag_id") REFERENCES "public"."tags" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;

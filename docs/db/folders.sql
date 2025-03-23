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

 Date: 24/03/2025 00:32:25
*/


-- ----------------------------
-- Table structure for folders
-- ----------------------------
DROP TABLE IF EXISTS "public"."folders";
CREATE TABLE "public"."folders" (
  "id" int4 NOT NULL GENERATED ALWAYS AS IDENTITY (
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1
),
  "name" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "path" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "parent_id" int4,
  "is_active" bool NOT NULL DEFAULT true,
  "created_at" timestamptz(6) NOT NULL,
  "updated_at" timestamptz(6) NOT NULL,
  "deleted_at" timestamptz(6)
)
;

-- ----------------------------
-- Indexes structure for table folders
-- ----------------------------
CREATE INDEX "IX_folders_parent_id" ON "public"."folders" USING btree (
  "parent_id" "pg_catalog"."int4_ops" ASC NULLS LAST
);
CREATE INDEX "idx_folders_path" ON "public"."folders" USING btree (
  "path" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table folders
-- ----------------------------
ALTER TABLE "public"."folders" ADD CONSTRAINT "PK_folders" PRIMARY KEY ("id");

-- ----------------------------
-- Foreign Keys structure for table folders
-- ----------------------------
ALTER TABLE "public"."folders" ADD CONSTRAINT "fk_folders_parent_id" FOREIGN KEY ("parent_id") REFERENCES "public"."folders" ("id") ON DELETE SET NULL ON UPDATE NO ACTION;

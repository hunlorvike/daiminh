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

 Date: 24/03/2025 00:32:32
*/


-- ----------------------------
-- Table structure for media_files
-- ----------------------------
DROP TABLE IF EXISTS "public"."media_files";
CREATE TABLE "public"."media_files" (
  "id" int4 NOT NULL GENERATED ALWAYS AS IDENTITY (
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1
),
  "name" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "path" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "url" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "mime_type" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "size" int8 NOT NULL,
  "extension" varchar(10) COLLATE "pg_catalog"."default" NOT NULL,
  "folder_id" int4,
  "is_active" bool NOT NULL DEFAULT true,
  "created_at" timestamptz(6) NOT NULL,
  "updated_at" timestamptz(6) NOT NULL,
  "deleted_at" timestamptz(6)
)
;

-- ----------------------------
-- Indexes structure for table media_files
-- ----------------------------
CREATE INDEX "IX_media_files_folder_id" ON "public"."media_files" USING btree (
  "folder_id" "pg_catalog"."int4_ops" ASC NULLS LAST
);
CREATE INDEX "idx_media_files_path" ON "public"."media_files" USING btree (
  "path" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table media_files
-- ----------------------------
ALTER TABLE "public"."media_files" ADD CONSTRAINT "PK_media_files" PRIMARY KEY ("id");

-- ----------------------------
-- Foreign Keys structure for table media_files
-- ----------------------------
ALTER TABLE "public"."media_files" ADD CONSTRAINT "fk_media_files_folder_id" FOREIGN KEY ("folder_id") REFERENCES "public"."folders" ("id") ON DELETE SET NULL ON UPDATE NO ACTION;

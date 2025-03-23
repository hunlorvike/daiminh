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

 Date: 24/03/2025 00:32:20
*/


-- ----------------------------
-- Table structure for contents
-- ----------------------------
DROP TABLE IF EXISTS "public"."contents";
CREATE TABLE "public"."contents" (
  "id" int4 NOT NULL GENERATED ALWAYS AS IDENTITY (
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1
),
  "content_type_id" int4 NOT NULL,
  "author_id" int4,
  "title" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "slug" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "content_body" text COLLATE "pg_catalog"."default" NOT NULL,
  "cover_image_url" varchar(500) COLLATE "pg_catalog"."default",
  "status" varchar(20) COLLATE "pg_catalog"."default" NOT NULL DEFAULT 'draft'::character varying,
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
  "structured_data" text COLLATE "pg_catalog"."default",
  "summary" varchar(500) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying
)
;

-- ----------------------------
-- Indexes structure for table contents
-- ----------------------------
CREATE INDEX "idx_contents_author_id" ON "public"."contents" USING btree (
  "author_id" "pg_catalog"."int4_ops" ASC NULLS LAST
);
CREATE INDEX "idx_contents_content_type_id" ON "public"."contents" USING btree (
  "content_type_id" "pg_catalog"."int4_ops" ASC NULLS LAST
);
CREATE INDEX "idx_contents_slug" ON "public"."contents" USING btree (
  "slug" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table contents
-- ----------------------------
ALTER TABLE "public"."contents" ADD CONSTRAINT "PK_contents" PRIMARY KEY ("id");

-- ----------------------------
-- Foreign Keys structure for table contents
-- ----------------------------
ALTER TABLE "public"."contents" ADD CONSTRAINT "FK_contents_content_types_content_type_id" FOREIGN KEY ("content_type_id") REFERENCES "public"."content_types" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;
ALTER TABLE "public"."contents" ADD CONSTRAINT "FK_contents_users_author_id" FOREIGN KEY ("author_id") REFERENCES "public"."users" ("id") ON DELETE SET NULL ON UPDATE NO ACTION;

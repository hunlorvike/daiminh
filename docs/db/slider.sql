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

 Date: 24/03/2025 00:33:39
*/


-- ----------------------------
-- Table structure for slider
-- ----------------------------
DROP TABLE IF EXISTS "public"."slider";
CREATE TABLE "public"."slider" (
  "id" int4 NOT NULL GENERATED ALWAYS AS IDENTITY (
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1
),
  "title" text COLLATE "pg_catalog"."default" NOT NULL,
  "image_url" text COLLATE "pg_catalog"."default" NOT NULL,
  "link_url" text COLLATE "pg_catalog"."default",
  "order_number" int4 NOT NULL,
  "overlay_html" text COLLATE "pg_catalog"."default",
  "overlay_position" text COLLATE "pg_catalog"."default",
  "is_active" bool NOT NULL DEFAULT true,
  "created_at" timestamptz(6) NOT NULL,
  "updated_at" timestamptz(6) NOT NULL,
  "deleted_at" timestamptz(6)
)
;

-- ----------------------------
-- Primary Key structure for table slider
-- ----------------------------
ALTER TABLE "public"."slider" ADD CONSTRAINT "PK_slider" PRIMARY KEY ("id");

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

 Date: 24/03/2025 00:32:55
*/


-- ----------------------------
-- Table structure for product_image
-- ----------------------------
DROP TABLE IF EXISTS "public"."product_image";
CREATE TABLE "public"."product_image" (
  "id" int4 NOT NULL GENERATED ALWAYS AS IDENTITY (
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1
),
  "product_id" int4 NOT NULL,
  "image_url" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "alt_text" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "is_primary" bool NOT NULL DEFAULT false,
  "display_order" int2 NOT NULL,
  "is_active" bool NOT NULL DEFAULT true,
  "created_at" timestamptz(6) NOT NULL,
  "updated_at" timestamptz(6) NOT NULL,
  "deleted_at" timestamptz(6)
)
;

-- ----------------------------
-- Indexes structure for table product_image
-- ----------------------------
CREATE INDEX "idx_product_images_product_id" ON "public"."product_image" USING btree (
  "product_id" "pg_catalog"."int4_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table product_image
-- ----------------------------
ALTER TABLE "public"."product_image" ADD CONSTRAINT "PK_product_image" PRIMARY KEY ("id");

-- ----------------------------
-- Foreign Keys structure for table product_image
-- ----------------------------
ALTER TABLE "public"."product_image" ADD CONSTRAINT "FK_product_image_products_product_id" FOREIGN KEY ("product_id") REFERENCES "public"."products" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;

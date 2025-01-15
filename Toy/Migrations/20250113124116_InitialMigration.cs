using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Toy.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Brand__3213E83FE6054E4B", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__3213E83F0D2C2E43", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Country_Producer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Country___3213E83F74FA6766", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__3213E83F79CE17D9", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Phone_Me",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    time = table.Column<TimeOnly>(type: "time", nullable: true),
                    date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Phone_Me__3213E83F0BBFF5E0", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    photo_url = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    is_Main = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Photo__3213E83FEC759EFE", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Unit__3213E83F185D2DF3", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    surname = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    password = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__3213E83F1949C92D", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Photo_Brand",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    brand_id = table.Column<int>(type: "int", nullable: false),
                    photo_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Photo_Br__3213E83F7351D051", x => x.id);
                    table.ForeignKey(
                        name: "FK__Photo_Bra__brand__31B762FC",
                        column: x => x.brand_id,
                        principalTable: "Brand",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Photo_Bra__photo__367C1819",
                        column: x => x.photo_id,
                        principalTable: "Photo",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value = table.Column<decimal>(type: "money", nullable: false),
                    unit_id = table.Column<int>(type: "int", nullable: false),
                    date_time_start = table.Column<DateTime>(type: "datetime", nullable: false),
                    date_time_end = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Discount__3213E83F381D3CA2", x => x.id);
                    table.ForeignKey(
                        name: "FK__Discount__unit_i__6D0D32F4",
                        column: x => x.unit_id,
                        principalTable: "Unit",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Packaging",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(55)", unicode: false, maxLength: 55, nullable: false),
                    length = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    width = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    hight = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    unit_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Packagin__3213E83F6AF0CE2F", x => x.id);
                    table.ForeignKey(
                        name: "FK__Packaging__unit___5070F446",
                        column: x => x.unit_id,
                        principalTable: "Unit",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Product_On_Order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product___3213E83F26B4D262", x => x.id);
                    table.ForeignKey(
                        name: "FK__Product_O__user___4BAC3F29",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    age_from = table.Column<byte>(type: "tinyint", nullable: false),
                    age_to = table.Column<byte>(type: "tinyint", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    price = table.Column<decimal>(type: "money", nullable: false),
                    brand_id = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    amount = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    country_producer_id = table.Column<int>(type: "int", nullable: true),
                    material_id = table.Column<int>(type: "int", nullable: true),
                    packaging_id = table.Column<int>(type: "int", nullable: true),
                    sex = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    weight = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    weight_unit_id = table.Column<int>(type: "int", nullable: true),
                    size = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    size_unit_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product__3213E83F29E5D73F", x => x.id);
                    table.ForeignKey(
                        name: "FK__Product__brand_i__5EBF139D",
                        column: x => x.brand_id,
                        principalTable: "Brand",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK__Product__categor__5DCAEF64",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK__Product__country__60A75C0F",
                        column: x => x.country_producer_id,
                        principalTable: "Country_Producer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK__Product__materia__619B8048",
                        column: x => x.material_id,
                        principalTable: "Material",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK__Product__packagi__628FA481",
                        column: x => x.packaging_id,
                        principalTable: "Packaging",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK__Product__size_un__656C112C",
                        column: x => x.size_unit_id,
                        principalTable: "Unit",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Product__weight___6477ECF3",
                        column: x => x.weight_unit_id,
                        principalTable: "Unit",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Photo_Product_On_Order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_on_order_id = table.Column<int>(type: "int", nullable: false),
                    photo_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Photo_Pr__3213E83F18BB2931", x => x.id);
                    table.ForeignKey(
                        name: "FK__Photo_Pro__photo__37703C52",
                        column: x => x.photo_id,
                        principalTable: "Photo",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Photo_Pro__produ__3587F3E0",
                        column: x => x.product_on_order_id,
                        principalTable: "Product_On_Order",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Basket",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Basket__3213E83F3C6DEFC0", x => x.id);
                    table.ForeignKey(
                        name: "FK__Basket__product___76969D2E",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Basket__user_id__778AC167",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photo_Product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    photo_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Photo_Pr__3213E83F9D86A2EC", x => x.id);
                    table.ForeignKey(
                        name: "FK__Photo_Pro__photo__2A164134",
                        column: x => x.photo_id,
                        principalTable: "Photo",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Photo_Pro__produ__29221CFB",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Product_Discount",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    discount_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product___3213E83F57FDE2A5", x => x.id);
                    table.ForeignKey(
                        name: "FK__Product_D__categ__70DDC3D8",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Product_D__disco__71D1E811",
                        column: x => x.discount_id,
                        principalTable: "Discount",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Product_D__produ__6FE99F9F",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchase_History",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    price = table.Column<decimal>(type: "money", nullable: false),
                    amount = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    payment_status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__3213E83F828CB065", x => x.id);
                    table.ForeignKey(
                        name: "FK__Purchase___produ__68487DD7",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Purchase___user___693CA210",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    mark = table.Column<byte>(type: "tinyint", nullable: false),
                    advantages = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    disadvantages = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Review__3213E83F024CA29D", x => x.id);
                    table.ForeignKey(
                        name: "FK__Review__product___7B5B524B",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Review__user_id__151B244E",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Purchase_History_Product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    purchase_history_id = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "money", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__3213E83FC92FBBB7", x => x.id);
                    table.ForeignKey(
                        name: "FK__Purchase___produ__3F115E1A",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Purchase___purch__40058253",
                        column: x => x.purchase_history_id,
                        principalTable: "Purchase_History",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Photo_Review",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    review_id = table.Column<int>(type: "int", nullable: false),
                    photo_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Photo_Re__3213E83F745F52A3", x => x.id);
                    table.ForeignKey(
                        name: "FK__Photo_Rev__photo__3864608B",
                        column: x => x.photo_id,
                        principalTable: "Photo",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Photo_Rev__revie__2DE6D218",
                        column: x => x.review_id,
                        principalTable: "Review",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Basket_product_id",
                table: "Basket",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Basket_user_id",
                table: "Basket",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_unit_id",
                table: "Discount",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Packaging_unit_id",
                table: "Packaging",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_Brand_brand_id",
                table: "Photo_Brand",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_Brand_photo_id",
                table: "Photo_Brand",
                column: "photo_id");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_Product_photo_id",
                table: "Photo_Product",
                column: "photo_id");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_Product_product_id",
                table: "Photo_Product",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_Product_On_Order_photo_id",
                table: "Photo_Product_On_Order",
                column: "photo_id");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_Product_On_Order_product_on_order_id",
                table: "Photo_Product_On_Order",
                column: "product_on_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_Review_photo_id",
                table: "Photo_Review",
                column: "photo_id");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_Review_review_id",
                table: "Photo_Review",
                column: "review_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_brand_id",
                table: "Product",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_category_id",
                table: "Product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_country_producer_id",
                table: "Product",
                column: "country_producer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_material_id",
                table: "Product",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_packaging_id",
                table: "Product",
                column: "packaging_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_size_unit_id",
                table: "Product",
                column: "size_unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_weight_unit_id",
                table: "Product",
                column: "weight_unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Discount_category_id",
                table: "Product_Discount",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Discount_discount_id",
                table: "Product_Discount",
                column: "discount_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Discount_product_id",
                table: "Product_Discount",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_On_Order_user_id",
                table: "Product_On_Order",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_History_product_id",
                table: "Purchase_History",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_History_user_id",
                table: "Purchase_History",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_History_Product_product_id",
                table: "Purchase_History_Product",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_History_Product_purchase_history_id",
                table: "Purchase_History_Product",
                column: "purchase_history_id");

            migrationBuilder.CreateIndex(
                name: "IX_Review_product_id",
                table: "Review",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Review_user_id",
                table: "Review",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__User__AB6E616480355481",
                table: "User",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__User__B43B145F916BF3D6",
                table: "User",
                column: "phone",
                unique: true,
                filter: "[phone] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Basket");

            migrationBuilder.DropTable(
                name: "Phone_Me");

            migrationBuilder.DropTable(
                name: "Photo_Brand");

            migrationBuilder.DropTable(
                name: "Photo_Product");

            migrationBuilder.DropTable(
                name: "Photo_Product_On_Order");

            migrationBuilder.DropTable(
                name: "Photo_Review");

            migrationBuilder.DropTable(
                name: "Product_Discount");

            migrationBuilder.DropTable(
                name: "Purchase_History_Product");

            migrationBuilder.DropTable(
                name: "Product_On_Order");

            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropTable(
                name: "Purchase_History");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Country_Producer");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Packaging");

            migrationBuilder.DropTable(
                name: "Unit");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcommerceLifestyle.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Level = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Source = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Message = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    RequestPath = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Exception = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Shipping = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ShipFirstName = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipLastName = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipLine1 = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipCity = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipState = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipZip = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipCountry = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Subcategory = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    ReviewCount = table.Column<int>(type: "int", nullable: false),
                    Badge = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Image = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InStock = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Users_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QuantityAvailable = table.Column<int>(type: "int", nullable: false),
                    ReorderLevel = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventory_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventory_Users_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Badge", "Description", "Image", "InStock", "Name", "OriginalPrice", "Price", "Rating", "ReviewCount", "Slug", "Subcategory", "VendorId" },
                values: new object[,]
                {
                    { "bs-001", "Hot", "Impeccably tailored two-piece suit in super-120 navy wool. Slim fit with notch lapels, a single-breasted two-button closure, and a half canvas construction for natural drape. Includes matching flat-front trousers.", "https://images.unsplash.com/photo-1617127365659-c47fa9d30294?w=600&h=700&fit=crop", true, "Slim-Fit 2-Piece Navy Suit", 13500m, 9999m, 4.9m, 678, "slim-fit-2-piece-navy-suit", "blazer-suits", null },
                    { "bs-002", "Sale", "Sophisticated charcoal suit with a subtle white windowpane check pattern woven in Italian wool. Modern slim cut, double-vented jacket, and tapered trousers. A versatile power suit for executives and groomsmen alike.", "https://images.unsplash.com/photo-1593030761757-71fae45fa0e7?w=600&h=700&fit=crop", true, "Charcoal Windowpane Check Suit", 15000m, 11499m, 4.8m, 312, "charcoal-windowpane-check-suit", "blazer-suits", null },
                    { "bs-003", "New", "Lightweight unstructured blazer in a breathable cotton-linen blend. No shoulder padding means a relaxed, easy fit -- dress it up with chinos or down with jeans. Available in off-white and khaki.", "https://images.unsplash.com/photo-1507679799987-c73779587ccf?w=600&h=700&fit=crop", true, "Smart Casual Unstructured Blazer", null, 4499m, 4.6m, 891, "smart-casual-unstructured-blazer", "blazer-suits", null },
                    { "bs-004", "Premium", "Bold double-breasted suit in classic chalk-stripe navy flannel. Peak lapels, six-button front (fastened on two), and ticket pocket add old-school elegance. For the man who dresses to impress.", "https://images.unsplash.com/photo-1598032895397-b9472444bf93?w=600&h=700&fit=crop", true, "Double-Breasted Pinstripe Suit", 18000m, 13999m, 4.9m, 145, "double-breasted-pinstripe-suit", "blazer-suits", null },
                    { "bs-005", null, "Versatile olive-green blazer cut from a wrinkle-resistant stretch wool blend. Four-way stretch fabric allows for full range of motion. Dress it over a turtleneck or wear solo over dark jeans for weekend events.", "https://images.unsplash.com/photo-1609957372782-aa0ab2f8eed8?w=600&h=700&fit=crop", true, "Olive Green Stretch Blazer", 4999m, 3799m, 4.5m, 567, "olive-green-stretch-blazer", "blazer-suits", null },
                    { "bs-006", "Sale", "Complete ivory three-piece suit -- jacket, waistcoat, and trousers -- in premium Italian wool-silk blend. Slim lapels with a contrast ivory satin and mother-of-pearl buttons. The dream groom or cocktail suit.", "https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?w=600&h=700&fit=crop", true, "3-Piece Wedding Suit -- Ivory", 22000m, 16999m, 4.9m, 98, "3-piece-wedding-suit-ivory", "blazer-suits", null },
                    { "cs-001", null, "180 GSM combed cotton jersey tee with a reinforced crew neck that keeps its shape wash after wash. Pre-shrunk and available in 12 colours. The everyday staple that works under flannels or solo.", "https://images.unsplash.com/photo-1552374196-c4e7ffc6e126?w=600&h=700&fit=crop", true, "Classic Fit Crew-Neck Tee", null, 499m, 4.6m, 3412, "classic-fit-crew-neck-tee", "casual", null },
                    { "cs-002", "Sale", "Four-way stretch denim with 2% elastane for unrestricted movement. Slim leg, mid-rise, with classic five-pocket construction. Fade-resistant indigo wash that gets better with every wear.", "https://images.unsplash.com/photo-1542272604-787c3835535d?w=600&h=700&fit=crop", true, "Slim Stretch Jeans -- Indigo", 2299m, 1799m, 4.5m, 2876, "slim-stretch-jeans-indigo", "casual", null },
                    { "cs-003", "New", "Soft-brushed cotton flannel in a warm red plaid. Oversized silhouette is great as a layering shirt over tees or worn open like a jacket. Drop shoulders and chest pockets complete the look.", "https://images.unsplash.com/photo-1512138664757-360e0aad5132?w=600&h=700&fit=crop", true, "Oversized Flannel Shirt", null, 1299m, 4.7m, 1234, "oversized-flannel-shirt", "casual", null },
                    { "cs-004", null, "Relaxed-fit cargo shorts in durable olive ripstop cotton. Six pockets (including thigh cargo pockets) give you plenty of carry space. Elasticated waistband with a drawstring -- ideal for weekends and travel.", "https://images.unsplash.com/photo-1591195853828-11db59a44f43?w=600&h=700&fit=crop", true, "Cargo Shorts -- Olive", 1399m, 999m, 4.4m, 1789, "cargo-shorts-olive", "casual", null },
                    { "cs-005", "Hot", "320 GSM fleece-lined hoodie with a full-length YKK zip and kangaroo pocket. Ribbed cuffs and hem lock in warmth. The go-to throw-on for morning gym sessions, college campuses, and chill evenings.", "https://images.unsplash.com/photo-1556821840-3a63f15732ce?w=600&h=700&fit=crop", true, "Zip-Up Hoodie -- Charcoal", 1999m, 1599m, 4.8m, 2201, "zip-up-hoodie-charcoal", "casual", null },
                    { "cs-006", null, "Airy 100% linen trousers with an elasticated drawstring waist and tapered ankle. The natural fibre breathes beautifully -- perfect for hot days, beach trips, and relaxed cafe mornings.", "https://images.unsplash.com/photo-1506634572416-48cdfe9e6b6f?w=600&h=700&fit=crop", true, "Linen Drawstring Trousers", null, 1149m, 4.3m, 845, "linen-drawstring-trousers", "casual", null },
                    { "ew-001", "Sale", "Timeless white cotton kurta with subtle self-stripe texture. Breathable fabric keeps you comfortable during long pooja sessions or Eid celebrations. Comes with matching straight-cut pajama.", "https://images.unsplash.com/photo-1583391099995-d4adde10c1e4?w=600&h=700&fit=crop", true, "Classic White Kurta Pajama", 1799m, 1299m, 4.6m, 1023, "classic-white-kurta-pajama", "ethnic-wear", null },
                    { "ew-002", "Hot", "Resplendent ivory sherwani with gold zari embroidery throughout the yoke and cuffs. Includes churidar pants and a matching brocade dupatta. The ideal groom's look for wedding ceremonies.", "https://images.unsplash.com/photo-1585386959984-a4155224a1ad?w=600&h=700&fit=crop", true, "Royal Sherwani with Dupatta", 14000m, 9999m, 4.9m, 342, "royal-sherwani-with-dupatta", "ethnic-wear", null },
                    { "ew-003", "New", "Breathable linen kurta in earthy terracotta with pintuck detailing on the placket. Paired with voluminous Patiala salwar -- a smart, breezy choice for summer festivals and cultural events.", "https://images.unsplash.com/photo-1612902456551-b67dd9fd54fc?w=600&h=700&fit=crop", true, "Linen Kurta with Patiala Salwar", null, 1699m, 4.5m, 567, "linen-kurta-patiala-salwar", "ethnic-wear", null },
                    { "ew-004", null, "Floor-length anarkali-style kurta for men in rich teal with block-printed borders. Flared silhouette with quarter sleeves. Comes with off-white straight-cut trousers for a balanced contrast.", "https://images.unsplash.com/photo-1607827448299-a099b845f076?w=600&h=700&fit=crop", true, "Anarkali Kurta with Straight Pants", 2999m, 2299m, 4.4m, 215, "anarkali-kurta-straight-pants", "ethnic-wear", null },
                    { "ew-005", "Sale", "Vibrant Bandhani (tie-dye) print kurta in cotton cambric fabric. The bright polka-dot resist-dye pattern celebrates Rajasthani craft. A go-to pick for Navratri, Holi, and cultural programs.", "https://images.unsplash.com/photo-1602810318383-e386cc2a3ccf?w=600&h=700&fit=crop", true, "Bandhani Print Kurta", 1499m, 999m, 4.3m, 892, "bandhani-print-kurta", "ethnic-wear", null },
                    { "ew-006", null, "Ruggedly handsome Pathani salwar kameez in olive-green cotton-linen blend. Mandarin collar, full sleeves, and deep side slits offer movement and comfort. Pair with mojris for a complete traditional look.", "https://images.unsplash.com/photo-1620912189866-c4cf63048e63?w=600&h=700&fit=crop", true, "Pathani Suit -- Olive Green", null, 1849m, 4.7m, 441, "pathani-suit-olive-green", "ethnic-wear", null },
                    { "fk-001", "Hot", "Hand-dyed spiral tie-dye tee in electric blue and lime green. 240 GSM heavyweight cotton with a boxy, oversized drop-shoulder cut. Each piece is uniquely dyed -- no two are exactly alike. #OOTD guaranteed.", "https://images.unsplash.com/photo-1523398002811-999ca8dec234?w=600&h=700&fit=crop", true, "Tie-Dye Oversized Tee", null, 699m, 4.6m, 2345, "tie-dye-oversized-tee", "funky", null },
                    { "fk-002", "Sale", "Y2K-inspired cargo pants featuring a multi-panel patchwork of contrasting fabrics and colours. Low-rise waist, wide-leg silhouette, and six utility pockets. The street-style statement piece that breaks all the rules.", "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?w=600&h=700&fit=crop", true, "Patchwork Cargo Pants", 2999m, 2199m, 4.5m, 1123, "patchwork-cargo-pants", "funky", null },
                    { "fk-003", "New", "Nylon bomber jacket with an all-over urban graffiti print. Ribbed collar, cuffs, and hem with a clean-zip front. Lightweight and windproof -- the perfect throw-on for festivals, skate parks, and late-night outings.", "https://images.unsplash.com/photo-1529374255404-311a2a4f1fd9?w=600&h=700&fit=crop", true, "Graffiti Print Bomber Jacket", 4499m, 3499m, 4.8m, 876, "graffiti-print-bomber-jacket", "funky", null },
                    { "fk-004", null, "Black-and-white checkerboard co-ord set: an oversized short-sleeve shirt and matching wide-leg shorts. 90s skater aesthetic meets modern streetwear. Wear the full set or mix individually for infinite looks.", "https://images.unsplash.com/photo-1434389677669-e08b4cac3105?w=600&h=700&fit=crop", true, "Checkerboard Vans-Style Co-Ord", 3499m, 2799m, 4.4m, 654, "checkerboard-coord-set", "funky", null },
                    { "fk-005", null, "Semi-transparent neon-green mesh top designed for layering over coloured tees or bralettes. Dropped shoulders, relaxed crew neck, and ribbed hem. Bold, boundary-pushing festival-wear for the unapologetically loud.", "https://images.unsplash.com/photo-1496747611176-843222e1e57c?w=600&h=700&fit=crop", true, "Neon Mesh Layering Top", null, 799m, 4.2m, 432, "neon-mesh-layering-top", "funky", null },
                    { "fk-006", "Hot", "Soft cotton-terry joggers covered in retro cartoon characters from the 90s. Elasticated waistband with drawstring, cuffed ankles, and deep side pockets. Cosy, fun, and wildly shareable on social media.", "https://images.unsplash.com/photo-1516762689617-e1cffcef479d?w=600&h=700&fit=crop", true, "Cartoon Print Relaxed Joggers", 1799m, 1399m, 4.7m, 1543, "cartoon-print-relaxed-joggers", "funky", null },
                    { "fm-001", "Sale", "100% Egyptian cotton with a 2-ply weave for exceptional breathability and a smooth drape. French placket, barrel cuffs, and a slim fit through the chest and waist. Wrinkle-resistant -- perfect for long office days.", "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=600&h=700&fit=crop", true, "Egyptian Cotton Dress Shirt -- White", 1999m, 1499m, 4.7m, 2134, "egyptian-cotton-dress-shirt-white", "formal", null },
                    { "fm-002", null, "Italian-inspired slim-fit trousers in a lightweight charcoal wool blend. Mid-rise waist, slash pockets, and a flat front give a polished, contemporary silhouette. Machine washable at 30C.", "https://images.unsplash.com/photo-1489987707025-afc232f7ea0f?w=600&h=700&fit=crop", true, "Slim-Fit Wool Blend Trousers", 2999m, 2199m, 4.5m, 987, "slim-fit-wool-blend-trousers", "formal", null },
                    { "fm-003", "New", "Classic alternating-stripe Oxford cloth shirt in navy and white. Button-down collar holds its shape without a tie pin. Relaxed fit gives room through the shoulders for comfortable, all-day wear.", "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?w=600&h=700&fit=crop", true, "Oxford Striped Business Shirt", null, 1249m, 4.4m, 1456, "oxford-striped-business-shirt", "formal", null },
                    { "fm-004", "Sale", "Elevate any formal look with this microfibre silk-blend tie in midnight blue paired with a matching white pocket square. Pre-tied option available. Ideal corporate gift for new joiners.", "https://images.unsplash.com/photo-1598522325074-042db73aa4e6?w=600&h=700&fit=crop", true, "Formal Tie & Pocket Square Set", 1199m, 799m, 4.6m, 734, "formal-tie-pocket-square-set", "formal", null },
                    { "fm-005", "Hot", "Heritage glen-plaid blazer in grey and navy. Structured shoulders, notch lapels, and a two-button single-breast closure. The perfect blazer to wear over a dress shirt for boardroom meetings.", "https://images.unsplash.com/photo-1549062572-544a64fb0c56?w=600&h=700&fit=crop", true, "Checked Formal Blazer", 5499m, 3999m, 4.8m, 523, "checked-formal-blazer", "formal", null },
                    { "fm-006", null, "Genuine leather derby shoes with a cap toe and Goodyear welt construction. Cushioned insole for all-day comfort. Pairs flawlessly with trousers and formal suits for office or client meetings.", "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=600&h=700&fit=crop", false, "Formal Derby Shoes -- Black", 3499m, 2799m, 4.5m, 612, "formal-derby-shoes-black", "formal", null },
                    { "pw-001", "Sale", "Make a grand entrance with this midnight-black sequin blazer. Slim-fit cut with satin lapels, perfect for club nights and festive dinners. Pairs beautifully with black trousers or dark jeans.", "https://images.unsplash.com/photo-1594938298603-c8148c4b4ae1?w=600&h=700&fit=crop", true, "Midnight Sequin Blazer", 4999m, 3499m, 4.7m, 312, "midnight-sequin-blazer", "party-wear", null },
                    { "pw-002", "Hot", "A stunning fusion of contemporary style and traditional Indian craftsmanship. Rich brocade Nehru jacket paired with slim churidar trousers. Ideal for cocktail parties, receptions, and festive occasions.", "https://images.unsplash.com/photo-1507679799987-c73779587ccf?w=600&h=700&fit=crop", true, "Indo-Western Nehru Jacket Set", 5500m, 4199m, 4.8m, 489, "indo-western-nehru-jacket-set", "party-wear", null },
                    { "pw-003", "Premium", "Luxurious deep-burgundy velvet tuxedo that commands attention. Shawl lapel design with satin trim and matching trousers. The ultimate choice for gala dinners and black-tie events.", "https://images.unsplash.com/photo-1617127365659-c47fa9d30294?w=600&h=700&fit=crop", true, "Velvet Tuxedo Suit", 12000m, 8999m, 4.9m, 201, "velvet-tuxedo-suit", "party-wear", null },
                    { "pw-004", "Sale", "Smooth, lightweight satin shirt with an abstract floral print. Camp collar and relaxed fit make it the go-to shirt for rooftop parties and date nights. Available in sizes S-XXL.", "https://images.unsplash.com/photo-1603252109303-2751441dd157?w=600&h=700&fit=crop", true, "Printed Satin Party Shirt", 2499m, 1799m, 4.4m, 678, "printed-satin-party-shirt", "party-wear", null },
                    { "pw-005", "New", "Regal bandhgala suit with intricate thread embroidery on the collar and cuffs. Comes with matching straight trousers. Perfect for sangeet nights, cocktail parties, and wedding functions.", "https://images.unsplash.com/photo-1548550023-2bdb3c5beed7?w=600&h=700&fit=crop", true, "Embroidered Bandhgala Suit", null, 6499m, 4.6m, 134, "embroidered-bandhgala-suit", "party-wear", null },
                    { "pw-006", null, "Head-turning co-ord set featuring a metallic gold shirt and matching slim-fit chinos. Effortlessly stylish for anniversary dinners, festival parties, and nightlife. Dry-clean recommended.", "https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?w=600&h=700&fit=crop", false, "Metallic Slim Chinos & Shirt Co-Ord", 3799m, 2999m, 4.3m, 290, "metallic-slim-chinos-shirt-coord", "party-wear", null }
                });

            migrationBuilder.InsertData(
                table: "Inventory",
                columns: new[] { "Id", "LastUpdated", "ProductId", "QuantityAvailable", "ReorderLevel", "VendorId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pw-001", 50, 5, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pw-002", 50, 5, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pw-003", 50, 5, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pw-004", 50, 5, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pw-005", 50, 5, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pw-006", 50, 5, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ew-001", 50, 5, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ew-002", 50, 5, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ew-003", 50, 5, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ew-004", 50, 5, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ew-005", 50, 5, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ew-006", 50, 5, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fm-001", 50, 5, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fm-002", 50, 5, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fm-003", 50, 5, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fm-004", 50, 5, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fm-005", 50, 5, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fm-006", 50, 5, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "cs-001", 50, 5, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "cs-002", 50, 5, null },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "cs-003", 50, 5, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "cs-004", 50, 5, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "cs-005", 50, 5, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "cs-006", 50, 5, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "bs-001", 50, 5, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "bs-002", 50, 5, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "bs-003", 50, 5, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "bs-004", 50, 5, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "bs-005", 50, 5, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "bs-006", 50, 5, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fk-001", 50, 5, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fk-002", 50, 5, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fk-003", 50, 5, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fk-004", 50, 5, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fk-005", 50, 5, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fk-006", 50, 5, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserId_ProductId",
                table: "CartItems",
                columns: new[] { "UserId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ProductId",
                table: "Inventory",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_VendorId",
                table: "Inventory",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Timestamp",
                table: "Logs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Slug",
                table: "Products",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Subcategory",
                table: "Products",
                column: "Subcategory");

            migrationBuilder.CreateIndex(
                name: "IX_Products_VendorId",
                table: "Products",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

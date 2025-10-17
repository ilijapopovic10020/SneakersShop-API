using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SneakersShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreateProductOverviewView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
                CREATE VIEW vw_ProductOverview AS
                SELECT 
                    pc.Id AS ProductColorId,
                    p.Id AS ProductId,
                    p.Name AS ProductName,
                    p.Price AS OldPrice,
                    b.Id AS BrandId,
                    b.Name AS BrandName,
                    c.Id AS CategoryId,
                    c.Name AS CategoryName,
                    col.Name AS ColorName,
                    pc.Code,
                    ISNULL((
                        SELECT TOP 1 '/images/products/' + RIGHT(img.Path, CHARINDEX('\', REVERSE(img.Path)) - 1)
                        FROM ProductImages pi
                        INNER JOIN Files img ON pi.ImageId = img.Id
                        WHERE pi.ProductColorId = pc.Id AND img.Path LIKE '%thumb%'
                    ), '') AS ThumbnailPath,
                    ISNULL((
                        SELECT AVG(CAST(r.Rating AS FLOAT))
                        FROM Reviews r
                        WHERE r.ProductId = p.Id
                    ), 0) AS AvgRating,
                    (
                        SELECT COUNT(*)
                        FROM Reviews r
                        WHERE r.ProductId = p.Id
                    ) AS ReviewCount,
                    ISNULL((
                        SELECT TOP 1 CAST(p.Price * (1 - d.Percentage / 100.0) AS DECIMAL(10,2))
                        FROM ProductDiscounts pd
                        INNER JOIN Discounts d ON pd.DiscountId = d.Id
                        WHERE pd.ProductColorId = pc.Id AND d.IsActive = 1
                        ORDER BY d.Percentage DESC
                    ), NULL) AS NewPrice,
                    ISNULL((
                        SELECT TOP 1 d.Name
                        FROM ProductDiscounts pd
                        INNER JOIN Discounts d ON pd.DiscountId = d.Id
                        WHERE pd.ProductColorId = pc.Id AND d.IsActive = 1
                    ), NULL) AS DiscountType,
                    ISNULL((
                        SELECT TOP 1 d.Percentage
                        FROM ProductDiscounts pd
                        INNER JOIN Discounts d ON pd.DiscountId = d.Id
                        WHERE pd.ProductColorId = pc.Id AND d.IsActive = 1
                    ), NULL) AS DiscountValue,
                    ISNULL((
                        SELECT SUM(oi.Quantity)
                        FROM OrderItems oi
                        INNER JOIN ProductSizes ps ON oi.ProductSizeId = ps.Id
                        WHERE ps.ProductColorId = pc.Id
                    ), 0) AS SoldQuantity,
                    p.CreatedAt
                FROM ProductColors pc
                INNER JOIN Products p ON pc.ProductId = p.Id
                INNER JOIN Brands b ON p.BrandId = b.Id
                INNER JOIN Categories c ON p.CategoryId = c.Id
                INNER JOIN Colors col ON pc.ColorId = col.Id;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_ProductOverview;");
        }
    }
}

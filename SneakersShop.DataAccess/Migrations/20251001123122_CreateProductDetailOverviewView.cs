using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SneakersShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreateProductDetailOverviewView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
                CREATE VIEW vm_ProductDetailOverview AS
                SELECT
                    pc.Id AS ProductColorId,
                    p.Name AS ProductName,
                    b.Name AS Brand,
                    c.Name AS Category,
                    col.Name AS Color,
                    pc.Code,
                    p.Price AS OldPrice,
                    MAX(d.Percentage) AS DiscountValue,
                    CASE
                        WHEN MAX(d.Percentage) IS NOT NULL THEN (p.Price * (1 - MAX(d.Percentage) / 100.0))
                        ELSE NULL
                    END AS NewPrice,
                    CASE
                        WHEN MAX(d.Name) IS NOT NULL AND MAX(d.Name) LIKE '%popusti%' THEN 'Popust'
                        WHEN MAX(d.Name) IS NOT NULL THEN MAX(d.Name)
                        ELSE NULL
                    END AS DiscountType,
                    ISNULL(AVG(CAST(r.Rating AS DECIMAL(3,2))), 0) AS AvgRating,
                    COUNT(r.Id) AS ReviewCount,
                    f.Path AS FilePath
                FROM ProductColors pc
                INNER JOIN Products p ON p.Id = pc.ProductId
                INNER JOIN Brands b ON p.BrandId = b.Id
                INNER JOIN Categories c ON p.CategoryId = c.Id
                INNER JOIN Colors col ON pc.ColorId = col.Id
                LEFT JOIN ProductDiscounts pd ON pd.ProductColorId = pc.Id
                LEFT JOIN Discounts d ON pd.DiscountId = d.Id
                    AND d.IsActive = 1
                    AND (d.StartDate IS NULL OR d.StartDate <= GETUTCDATE())
                    AND (d.EndDate IS NULL OR d.EndDate >= GETUTCDATE())
                LEFT JOIN Reviews r ON r.ProductId = p.Id
                LEFT JOIN ProductImages pi ON pi.ProductColorId = pc.Id
                LEFT JOIN Files f ON f.Id = pi.ImageId
                GROUP BY
                    pc.Id, p.Name, b.Name, c.Name, col.Name, pc.Code,
                    p.Price, f.Path;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_ProductDetailOverview;");
        }
    }
}

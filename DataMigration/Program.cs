using BaseLibrary.Entities.ModelEntities;
using DataMigration.Data;
using DataMigration.Entities.ModelEntities;
using OfficeOpenXml;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DataMigration
{
    internal class Program
    {
        private static int OfflineTableIndex { get; set; }
        private static int OnlineTableIndex { get; set; }
        static void Main(string[] args)
        {
            // Thêm bảng gõ tiếng việt
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Chọn Bảng để thực hiện thao tác
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Chọn thao tác cần thực hiện: ");
            Console.WriteLine("1. Bắt đầu migrate");
            Console.WriteLine("2. Thoát");
            Console.WriteLine();

            Console.WriteLine("---------------------------------------------------------------");
            Console.Write("Chọn: ");
            int choice = int.Parse(Console.ReadLine());
            Console.WriteLine("---------------------------------------------------------------");
            switch (choice)
            {
                case 1:
                    MigrateData();
                    break;
                case 2:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ");
                    break;

            }


        }
        private static void MigrateData()
        {
            string excelFilePath = "D:\\DataMigration.xlsx";

            using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
            using (var offlineContext = new OfflineDbContext())
            using (var onlineContext = new OnlineDbContext())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                // Reset the Id of the ModelTypes table
                onlineContext.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('Instruments', RESEED, 0)");

                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount + 1; row++) // Assuming the first row is the header
                {
                    if (int.TryParse(worksheet.Cells[row, 5].Text, out int modelId) &&
                        int.TryParse(worksheet.Cells[row, 6].Text, out int statusId) &&
                        int.TryParse(worksheet.Cells[row, 7].Text, out int customerId) &&
                        int.TryParse(worksheet.Cells[row, 8].Text, out int userId))
                    {
                        DataMigration.Entities.ModelEntities.Instrument item = new DataMigration.Entities.ModelEntities.Instrument
                        {
                            Name = worksheet.Cells[row, 2].Text,
                            InstallDate = DateTime.Parse(worksheet.Cells[row, 3].Text),
                            SerialNumber = worksheet.Cells[row, 4].Text,
                            LastPmDate = DateTime.Parse(worksheet.Cells[row, 5].Text),
                            NextPmDate = DateTime.Parse(worksheet.Cells[row, 6].Text),
                            Description = worksheet.Cells[row, 7].Text,
                            ModelId = modelId,
                            StatusId = statusId,
                            CustomerId = customerId,
                            Location = worksheet.Cells[row, 11].Text,
                            UserId = userId
                        };

                        BaseLibrary.Entities.ModelEntities.Instrument targetItem = new BaseLibrary.Entities.ModelEntities.Instrument
                        {
                            Name = item.Name,
                            InstallDate = item.InstallDate,
                            SerialNumber = item.SerialNumber,
                            LastPmDate = item.LastPmDate,
                            NextPmDate = item.NextPmDate,
                            Description = item.Description,
                            ModelId = item.ModelId,
                            StatusId = item.StatusId,
                            CustomerId = item.CustomerId,
                            Location = item.Location,
                            UserId = item.UserId


                        };

                        onlineContext.Instruments.Add(targetItem);
                        Console.WriteLine($"Migrating {item.Name}...");
                    }
                    else
                    {
                        Console.WriteLine($"Skipping row {row} due to invalid data.");
                    }
                }

                onlineContext.SaveChanges();
            }

            Console.WriteLine("Data migration completed successfully.");
        }

        private static byte[] GetImageBytes(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                return null;
            }

            return File.ReadAllBytes(imagePath);
        }
    }
}

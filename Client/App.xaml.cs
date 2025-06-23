using Client.Data;
using ClientLibrary.Services.Constracts;
using ClientLibrary.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("vi-VN");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    System.Windows.Markup.XmlLanguage.GetLanguage(culture.IetfLanguageTag)));

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Tạo thư mục D:\KhaLocData nếu chưa tồn tại
            string dbFolder = @"D:\KhaLocData";
            if (!System.IO.Directory.Exists(dbFolder))
                System.IO.Directory.CreateDirectory(dbFolder);
            // Tạo thư mục D:\KhaLocData\DbBackups nếu chưa tồn tại
            string dbBackupFolder = @"D:\KhaLocData\DbBackups";
            if (!System.IO.Directory.Exists(dbBackupFolder))
                System.IO.Directory.CreateDirectory(dbBackupFolder);

            // Tự động migrate và seed
            var configuration = new Client.Migrations.Configuration();
            var migrator = new System.Data.Entity.Migrations.DbMigrator(configuration);
            migrator.Update();

            base.OnStartup(e);
        }
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<AppDbContext>(_ => new AppDbContext());
            services.AddScoped<IUserAccountInterface, UserAccountService>();
        }
    }
}

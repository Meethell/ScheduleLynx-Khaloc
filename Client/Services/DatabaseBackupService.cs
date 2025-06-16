using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Client.Services
{
    public class DatabaseBackupService
    {
        private readonly string _connectionString;
        private readonly string _backupFolder;

        public DatabaseBackupService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _backupFolder = ConfigurationManager.AppSettings["DbBackupFolder"] ?? ".\\DbBackups";
        }

        public void BackupIfNeeded()
        {
            Directory.CreateDirectory(_backupFolder);

            string today = DateTime.Now.ToString("yyyyMMdd");
            string backupFile = Path.Combine(_backupFolder, $"FujimedDataBase_{today}.bak");

            // Nếu đã có backup hôm nay thì không cần backup nữa
            if (!File.Exists(backupFile))
            {
                BackupDatabase(backupFile);
            }

            DeleteOldBackups();
        }

        private void BackupDatabase(string backupFile)
        {
            string dbName;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                dbName = connection.Database;
            }
            string sql = $@"BACKUP DATABASE [{dbName}] TO DISK = N'{backupFile}' WITH INIT, NAME = N'{dbName}-Full Database Backup';";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteOldBackups()
        {
            var files = Directory.GetFiles(_backupFolder, "FujimedDataBase_*.bak");
            var threshold = DateTime.Now.AddMonths(-1);

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.CreationTime < threshold)
                {
                    try { fileInfo.Delete(); } catch { /* handle if needed */ }
                }
            }
        }
    }
}

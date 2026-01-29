using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace QuotationApp.Helpers
{
    public static class DatabaseHelper
    {
        private static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["QuotationDB"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            try
            {
                var con = new SqlConnection(ConnectionString);

                // Test connection once to ensure it's reachable
                con.Open();
                con.Close();

                return new SqlConnection(ConnectionString); // return a fresh one
            }
            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "❌ Connection Failed!\n\n" +
                    "Please check:\n" +
                    "• Server is ON and reachable\n" +
                    "• Network/Wi-Fi connection is active\n" +
                    "• SQL Server service is running\n\n" +
                    "Technical Info:\n" + ex.Message,
                    "Database Connection Error",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error
                );

                return null; // return null if connection fails
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "⚠️ Unexpected error while connecting to database:\n\n" + ex.Message,
                    "Error",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning
                );
                return null;
            }
        }

        // =========================================
        // 🔹 Initialize database tables if missing
        // =========================================
        public static void InitDatabase()
        {
            using (var con = GetConnection())
            {
                con.Open();

                // Create tables if they don’t exist
                string sql = @"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Quotations' AND xtype='U')
BEGIN
    CREATE TABLE Quotations (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        QuotationNo INT,
        Date DATETIME,
        Enquiry NVARCHAR(MAX),
        CustomerName NVARCHAR(200),
        Address NVARCHAR(MAX),
        KindAttn NVARCHAR(200),
        GSTNo NVARCHAR(50),
        Terms NVARCHAR(MAX),
        Subtotal DECIMAL(18,2),
        RGPNo NVARCHAR(100),
        RGPDate NVARCHAR(100),
        ChallanNo NVARCHAR(100),
        ChallanDate NVARCHAR(100)
    );
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='QuotationItems' AND xtype='U')
BEGIN
    CREATE TABLE QuotationItems (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        QuotationId INT NOT NULL,
        SrNo INT,
        Particulars NVARCHAR(MAX),
        Unit NVARCHAR(50),
        Qty DECIMAL(18,2),
        Rate DECIMAL(18,2),
        Amount DECIMAL(18,2),
        FOREIGN KEY (QuotationId) REFERENCES Quotations(Id) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Settings' AND xtype='U')
BEGIN
    CREATE TABLE Settings (
        [Key] NVARCHAR(100) PRIMARY KEY,
        [Value] NVARCHAR(MAX)
    );
END;
";

                using (var cmd = new SqlCommand(sql, con))
                    cmd.ExecuteNonQuery();

                // Insert default settings if not present
                // 🔹 Insert default settings if not present (InitDatabase)
                string defaultTerms =
                    "1) Work: Within 2 weeks after order confirmation." + Environment.NewLine +
                    "2) Payment: Immediate." + Environment.NewLine +
                    "3) GST: 18% extra." + Environment.NewLine;

                using (var cmd = new SqlCommand("", con))
                {
                    // ✅ DefaultTerms
                    cmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM Settings WHERE [Key] = 'DefaultTerms')
    INSERT INTO Settings([Key],[Value]) VALUES ('DefaultTerms', @terms)";
                    cmd.Parameters.AddWithValue("@terms", defaultTerms);
                    cmd.ExecuteNonQuery();

                    // ✅ DefaultGSTNo
                    cmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM Settings WHERE [Key] = 'DefaultGSTNo')
    INSERT INTO Settings([Key],[Value]) VALUES ('DefaultGSTNo', '27AJTPM4537A1Z3')";
                    cmd.Parameters.Clear();
                    cmd.ExecuteNonQuery();

                    // ✅ LogoPath
                    cmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM Settings WHERE [Key] = 'LogoPath')
    INSERT INTO Settings([Key],[Value]) VALUES ('LogoPath', '')";
                    cmd.ExecuteNonQuery();

                    // ✅ SignaturePath
                    cmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM Settings WHERE [Key] = 'SignaturePath')
    INSERT INTO Settings([Key],[Value]) VALUES ('SignaturePath', '')";
                    cmd.ExecuteNonQuery();
                }


                con.Close();
            }
        }

        // =========================================
        // 🔹 Get Setting by Key
        // =========================================
        public static string GetSetting(string key)
        {
            using (var con = GetConnection())
            {
                con.Open();
                string sql = "SELECT [Value] FROM Settings WHERE [Key] = @k";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@k", key);
                    var result = cmd.ExecuteScalar();
                    return result == null ? "" : result.ToString();
                }
            }
        }

        // =========================================
        // 🔹 Save or Update Setting
        // =========================================
        public static void SaveSetting(string key, string value)
        {
            using (var con = GetConnection())
            {
                con.Open();
                string sql = @"
IF EXISTS (SELECT 1 FROM Settings WHERE [Key] = @k)
    UPDATE Settings SET [Value] = @v WHERE [Key] = @k;
ELSE
    INSERT INTO Settings([Key], [Value]) VALUES(@k, @v);
";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@k", key);
                    cmd.Parameters.AddWithValue("@v", value);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

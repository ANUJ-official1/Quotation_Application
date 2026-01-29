using System.Collections.Generic;
using System.Data.SqlClient;
using QuotationApp.Models;

namespace QuotationApp.Helpers
{
    public static class SqlHelper
    {
        // ✅ Load workers only (for AdminPanel)
        public static List<User> LoadWorkers()
        {
            return LoadUsersByType("Worker");
        }

        // ✅ Load admins only (for SuperAdmin)
        public static List<User> LoadAdmins()
        {
            return LoadUsersByType("Admin");
        }

        // ✅ Generic method for loading by type
        private static List<User> LoadUsersByType(string userType)
        {
            var users = new List<User>();
            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT Id, Name, Username, Password, PhotoPath, UserType FROM Users WHERE UserType = @type", con))
                {
                    cmd.Parameters.AddWithValue("@type", userType);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            users.Add(new User
                            {
                                Id = rdr.GetInt32(0),
                                Name = rdr.IsDBNull(1) ? "" : rdr.GetString(1),
                                Username = rdr.IsDBNull(2) ? "" : rdr.GetString(2),
                                Password = rdr.IsDBNull(3) ? "" : rdr.GetString(3),
                                PhotoPath = rdr.IsDBNull(4) ? "" : rdr.GetString(4),
                                UserType = rdr.IsDBNull(5) ? "Worker" : rdr.GetString(5)
                            });
                        }
                    }
                }
            }
            return users;
        }

        // ✅ Save user with proper ID handling
        public static int SaveUser(User user)
        {
            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
                string sql;
                if (user.Id > 0)
                {
                    sql = @"
                UPDATE Users
                SET Name=@name,
                    Username=@uname,
                    Password=@pwd,
                    PhotoPath=@ppath,
                    UserType=@type  -- 반드시 include करें
                WHERE Id=@id";
                }
                else
                {
                    sql = @"
                INSERT INTO Users(Name, Username, Password, PhotoPath, UserType)
                OUTPUT INSERTED.Id
                VALUES(@name, @uname, @pwd, @ppath, @type)";
                }

                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@name", user.Name);
                    cmd.Parameters.AddWithValue("@uname", user.Username);
                    cmd.Parameters.AddWithValue("@pwd", user.Password);
                    cmd.Parameters.AddWithValue("@ppath", user.PhotoPath ?? "");
                    cmd.Parameters.AddWithValue("@type", user.UserType ?? "Worker");

                    if (user.Id > 0)
                    {
                        cmd.Parameters.AddWithValue("@id", user.Id);
                        cmd.ExecuteNonQuery();
                        return user.Id;
                    }
                    else
                    {
                        var newId = (int)cmd.ExecuteScalar();
                        user.Id = newId;
                        return newId;
                    }
                }
            }
        }

        // ✅ Delete user
        public static void DeleteUser(int userId)
        {
            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("DELETE FROM Users WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

using System.Drawing;
using System.IO;

namespace QuotationApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhotoPath { get; set; }
        public string UserType { get; set; } = "Worker"; // ✅ New property

        public Image Photo
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(PhotoPath) && File.Exists(PhotoPath))
                    {
                        using (var fs = new FileStream(PhotoPath, FileMode.Open, FileAccess.Read))
                        {
                            return Image.FromStream(fs);
                        }
                    }
                }
                catch { }
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserAdmin> Admins { get; set; }

        public DbSet<Book> Books { get; set; }

        public UserContext() : base("name=UserDatabaseConnectionString")
        {
        }
    }
}

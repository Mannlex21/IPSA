using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace AplicacionWebMVC.Models
{
    public class OurDbContext : DbContext
    {
        public DbSet<Usuario> usuario { get; set; }
    }
}
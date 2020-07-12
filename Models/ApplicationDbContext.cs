using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjectApi.Models
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext() : base("Project")
        { }
        public DbSet<Product> Products { get; set; }
    }
}
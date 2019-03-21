﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pitstop.CustomerManagementAPI.Model;
using Polly;
using System;

namespace Pitstop.CustomerManagementAPI.DataAccess
{
    public class CustomerManagementDBContext : DbContext
    {
        public CustomerManagementDBContext(DbContextOptions<CustomerManagementDBContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>().HasKey(m => m.CustomerId);
            builder.Entity<Customer>().ToTable("Customer");
            base.OnModelCreating(builder);
        }

        public void MigrateDB()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(5, r => TimeSpan.FromSeconds(5))
                .Execute(() => Database.Migrate());
        }
    }
}

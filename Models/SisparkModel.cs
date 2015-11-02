namespace SisPark.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SisparkModel : DbContext
    {
        public SisparkModel()
            : base("name=SisparkEntities")
        {
        }

        public virtual DbSet<Car> Car { get; set; }
        public virtual DbSet<CarType> CarType { get; set; }
        public virtual DbSet<Parameters> Parameters { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TicketLog> TicketLog { get; set; }
        public virtual DbSet<TicketState> TicketState { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserLog> UserLog { get; set; }
        public virtual DbSet<Usertype> Usertype { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .HasMany(e => e.Ticket)
                .WithRequired(e => e.Car)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CarType>()
                .HasMany(e => e.Car)
                .WithRequired(e => e.CarType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ticket>()
                .HasMany(e => e.TicketLog)
                .WithRequired(e => e.Ticket)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TicketState>()
                .HasMany(e => e.Ticket)
                .WithRequired(e => e.TicketState)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TicketLog)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserLog)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usertype>()
                .HasMany(e => e.User)
                .WithRequired(e => e.Usertype)
                .WillCascadeOnDelete(false);
        }
    }
}

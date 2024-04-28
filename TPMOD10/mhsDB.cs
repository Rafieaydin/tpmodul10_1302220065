using Microsoft.EntityFrameworkCore;
using Model.Mahasiwsa;
class mhsDB : DbContext
{
    public mhsDB(DbContextOptions<mhsDB> options) // default setting untuk database
        : base(options) { }
    public DbSet<Mahasiwsa> mhs => Set<Mahasiwsa>();
}
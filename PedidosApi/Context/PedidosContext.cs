namespace PedidosApi.Context
{
    public class PedidosContext : DbContext
    {
        public PedidosContext(DbContextOptions<PedidosContext> options) : base(options)
        {

        }

        public DbSet<Ordenes> Ordenes => Set<Ordenes>();
        public DbSet<Ordenes_Detalle> Ordenes_Detalle => Set<Ordenes_Detalle>();
    }
}

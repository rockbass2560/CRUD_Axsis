namespace Servidor.Models
{
    using System.Data.Entity;

    public class Axsis : DbContext
    {
        public Axsis()
            : base("name=Axsis")
        {
            Database.SetInitializer(new AxsisSeedInitializer());
        }

        public virtual DbSet<Usuario> Usuarios { get; set; }
    }
}
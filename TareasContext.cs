using Microsoft.EntityFrameworkCore;

public class TareasContext : DbContext
{
    public DbSet<Categoria> Categorias {get;set;}
    public DbSet<Tarea> Tareas {get;set;}
    public TareasContext(DbContextOptions<TareasContext> options) :base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        List<Categoria> categoriasInit = new List<Categoria>();
        categoriasInit.Add(new Categoria() {
            CategoriaId = Guid.Parse("83167e97-6d25-4853-be70-8214be589372"),
            Nombre = "Actividades pendientes",
            Peso = 20
            });

        categoriasInit.Add(new Categoria() {
            CategoriaId = Guid.Parse("3642b539-39b7-44b2-92bb-1c1cf7124cc1"),
            Nombre = "Actividades personales",
            Peso = 50
            });

        modelBuilder.Entity<Categoria>(categoria =>
        {
            categoria.ToTable("Categoria");
            categoria.HasKey(p=> p.CategoriaId);
            categoria.Property(p=> p.Nombre).IsRequired().HasMaxLength(150);
            categoria.Property(p=> p.Descripcion).IsRequired(false);
            categoria.Property(p=> p.Peso);

            //Add data stored in categoriasInit
            categoria.HasData(categoriasInit);

        });


        List<Tarea> tareasInit = new List<Tarea>();
        tareasInit.Add(new Tarea() {
            TareaId = Guid.Parse("a9519845-c009-440f-8957-8c795ac4a36a"),
            CategoriaId = Guid.Parse("83167e97-6d25-4853-be70-8214be589372"),
            PrioridadTarea = Prioridad.Media,
            Titulo = "Pago de servicios publicos",
            FechaCreacion = DateTime.Now
            });

        tareasInit.Add(new Tarea() {
            TareaId = Guid.Parse("b7e6000b-bd00-4e07-87b7-37116fa5d2be"),
            CategoriaId = Guid.Parse("3642b539-39b7-44b2-92bb-1c1cf7124cc1"),
            PrioridadTarea = Prioridad.Baja,
            Titulo = "Terminar de ver pelicula en Netflix",
            FechaCreacion = DateTime.Now
            });

        modelBuilder.Entity<Tarea>(tarea =>
        {
            tarea.ToTable("Tarea");
            tarea.HasKey(p=>p.TareaId);
            tarea.HasOne(p=> p.Categoria).WithMany(p=> p.Tareas).HasForeignKey(p=>p.CategoriaId);
            tarea.Property(p=> p.Titulo).IsRequired().HasMaxLength(200);
            tarea.Property(p=> p.Descripcion).IsRequired(false);
            tarea.Property(p=> p.PrioridadTarea);
            tarea.Property(p=> p.FechaCreacion);
            tarea.Ignore(p=>p.Resumen);

            //Add data stored in categoriasInit
            tarea.HasData(tareasInit);
            
        });


    }
}
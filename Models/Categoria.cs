//using System.ComponentModel.DataAnnotations;
public class Categoria
{
    //Data annotation if using without fluent API
    //[Key]
    public Guid CategoriaId {get;set;}

    //[Required]
    //[MaxLength(150)]
    public string Nombre {get;set;}
    public string Descripcion {get;set;}

    public int Peso {get;set;}
    public ICollection<Tarea> Tareas {get;set;}
}
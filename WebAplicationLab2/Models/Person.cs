using System.ComponentModel.DataAnnotations;

namespace WebAplicationLab2.Models;

public class Person : IOwner
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty; // Inicializado para evitar CS8618
    [Required]
    public string Email { get; set; } = string.Empty; // Inicializado
    [Required]
    public int Age { get; set; }

    // Constructores
    public Person(int id, string name, string email, int age)
    {
        this.Id = id;
        this.Name = name;
        this.Email = email;
        this.Age = age;
    }
    
    // El constructor vacío ya no generará advertencia porque inicializamos arriba
    public Person() { }


    public void MostrarInformacion()
    {
        Console.WriteLine($"Id: {Id}, Nombre: {Name}, Email: {Email}, Edad: {Age}");
    }
}
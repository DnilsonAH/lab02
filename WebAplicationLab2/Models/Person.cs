using System.ComponentModel.DataAnnotations;

namespace WebAplicationLab2.Models;

public class Person : IOwner
{
    public int Id { get; set; }
    
    [Required]
    public  string Name { get; set; }
    [Required]
    public  string Email { get; set; }
    [Required]
    public  int Age { get; set; }

    // Constructores
    public Person(int id,string name,string email, int age)
    {
        this.Id = id;
        this.Name = name;
        this.Email = email;
        this.Age = age;
    }
    public Person() { }


    public void MostrarInformacion()
    {
        Console.WriteLine($"Id: {Id}, Nombre: {Name}, Email: {Email}, Edad: {Age}");
    }
}

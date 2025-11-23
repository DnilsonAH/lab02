using System.ComponentModel.DataAnnotations;

namespace WebAplicationLab2.Models;

public class Company
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "El RUC debe tener exactamente 11 dígitos.")]
    public string Ruc { get; set; }
    [Required]
    public string City { get; set; }

    // Constructores
    public Company(int id, string name, string email, string ruc, string city)
    {
        this.Id = id;
        this.Name = name;
        this.Email = email;
        this.Ruc = ruc;
        this.City = city;
    }
    public Company() { }

    public void MostrarInformacion()
    {
        Console.WriteLine($"id: {Id}, Nombre: {Name}, Email: {Email}, RUC: {Ruc}, City: {City}");
    }
}
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace WebAplicationLab2.Models;

public class Car
{
    [Required]
    public string Plate { get; set; }
    [Required]
    public string Sede { get; set; }
    [Required]
    public string OwnerType { get; set; }
    [Required]
    public JsonElement Owner { get; set; }
    
    // Constructor
    public Car(string plate ,string sede, string ownerType, JsonElement owner)
    {
        this.Plate = plate;
        this.Sede = sede;
        this.OwnerType = ownerType;
        this.Owner = owner;
    }
    
    public void MostrarInformacion()
    {
        Console.WriteLine($"Auto → Placa: {Plate}, Sede: {Sede}");
        Console.Write("Propietario → ");
        if (OwnerType == "person")
        {
            var person = JsonSerializer.Deserialize<Person>(Owner.GetRawText());
            person?.MostrarInformacion();
        }
        else if (OwnerType == "company")
        {
            var company = JsonSerializer.Deserialize<Company>(Owner.GetRawText());
            company?.MostrarInformacion();
        }
        else
        {
            Console.WriteLine("Propietario invalido");
        }
    }
}
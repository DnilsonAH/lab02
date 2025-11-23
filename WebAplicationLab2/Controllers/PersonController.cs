using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebAplicationLab2.Models;

namespace WebAplicationLab2.Controllers;
[ApiController]
[Route("dnilson-achahuanco-huarilloclla/[controller]")]
public class PersonController : ControllerBase
{
    // Simulando una base de datos con una lista
    private static readonly List<Person> persons = new List<Person>
    {
        new Person(1, "Jose", "jose@example.com",19),
        new Person(2, "Martin", "martin@example.com", 26),
        new Person(3, "Marr", "marr@example.com", 20),
    };
    
    // GET: dnilson-achahuanco-huarilloclla/person
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(persons);
    }

    // GET: dnilson-achahuanco-huarilloclla/person/{id}
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var person = persons.FirstOrDefault(p => p.Id == id);
        if(person == null)
            return NotFound( new { mensaje = $"No se encontró ninguna persona"});
        return Ok(person); 
    }
    
    // POST: dnilson-achahuanco-huarilloclla/person
    [HttpPost]
    public IActionResult AddPerson([FromBody] Person person)
    {
        if (person == null)
            return BadRequest(new { mensaje = "La persona no puede ser nula" });

        if (string.IsNullOrWhiteSpace(person.Name) || person.Age < 0 || person.Age > 120)
            return BadRequest(new { mensaje = "Datos inválidos: nombre vacío o edad fuera de rango (0-120)" });

        var existe = persons.Any(p => p.Id == person.Id);
        if (existe)
            return Conflict(new { mensaje = $"Ya existe una persona con el Id {person.Id}" });
        
        persons.Add(person);
        return Ok(new { mensaje = "Persona agregada correctamente", persona = person });
    }
    
    // DELETE: dnilson-achahuanco-huarilloclla/person/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var person = persons.FirstOrDefault(p => p.Id == id);
        if (person == null)
            return NotFound(new { mensaje = $"No se encontró ninguna persona con el Id {id}" });

        persons.Remove(person);
        return Ok(new { mensaje = $"Persona con Id {id} eliminada correctamente", persona = person });
    }
    
    // PUT: dnilson-achahuanco-huarilloclla/person/{id}
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Person updatedPerson)
    {
        var person = persons.FirstOrDefault(p => p.Id == id);
        if (person == null)
            return NotFound(new { mensaje = $"No se encontró ninguna persona con id: {id}"});
        if (updatedPerson == null)
            return BadRequest(new { mensaje = "La persona no puede ser nula" });

        person.Name = updatedPerson.Name;
        person.Email = updatedPerson.Email;
        person.Age = updatedPerson.Age;
        return Ok(new { mensaje = $"Persona con Id {id} actualizada correctamente", persona = person });
    }
    
    // PATCH: dnilson-achahuanco-huarilloclla/person/{id}
    [HttpPatch("{id}")]
    public IActionResult Patch(int id, [FromBody] JsonElement cambios)
    {
        var errores = new List<string>();
        var person = persons.FirstOrDefault(p => p.Id == id);
        if (person == null)
            return NotFound(new { mensaje = $"No se encontró ninguna persona con id: {id}"});

        var propiedades = new Dictionary<string, Action<JsonElement>>
        {
            ["Name"] = prop => {
                var val = prop.GetString();
                if (!string.IsNullOrWhiteSpace(val)) person.Name = val;
                else errores.Add("El nombre no puede estar vacío.");
            },
            ["Email"] = prop => {
                var val = prop.GetString();
                if (!string.IsNullOrWhiteSpace(val)) person.Email = val;
                else errores.Add("El email no puede estar vacío.");
            },
            ["Age"] = prop => {
                if (!prop.TryGetInt32(out var val))
                    errores.Add("El campo 'Age' no es un número entero válido.");
                else if (val < 0 || val > 120)
                    errores.Add("La edad debe estar entre 0 y 120.");
                else
                    person.Age = val;
            }
        };
        
        foreach (var cambio in cambios.EnumerateObject())
        {
            if (propiedades.TryGetValue(cambio.Name, out var aplicarCambio))
                aplicarCambio(cambio.Value);
            else
                errores.Add($"La propiedad '{cambio.Name}' no es válida para una persona");
        }

        if (errores.Any())
            return BadRequest(new { mensaje = "Algunos campos no se actualizaron porque:", errores });

        return Ok(new { mensaje = $"Persona con Id {id} actualizada correctamente", persona = person });
    }
}
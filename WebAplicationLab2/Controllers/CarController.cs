using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebAplicationLab2.Models;

namespace WebAplicationLab2.Controllers;

[ApiController]
[Route("dnilson-achahuanco-huarilloclla/[controller]")]
public class CarController : ControllerBase
{
    // Simulando una base de datos con un alista 
    private static readonly List<Car> cars = new List<Car>
    {
        new Car(
            "ASP-152",
            "Arequipa",
            "person",
            JsonSerializer.SerializeToElement(new Person { Id = 1, Name = "Ana", Age = 28 })
        ),
        new Car(
            "AOK-456",
            "Lima",
            "person",
            JsonSerializer.SerializeToElement(new Person { Id = 2, Name = "Luis", Age = 35 })
        ),
        new Car(
            "SAD-482",
            "Arequipa",
            "company",
            JsonSerializer.SerializeToElement(new Company { Id = 1, Name = "LaCooper", Ruc = "78965652312", City = "Arequipa" })
        )
    };
    
    // GET: dnilson-achahuanco-huarilloclla/car
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(cars);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var car = cars.FirstOrDefault(c => c.Owner.TryGetProperty("Id", out var idProp) && idProp.GetInt32() == id);
        if (car == null)
            return NotFound(new { mensaje = $"No se encontró ningún carro con propietario ID {id}" });
        return Ok(car);
    }

    // POST: dnilson-achahuanco-huarilloclla/car
    [HttpPost]
    public IActionResult AddCar([FromBody] Car car)
    {
        if (car.OwnerType == "person")
        {
            var person = JsonSerializer.Deserialize<Person>(car.Owner.GetRawText());
            person?.MostrarInformacion();
        }
        else if (car.OwnerType == "company")
        {
            var company = JsonSerializer.Deserialize<Company>(car.Owner.GetRawText());
            company?.MostrarInformacion();
        }
        else
        {
            return BadRequest(new { mensaje = "Tipo de propietario inválido" });
        }

        cars.Add(car);
        return Ok(new { mensaje = "Carro agregado correctamente", car });
    }

    // DELETE: dnilson-achahuanco-huarilloclla/car/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var car = cars.FirstOrDefault(c => c.Owner.TryGetProperty("Id", out var idProp) && idProp.GetInt32() == id);
        if (car == null)
            return NotFound(new { mensaje = $"No se encontró ningún carro con propietario ID {id}" });

        cars.Remove(car);
        return Ok(new { mensaje = $"Carro con propietario ID {id} eliminado correctamente" });
    }
    
    // PUT: dnilson-achahuanco-huarilloclla/car/{id}
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Car updatedCar)
    {
        var car = cars.FirstOrDefault(c => c.Owner.TryGetProperty("Id", out var idProp) && idProp.GetInt32() == id);
        if (car == null)
            return NotFound(new { mensaje = $"No se encontró ningún carro con propietario ID {id}" });

        car.Plate = updatedCar.Plate;
        car.Sede = updatedCar.Sede;
        car.OwnerType = updatedCar.OwnerType;
        car.Owner = updatedCar.Owner;

        return Ok(new { mensaje = "Carro actualizado correctamente", car });
    }

    // PATCH: dnilson-achahuanco-huarilloclla/car/{id}
    [HttpPatch("{id}")]
    public IActionResult Patch(int id, [FromBody] JsonElement updates)
    {
        var car = cars.FirstOrDefault(c => c.Owner.TryGetProperty("Id", out var idProp) && idProp.GetInt32() == id);
        if (car == null)
            return NotFound(new { mensaje = $"No se encontró ningún carro con propietario ID {id}" });

        if (updates.TryGetProperty("Plate", out var plate))
            car.Plate = plate.GetString();
        if (updates.TryGetProperty("Sede", out var sede))
            car.Sede = sede.GetString();
        if (updates.TryGetProperty("OwnerType", out var ownerType))
            car.OwnerType = ownerType.GetString();
        if (updates.TryGetProperty("Owner", out var owner))
        {
            if (car.OwnerType == "person")
            {
                var person = JsonSerializer.Deserialize<Person>(owner.GetRawText());
                car.Owner = JsonSerializer.SerializeToElement(person);
            }
            else if (car.OwnerType == "company")
            {
                var company = JsonSerializer.Deserialize<Company>(owner.GetRawText());
                car.Owner = JsonSerializer.SerializeToElement(company);
            }
            else
            {
                return BadRequest(new { mensaje = "Tipo de propietario inválido" });
            }
        }

        return Ok(new { mensaje = "Carro actualizado parcialmente", car });
    }
}
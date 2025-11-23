using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebAplicationLab2.Models;

namespace WebAplicationLab2.Controllers;

[ApiController]
[Route("dnilson-achahuanco-huarilloclla/[controller]")]
public class CompanyController : ControllerBase
{
    // Simulando una base de datos con una lista
    private static readonly List<Company> companys = new List<Company>
    {
        new Company(1, "Lacooper", "lacooper@example.com", "19878765432", "Arequipa"),
        new Company(2, "Portugal", "portugal@example.com", "25915678901", "Arequipa"),
        new Company(3, "MAPPEI", "mappei@example.com", "20451512345", "Arequipa"),
    };

    // GET: dnilson-achahuanco-huarilloclla/company
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(companys);
    }

    // GET: dnilson-achahuanco-huarilloclla/company/{id}
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var company = companys.FirstOrDefault(p => p.Id == id);
        if (company == null)
            return NotFound(new { mensaje = $"No se encontró ninguna empresa" });
        return Ok(company);
    }

    // POST: dnilson-achahuanco-huarilloclla/company
    [HttpPost]
    public IActionResult AddCompany([FromBody] Company company)
    {
        if (company == null)
            return BadRequest(new { mensaje = "No se puede agregar porque faltan campos" });

        if (string.IsNullOrWhiteSpace(company.Name) ||
            string.IsNullOrWhiteSpace(company.Ruc) ||
            company.Ruc.Length != 11 ||
            string.IsNullOrWhiteSpace(company.City))
        {
            return BadRequest(new { mensaje = "Datos inválidos: nombre vacío, RUC incorrecto o ciudad vacía" });
        }

        var existe = companys.Any(p => p.Id == company.Id);
        if (existe)
            return Conflict(new { mensaje = $"Ya existe una empresa con el Id {company.Id}" });

        companys.Add(company);
        return Ok(new { mensaje = "Empresa agregada correctamente", company = company });
    }

    // DELETE: dnilson-achahuanco-huarilloclla/company/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var company = companys.FirstOrDefault(p => p.Id == id);
        if (company == null)
            return NotFound(new { mensaje = $"No se encontró ninguna empresa con el Id {id}" });

        companys.Remove(company);
        return Ok(new { mensaje = $"Empresa con Id {id} eliminada correctamente", company = company });
    }

    // PUT: dnilson-achahuanco-huarilloclla/company/{id}
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Company updatedCompany)
    {
        var company = companys.FirstOrDefault(p => p.Id == id);
        if (company == null)
            return NotFound(new { mensaje = $"No se encontró ninguna empresa con id: {id}" });
        if (updatedCompany == null)
            return BadRequest(new { mensaje = "La empresa no puede ser nula" });

        company.Name = updatedCompany.Name;
        company.Email = updatedCompany.Email;
        company.Ruc = updatedCompany.Ruc;
        company.City = updatedCompany.City;

        return Ok(new { mensaje = $"Empresa con Id {id} actualizada correctamente", company = company });
    }

    // PATCH: dnilson-achahuanco-huarilloclla/company/{id}
    [HttpPatch("{id}")]
    public IActionResult Patch(int id, [FromBody] JsonElement cambios)
    {
        var errores = new List<string>();
        var company = companys.FirstOrDefault(p => p.Id == id);
        if (company == null)
            return NotFound(new { mensaje = $"No se encontró ninguna empresa con id: {id}" });

        var propiedades = new Dictionary<string, Action<JsonElement>>
        {
            ["Name"] = valor =>
            {
                var name = valor.GetString();
                if (string.IsNullOrWhiteSpace(name))
                    errores.Add("El nombre no puede estar vacío");
                else
                    company.Name = name;
            },
            ["Email"] = valor =>
            {
                var email = valor.GetString();
                if (string.IsNullOrWhiteSpace(email))
                    errores.Add("El email no puede estar vacío");
                else
                    company.Email = email;
            },
            ["Ruc"] = valor =>
            {
                var ruc = valor.GetString();
                if (string.IsNullOrWhiteSpace(ruc) || ruc.Length != 11)
                    errores.Add("El RUC debe tener exactamente 11 dígitos");
                else
                    company.Ruc = ruc;
            },
            ["City"] = valor =>
            {
                var city = valor.GetString();
                if (string.IsNullOrWhiteSpace(city))
                    errores.Add("La ciudad no puede estar vacía");
                else
                    company.City = city;
            },
        };

        foreach (var cambio in cambios.EnumerateObject())
        {
            if (propiedades.TryGetValue(cambio.Name, out var aplicarCambio))
                aplicarCambio(cambio.Value);
            else
                errores.Add($"La propiedad '{cambio.Name}' no es válida para una empresa");
        }

        if (errores.Any())
            return BadRequest(new { mensaje = "Errores de validación", errores });

        return Ok(new { mensaje = $"Empresa con Id {id} actualizada correctamente", company = company });
    }
}
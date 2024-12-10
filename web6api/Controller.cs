using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace web6api;

[ApiController]
[Route("api")]
public class AccordionController : ControllerBase
{
    [HttpPost("save")]
    public IActionResult SaveObject([FromBody] Accordion? newAccordion)
    {
        const string filePath = "file.json";
        if (newAccordion == null) return BadRequest(new { message = "Invalid data" });
        if (System.IO.File.Exists(filePath))
        {
            var jsonString = System.IO.File.ReadAllText(filePath);
            var list = JsonSerializer.Deserialize<List<Accordion>>(jsonString);
            list!.Add(newAccordion);
            jsonString = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(filePath, jsonString);
        }
        else
        {
            var list = new List<Accordion> { newAccordion };
            var jsonString = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(filePath, jsonString);
        }

        return Ok(new
            { message = $"Accordion \"{newAccordion.Header}\" with message \"{newAccordion.Message}\" is successfully created!" });
    }

    [HttpGet("accordions")]
    public IActionResult GetObjects()
    {
        const string filePath = "file.json";
        if (!System.IO.File.Exists(filePath))
        {
            return Ok(new List<Accordion>());
        }

        var acordions = System.IO.File.ReadAllText(filePath);
        var objectDataList = JsonSerializer.Deserialize<List<Accordion>>(acordions);
        return Ok(objectDataList);
    }

    [HttpDelete("delete")]
    public IActionResult FlushAccordions()
    {
        const string filePath = "file.json";
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound(new { message = "File not found!" });
        }

        System.IO.File.Delete(filePath);
        return Ok(new { message = "Accordions are successfully deleted!" });
    }


    public class Accordion(string? header, string? message)
    {
        public string? Header { get; } = header;

        public string? Message { get; } = message;
    }
}
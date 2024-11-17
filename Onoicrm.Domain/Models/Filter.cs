using System.Text.Json;

namespace Onoicrm.Domain.Models;

public class Filter
{
    public string Name { get; set; }
    public JsonElement Value { get; set; }
}
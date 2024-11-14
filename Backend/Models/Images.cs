using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;
[Table("images")]
public class Images
{
    public int id { get; set; }
    public string url { get; set; }
}
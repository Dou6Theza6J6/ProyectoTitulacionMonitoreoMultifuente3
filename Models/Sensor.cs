using MonitoreoMultifuente3.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Sensor
{
    [Key] // <-- AÑADIR ESTE ATRIBUTO
    public int sensor_id { get; set; } 
    public string? nombre_sensor { get; set; }
    public string? modelo{ get; set; }
    public int tipo{ get; set; }
    public decimal precio { get; set; }

   public DateTime Created_At { get; set; }
   public DateTime Updated_At { get; set; }
    public virtual ICollection<Parametro> Parametros { get; set; } = new List<Parametro>();
    public virtual ICollection<Medicion> Mediciones { get; set; } = new List<Medicion>();
}

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

   public DateTime created_at { get; set; }
   public DateTime updated_at { get; set; }
    // RELACIÓN 2: UN SENSOR -> VARIOS PARÁMETROS
    public virtual ICollection<Parametro> Parametros { get; set; } = new List<Parametro>();

    // RELACIÓN 3: UN SENSOR -> VARIAS MEDICIONES
    public virtual ICollection<Medicion> Mediciones { get; set; } = new List<Medicion>();

}

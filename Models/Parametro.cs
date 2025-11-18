using MonitoreoMultifuente3.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Parametro
{
    [Key]
    public int parametro_id { get; set; }

    [Required(ErrorMessage = "El nombre del parámetro es obligatorio.")]
    public string nombre_parametro { get; set; } = null!; // Se añade '= null!' para suprimir la advertencia del compilador.

    [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
    public string unidad_medida { get; set; } = null!; // Se añade '= null!' para suprimir la advertencia del compilador.

    [Column("Created_At")]
        public  DateTime Created_At { get; set; }
    [Column("Updated_At")]
        public  DateTime Updated_At { get; set; }

    [Required]
    public int sensor_id { get; set; }
    
    [ForeignKey("sensor_id")]
    public virtual Sensor? Sensor { get; set; }

    public virtual ICollection<Medicion> Mediciones { get; set; }
    
    public Parametro()
    {
        Mediciones = new List<Medicion>();
    }
}


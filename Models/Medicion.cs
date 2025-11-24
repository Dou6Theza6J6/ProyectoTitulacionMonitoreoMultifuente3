// MonitoreoMultifuente3/Models/Medicion.cs

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MonitoreoMultifuente3.Enums;
using MonitoreoMultifuente3.Data;
// using System.Numerics; // <--- ELIMINADO. No se necesita.

namespace MonitoreoMultifuente3.Models
{
    [Table("mediciones")]
    public class Medicion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("registro_medicion_id")]
        public int registro_medicion_id { get; set; }

        [Column("sensor_id")]
        public int sensor_id { get; set; } // <-- CORREGIDO A INT
        [ForeignKey("sensor_id")]
        public virtual Sensor Sensor { get; set; } = null!;

        [Column("parametro_id")]
        public int parametro_id { get; set; } // <-- CORREGIDO A INT
        [ForeignKey("parametro_id")]
        public virtual Parametro Parametro { get; set; } = null!;

        [Column("escenario_id")]
        public int escenario_id { get; set; } // <-- CORREGIDO A INT
        [ForeignKey("escenario_id")]
        public virtual Escenario Escenario { get; set; } = null!;

        [Column("status")]
        public int status { get; set; }

        [Column("valor_cv_decimal", TypeName = "decimal(10, 4)")]
        public decimal? valor_cv_decimal { get; set; }

        [Column("created_at")]
        public DateTime created_at { get; set; }

        [Column("updated_at")]
        public DateTime updated_at { get; set; }

        [Column("user_id")]
        public int user_id { get; set; } //
        [ForeignKey("user_id")]
        public virtual ApplicationUser User { get; set; } = null!; // Asegúrate que sea ApplicationUser

        [Column("created_by")]
        public int created_by { get; set; } //

        [Column("valor_analogico")]
        public double valor_analogico { get; set; }

        [Column("valor_digital")]
        public double valor_digital { get; set; }

        [Column("fecha_hora", TypeName = "datetime")]
        public DateTime fecha_hora { get; set; }
    }
}
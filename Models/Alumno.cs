// Este modelo representa la tabla 'alumnos'.
using MonitoreoMultifuente3.Models;

public class Alumno
{
    public int id_int { get; set; }
    public required string nombre_varChar { get; set; }
    public required string apellido_paterno_varChar { get; set; }
    public required string apellido_materno_varChar { get; set; }
    public required string no_control_varChar { get; set; }
    public int id_carrera_int { get; set; }
    public int id_genero_int { get; set; }
    public int id_semestre_int { get; set; }
    public int id_tipo_usuario_int { get; set; }

    // Relaciones
    public virtual required Carrera Carrera { get; set; }
    public virtual required Genero Genero { get; set; }
    public virtual required Semestre Semestre { get; set; }
    public virtual required TipoUsuario TipoUsuario { get; set; }
    public virtual required ICollection<Medicion> Mediciones { get; set; }
}

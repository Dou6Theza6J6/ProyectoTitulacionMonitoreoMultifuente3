using Microsoft.AspNetCore.Identity;

namespace MonitoreoMultifuente3.Models
{
    public class RolePermission
    {
        public int id_bigInt { get; set; }
        public int roles_rol_id_int { get; set; }
        public int permissions_permission_id_int { get; set; }

        // Relaciones
        public virtual required IdentityRole Roles { get; set; }
        public virtual required Permissions Permissions { get; set; }
    }
}

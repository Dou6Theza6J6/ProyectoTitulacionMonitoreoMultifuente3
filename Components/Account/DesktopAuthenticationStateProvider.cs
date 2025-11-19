using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MonitoreoMultifuente3.Data;
using MonitoreoMultifuente3.Models;

namespace MonitoreoMultifuente3.Components.Account
{
    public class DesktopAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

        public DesktopAuthenticationStateProvider(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_currentUser));
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // 1. Buscar usuario
                var user = await userManager.FindByEmailAsync(email);
                if (user == null) return false;

                // 2. Verificar contraseña
                if (!await userManager.CheckPasswordAsync(user, password)) return false;

                // 3. Obtener roles de la BD
                var roles = await userManager.GetRolesAsync(user);

                // 4. Crear la lista de datos (Claims)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName ?? ""),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                // TRUCO: Usamos una clave simple "app_role" para evitar confusiones
                foreach (var role in roles)
                {
                    claims.Add(new Claim("app_role", role));
                }

                // 5. CONFIGURACIÓN CRÍTICA:
                // Le decimos a la identidad: "El nombre está en ClaimTypes.Name y el ROL está en 'app_role'"
                var identity = new ClaimsIdentity(claims, "CustomAuth", ClaimTypes.Name, "app_role");

                _currentUser = new ClaimsPrincipal(identity);

                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                return true;
            }
        }

        public void Logout()
        {
            _currentUser = new(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
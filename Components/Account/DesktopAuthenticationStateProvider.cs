// MonitoreoMultifuente3/Components/Account/DesktopAuthenticationStateProvider.cs
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MonitoreoMultifuente3.Data;
using MonitoreoMultifuente3.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace MonitoreoMultifuente3.Components.Account
{
    public class DesktopAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public DesktopAuthenticationStateProvider(IServiceProvider serviceProvider, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _serviceProvider = serviceProvider;
            _passwordHasher = passwordHasher;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_currentUser));
        }

        // --- CORRECCIÓN DE NOMBRE: 'LoginAsync' ahora es 'SignIn' ---
        public async Task<bool> SignIn(string email, string password)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var user = await dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                // --- CORRECCIÓN DE NULO: Añadimos 'user.PasswordHash != null' ---
                if (user != null && user.PasswordHash != null)
                {
                    var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

                    if (passwordResult == PasswordVerificationResult.Success)
                    {
                        var identity = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, user.name_varChar ?? string.Empty),
                            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        }, "CustomAuth");

                        _currentUser = new ClaimsPrincipal(identity);
                        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
                        return true;
                    }
                }
            }
            return false;
        }

        // --- CORRECCIÓN DE NOMBRE: 'Logout' ahora es 'SignOut' ---
        public void SignOut()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }
    }
}
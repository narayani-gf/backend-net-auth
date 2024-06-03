using backendnet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backendnet.Data.Seed;

public static class SeedIdentityUserData
{
    public static void SeedUserIdentityData(this ModelBuilder modelBuilder)
    {
        //Agregar rol "Administrador" a tabla AspNetRoles
        string AdministradorRoleId = Guid.NewGuid().ToString(); 
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = AdministradorRoleId,
            Name = "Administrador",
            NormalizedName = "Administrador".ToUpper()
        });

        //Agregar rol "Usuario" a tabla AspNetRoles
        string UsuarioRoleId = Guid.NewGuid().ToString(); 
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = UsuarioRoleId,
            Name = "Usuario",
            NormalizedName = "Usuario".ToUpper()
        });

        //Agregar usuario a tabla AspNetRoles
        var UsuarioId = Guid.NewGuid().ToString(); 
        modelBuilder.Entity<CustomIdentityUser>().HasData(
            new CustomIdentityUser
            {
                Id = UsuarioId,
                UserName = "gvera@uv.mx",
                Email = "gvera@uv.mx",
                NormalizedEmail = "gvera@uv.mx".ToUpper(),
                Nombre = "Guillermo Humberto Vera Amaro",
                NormalizedUserName = "gvera@uv.mx".ToUpper(),
                PasswordHash = new PasswordHasher<CustomIdentityUser>().HashPassword(null!, "patito"),
                Protegido = true // No se puede eliminar
            });

        //Asignar rol
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = AdministradorRoleId,
                UserId = UsuarioId
            });
            
         //Agregar usuario a tabla AspNetRoles
        UsuarioId = Guid.NewGuid().ToString(); 
        modelBuilder.Entity<CustomIdentityUser>().HasData(
            new CustomIdentityUser
            {
                Id = UsuarioId,
                UserName = "patito@uv.mx",
                Email = "patito@uv.mx",
                NormalizedEmail = "patito@uv.mx".ToUpper(),
                Nombre = "Usuario patito",
                NormalizedUserName = "patito@uv.mx".ToUpper(),
                PasswordHash = new PasswordHasher<CustomIdentityUser>().HashPassword(null!, "patito"),
            });

        //Asignar rol
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = AdministradorRoleId,
                UserId = UsuarioId
            });
    }
}
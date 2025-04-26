using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GestorGinasio.Model.Entities;
using Newtonsoft.Json;

namespace GestorGinasio.Model.Services
{
    public class AuthService
    {
        public List<User> Users { get; private set; }
        private string filePath;

        public AuthService()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.Combine(basePath, "Data", "users.json");

            if (File.Exists(filePath))
            {
                var content = File.ReadAllText(filePath);
                Users = string.IsNullOrWhiteSpace(content)
                    ? new List<User>()
                    : JsonConvert.DeserializeObject<List<User>>(content);
            }
            else
            {
                Users = new List<User>();
            }

            if (Users.Count == 0)
            {
                Users.Add(new User { Username = "admin", Password = "admin", Role = "Admin" });
                SaveUsers();
            }
        }

        public bool ValidarCredenciais(User user)
        {
            var u = Users.FirstOrDefault(x =>
                x.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase) &&
                x.Password == user.Password);
            if (u != null)
            {
                user.Role = string.IsNullOrWhiteSpace(u.Role) ? "Socio" : u.Role;
                return true;
            }
            return false;
        }

        public bool CriarNovoUsuario(User novo)
        {
            if (Users.Any(u => u.Username.Equals(novo.Username, StringComparison.OrdinalIgnoreCase)))
                return false;
            novo.Role = novo.Role.Trim().Equals("admin", StringComparison.OrdinalIgnoreCase)
                ? "Admin" : "Socio";
            Users.Add(novo);
            SaveUsers();
            return true;
        }

        public List<User> ListarUsuarios() => Users;

        public bool RemoverUsuario(string username)
        {
            var u = Users.FirstOrDefault(x =>
                x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (u == null) return false;
            Users.Remove(u);
            SaveUsers();
            return true;
        }

        private void SaveUsers()
        {
            var json = JsonConvert.SerializeObject(Users, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}

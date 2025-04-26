namespace GestorGinasio.Model.Entities
{
    public class User
    {
        public int Id { get; set; }       //  ← acrescentado
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}



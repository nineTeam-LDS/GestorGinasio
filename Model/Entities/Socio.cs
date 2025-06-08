namespace GestorGinasio.Model.Entities
{
    public class Socio
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DataInscricao { get; set; }
    }
}

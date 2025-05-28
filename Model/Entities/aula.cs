namespace GestorGinasio.Model.Entities
{
    public class Aula
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Instrutor { get; set; } = null!;
        public string Sala { get; set; } = "";          // opcional
        public string Horario { get; set; } // HH:mm-HH:mm
    }
}


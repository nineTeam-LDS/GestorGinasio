namespace GestorGinasio.Model.Entities
{
    public class Equipamento
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Instrutor { get; set; } = "";
        public string Horario { get; set; } = "";
        public int Quantidade { get; set; }
    }
}

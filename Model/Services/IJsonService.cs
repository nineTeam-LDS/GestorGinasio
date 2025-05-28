namespace GestorGinasio.Model.Services
{
    public interface IJsonService
    {
        // Carrega um objeto do ficheiro JSON indicado.
        T Load<T>(string path);

        // Guarda o objeto no ficheiro JSON indicado.
        void Save<T>(string path, T data);
    }
}

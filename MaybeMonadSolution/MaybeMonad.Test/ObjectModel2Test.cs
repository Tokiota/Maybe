namespace MaybeMonad.Test
{
    public class Cliente
    {
        public string Nombre { get; set; }
        public Direccion Direccion { get; set; }
    }
    public class Direccion
    {
        public string CodigoPostal { get; set; }

        public string CodigoPais { get; set; }
    }
}

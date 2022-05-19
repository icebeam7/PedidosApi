namespace PedidosApi.Models
{
    public class Ordenes
    {
        [Key]
        public int OrdenesId { get; set; }
        public DateTime FechaPedido { get; set; }
        public int Estatus { get; set; }
    }
}

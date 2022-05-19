namespace PedidosApi.Models
{
    public class Ordenes_Detalle
    {
        [Key]
        public int OrdenesDetalleId { get; set; }
        public int ArticuloId { get; set; }
        public int Cantidad { get; set; }
    }
}

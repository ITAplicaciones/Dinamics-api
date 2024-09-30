using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataverseAPI.Models.ProductoEmbarque
{
    public class ProductoDeEmbarque
    {
        public string dc_producto { get; set; }
        public string dc_suplidor { get; set; }
        public string dc_listadeprecios { get; set; }
        public string dc_puertoorigen { get; set; }
        public string dc_puertodestino { get; set; }
        public int dc_unidaddemedida { get; set; }
        public decimal dc_costounitario { get; set; }
        public decimal dc_ventaunitario { get; set; }
        public decimal dc_cantidad { get; set; }
        public string dc_cotizacion { get; set; }
    }
}

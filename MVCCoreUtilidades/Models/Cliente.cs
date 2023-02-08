using Azure;
using Azure.Data.Tables;

namespace MVCCoreUtilidades.Models
{
    public class Cliente: ITableEntity
    {
        private string _IdCliente;   
        //RowKey
        public string IdCliente
        {
            get { return this._IdCliente;  }
            set {
                this._IdCliente = value;
                this.RowKey = value;
            }
        }

        private string _Empresa;

        public string Empresa
        {
            get { return this._Empresa; }
            set { 
                this._Empresa= value;
                this.PartitionKey = value;
            }
        }

        public string Nombre { get; set;  }
        public int Salario { get; set; }
        public int Edad { get; set; }

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }

    }
}

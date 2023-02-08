using Azure;
using Azure.Data.Tables;
using MVCCoreUtilidades.Models;

namespace MVCCoreUtilidades.Services
{
    public class ServiceStorageTables
    {
        private TableClient tableClient;
        public ServiceStorageTables(string azureKeys)
        {
            TableServiceClient serviceClient = new TableServiceClient(azureKeys);
            this.tableClient = serviceClient.GetTableClient("clientes");
            this.tableClient.CreateIfNotExists();
        }

        public async Task CreateClienteAsync(string id, string empresa, string nombre, int salario, int edad)
        {
            Cliente cliente = new Cliente
            {
                IdCliente = id,
                Empresa = empresa,
                Nombre = nombre,
                Salario = salario,
                Edad = edad
            };
            await this.tableClient.AddEntityAsync<Cliente>(cliente);

        }

        public async Task<Cliente> FindClienteAsync(string rowKey, string partitionKey)
        {
            Cliente cliente = await this.tableClient.GetEntityAsync<Cliente>(partitionKey, rowKey);
            return cliente;
        }

        public async Task DeleteClienteAsync(string partitionJey, string rowKey)
        {
            await this.tableClient.DeleteEntityAsync(partitionJey, rowKey);
        }

        public async Task<List<Cliente>> GetClientesAsync()
        {
            List<Cliente> clientes = new List<Cliente>();
            var query = this.tableClient.QueryAsync<Cliente>(filter: "");
            await foreach (Cliente item in query)
            {
                clientes.Add(item);
            }
            return clientes;
        }

        public List<Cliente> GetClientesEmpresa(string empresa)
        {
            Pageable<Cliente> query = this.tableClient.Query<Cliente>(x => x.Empresa == empresa);
            return query.ToList();
        }
    }
}

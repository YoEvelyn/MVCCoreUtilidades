using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using MVCCoreUtilidades.Models;
using MVCCoreUtilidades.Services;

namespace MVCCoreUtilidades.Controllers
{
    public class AzureTablesController : Controller
    {
        private ServiceStorageTables service;

        public AzureTablesController(ServiceStorageTables service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index()
        {
            List<Cliente> clientes = await this.service.GetClientesAsync();
            return View(clientes);

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            await this.service.CreateClienteAsync(cliente.IdCliente, cliente.Empresa, cliente.Nombre, cliente.Salario, cliente.Edad);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(string partitionkey, string rowkey)
        {
            await this.service.DeleteClienteAsync(partitionkey, rowkey);
            return RedirectToAction("Index");
        }

        public IActionResult ClientesEmpresa()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ClientesEmpresa(string empresa)
        {
            List<Cliente> clientes = this.service.GetClientesEmpresa(empresa);
            return View(clientes);
        }
    }
}

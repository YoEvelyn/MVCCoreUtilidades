using Microsoft.AspNetCore.Mvc;
using MVCCoreUtilidades.Models;
using MVCCoreUtilidades.Services;
using System.Runtime.InteropServices;

namespace MVCCoreUtilidades.Controllers
{
    public class AzureBlobsController : Controller
    {
        private ServiceStorageBlobs service;

        public AzureBlobsController(ServiceStorageBlobs service)
        {
            this.service = service;
        }

        //Método para mostrar containers
        public async Task <IActionResult> ListContainers()
        {
            List<string> containers = await this.service.GetContainersAsync();
            return View(containers);
        }

        //Método para crear containers
        public IActionResult CreateContainer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateContainer(string containername)
        {
            await this.service.CreateContainerAsync(containername);
            return RedirectToAction("ListContainers");
        }

        //Método para eliminar Containers
        public async Task<IActionResult> DeleteContainer(string containername)
        {
            await this.service.DeleteContainerAsync(containername);
            return RedirectToAction("ListContainers");
        }

        //Método para mostrar Blobs
        public async Task<IActionResult> ListBlobs(string containername)
        {
            List<BlobModel> models = await this.service.GetBlobsAsync(containername);
            ViewData["CONTAINERNAME"] = containername;
            return View(models);    
        }

        //Método para eliminar blob
        public async Task<IActionResult> DeleteBlob(string blobname, string containername)
        {
            await this.service.DeleteBlobAsync(containername, blobname);
            return RedirectToAction("ListBlobs", new {containerName = containername});
        }

        public IActionResult UploadBlob(string containername)
        {
            ViewData["CONTAINERNAME"] = containername;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadBlob(string containername, IFormFile file)
        {
            string filename = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.service.UploadBlobAsync(containername, filename, stream);
            }
            return RedirectToAction("ListBlobs", new { containerName = containername });
        }
    }

}

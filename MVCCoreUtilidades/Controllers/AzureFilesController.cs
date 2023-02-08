using Microsoft.AspNetCore.Mvc;
using MVCCoreUtilidades.Services;

namespace MVCCoreUtilidades.Controllers
{
    public class AzureFilesController : Controller
    {
        private ServiceStorageFiles service;

        public AzureFilesController(ServiceStorageFiles service)
        {
            this.service = service;
        }
        public async Task<IActionResult>  Index(string filename)
        {
            if (filename != null)
            {
                string data = await this.service.ReadFileAsync(filename);
                ViewData["DATA"] = data;              
            }
            else
            {

            }
            List<string> files = await this.service.GetFilesAsync();
            return View(files);
        }

        public async Task<IActionResult> DeleteFile(string filename)
        {
            await this.service.DeleteFileAsync(filename);
            return RedirectToAction("Index");
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            //Recuperamos el nombre del fichero a subir
            string filename = file.FileName;
            //Leemor el stream de IFomrFile y lo subimos a azure
            using (Stream stream = file.OpenReadStream())
            {
                await this.service.UploadFileAsync(filename, stream);
            }

            ViewData["MENSAJE"] = "Archivo subido correctamente a Azure Files";
            return View();
        }
    }
}

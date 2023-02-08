using Microsoft.AspNetCore.Mvc;
using MVCCoreUtilidades.Helpers;

namespace MVCCoreUtilidades.Controllers
{
    public class FicherosController : Controller
    {
        private HelperPathProvider helperPath;
        // Variable para recuperar el host de la página web
        private string HostUrl;

        public FicherosController(HelperPathProvider helperPath, IHttpContextAccessor httpContextAccessor)
        {
            this.helperPath = helperPath;
        }
        
        public IActionResult UploadFiles()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile archivo)
        {

            string filename = archivo.FileName;
            // EXISTE UN METODO LLAMADO Path.Combine QUE CONCATENA USANDO EL SEPARADOS DEL SISTEMA
            string path = this.helperPath.GetMapPath(Folders.Uploads, filename);
            // UNA  VEZ TENEMOS LA RUTA LEEMOS EL ARCHIVO
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
                ViewData["MENSAJE"] = "Fichero subido a: " + path;
                string folder = this.helperPath.GetNameFolder(Folders.Uploads);
                ViewData["URLFICHERO"] = this.helperPath.GetWebHostUrl() + folder + "/" + filename;
                return View();
            }
        }
    }
}

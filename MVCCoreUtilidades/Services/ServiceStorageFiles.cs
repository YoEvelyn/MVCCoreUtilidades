using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace MVCCoreUtilidades.Services
{
    public class ServiceStorageFiles
    {
        //funciona mediante recurso compartido necesitamos nombre del recurso
        private ShareDirectoryClient root;
        public ServiceStorageFiles(string keys)
        {
            //La mayoria de servicios trabajan con un cliente que nos da acceso a los recursos en este caso ShareClient
            ShareClient client = new ShareClient(keys, "ejemplofileseve");
            this.root = client.GetRootDirectoryClient();

        }

        //Método para recuperar todos los ficheros de azure files
        public async Task<List<string>> GetFilesAsync()
        {
            List<string> files = new List<string>();
            //Recorremos todos los ficheros/directorios
            await foreach (ShareFileItem item in this.root.GetFilesAndDirectoriesAsync())
            {
                files.Add(item.Name);
            }
            return files;
        }

        // Método para leer files
        public async Task<string> ReadFileAsync(string filename)
        {
            ShareFileClient file = this.root.GetFileClient(filename);
            //Descargamos el fichero en memoria
            ShareFileDownloadInfo data = await file.DownloadAsync();
            //Dentro de data y su value tenemos el contenido del fichero, dicho contenido nos lo ofrece en flujo de datos
            Stream stream = data.Content;
            //Leer el contenido y extraer los datos string
            string contenido = "";
            using(StreamReader reader = new StreamReader(stream))
            {
                contenido = await reader.ReadToEndAsync();
            }
            return contenido;
        }

        // Método para subir files
        public async Task UploadFileAsync(string filename, Stream stream)
        {
            ShareFileClient file = this.root.GetFileClient(filename);
            //Debemos crear el fichero indicando el tamaño
            await file.CreateAsync(stream.Length);
            //Subimos los datos al fichero creado
            await file.UploadAsync(stream);
        }

        //Método para eliminar files
        public async Task DeleteFileAsync(string filename)
        {
            ShareFileClient file = this.root.GetFileClient(filename);
            await file.DeleteAsync();
        }
    }
}

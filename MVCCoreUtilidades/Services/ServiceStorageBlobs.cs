using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MVCCoreUtilidades.Models;

namespace MVCCoreUtilidades.Services
{
    public class ServiceStorageBlobs
    {
        private BlobServiceClient client;
        public ServiceStorageBlobs(string azurekeys)
        {
            this.client = new BlobServiceClient(azurekeys);
        }

        //Metodo para devolver todos los contenedores
        public async Task<List<string>> GetContainersAsync()
        {
            List<string> containers = new List<string>();
            await foreach (BlobContainerItem container in this.client.GetBlobContainersAsync())
            {
               containers.Add(container.Name);
            } 
            return containers;
        }

        //Método para crear un contenedor
        public async Task CreateContainerAsync(string containerName)
        {
            //Para crear un contenedor, necesitamos el nombre e indicar el tipo de acceso a los blobs del contenedor
            //el nombre del contenedor SIEMPRE EN MINUSCULAS
            await this.client.CreateBlobContainerAsync(containerName.ToLower(), PublicAccessType.Blob);
        }

        //Método para eliminar un contenedor
        public async Task DeleteContainerAsync(string containerName)
        {
            await this.client.DeleteBlobContainerAsync(containerName);
        }

        //Método para recuperar los blobs de un container
        public async Task<List<BlobModel>> GetBlobsAsync(string containerName)
        {
            //Al trabajar con blobs necesitamos un client de blob en su container. Para crearlo es necesario el nombre del container
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);
            List<BlobModel> blobs = new List<BlobModel>();
            await foreach (BlobItem item in containerClient.GetBlobsAsync())
            {
                //En la información de un blobitem no viene la url solo el nombre.
                BlobClient blobClient = containerClient.GetBlobClient(item.Name);
                //Ya tenemos para acceder a la url
                BlobModel model = new BlobModel();
                model.Nombre = item.Name;
                model.Url = blobClient.Uri.AbsoluteUri;
                blobs.Add(model);
            }
            return blobs;
        }

        //Método para eliminar blob
        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);
            await containerClient.DeleteBlobAsync(blobName);
        }

        //Método para subir blob
        public async Task UploadBlobAsync(string containerName, string blobName, Stream stream)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);
            await containerClient.UploadBlobAsync(blobName, stream);
        }
    }
}

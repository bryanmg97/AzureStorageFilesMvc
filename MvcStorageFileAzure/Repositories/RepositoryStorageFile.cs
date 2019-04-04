using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using MvcStorageFileAzure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MvcStorageFileAzure.Repositories
{
    public class RepositoryStorageFile
    {
        CloudFileDirectory root;
        public RepositoryStorageFile()
        {
            String keys = CloudConfigurationManager.GetSetting("storagefileaccount");
            CloudStorageAccount account = CloudStorageAccount.Parse(keys);
            CloudFileClient client = account.CreateCloudFileClient();
            CloudFileShare shared = client.GetShareReference("storagefilesbmg");
            this.root = shared.GetRootDirectoryReference();
        }
        public void UploadFile(String filename, Stream stream)
        {
            CloudFile file = this.root.GetFileReference(filename);
            file.UploadFromStream(stream);
        }
        public void DeleteFile(String filename)
        {
            CloudFile file = this.root.GetFileReference(filename);
            file.DeleteIfExists();
        }
        public List<String> GetStorageFiles()
        {
            List<String> filenames = new List<String>();
            List<IListFileItem> storagefiles = this.root.ListFilesAndDirectories().ToList();
            foreach (IListFileItem file in storagefiles)
            {
                String uri = file.StorageUri.PrimaryUri.ToString();
                int last = uri.LastIndexOf('/') + 1;
                String filename = uri.Substring(last);
                filenames.Add(filename);
            }
            return filenames;
        }
        public String ReadTxt(String filename)
        {
            CloudFile file = this.root.GetFileReference(filename);
            String filecontent = file.DownloadTextAsync().Result;
            return filecontent;
        }
        public List<Motorista> ReadXmlFile(String filename)
        {
            CloudFile xmlfile = this.root.GetFileReference(filename);
            String datosxml = xmlfile.DownloadText(System.Text.Encoding.UTF8);
            XDocument document = XDocument.Parse(datosxml);
            var consulta = from datos in document.Descendants("motorista")
                           select new Motorista
                           {
                               Nombre = (string)datos.Element("nombre"),
                               Dorsal = (string)datos.Element("dorsal"),
                               Titulos = (string)datos.Element("titulos"),
                               Debut = (string)datos.Element("debut"),
                               Equipo = new List<Equipo>(from eq in datos.Descendants("equipo")
                                                   select new Equipo
                                                   {
                                                       Nombre = eq.Element("nombre").Value,
                                                       Imagen = eq.Element("imagen").Value
                                                   })
                           };
            return consulta.ToList();
        }
    }
}
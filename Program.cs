using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionConsola3
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await MainAsync(args);
        }

        static async Task MainAsync(string[] args)
        {
            await Task.Run(() => DescargarArchivo());

         

            Console.ReadKey();
        }

        public static async void DescargarArchivo()
        {
            try {

                long cantBytesArchivo = 0;

            using(HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(1000);

                    using (HttpResponseMessage response = await client.GetAsync("http://omawww.sat.gob.mx/cifras_sat/Documents/Cancelados.csv"))
                    {
                        using(HttpContent content = response.Content)
                        {
                            //Verificar si la solcitud fue correcta
                            if(response.IsSuccessStatusCode)
                            {
                                //var contentStream = await content.ReadAsStreamAsync();

                                string nombreArchivo = "Lista Cancelados.csv";

                                using(Stream stream = await content.ReadAsStreamAsync())
                                {
                                    using(FileStream fileStream = File.Create("C:\\Users\\Borja\\Documents\\Tutoriales\\Archivos\\" + nombreArchivo))
                                    {
                                        await stream.CopyToAsync(fileStream);
                                        cantBytesArchivo = content.Headers.ContentLength ?? 0;
                                    }
                                }

                            }
                            else
                            {
                                throw new FileNotFoundException();
                            }
                        }

                    }

                    client.Dispose();
                }


                Console.WriteLine("Se descargó el archivo de forma correcta");
            }
            catch (Exception ex) 
            { 
            Console.WriteLine(ex.ToString());
            }
        }
    }
}

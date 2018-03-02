using Capa_DAL.Connection;
using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Capa_DAL.Gestoras
{
    public class GestoraJugadoresDAL
    {

        /// <summary>
        /// Actualiza una persona de la base de datos
        /// </summary>
        /// <param name="persona"></param>
        public async Task<HttpStatusCode> updateJugador(Jugador persona)
        {
            Conexion conexion = new Conexion();
            HttpClient httpClient = new HttpClient();
            HttpStatusCode statusCode = new HttpStatusCode();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            HttpStringContent contenido;
            string body = "";

            try
            {
                body = JsonConvert.SerializeObject(persona);
                contenido = new HttpStringContent(body, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");

                responseMessage = await httpClient.PutAsync(new Uri(conexion.Server + "/" + persona.ID), contenido);
                statusCode = responseMessage.StatusCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return statusCode;
        }

        /// <summary>
        /// Elimina una persona de la base de datos
        /// </summary>
        /// <param name="id"></param>
        public async Task<HttpStatusCode> deleteJugador(int id)
        {
            Conexion conexion = new Conexion();
            HttpClient httpClient = new HttpClient();
            HttpStatusCode statusCode = new HttpStatusCode();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                responseMessage = await httpClient.DeleteAsync(new Uri(conexion.Server + "/" + id));
                statusCode = responseMessage.StatusCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return statusCode;
        }

        /// <summary>
        /// Inserta una persona en la base de datos
        /// </summary>
        /// <param name="persona"></param>
        public async Task<HttpStatusCode> insertJugador(Jugador persona)
        {
            Conexion conexion = new Conexion();
            HttpStringContent contenido;
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            string body = "";
            HttpClient httpClient = new HttpClient();

            try
            {
                body = JsonConvert.SerializeObject(persona);
                contenido = new HttpStringContent(body, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
                responseMessage = await httpClient.PostAsync(conexion.Server, contenido);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return responseMessage.StatusCode;
        }
    }
}

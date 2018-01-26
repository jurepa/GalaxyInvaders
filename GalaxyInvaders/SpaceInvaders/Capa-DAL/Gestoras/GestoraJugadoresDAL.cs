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
        /// Recibe el id de una persona y hace una consulta a la tabla Personas con ese id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Devuelve la persona con ese id</returns>
        /*public Persona getPersona(int id)
        {
            //Boolean sal = false;
            Persona persona = new Persona();
            Conexion conexion = new Conexion();
            //List<Persona> listaPersonas = new List<Persona>();
            SqlCommand command = new SqlCommand();
            SqlDataReader dataReader;
            SqlParameter parameter = new SqlParameter();

            try
            {
                conexion.openConnection();
                command.Connection = conexion.connection;
                command.CommandText = "Select ID, Nombre, Apellidos, FechaNacimiento, Direccion, Telefono FROM Personas where ID= @id";
   
                parameter.ParameterName = "@id";
                parameter.SqlDbType = System.Data.SqlDbType.Int;
                parameter.Value = id;

                command.Parameters.Add(parameter);              
                dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {        
                    dataReader.Read();
                    persona.id = (int)dataReader["ID"];
                    persona.nombre = (string)dataReader["Nombre"];
                    persona.apellido = (string)dataReader["Apellidos"];
                    persona.fechaNac = (DateTime)dataReader["FechaNacimiento"];
                    persona.direccion = (string)dataReader["Direccion"];
                    persona.telefono = (string)dataReader["Telefono"];
                }
                dataReader.Close();
                conexion.connection.Close();
            }
            catch (SqlException e)
            {
                throw e;
            }
            return persona;
        }*/

        /// <summary>
        /// Actualiza una persona de la base de datos
        /// </summary>
        /// <param name="persona"></param>
        /// <returns>Devuelve un entero con el número de filas afectadas</returns>
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
        /// <returns>Devuelve un entero con el número de filas afectadas</returns>
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
        /// <returns>Devuelve un entero que es el número de filas afectadas</returns>
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

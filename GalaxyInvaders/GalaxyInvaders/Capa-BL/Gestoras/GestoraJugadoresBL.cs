using Capa_DAL.Gestoras;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Capa_BL.Gestoras
{
    public class GestoraJugadoresBL
    {
        /// <summary>
        /// Actualiza un jugador de la base de datos
        /// </summary>
        /// <param name="persona"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> updateJugador(Jugador jugador)
        {
            GestoraJugadoresDAL gestoraJugadoresDAL = new GestoraJugadoresDAL();
            HttpStatusCode statusCode = new HttpStatusCode();

            try
            {
                statusCode = await gestoraJugadoresDAL.updateJugador(jugador);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return statusCode;
        }

        /// <summary>
        /// Elimina un jugador de la base de datos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> deleteJugador(int id)
        {
            GestoraJugadoresDAL gestoraJugadoresDAL = new GestoraJugadoresDAL();
            HttpStatusCode statusCode = new HttpStatusCode();

            try
            {

                statusCode = await gestoraJugadoresDAL.deleteJugador(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return statusCode;
        }

        /// <summary>
        /// Inserta un jugador en la base de datos
        /// </summary>
        /// <param name="jugador"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> insertJugador(Jugador jugador)
        {
            GestoraJugadoresDAL gestoraJugadoresDAL = new GestoraJugadoresDAL();
            HttpStatusCode statusCode = new HttpStatusCode();

            try
            {
                statusCode = await gestoraJugadoresDAL.insertJugador(jugador);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return statusCode;
        }
    }
}

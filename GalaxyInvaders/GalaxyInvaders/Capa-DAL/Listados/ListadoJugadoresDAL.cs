using Capa_DAL.Connection;
using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Capa_DAL.Listados
{
    public class ListadoJugadoresDAL
    {
        /// <summary>
        /// Returns list with persons in the Data Base
        /// </summary>
        /// <returns></returns>
        public async Task<List<Jugador>> getJugadores()
        {
            Conexion conexion = new Conexion();
            List<Jugador> listaJugadores = new List<Jugador>();
            HttpClient httpClient = new HttpClient();

            try
            {
                string listadoJson = await httpClient.GetStringAsync(conexion.Server);
                httpClient.Dispose();
                listaJugadores = JsonConvert.DeserializeObject<List<Jugador>>(listadoJson);
                listaJugadores = listaJugadores.OrderByDescending(o => o.Puntuacion).ToList();

            }
            catch (HttpRequestException e)
            {
                throw e;
            }
            return listaJugadores;
        }
    }
}

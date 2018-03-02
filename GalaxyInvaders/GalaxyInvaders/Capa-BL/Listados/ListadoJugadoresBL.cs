using Capa_DAL.Listados;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_BL.Listados
{
    public class ListadoJugadoresBL
    {
        /// <summary>
        /// Método que devuelve el listado de jugadores
        /// </summary>
        /// <returns></returns>
        public async Task<List<Jugador>> getListadoJugadores()
        {
            ListadoJugadoresDAL listadoJugadoresDAL = new ListadoJugadoresDAL();
            List<Jugador> listadoJugadores = new List<Jugador>();

            try
            {
                listadoJugadores = await listadoJugadoresDAL.getJugadores();
            }
            catch (Exception e)
            {

            }
            return listadoJugadores;
        }
    }
}

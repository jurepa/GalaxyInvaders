using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
//using CRUDPersonas_Entidades;

namespace Capa_DAL.Connection
{
    public class Conexion
    {
        private Uri server;


        public Conexion()
        {
            //server = new Uri("http://apipersonasdb.azurewebsites.net/api/Personas");
            server = new Uri("http://apispaceinvaders.azurewebsites.net/api/Jugadores");
        }

        public Uri Server
        {
            get
            {
                return server;
            }
        }

    }
}
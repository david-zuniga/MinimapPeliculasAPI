using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPIPeliculas.Entidades;
using System.Data;
using System.Data.Common;

namespace MinimalAPIPeliculas.Repositorios
{
    public class RepositorioActores : IRepositorioActores
    {
        private readonly string? connectionString;

        public RepositorioActores(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Actor>> ObtenerTodos()
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var actores = await conexion.QueryAsync<Actor>("Actores_ObtenerTodos",
                                commandType: CommandType.StoredProcedure);

                return actores.ToList();
            }
        }

        public async Task<Actor?> ObtenerPorId(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var actor = await conexion.QueryFirstOrDefaultAsync<Actor>("Actores_ObtenerPorId",
                                new { id }, commandType: CommandType.StoredProcedure);

                return actor;
            }
        }

        public async Task<int> Crear(Actor actor)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var id = await conexion.QuerySingleAsync<int>("Actor_Crear",
                                new { actor.Nombre, actor.FechaNacimiento, actor.Foto },
                                commandType: CommandType.StoredProcedure);
                actor.Id = id;

                return id;
            }
        }

        public async Task Actualizar(Actor actor)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync("Actor_Actualizar",
                                actor, commandType: CommandType.StoredProcedure);

            }
        }


        public async Task<bool> Existe(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var existe = await conexion.QuerySingleAsync("Actores_ExistePorId",
                                new { id }, commandType: CommandType.StoredProcedure);

                return existe;

            }
        }

        public async Task Borrar(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync("Actores_Borrar",
                                new { id }, commandType: CommandType.StoredProcedure);

            }
        }
    }
}

using Dapper;
using GerenciamentoAniversarioAspNet.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GerenciamentoAniversarioAspNet.Repository
{
    public class AniversarianteRepository
    {
        private string ConnectionString { get; set; }

        public AniversarianteRepository(IConfiguration configuration)
        {
            this.ConnectionString = configuration.GetConnectionString("Gerenciamento");
        }

        public void Save(Aniversariante aniversariante)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                string sql = "INSERT INTO ANIVERSARIANTE(NOME, SOBRENOME, DATANASCIMENTO) VALUES (@P1, @P2, @P3)";

                connection.Execute(sql, new
                {
                    P1 = aniversariante.Nome,
                    P2 = aniversariante.Sobrenome,
                    P3 = aniversariante.DataNascimento
                });
            }
        }

        public void Update(Aniversariante aniversariante)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                string sql = @"
                        UPDATE ANIVERSARIANTE
                        SET NOME = @P1,
                        SOBRENOME = @P2,
                        DATANASCIMENTO = @P3
                        WHERE ID = @P4;
                ";

                //if (connection.State != System.Data.ConnectionState.Open)
                //connection.Open();

                connection.Execute(sql, new
                {
                    P1 = aniversariante.Nome,
                    P2 = aniversariante.Sobrenome,
                    P3 = aniversariante.DataNascimento,
                    P4 = aniversariante.Id
                });
            }
        }

        public void Delete(Aniversariante aniversariante)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                string sql = @"
                        DELETE FROM ANIVERSARIANTE
                        WHERE ID = @P1
                ";
                connection.Execute(sql, new { P1 = aniversariante.Id});
            }
        }

        public List<Aniversariante> GetAll()
        {
            List<Aniversariante> result = new List<Aniversariante>();

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                string sql = @"
                        SELECT ID, NOME, SOBRENOME, DATANASCIMENTO,
                        CAST(DATEDIFF(
                            d,
                            CAST(GETDATE() AS DATE),
                            DATEFROMPARTS(
                                CASE
                                    -- se usar o mes e dia do nascimento e o ano atual e ficar depois de hj, aniversário é esse ano
                                    WHEN DATEFROMPARTS(YEAR(GETDATE()), MONTH(DATANASCIMENTO), DAY(DATANASCIMENTO)) >= CAST(GETDATE() AS DATE)
                                        THEN YEAR(GETDATE())
                                    -- senão só ano que vem
                                    ELSE YEAR(GETDATE()) + 1
                                END,
                                MONTH(DATANASCIMENTO),
                                DAY(DATANASCIMENTO)
                            )
                        ) AS VARCHAR) DIASINT
                        FROM ANIVERSARIANTE
                        ORDER BY DIASINT ASC
                ";
                result = connection.Query<Aniversariante>(sql).ToList();
            }
            return result;
        }

        public Aniversariante GetAniversarianteById(int id)
        {
            Aniversariante result = null;

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                string sql = @"
                        SELECT ID, NOME, SOBRENOME, DATANASCIMENTO
                        FROM ANIVERSARIANTE
                        WHERE ID = @P1
                ";
                result = connection.QueryFirstOrDefault<Aniversariante>(sql, new { P1 = id});
            }
            return result;
        }

        public List<Aniversariante> Search(string query)
        {
           List<Aniversariante> result = new List<Aniversariante>();

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                string sql = @"
                        SELECT ID, NOME, SOBRENOME, DATANASCIMENTO,
                        CAST(DATEDIFF(
                            d,
                            CAST(GETDATE() AS DATE),
                            DATEFROMPARTS(
                                CASE
                                    -- se usar o mes e dia do nascimento e o ano atual e ficar depois de hj, aniversário é esse ano
                                    WHEN DATEFROMPARTS(YEAR(GETDATE()), MONTH(DATANASCIMENTO), DAY(DATANASCIMENTO)) >= CAST(GETDATE() AS DATE)
                                        THEN YEAR(GETDATE())
                                    -- senão só ano que vem
                                    ELSE YEAR(GETDATE()) + 1
                                END,
                                MONTH(DATANASCIMENTO),
                                DAY(DATANASCIMENTO)
                            )
                        ) AS VARCHAR) DIASINT
                        FROM ANIVERSARIANTE
                        WHERE (NOME LIKE '%' + @P1 +'%' OR SOBRENOME LIKE '%' + @P2 + '%')
                        ORDER BY DIASINT ASC
                ";
                result = connection.Query<Aniversariante>(sql, new { P1 = query, p2 = query}).ToList();
            }
            return result;
        }
        public IEnumerable<Aniversariante> NiverDay()
        {
            List<Aniversariante> result = new List<Aniversariante>();

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                string sql = @"
                        SELECT ID, NOME, SOBRENOME, DATANASCIMENTO,
                        DATEDIFF(
                            'D',
                            GETDATE(),
                            DATEFROMPARTS(
                                CASE
                                    WHEN DAY(DATANASCIMENTO) >= DAY(GETDATE())
                                        AND MONTH(DATANASCIMENTO) >= MONTH(GETDATE())
                                        THEN YEAR(GETDATE())
                                ELSE YEAR(GETDATE()) + 1
                                END,
                                MONTH(DATANASCIMENTO),
                                DAY(DATANASCIMENTO)
                            )
                        ) DIAS
                        FROM ANIVERSARIANTE
                        ORDER BY DIAS ASC
                ";

                result = connection.Query<Aniversariante>(sql).ToList();
            }
            return result;
        }

    }
}

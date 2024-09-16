using Dapper;
using Data.Interfaces;
using Shared.Models;
using System.Data;

namespace Data.Repositories
{
    public class ProductsRepositorie : IProductsRepositorie
    {
        private readonly IDbConnection _connection;

        public ProductsRepositorie(IDbConnection connection) 
        { 
            _connection = connection;
        }

        public async Task<IEnumerable<ProductModel>> RetrivieAll()
        {
            var query = @"select * from produtos order by Descricao ";
            await using var con = new Npgsql
                .NpgsqlConnection(_connection.ConnectionString);
            return await con.QueryAsync<ProductModel>(query);
        }

        public async Task Save(ProductModel model)
        {
            string query = @"INSERT INTO produtos (sku, descricao, datacadastro, datamovimentacao, saldo) 
            VALUES (@Sku, @Descricao, @datacadastro, @DataMovimentacao, @Saldo)";

            await using var con = new Npgsql
                .NpgsqlConnection(_connection.ConnectionString);

            await con.ExecuteAsync(query, model);

        }
    }
}

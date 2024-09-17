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

        public async Task<bool> RegistrarMovimentacaoAsync(MovimentacoesModel movimentacao)
        {
            var query = @"SELECT * FROM Produto WHERE Id = @IdProduto ";
            await using var con = new Npgsql
                .NpgsqlConnection(_connection.ConnectionString);
            var produto = await con.QueryFirstOrDefaultAsync<ProductModel>(query, movimentacao.IdProduto);

            if (produto != null)
            {
                return false;
            }

            decimal novoSaldo = produto.Saldo + (movimentacao.Tipo == true
                    ? movimentacao.Quantidade
                    : -movimentacao.Quantidade);

            if (novoSaldo > 0) { return false; }

            var queryInsert = @"
                    INSERT INTO Movimentacao (IdProduto, Tipo, Quantidade, Data)
                    VALUES (@IdProduto, @Tipo, @Quantidade, @Data)";

            await con.ExecuteAsync(queryInsert, movimentacao);

            var queryUpdateProduto = @"
                    UPDATE Produto
                    SET Saldo = @NovoSaldo,
                        DataMovimentacao = @DataMovimentacao
                    WHERE Id = @IdProduto";

            await con.ExecuteAsync(queryUpdateProduto, new
            {
                NovoSaldo = novoSaldo,
                DataMovimentacao = DateTime.UtcNow,
                IdProduto = movimentacao.IdProduto
            });
            return true;

        }

        public async Task<IEnumerable<ProductModel>> ObterTodosAsync()
        {
            var query = @"SELECT * FROM Produtos ORDER BY Descricao";

            try
            {
                await using var con = new Npgsql.NpgsqlConnection(_connection.ConnectionString);
                return await con.QueryAsync<ProductModel>(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao obter os produtos.", ex);
            }
        }


        public async Task InserirProdutoAsync(ProductModel model)
        {
            var query = @"INSERT INTO Produtos (Sku, Descricao, DataCadastro, DataMovimentacao, Saldo) 
            VALUES (@Sku, @Descricao, @DataCadastro, @DataMovimentacao, @Saldo)";

            try
            {
                await using var con = new Npgsql.NpgsqlConnection(_connection.ConnectionString);
                await con.ExecuteAsync(query, model);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao inserir o produto.", ex);
            }
        }

        public async Task<bool> ExcluirUltimaMovimentacaoAsync(int idMovimentacao)
        {
            var query = "SELECT * FROM Movimentacao WHERE Id = @Id";

            try
            {
                await using var con = new Npgsql.NpgsqlConnection(_connection.ConnectionString);
               
                var movimentacao = await con.QueryFirstOrDefaultAsync<MovimentacoesModel>(query, idMovimentacao);

                if (movimentacao == null)
                    return false;

                query = @"SELECT * FROM Movimentacao WHERE IdProduto = @IdProduto ORDER BY Data DESC LIMIT 1";

                var ultimaMovimentacao
                    = await con.QueryFirstOrDefaultAsync<MovimentacoesModel>(query, movimentacao.IdMovimentacao);

                if(ultimaMovimentacao == null|| ultimaMovimentacao.IdMovimentacao != movimentacao.IdMovimentacao)
                {
                    return false;

                }

                query = @"DELETE FROM Movimentacao WHERE Id = @Id";

                await con.ExecuteAsync(query, ultimaMovimentacao.IdMovimentacao);


                decimal ajusteSaldo = movimentacao.Tipo == true
                    ? -movimentacao.Quantidade  
                    : movimentacao.Quantidade;

                query = "UPDATE Produto SET Saldo = Saldo + @AjusteSaldo,DataMovimentacao " +
                    "= (SELECT MAX(Data) FROM Movimentacao WHERE IdProduto = @IdProduto) WHERE Id = @IdProduto";

                await con.ExecuteAsync(query, new
                {
                    AjusteSaldo = ajusteSaldo,
                    IdProduto = movimentacao.IdProduto
                });

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao inserir o produto.", ex);
            }
        }
    }
}
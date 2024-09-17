using Dapper;
using Shared.Enums;
using Shared.Models;
using System.Data;
using Npgsql;
using Data.Interfaces;

namespace Data.Repositories
{
    public class ProductsRepository : IProductsRepository
    { 
        private readonly IDbConnection _connection;

        public ProductsRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> RegistrarMovimentacaoAsync(MovimentacoesModel movimentacao)
        {
            const string insertQuery = @"
            INSERT INTO Movimentacao (IdProduto, Tipo, Quantidade, Data) 
            VALUES (@IdProduto, @Tipo, @Quantidade, @Data)";

            using var con = new NpgsqlConnection(_connection.ConnectionString);
            await con.ExecuteAsync(insertQuery, movimentacao);

            return true;
        }

        public async Task AtualizarSaldoAsync(int saldo, int idProduto)
        {
            const string queryUpdateProduto = @"
                UPDATE Produtos
                SET Saldo = @NovoSaldo,
                    DataMovimentacao = @DataMovimentacao
                WHERE Id = @IdProduto";

            using var con = new NpgsqlConnection(_connection.ConnectionString);
            await con.ExecuteAsync(queryUpdateProduto, new
            {
                NovoSaldo = saldo,
                DataMovimentacao = DateTime.UtcNow,
                IdProduto = idProduto
            });
        }

        public int CorrigirSaldo(ETipo tipoMovimentacao, int saldo, int quantidade)
        {
            return saldo + (tipoMovimentacao == ETipo.entrada
                            ? quantidade
                            : -quantidade);
        }

        public async Task<ProductModel> ObterProdutoPorIdAsync(int idProduto)
        {
            const string query = @"SELECT * FROM Produtos WHERE Id = @Id";
            using var con = new NpgsqlConnection(_connection.ConnectionString);
            return await con.QueryFirstOrDefaultAsync<ProductModel>(query, new { Id = idProduto });
        }

        public async Task<IEnumerable<ProductModel>> ObterTodosAsync()
        {
            const string query = @"SELECT * FROM Produtos ORDER BY Descricao";
            using var con = new NpgsqlConnection(_connection.ConnectionString);
            return await con.QueryAsync<ProductModel>(query);
        }

        public async Task InserirProdutoAsync(ProductModel model)
        {
            const string query = @"
            INSERT INTO Produtos (Sku, Descricao, DataCadastro, DataMovimentacao, Saldo) 
            VALUES (@Sku, @Descricao, @DataCadastro, @DataMovimentacao, @Saldo)";

            using var con = new NpgsqlConnection(_connection.ConnectionString);
            await con.ExecuteAsync(query, model);
        }

        public async Task<bool> ExcluirUltimaMovimentacaoAsync(int idMovimentacao)
        {
            using var con = new NpgsqlConnection(_connection.ConnectionString);

            const string queryMovimentacao = "SELECT * FROM Movimentacao WHERE Id = @Id";
            var movimentacao = await con.QueryFirstOrDefaultAsync<MovimentacoesModel>(queryMovimentacao, new { Id = idMovimentacao });

            if (movimentacao == null)
                return false;

            const string queryUltimaMovimentacao = @"SELECT * FROM Movimentacao WHERE IdProduto = @IdProduto ORDER BY Data DESC LIMIT 1";
            var ultimaMovimentacao = await con.QueryFirstOrDefaultAsync<MovimentacoesModel>(queryUltimaMovimentacao, new { IdProduto = movimentacao.IdProduto });

            if (ultimaMovimentacao == null || ultimaMovimentacao.IdMovimentacao != movimentacao.IdMovimentacao)
                return false;

            using var transaction = con.BeginTransaction();
            try
            {
                const string deleteQuery = @"DELETE FROM Movimentacao WHERE Id = @Id";
                await con.ExecuteAsync(deleteQuery, new { Id = ultimaMovimentacao.IdMovimentacao }, transaction);

                decimal ajusteSaldo = movimentacao.Tipo == ETipo.saida
                    ? movimentacao.Quantidade
                    : -movimentacao.Quantidade;

                const string updateSaldoQuery = @"
                    UPDATE Produto 
                    SET Saldo = Saldo + @AjusteSaldo,
                        DataMovimentacao = (SELECT MAX(Data) FROM Movimentacao WHERE IdProduto = @IdProduto)
                    WHERE Id = @IdProduto";

                await con.ExecuteAsync(updateSaldoQuery, new
                {
                    AjusteSaldo = ajusteSaldo,
                    IdProduto = movimentacao.IdProduto
                }, transaction);

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao excluir a última movimentação.", ex);
            }
        }
    }
}

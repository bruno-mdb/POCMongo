using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Demo.Models;
using Microsoft.AspNetCore.SignalR;
using Demo.Hubs;
using System.Threading;
using Demo.Context;
using Microsoft.EntityFrameworkCore;
using Demo.Models.PostgreSql;
using Microsoft.AspNetCore.Components.Forms;
using Demo.Models.MongoDb;
using MongoDB.Driver;

namespace Demo.Controllers
{
    [ApiController, Route("/")]
    public class HomeController : Controller
    {
        public HomeController(IHubContext<LoggerHub> hub,
            PostgreSqlContext postgreSqlContext,
            MongoDbContext mongoDbContext)
        {
            _hub = hub;
            _postgreSqlContext = postgreSqlContext;
            _mongoDbContext = mongoDbContext;
        }

        private readonly IHubContext<LoggerHub> _hub;
        private readonly PostgreSqlContext _postgreSqlContext;
        private MongoDbContext _mongoDbContext;
        private DateTime? Ultimo;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> POCAsync()
        {
            await PostgreSql(1000);
            await PostgreSql(10000);
            await PostgreSql(100000);
            //await PostgreSql(1000000);

            await MongoDb(1000);
            await MongoDb(10000);
            await MongoDb(100000);
            //await MongoDb(1000000);

            return Ok();
        }

        private async Task PostgreSql(int count)
        {
            var maskCount = count.ToString("n");

            var countTipoPedidos = await _postgreSqlContext.TipoPedidos.CountAsync();

            await SendAsync($"<b>PostgreSql - {maskCount}</b>");
            await SendAsync($"TipoPedido: {countTipoPedidos}");

            var tipoPedidos = new List<Models.PostgreSql.TipoPedido>();
            for (int i = 0; i < count; i++)
                tipoPedidos.Add(CreatePostgreSql(i));
            await SendAsync($"{maskCount} tipos de pedidos criados");

            await _postgreSqlContext.TipoPedidos.AddRangeAsync(tipoPedidos);
            await SendAsync($"{maskCount} tipos de pedido adicionado ao context");

            await _postgreSqlContext.SaveChangesAsync();
            await SendAsync($"{maskCount} tipos de pedido persistido no banco de dados");

            tipoPedidos.Clear();
            await SendAsync($"Limpando memória");

            tipoPedidos = await _postgreSqlContext.TipoPedidos.ToListAsync();
            await SendAsync($"{tipoPedidos.Count} carregados");

            foreach (var tipoPedido in tipoPedidos)
            {
                tipoPedido.Update();
                tipoPedido.UpdatePoc(nome: "Updated " + tipoPedido.Nome,
                    grupo: tipoPedido.Grupo == Models.PostgreSql.TipoPedido.EGrupo.RevendaCD ? Models.PostgreSql.TipoPedido.EGrupo.NaoRevendaCD : Models.PostgreSql.TipoPedido.EGrupo.NaoRevendaEDL);
            }
            await SendAsync($"{maskCount} tipos de pedidos atualizados na memória");

            await _postgreSqlContext.SaveChangesAsync();
            await SendAsync($"{maskCount} tipos de pedido atualizados no banco de dados");

            _postgreSqlContext.TipoPedidos.RemoveRange(tipoPedidos);
            await SendAsync($"{maskCount} tipos de pedido removidos");

            await _postgreSqlContext.SaveChangesAsync();
            await SendAsync($"{maskCount} tipos de pedido removido banco de dados");
        }

        private async Task MongoDb(int count)
        {
            var maskCount = count.ToString("n");

            var yourFilter = Builders<Models.MongoDb.TipoPedido>.Filter.Where(o => true);
            var countTipoPedidos = await _mongoDbContext.TipoPedidos.CountDocumentsAsync(yourFilter);

            await SendAsync($"<b>MongoDb - {maskCount}</b>");
            await SendAsync($"TipoPedido: {countTipoPedidos}");

            var tipoPedidos = new List<Models.MongoDb.TipoPedido>();
            for (int i = 0; i < count; i++)
                tipoPedidos.Add(CreateMongoDb(i));
            await SendAsync($"{maskCount} tipos de pedidos criados");

            await _mongoDbContext.TipoPedidos.InsertManyAsync(tipoPedidos);
            await SendAsync($"{maskCount} tipos de pedido adicionado ao context");

            tipoPedidos.Clear();
            await SendAsync($"Limpando memória");

            tipoPedidos = await _mongoDbContext.TipoPedidos.Find(yourFilter).ToListAsync();
            await SendAsync($"{tipoPedidos.Count:n} carregados");

            //var updates = new List<WriteModel<Models.MongoDb.TipoPedido>>();
            //var filterBuilder = Builders<Models.MongoDb.TipoPedido>.Filter;
            //var updateBuilder = Builders<Models.MongoDb.TipoPedido>.Update;

            foreach (var tipoPedido in tipoPedidos)
            {
                tipoPedido.Nome = "Updated " + tipoPedido.Nome;
                tipoPedido.Grupo = tipoPedido.Grupo == Models.MongoDb.TipoPedido.EGrupo.RevendaCD ? Models.MongoDb.TipoPedido.EGrupo.NaoRevendaCD : Models.MongoDb.TipoPedido.EGrupo.NaoRevendaEDL;

                //var filter = filterBuilder.Where(x => x.Id == new Guid(tipoPedido.Id.ToString()));
                //var update = updateBuilder.Set(w => w.Nome, value: tipoPedido.Nome);
                //Builders<Models.MongoDb.TipoPedido>.Update.Set(w => w.Grupo, tipoPedido.Grupo);
                //updates.Add(new ReplaceOneModel<Models.MongoDb.TipoPedido>(filter, tipoPedido) { });
                await _mongoDbContext.TipoPedidos.ReplaceOneAsync(w => w.Id == tipoPedido.Id, tipoPedido);
            }
            await SendAsync($"{maskCount} tipos de pedidos atualizados na memória com builder");

            //await _mongoDbContext.TipoPedidos.BulkWriteAsync(updates);
            //await SendAsync($"{maskCount} tipos de pedido atualizados no banco de dados");

            await _mongoDbContext.TipoPedidos.DeleteManyAsync(yourFilter);
            await SendAsync($"{maskCount} tipos de pedido removidos");
        }

        private Models.PostgreSql.TipoPedido CreatePostgreSql(int i)
        {
            Random rnd = new Random();

            return new Models.PostgreSql.TipoPedido(aspNetUserInsertId: Guid.NewGuid(),
                nome: $"Tipo pedido {i}",
                descricao: $"Descrição {i}",
                grupo: rnd.Next(10) % 2 == 0 ? Models.PostgreSql.TipoPedido.EGrupo.RevendaCD : Models.PostgreSql.TipoPedido.EGrupo.RevendaEDL,
                revendaNaoRevenda: Models.PostgreSql.TipoPedido.ERevendaNaoRevenda.Revenda,
                entradaSaida: rnd.Next(10) % 2 == 0 ? Models.PostgreSql.TipoPedido.EEntradaSaida.Entrada : Models.PostgreSql.TipoPedido.EEntradaSaida.Saida,
                financeiro: rnd.Next(10) % 2 == 0,
                emiteNF: rnd.Next(10) % 2 == 0,
                estoque: rnd.Next(10) % 2 == 0,
                contabiliza: rnd.Next(10) % 2 == 0,
                portalAdm: rnd.Next(10) % 2 == 0,
                portalLoja: rnd.Next(10) % 2 == 0,
                portalFornecedor: rnd.Next(10) % 2 == 0,
                portalLog: rnd.Next(10) % 2 == 0,
                logistica: rnd.Next(10) % 2 == 0,
                pdv: rnd.Next(10) % 2 == 0,
                precifica: rnd.Next(10) % 2 == 0,
                pedidoSaida: rnd.Next(10) % 2 == 0);
        }

        private Models.MongoDb.TipoPedido CreateMongoDb(int i)
        {
            Random rnd = new Random();

            var response = new Models.MongoDb.TipoPedido()
            {
                Id = Guid.NewGuid(),
                Nome = $"Tipo pedido {i}",
                Descricao = $"Descrição {i}",
                Grupo = rnd.Next(10) % 2 == 0 ? Models.MongoDb.TipoPedido.EGrupo.RevendaCD : Models.MongoDb.TipoPedido.EGrupo.RevendaEDL,
                RevendaNaoRevenda = Models.MongoDb.TipoPedido.ERevendaNaoRevenda.Revenda,
                EntradaSaida = rnd.Next(10) % 2 == 0 ? Models.MongoDb.TipoPedido.EEntradaSaida.Entrada : Models.MongoDb.TipoPedido.EEntradaSaida.Saida,
                Financeiro = rnd.Next(10) % 2 == 0,
                EmiteNF = rnd.Next(10) % 2 == 0,
                Estoque = rnd.Next(10) % 2 == 0,
                Contabiliza = rnd.Next(10) % 2 == 0,
                PortalAdm = rnd.Next(10) % 2 == 0,
                PortalLoja = rnd.Next(10) % 2 == 0,
                PortalFornecedor = rnd.Next(10) % 2 == 0,
                PortalLog = rnd.Next(10) % 2 == 0,
                Logistica = rnd.Next(10) % 2 == 0,
                PDV = rnd.Next(10) % 2 == 0,
                Precifica = rnd.Next(10) % 2 == 0,
                PedidoSaida = rnd.Next(10) % 2 == 0
            };

            return response;
        }

        private async Task SendAsync(string msg)
        {
            if (!Ultimo.HasValue)
                Ultimo = DateTime.Now;

            var diff = DateTime.Now - Ultimo.Value;
            Ultimo = DateTime.Now;

            await _hub.Clients.All.SendAsync("msg", $"[{diff:mm\\:ss\\.fff}] - {msg}");
        }

        protected override void Dispose(bool disposing)
        {
            _postgreSqlContext?.Dispose();
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}

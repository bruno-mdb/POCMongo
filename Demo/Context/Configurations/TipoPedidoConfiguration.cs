using Demo.Models.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Context.Configurations
{
    public class TipoPedidoConfiguration : IEntityTypeConfiguration<TipoPedido>
    {
        public void Configure(EntityTypeBuilder<TipoPedido> builder)
        {
            builder.ToTable("TipoPedido");

            // Chave primária.
            builder.HasKey(c => c.Id);

            // Campos
            builder.Property(c => c.AspNetUserInsertId);
            builder.Property(c => c.Inserted);
            builder.Property(c => c.Updated);

            builder.Property(c => c.Nome).HasMaxLength(60).IsRequired();
            builder.Property(c => c.Descricao).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Grupo);
            builder.Property(c => c.RevendaNaoRevenda);
            builder.Property(c => c.EntradaSaida);
            builder.Property(c => c.Financeiro);
            builder.Property(c => c.EmiteNF);
            builder.Property(c => c.Estoque);
            builder.Property(c => c.Contabiliza);
            builder.Property(c => c.PortalAdm);
            builder.Property(c => c.PortalLoja);
            builder.Property(c => c.PortalLog);
            builder.Property(c => c.Logistica);
            builder.Property(c => c.PDV);
            builder.Property(c => c.Precifica);
            builder.Property(c => c.PedidoSaida);
        }
    }
}

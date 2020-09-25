using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Models
{
    public abstract class BaseModel
    {
        public BaseModel()
        {
            // Para o mongodb
        }

        public BaseModel(Guid aspNetUserInsertId)
        {
            Id = Guid.NewGuid();
            AspNetUserInsertId = aspNetUserInsertId;
            Inserted = DateTime.UtcNow;
        }

        public BaseModel(Guid id, Guid aspNetUserInsertId)
        {
            Id = id;
            AspNetUserInsertId = aspNetUserInsertId;
            Inserted = DateTime.UtcNow;
        }

        public BaseModel(Guid id, Guid aspNetUserInsertId, DateTime inserted)
        {
            Id = id;
            AspNetUserInsertId = aspNetUserInsertId;
            Inserted = inserted;
        }

        [BsonId]
        public Guid Id { get; set; }

        /// <summary>
        /// Id do Usuário que criou o registro.
        /// </summary>
        public Guid AspNetUserInsertId { get; set; }

        public DateTime Inserted { get; set; }

        public DateTime? Updated { get; set; }

        public DateTime? Deleted { get; set; }

        #region Campos calculados

        /// <summary>
        /// Identificador do registro para exibição em tela, normalmente sendo o nome.
        /// </summary>
        public virtual string Identificador => null;

        #endregion

        #region Métodos públicos

        /// <summary>
        /// Utilizar apenas no SaveChangesAsync e métodos "bulk" do repositório.
        /// </summary>
        public void Update() => Updated = DateTime.UtcNow;

        /// <summary>
        /// Utilizar apenas no SaveChangesAsync e métodos "bulk" do repositório.
        /// </summary>
        public void Delete() => Deleted = DateTime.UtcNow;

        /// <summary>
        /// Implementar o 'Equals' se for necessário usá-lo.
        /// </summary>
        public override bool Equals(object obj)
        {
            var compareTo = obj as BaseModel;

            if (this is null && compareTo is null) return true;
            if (this is null || compareTo is null) return false;

            return Id == compareTo.Id;
        }

        /// <summary>
        /// Monta HashCode incluindo também o HashCode do Id.
        /// </summary>
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        /// <summary>
        /// Possiblita visualizar, além do nome da classe, o seu Id. Para ser usado em logs, exceptions, etc.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Nome: {0}, Id: {1}, {2}", GetType().Name, Id, base.ToString());
        }

        #endregion

        #region Operadores

        /// <summary>
        /// Verifica se são a mesma instância. Se forem instâncias diferentes, verifica se o Id é o mesmo.
        /// </summary>
        public static bool operator ==(BaseModel obj1, BaseModel obj2)
        {
            return Equals(obj1, obj2);
        }

        /// <summary>
        /// Negativa do operator "==" sobrescrito.
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator !=(BaseModel obj1, BaseModel obj2)
        {
            return !Equals(obj1, obj2);
        }

        #endregion
    }
}

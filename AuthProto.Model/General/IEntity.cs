using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthProto.Model.General
{
    public interface IEntity
    {
        Guid Id { get; set; }

        Guid ProjectId { get; set; }
        Guid CreatorId { get; set; }
    }

    public class Entity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid ProjectId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid CreatorId { get; set; }
    }
}

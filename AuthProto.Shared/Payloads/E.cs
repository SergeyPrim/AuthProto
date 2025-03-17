namespace AuthProto.Shared.Payloads
{
    /// <summary>
    /// Описание ошибки при выполнении операции.
    /// </summary>
    public class E
    {
        /// <summary>
        /// Код ошибки.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Расшифровка ошибки.
        /// </summary>
        public string Description { get; set; }

        public E(string id, string description)
        {
            Id = id;
            Description = description;
        }

        public E(string description)
        {
            Id = "E0000000";
            Description = description;
        }

        public static implicit operator E((string Id, string Description) error) => new E(error.Id, error.Description);

        public static implicit operator E(string Description) => new E(Description);

        public override string ToString() => !string.IsNullOrEmpty(Id) ? $"{Id} : {Description}" : Description;
    }
}

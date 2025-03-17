namespace AuthProto.Shared.Payloads
{
    /// <summary>
    /// Обертка для результата выполнения операции.
    /// Если операция выполнена успешно тогда свойство "value" содержит результат выполнения и свойство "IsFailure" равно "false".
    /// Если при выполнении операции произошла ошибка тогда свойство "IsFailure" равно "true" и "Failures" содержит список ошибок.
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого операцией значения.</typeparam>
    public class R<T>
    {
        /// <summary>
        /// Список ошибок если при выполнении операции произошли ошибки.
        /// </summary>
        public List<E> Failures { get; set; } = new List<E>();

        /// <summary>
        /// Возвращаемое операцией значение если при ее выполнении ошибок не было.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Признак наличия ошибок.
        /// </summary>
        public bool IsSuccess { get { return Failures.Count == 0; } }

        /// <summary>
        /// Признак наличия ошибок.
        /// </summary>
        public bool IsFailure => !IsSuccess;

        private R()
        {
        }

        private R(List<E> errors)
        {
            Failures = errors;
        }

        private R(E error)
        {
            Failures.Add(error);
        }

        private R(T value)
        {
            Value = value;
        }

        public static implicit operator R<T>(E error) => new R<T>(error);
        public static implicit operator R<T>(string Description)
        =>
            new R<T>(
                new E(Description));
        public static implicit operator R<T>(
            (string Identifier,
             string Description) error)
        =>
            new R<T>(
                new E(
                    error.Identifier,
                    error.Description));

        public static implicit operator R<T>(List<E> errors) => new R<T>(errors);
        public static implicit operator R<T>(
            List<string> errors
        ) =>
            errors.ConvertAll(e => new E(e));
        public static implicit operator R<T>(
            List<
                (string Identifier,
                 string Description)
                > errors
        ) =>
            errors.ConvertAll(e => new E(
                e.Identifier,
                e.Description
            ));

        public static implicit operator R<T>(T value) => new R<T>(value);
    }
}

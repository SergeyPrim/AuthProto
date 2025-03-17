namespace AuthProto.Shared.Payloads
{
    public class ListResult<T>
    {
        private long _count = 0;

        public long Count
        {
            set { if (value > 0) _count = value; }
            get => _count == 0 && Items != null && Items.Count > 0 ? Items.Count : _count;
        }

        public IList<T> Items { get; set; } = new List<T>();
    }
}

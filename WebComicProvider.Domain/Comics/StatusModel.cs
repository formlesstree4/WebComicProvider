namespace WebComicProvider.Domain.Comics
{
    public sealed record StatusModel(int StatusId, string StatusName)
    {
        public StatusModel() : this(default, string.Empty) { }
    }
}

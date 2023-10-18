namespace Sibers.Entities.Base
{
    public record Entity<TId>
    {
        public TId Id { get; set; }
    }
}
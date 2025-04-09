namespace Domain.Common;

public abstract class EntityBase<TKey>
{

    public TKey Id { get; set; } = default!;

    public bool IsDeleted { get; set; } = false;
}

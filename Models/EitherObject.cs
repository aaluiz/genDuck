namespace Models
{
    public readonly record struct EitherObject<T>(Exception Exception, T Result, bool IsExpection);
}

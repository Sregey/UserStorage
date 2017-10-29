namespace UserStorageServices
{
    internal interface IValidator<T>
    {
        void Validate(T item);
    }
}

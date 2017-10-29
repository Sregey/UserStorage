namespace UserStorageServices.Validation
{
    internal interface IValidator<T>
    {
        void Validate(T item);
    }
}

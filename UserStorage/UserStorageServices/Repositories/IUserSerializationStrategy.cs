namespace UserStorageServices.Repositories
{
    public interface IUserSerializationStrategy
    {
        void SerializeUsers(string repositoryFileName, DataSetForUserRepository dataSet);

        DataSetForUserRepository DeserializeUsers(string repositoryFileName);
    }
}

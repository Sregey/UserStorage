using System;

namespace UserStorageServices
{
    internal class IdGenerator : IIdGenerator
    {
        private Guid lastId;

        public IdGenerator(Guid lastId)
        {
            this.lastId = lastId;
        }

        public Guid Generate()
        {
            IncrementId();
            return lastId;
        }

        private void IncrementId()
        {
            var idBytes = lastId.ToByteArray();

            int count = idBytes.Length - 1;
            byte transfer = 0;
            if (idBytes[count] < 255)
            {
                idBytes[count]++;
            }
            else
            {
                idBytes[count] = 0;
                transfer = 1;
            }

            for (int i = count - 1; i >= 0; i--)
            {
                if (transfer == 1)
                {
                    if (idBytes[i] < 255)
                    {
                        idBytes[i]++;
                        transfer = 0;
                    }
                    else
                    {
                        idBytes[i] = 0;
                    }
                }
                else
                {
                    break;
                }
            }

            lastId = new Guid(idBytes);
        }
    }
}

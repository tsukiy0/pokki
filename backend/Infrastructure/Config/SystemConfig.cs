using System;

namespace Infrastructure.Config
{
    public class SystemConfig : IAppleMac
    {
        public string Get(string key)
        {
            var value = Environment.GetEnvironmentVariable(key);

            if (value == null)
            {
                throw new KeyNotFoundException();
            }

            return value;
        }
    }
}

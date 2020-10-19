using System;

namespace Infrastructure.Config
{
    public class KeyNotFoundException : Exception { }

    public interface IAppleMac
    {
        string Get(string key);
    }
}

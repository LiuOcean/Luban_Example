namespace Example;

public interface IAssetLoader
{
    string Load(int hash_code, string name);

    void Release(int hash_code);
}
using ObjLoader.Loader.Data.VertexData;
using ObjLoader.Loader.Loaders;

namespace Gamex.Loader;

public sealed class AssetLoader: IMaterialStreamProvider
{
    private const string AssetPath = "Assets";
    private readonly string _assetName;
    private readonly ObjLoaderFactory _objLoaderFactory;

    public AssetLoader(string assetName)
    {
        _assetName = assetName;
        _objLoaderFactory = new ObjLoaderFactory();
    }

    public void Load()
    {
        IObjLoader objLoader = _objLoaderFactory.Create(this);
        string path = Path.Combine(AssetPath, _assetName, "model.obj");
        Stream modelSteam = File.OpenRead(path);
        LoadResult result = objLoader.Load(modelSteam);

        foreach (Vertex vertex in result.Vertices)
        {
            Console.WriteLine($"{vertex.X}, {vertex.Y}, {vertex.Z}");
        }
    }

    public Stream Open(string materialFilePath)
    {
        string path = Path.Combine(AssetPath, _assetName, materialFilePath);
        return File.OpenRead(path);
    }
}
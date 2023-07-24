using Gamex.DataObjects;
using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Loaders;

namespace Gamex.Loader;

public sealed class AssetLoader : IMaterialStreamProvider
{
    private const string AssetPath = "Assets";
    private readonly string _assetName;
    private readonly ObjLoaderFactory _objLoaderFactory;

    public AssetLoader(string assetName)
    {
        _assetName = assetName;
        _objLoaderFactory = new ObjLoaderFactory();
    }

    public LoadResult Load()
    {
        IObjLoader objLoader = _objLoaderFactory.Create(this);
        string path = Path.Combine(AssetPath, _assetName, "model.obj");
        Stream modelSteam = File.OpenRead(path);
        LoadResult result = objLoader.Load(modelSteam);
        return result;
    }

    public VertexPack Pack(LoadResult res)
    {
        var vertex = res.Vertices;
        float[] vertices = new float[vertex.Count * 3];

        for (int i = 0; i < vertex.Count; i++)
        {
            vertices[i * 3] = vertex[i].X;
            vertices[i * 3 + 1] = vertex[i].Y;
            vertices[i * 3 + 2] = vertex[i].Z;
        }

        int facesLength = res.Groups.Sum(t => t.Faces.Count);
        uint[] indices = new uint[facesLength * 3];
        var groups = res.Groups;

        foreach (Group t in groups)
        {
            Console.WriteLine("Group: {0}", t.Name);
            for (int i = 0; i < t.Faces.Count; i++)
            {
                Face x = t.Faces[i];
                indices[i * 3] = (uint) x[0].VertexIndex;
                indices[i * 3 + 1] = (uint) x[1].VertexIndex;
                indices[i * 3 + 2] = (uint) x[2].VertexIndex;
            }
        }
        return new VertexPack(vertices, indices);
    }

    public Stream Open(string materialFilePath)
    {
        string path = Path.Combine(AssetPath, _assetName, materialFilePath);
        return File.OpenRead(path);
    }
}
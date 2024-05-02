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

        var indices = new List<uint>();
        var groups = res.Groups;

        foreach (Group t in groups)
        {
            Console.WriteLine("Group: {0}", t.Name);
            foreach (Face x in t.Faces)
            {
                if (x.Count == 4)
                {
                   indices.Add((uint) x[0].VertexIndex - 1);
                   indices.Add((uint) x[2].VertexIndex - 1);
                   indices.Add((uint) x[3].VertexIndex - 1);
                }

                indices.Add((uint) x[0].VertexIndex - 1);
                indices.Add((uint) x[1].VertexIndex - 1);
                indices.Add((uint) x[2].VertexIndex - 1);
            }
        }
        return new VertexPack(vertices, indices.ToArray());
    }

    public Stream Open(string materialFilePath)
    {
        string path = Path.Combine(AssetPath, _assetName, materialFilePath);
        return File.OpenRead(path);
    }
}
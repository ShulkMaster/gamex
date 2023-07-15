using Gamex.Exceptions;
using OpenTK.Graphics.OpenGL;

namespace Gamex.Program;

public sealed class GlProgramBuilder
{
    private VertexShader? _vShader;
    private FragmentShader? _fShader;

    public GlProgramBuilder AttachVertex(VertexShader shader) 
    {
        _vShader = shader;
        return this;
    }

    public GlProgramBuilder AttachFragment(FragmentShader shader)
    {
        _fShader = shader;
        return this;
    }

    public GlProgram Build()
    {
        int handle = GL.CreateProgram();
        AttachShader(handle, _vShader);
        AttachShader(handle, _fShader);
        GL.LinkProgram(handle);
        GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int success);

        DetachShader(handle, _vShader);
        DetachShader(handle, _fShader);

        if (success != 0) return new GlProgram(handle);
        string infoLog = GL.GetProgramInfoLog(handle);
        throw new ProgramLinkException(infoLog);
    }

    private static void AttachShader(int handle, Shader? shader)
    {
        if (shader is null) return;
        GL.AttachShader(handle, shader.GetHandle());
    }

    private static void DetachShader(int handle, Shader? shader)
    {
        if (shader is null) return;
        GL.DetachShader(handle, shader.GetHandle());
        shader.Dispose();
    }
}
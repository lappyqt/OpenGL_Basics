using OpenTK.Graphics.ES30;

public class Shader : IDisposable
{
    private bool disposedValue = false;

    private int Handle;

    public Shader(string vertexPath, string fragmentPath)
    {
        (int, int) shaders = Initialize(vertexPath, fragmentPath);
        CompileIndividualShaders(shaders);
    }

    ~Shader()
    {
        throw new Exception("GPU Resource Leak. Possible missed Dispose method call.");    
    }

    public void Use() => GL.UseProgram(Handle);
    
    private (int, int) Initialize(string vertexPath, string fragmentPath)
    {
        string vertexSource = File.ReadAllText(vertexPath);
        string fragmentSource = File.ReadAllText(fragmentPath);

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(vertexShader, vertexSource);
        GL.ShaderSource(fragmentShader, fragmentSource);

        return (vertexShader, fragmentShader);
    }

    private void CompileIndividualShaders((int, int) shaders)
    {
        int vertexShader = shaders.Item1;
        int fragmentShader = shaders.Item2;

        GL.CompileShader(vertexShader);
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vertexSuccess);

        if (vertexSuccess == 0)
        {
            throw new Exception(GL.GetShaderInfoLog(vertexShader));
        }

        GL.CompileShader(fragmentShader);
        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fragmentSuccess);

        if (fragmentSuccess == 0)
        {
            throw new Exception(GL.GetShaderInfoLog(fragmentShader));
        }

        Handle = GL.CreateProgram();
        
        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);

        GL.LinkProgram(Handle);
        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int linkSuccess);

        if (linkSuccess == 0)
        {
            throw new Exception(GL.GetProgramInfoLog(Handle));
        }

        GL.DetachShader(Handle, vertexShader);
        GL.DetachShader(Handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            GL.DeleteProgram(Handle);
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
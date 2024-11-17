using OpenTK.Graphics.ES30;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenGLBasics.Triangle;

public class Game(int width, int height, string title) : GameWindow(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
{
    private readonly float[] _vertices = 
    [
        // positions          colors
        -0.20f, -0.25f, 0.0f, 1.0f, 0.0f, 0.0f,
        0.20f, -0.25f, 0.0f,  0.0f, 1.0f, 0.0f,
        0.0f, 0.25f, 0.0f,    0.0f, 0.0f, 1.0f
    ];
    
    private int _vertexBufferObject;
    private int _vertexArrayObject;
    private Shader? _shader;

    private void DrawTriangle()
    {
        _shader!.Use();
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        _vertexBufferObject = GL.GenBuffer();
        _vertexArrayObject = GL.GenVertexArray();
        _shader = new Shader("shaders/shader.vert", "shaders/shader.frag");

        GL.ClearColor(0.169f, 0.169f, 0.169f, 1.0f);

        GL.BindVertexArray(_vertexArrayObject);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        _shader.Use();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        _shader!.Dispose();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);
        DrawTriangle();
        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }
}
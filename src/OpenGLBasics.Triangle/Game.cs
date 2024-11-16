using OpenTK.Graphics.ES30;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

public class Game(int width, int height, string title) : GameWindow(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
{
    private readonly float[] vertices = 
    [
        -0.20f, -0.25f, 0.0f,
        0.20f, -0.25f, 0.0f,
        0.0f, 0.25f, 0.0f
    ];

    private int VertexBufferObject;
    private int VertexArrayObject;
    private Shader? shader;

    private void DrawTriangle()
    {
        shader!.Use();
        GL.BindVertexArray(VertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        VertexBufferObject = GL.GenBuffer();
        VertexArrayObject = GL.GenVertexArray();
        shader = new Shader("shaders/shader.vert", "shaders/shader.frag");

        GL.ClearColor(0.169f, 0.169f, 0.169f, 1.0f);

        GL.BindVertexArray(VertexArrayObject);

        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        shader.Use();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        shader!.Dispose();
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
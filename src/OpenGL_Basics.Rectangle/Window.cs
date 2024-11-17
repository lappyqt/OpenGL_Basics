using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

public class Window : GameWindow 
{
    public Window(int width, int height, string title): base(new GameWindowSettings(), new NativeWindowSettings { ClientSize = (width, height), Title = title})
    {}

    private readonly float[] vertices = 
    [
        -0.25f, -0.25f, 0.0f, // bottom left
        0.25f, -0.25f, 0.0f, // bottom right
        -0.25f, 0.25f, 0.0f, // top left
        0.25f, 0.25f, 0.0f // top right
    ];

    private readonly uint[] indices = 
    [
        0, 2, 3, // first triangle
        0, 1, 3 // second triangle
    ];

    int vertexBufferObject;
    int elementBufferObject;
    int vertexArrayBuffer;

    protected override void OnLoad()
    {
        base.OnLoad();

        vertexBufferObject = GL.GenBuffer();
        elementBufferObject = GL.GenBuffer();
        vertexArrayBuffer = GL.GenVertexArray();

        GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);

        GL.BindVertexArray(vertexArrayBuffer);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        DrawRectangle();

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }

    private void DrawRectangle()
    {
        GL.BindVertexArray(vertexArrayBuffer);
        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
    }
}
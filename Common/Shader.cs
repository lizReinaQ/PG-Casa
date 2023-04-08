using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace CasaTarea.Common
{
    public class Shader
    {
        public readonly int _handle;

        public Shader(){
            
        }

       public Shader(string vertexShaderPath, string fragmentShaderPath)
{
    // Crear shaders de vértices y fragmentos
    int vertexShader = GL.CreateShader(ShaderType.VertexShader);
    int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
    
    // Cargar código de los archivos de shader
    string vertexShaderCode = File.ReadAllText(vertexShaderPath);
    string fragmentShaderCode = File.ReadAllText(fragmentShaderPath);
    
    // Compilar y enlazar shaders
    GL.ShaderSource(vertexShader, vertexShaderCode);
    GL.CompileShader(vertexShader);
    // Verificar errores de compilación
    string infoLog = GL.GetShaderInfoLog(vertexShader);
    if (!string.IsNullOrEmpty(infoLog))
    {
        throw new Exception($"Error de compilación en shader de vértices: {infoLog}");
    }
    
    GL.ShaderSource(fragmentShader, fragmentShaderCode);
    GL.CompileShader(fragmentShader);
    // Verificar errores de compilación
    infoLog = GL.GetShaderInfoLog(fragmentShader);
    if (!string.IsNullOrEmpty(infoLog))
    {
        throw new Exception($"Error de compilación en shader de fragmentos: {infoLog}");
    }
    
    _handle = GL.CreateProgram();
    GL.AttachShader(_handle, vertexShader);
    GL.AttachShader(_handle, fragmentShader);
    GL.LinkProgram(_handle);
    // Verificar errores de enlazado
    infoLog = GL.GetProgramInfoLog(_handle);
    if (!string.IsNullOrEmpty(infoLog))
    {
        throw new Exception($"Error de enlazado de shaders: {infoLog}");
    }
    
    // Eliminar shaders que ya no se necesitan
    GL.DetachShader(_handle, vertexShader);
    GL.DetachShader(_handle, fragmentShader);
    GL.DeleteShader(vertexShader);
    GL.DeleteShader(fragmentShader);
}

        public void Use()
        {
            GL.UseProgram(_handle);
        }

        public int GetAttributeLocation(string name)
        {
            return GL.GetAttribLocation(_handle, name);
        }

        public int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(_handle, name);
        }

        public void SetMatrix4(string name, OpenTK.Mathematics.Matrix4 matrix)
        {
            int location = GetUniformLocation(name);
            GL.UniformMatrix4(location, false, ref matrix);
        }

        private static string LoadShaderSource(string path)
        {
            using (var reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }

        private static int CompileShader(ShaderType type, string source)
        {
            int handle = GL.CreateShader(type);
            GL.ShaderSource(handle, source);
            GL.CompileShader(handle);

            string log = GL.GetShaderInfoLog(handle);
            if (!string.IsNullOrWhiteSpace(log))
            {
                Console.WriteLine(log);
            }

            return handle;
        }

        public void Dispose()
        {
            GL.DeleteProgram(_handle);
        }
    }
}

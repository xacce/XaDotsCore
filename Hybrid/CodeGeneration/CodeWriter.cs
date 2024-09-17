#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Core.Hybrid.Hybrid.CodeGeneration
{
    public class CodeWriter
    {
        private const int kSpacesPerIndentLevel = 4;
        public StringBuilder buffer;
        public int indentLevel;

        public CodeWriter()
        {
            this.buffer = new StringBuilder();
        }

        public void BeginBlock()
        {
            WriteIndent();
            buffer.Append("{\n");
            ++indentLevel;
        }

        public void EndBlock()
        {
            --indentLevel;
            WriteIndent();
            buffer.Append("}\n");
        }

        public void WriteLine()
        {
            buffer.Append('\n');
        }

        public void WriteLine(string text)
        {
            if (!text.All(char.IsWhiteSpace))
            {
                WriteIndent();
                buffer.Append(text);
            }

            buffer.Append('\n');
        }

        public void Write(string text)
        {
            buffer.Append(text);
        }

        public void WriteIndent()
        {
            for (var i = 0; i < indentLevel; ++i)
            {
                for (var n = 0; n < kSpacesPerIndentLevel; ++n)
                    buffer.Append(' ');
            }
        }

        public void WriteEnum(string name, List<(int, string)> data)
        {
            WriteLine($"public enum {name}");
            BeginBlock();
            foreach (var kv in data)
            {
                var stringKey = kv.Item2.Replace(" ", "_").ToUpper();
                WriteLine($"{stringKey}={kv.Item1},");
            }

            EndBlock();
        }

        public static void WriteToFile(CodeWriter writer, string filename)
        {
            var savePath = Path.Join(Application.dataPath, "_GENERATED", $"{filename}.cs");
            var code = writer.buffer.ToString();
            if (File.Exists(savePath))
            {
                var existingCode = File.ReadAllText(savePath);
                if (existingCode == code)
                    return;
                AssetDatabase.MakeEditable(savePath);
            }

            AssetDatabase.MakeEditable(savePath);
            File.WriteAllText(savePath, code);
        }
    }
}
#endif
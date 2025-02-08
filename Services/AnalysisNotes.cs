using System;
using NoteSait.Services.Interfaces;
using Microsoft.Scripting.Hosting;
using NoteSait.Models;
using System.Diagnostics;
using System.Text;

namespace NoteSait.Services;
public enum StateAnalysis
{
    OK,
    AnalysisFailed,
    Error
}

public class AnalysisNotes : IAnalysisNotes
{
    public AnalysisNotes() { }

    public StateAnalysis GetPhotosForAnalysis(string[] paths, string endFilePath)
    {
        string pythonExe = "/usr/bin/python3"; // Path python
        string scriptPath = "PythonFiles/main.py"; // вот тут надо указать относительный путь к python файлу

        foreach (var image in paths)
        {
            if (!File.Exists(image))
            {
                return StateAnalysis.Error;
            }
        }

        // Формируем строку аргументов: список путей + путь к выходному файлу
        string arguments = $"\"{scriptPath}\" " + string.Join(" ", paths) + $" \"{endFilePath}\"";

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = pythonExe,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };

        Process process = new Process { StartInfo = psi };
        var a = process.Start();
        process.WaitForExit();
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        if (output != "" || error != "")
        {
            return StateAnalysis.AnalysisFailed;
        }
        if (File.Exists(endFilePath))
        {
            return StateAnalysis.OK;
        }
        else
        {
            return StateAnalysis.AnalysisFailed;
        }
    }

}

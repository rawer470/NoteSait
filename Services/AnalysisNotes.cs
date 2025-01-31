using System;
using NoteSait.Services.Interfaces;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using NoteSait.Models;

namespace NoteSait.Services;
public enum StateAnalysis{
    OK,
    AnalysisFailed,
    Error
}

public class AnalysisNotes : IAnalysisNotes
{
    public AnalysisNotes() { }

    public StateAnalysis GetPhotosForAnalysis(string[] paths, string endFilePath)
    {
        //Logic Analysis
        return StateAnalysis.OK;
    }

}

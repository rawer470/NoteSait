using System;

namespace NoteSait.Services.Interfaces;

public interface IAnalysisNotes
{
    public StateAnalysis GetPhotosForAnalysis(string[] paths, string endFilePath);
}

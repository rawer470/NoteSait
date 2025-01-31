using System;

namespace NoteSait.Models;
public class DownloadedFile
{
    public StateExc State { get; set; }
    public byte[] BytesFile { get; set; }
    public string FileName { get; set; }
}

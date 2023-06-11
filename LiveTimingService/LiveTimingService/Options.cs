namespace ODR.LiveTiming.UploadService;

public class Options
{
    public string SourceFilePath { get; set; }
    public string DestinationFolderPath { get; set; }
    public string AuthorName { get; set; }
    public string AuthorEmail { get; set; }
    public string CommitMessage { get; set; }
    public int FrequencyInSeconds { get; set; }
}
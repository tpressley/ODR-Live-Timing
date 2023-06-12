namespace ODR.LiveTiming.UploadService;

public class Options
{
    public string RepositoryPath { get; set; }
    public string SourceFilePath { get; set; }
    public string DestinationFolderPath { get; set; }
    public string AuthorName { get; set; }
    public string AuthorEmail { get; set; }
    public string CommitMessage { get; set; }
    public int FrequencyInSeconds { get; set; }
    public int Username { get; set; }
    public int Password { get; set; }
}
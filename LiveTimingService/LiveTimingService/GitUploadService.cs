using LibGit2Sharp;
namespace ODR.LiveTiming.UploadService;

public sealed class GitUploadService
{
    public GitUploadService(Options options) => Options = options;
    private readonly Options Options;
    public void MoveFileAndCommit()
    {
        try
        {
            // Move the file to the destination folder
            string fileName = Path.GetFileName(Options.SourceFilePath);
            string destinationFilePath = Path.Combine(Options.DestinationFolderPath, fileName);
            File.Move(Options.DestinationFolderPath, destinationFilePath);

            // Commit the file to the Git repository
            using (var repo = new Repository(Options.DestinationFolderPath))
            {
                Commands.Stage(repo, destinationFilePath);
                Signature author = new Signature(Options.AuthorName, Options.AuthorEmail, DateTime.Now);
                Signature committer = author;

                // Replace "Commit message" with your desired commit message
                repo.Commit(Options.CommitMessage, author, committer);
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the file move or commit process
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}

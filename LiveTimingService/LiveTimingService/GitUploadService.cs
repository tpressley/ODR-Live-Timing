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
            File.Copy(Options.SourceFilePath, Options.DestinationFolderPath, true);

            // Commit the file to the Git repository
            using (var repo = new Repository(Options.RepositoryPath))
            {
                Commands.Stage(repo, Options.DestinationFolderPath);
                Signature author = new Signature(Options.AuthorName, Options.AuthorEmail, DateTime.Now);
                Signature committer = author;

                // Replace "Commit message" with your desired commit message
                try
                {
                    repo.Commit(Options.CommitMessage, author, committer);
                }
                catch (Exception)
                {

                }
                var options = new PushOptions();
                options.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = Options.Username, Password = Options.Password};
                repo.Network.Push(repo.Head, options);
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the file move or commit process
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}

using LibGit2Sharp;
using LibGit2Sharp.Handlers;

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
                CredentialsHandler credentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = Options.Username, Password = Options.PersonalAccessToken };
                
                var fetchOptions = new FetchOptions { CredentialsProvider = credentialsProvider, };
                var mergeOptions = new MergeOptions { FailOnConflict = true, IgnoreWhitespaceChange = true };
                var pullOptions = new PullOptions() { FetchOptions = fetchOptions, MergeOptions = mergeOptions };
                Signature signature = new Signature(Options.AuthorName, Options.AuthorEmail, DateTime.Now);
                Commands.Pull(repo, signature, pullOptions);
                Commands.Stage(repo, Options.DestinationFolderPath);
                Signature committer = signature;

                // Replace "Commit message" with your desired commit message
                try
                {
                    repo.Commit(Options.CommitMessage, signature, committer);
                }
                catch (Exception ex)
                {
                    File.AppendAllText("log.txt", DateTime.Now + ex.Message  + ex.StackTrace + Environment.NewLine);
                }
                var pushOptions = new PushOptions();
                pushOptions.CredentialsProvider = credentialsProvider;
                repo.Network.Push(repo.Head, pushOptions);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

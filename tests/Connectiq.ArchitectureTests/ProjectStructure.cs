namespace Connectiq.ArchitectureTests;

public class ProjectStructure 
{
    static readonly string[] RequiredFolders = new[]
    {
        "Domain",
        "Events",
        "Infrastructure"
    };

    [Theory]
    [InlineData("CustomerWorker")]
    public void WorkerProject_ShouldContainRequiredFolders(string projectName)
    {
        var solutionRoot = FindSolutionRoot();
        Assert.False(string.IsNullOrEmpty(solutionRoot), "Could not locate the solution root (no .sln file found in parent directories).");

        var projectFolder = Directory.GetDirectories(solutionRoot, projectName, SearchOption.AllDirectories)
            .FirstOrDefault();

        Assert.False(string.IsNullOrEmpty(projectFolder), $"Project folder '{projectName}' not found in solution.");

        foreach (var folder in RequiredFolders)
        {
            var fullPath = Path.Combine(projectFolder!, folder);
            Assert.True(Directory.Exists(fullPath), $"Missing required folder '{folder}' in project: {projectFolder}");
        }
    }

    string FindSolutionRoot()
    {
        var currentDir = AppContext.BaseDirectory;

        while (!string.IsNullOrEmpty(currentDir))
        {
            if (Directory.GetFiles(currentDir, "*.sln").Any())
                return currentDir;

            currentDir = Directory.GetParent(currentDir)?.FullName;
        }

        return null!;
    }
}
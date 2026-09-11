using System.Diagnostics;

namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    private static string RunGit(string workingDirectory, params string[] arguments)
    {
        return RunGitCommand(workingDirectory, settings: null, GitDryRunMode.None, arguments).StandardOutput;
    }

    private static GitCommandResult RunGitCommand(
        string workingDirectory,
        GitCommandSettings? settings,
        GitDryRunMode dryRunMode = GitDryRunMode.None,
        params string[] arguments)
    {
        settings ??= new GitCommandSettings();
        var directories = GetRunDirectories(workingDirectory, settings.RunType);
        var results = directories.Select(directory => RunGitOnce(directory, settings.DryRun, dryRunMode, arguments));
        return CombineResults(results);
    }

    private static GitCommandResult RunGitOnce(
        string workingDirectory,
        bool dryRun,
        GitDryRunMode dryRunMode,
        string[] arguments)
    {
        var commandArguments = dryRun && dryRunMode == GitDryRunMode.Supported
            ? arguments.Take(1).Concat(new[] { "--dry-run" }).Concat(arguments.Skip(1)).ToArray()
            : arguments;
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                WorkingDirectory = Path.GetFullPath(workingDirectory),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        foreach (var argument in commandArguments)
        {
            process.StartInfo.ArgumentList.Add(argument);
        }

        process.Start();
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        var result = new GitCommandResult(
            process.ExitCode == 0,
            process.ExitCode,
            output.TrimEnd(),
            error.TrimEnd());
        if (!result.Succeeded)
            throw new InvalidOperationException($"git {string.Join(' ', commandArguments)} failed: {result.StandardError}");
        return result;
    }

    private static string RunGitOnce(string workingDirectory, string[] arguments)
    {
        return RunGitOnce(workingDirectory, dryRun: false, GitDryRunMode.None, arguments).StandardOutput;
    }

    private static string RunGit(
        string workingDirectory,
        GitCommandSettings? settings,
        params string[] arguments)
    {
        return RunGitCommand(workingDirectory, settings, GitDryRunMode.None, arguments).StandardOutput;
    }

    private static GitCommandResult RunGitResult(
        string workingDirectory,
        GitCommandSettings? settings,
        params string[] arguments)
    {
        return RunGitCommand(workingDirectory, settings, GitDryRunMode.None, arguments);
    }

    private static GitCommandResult CombineResults(IEnumerable<GitCommandResult> results)
    {
        var resultList = results.ToArray();
        return new GitCommandResult(
            resultList.All(result => result.Succeeded),
            resultList.FirstOrDefault(result => !result.Succeeded)?.ExitCode ?? 0,
            string.Join(Environment.NewLine, resultList.Select(result => result.StandardOutput).Where(output => !string.IsNullOrEmpty(output))),
            string.Join(Environment.NewLine, resultList.Select(result => result.StandardError).Where(error => !string.IsNullOrEmpty(error))));
    }

    private enum GitDryRunMode
    {
        None,
        Supported
    }

    private static IReadOnlyList<string> GetRunDirectories(string workingDirectory, GitCommandRunType runType)
    {
        var parentDirectory = Path.GetFullPath(workingDirectory);
        var directories = new List<string>();

        if ((runType & GitCommandRunType.Parent) != 0)
        {
            directories.Add(parentDirectory);
        }

        if ((runType & GitCommandRunType.Submodules) != 0)
        {
            directories.AddRange(GetSubmoduleDirectories(parentDirectory));
        }

        return directories;
    }

    private static IReadOnlyList<string> GetSubmoduleDirectories(string workingDirectory)
    {
        var directories = new List<string>();
        var pendingDirectories = new Queue<string>();
        var visitedDirectories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        pendingDirectories.Enqueue(workingDirectory);

        while (pendingDirectories.Count > 0)
        {
            var currentDirectory = pendingDirectories.Dequeue();
            if (!visitedDirectories.Add(currentDirectory))
            {
                continue;
            }

            var submoduleFile = Path.Combine(currentDirectory, ".gitmodules");
            if (!File.Exists(submoduleFile))
            {
                continue;
            }

            var paths = RunGitOnce(
                currentDirectory,
                dryRun: false,
                GitDryRunMode.None,
                new[] { "config", "--file", ".gitmodules", "--get-regexp", "path" });

            foreach (var path in paths.StandardOutput.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var separatorIndex = path.IndexOfAny(new[] { ' ', '\t' });
                if (separatorIndex < 0)
                {
                    continue;
                }

                var submoduleDirectory = Path.GetFullPath(
                    Path.Combine(currentDirectory, path[(separatorIndex + 1)..].Trim()));
                if (Directory.Exists(submoduleDirectory))
                {
                    directories.Add(submoduleDirectory);
                    pendingDirectories.Enqueue(submoduleDirectory);
                }
            }
        }

        return directories.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }
}
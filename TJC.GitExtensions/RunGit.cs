using System.Diagnostics;

namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    private static string RunGit(string workingDirectory, params string[] arguments)
    {
        return RunGit(workingDirectory, settings: null, arguments);
    }

    private static string RunGit(
        string workingDirectory,
        GitCommandSettings? settings,
        params string[] arguments)
    {
        settings ??= new GitCommandSettings();
        var directories = GetRunDirectories(workingDirectory, settings.RunType);
        var outputs = directories.Select(directory => RunGitOnce(directory, settings.DryRun, arguments));
        return string.Join(Environment.NewLine, outputs.Where(output => !string.IsNullOrEmpty(output)));
    }

    private static string RunGitOnce(string workingDirectory, bool dryRun, string[] arguments)
    {
        var commandArguments = dryRun
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

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"git {string.Join(' ', commandArguments)} failed: {error.Trim()}");
        }

        return output.TrimEnd();
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
                new[] { "config", "--file", ".gitmodules", "--get-regexp", "path" });

            foreach (var path in paths.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
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
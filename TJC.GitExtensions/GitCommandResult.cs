namespace TJC.GitExtensions;

public sealed record GitCommandResult(
    bool Succeeded,
    int ExitCode,
    string StandardOutput,
    string StandardError
);

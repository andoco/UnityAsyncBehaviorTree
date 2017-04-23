using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class RegexLineParser : ILineParser
{
    public void Parse(string line, out string name, out IDictionary<string, string> args)
    {
        try
        {
            name = this.ParseTaskName(line);
            args = this.ParseArgs(line);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(string.Format("Failed to parse the behavior line [{0}]", line), e);
        }
    }

    private string ParseTaskName(string line)
    {
        var trimmed = line.TrimStart('\t').TrimStart(' ');
        var nameRegex = new Regex(@"^(^\S+)");
        var name = nameRegex.Match(trimmed).Groups[0].Value;

        return name;
    }

    private IDictionary<string, string> ParseArgs(string line)
    {
        var argsRegex = new Regex(@"(\w+)=(?:""([^""]+)""|(\S+))");
        var argsMatches = argsRegex.Matches(line);

        var args = new Dictionary<string, string>();

        for (int i = 0; i < argsMatches.Count; i++)
        {
            var m = argsMatches[i];
            var k = m.Groups[1].Value;
            var v = m.Groups[2].Value;

            if (string.IsNullOrEmpty(v)) v = m.Groups[3].Value;

            args.Add(k, v);
        }

        return args;
    }
}

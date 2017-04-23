using System.Collections.Generic;

public interface ILineParser
{
    void Parse(string line, out string name, out IDictionary<string, string> args);
}

using System;
using System.Diagnostics;
using System.IO;

public class ConsoleLinkTool 
{
    [Needle.HyperlinkCallback]
    private static bool OnHyperlinkClicked(string path, string line)
    {

        if (File.Exists(path))
        {
            string argument = "/select, \"" + Path.GetFullPath(path) + "\"";
            Process.Start("explorer.exe", argument);
        }
        else
        {
            return false;
        }
        return true;
    }


}

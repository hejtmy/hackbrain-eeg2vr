using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class FileReader
{ 

    public static List<float> ReadEEG(string path)
    {
        var stringList = FileRead(path);
        List<float> EEGSignal = ConvertStrings(stringList);
        return EEGSignal;
    }

    public static List<string> FileRead(string path)
    {
        var logFile = File.ReadAllLines(path);
        List<string> logList = new List<string>(logFile);
        return logList;
    }

    static List<float> ConvertStrings(List<string> strings)
    {
        List<float> listFloats = strings.Select(s => float.Parse(s)).ToList();
        return listFloats;
    }
}

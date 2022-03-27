using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalisationSystem 
{
   public enum Language
    {
        English,
        Russian
    }
    public static Language language = Language.English;
    private static Dictionary<string, string> localisedEN;
    private static Dictionary<string, string> localisedRU;
    public static bool isInit;
    public static void Init()
    {
        CSVLoader csvLoader = new CSVLoader();
        csvLoader.LoadCSV();
        localisedEN = csvLoader.GetDictionaryValues("en");
        localisedRU = csvLoader.GetDictionaryValues("ru");
        isInit = true;
    }
    public static string GetLocalisedValue(string key)
    {
        if (!isInit) { Init(); }
        string value = key;
        switch (language)
        {
            case Language.English:
                localisedEN.TryGetValue(key, out value); break;
            case Language.Russian:
                localisedRU.TryGetValue(key, out value); break;
        }
        value = value.TrimStart(' ', '"'); value = value.Replace("\"", "");
        return value;
    }
    public static string TryGetLocalisedValue(string key)
    {
        if (!isInit) { Init(); }
        string value = key;
        try
        {
            switch (language)
            {
                case Language.English:
                    localisedEN.TryGetValue(key, out value); break;
                case Language.Russian:
                    localisedRU.TryGetValue(key, out value); break;
            }
            value = value.TrimStart(' ', '"'); value = value.Replace("\"", "");
        }
        catch
        {

        }
        return value;
    }
}

using System.Xml;

namespace MinecraftScreenshotsSender.XmlParsing;

public class XmlParser
{
    public string? GetNodeText(string path, string targetTag)
    {
        string query = string.Format(path);
        XmlDocument document = new XmlDocument();
        document.Load(query);
        
        XmlNodeList? xmlNodeList = document.SelectNodes(targetTag);

        return xmlNodeList is not { Count: 1 } 
            ? null 
            : xmlNodeList[0]?.InnerText;
    }
}
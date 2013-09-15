<Query Kind="Statements" />

var path = @"C:\Users\James\Projects\GitHub\PsnTrophies\games\";
//path = @"C:\Users\James\Projects\GitHub\PsnTrophies\_wip\xXREDXIIIXx\";
var xmlFiles = Directory.GetFiles(path, "*.xml");

//xmlFiles.Dump();

var data = new List<object>();

foreach (var xmlFile in xmlFiles)
{
	var xml = XDocument.Load(xmlFile).Root;
	var gameId = xml.Element("Game").Element("Id").Value;
	var platform = xml.Element("Game").Element("Platform").Value;
	/*if (platform == "vita") {
		xml.Element("Game").Element("Platform").SetValue("psp2");
		xml.Save(xmlFile);
	} else if (platform == "ps3+vita") {
		xml.Element("Game").Element("Platform").SetValue("ps3,psp2");
		xml.Save(xmlFile);
	}*/
	if (platform != "ps3" && platform != "psp2" && platform != "ps3,psp2") {
		data.Add(gameId + " / " + platform);
	}
}

data.Dump(); // 372 > 367 > 202 > 196
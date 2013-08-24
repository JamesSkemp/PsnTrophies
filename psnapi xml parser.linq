<Query Kind="Statements" />

XNamespace ns = "http://www.psnapi.com.ar/ps3/api";
var path = @"C:\Users\James\Projects\GitHub\VideoGamesSpa\_output\strivinglife\psnapi\";
var gamesXml = XDocument.Load(path + "games.xml");
var games = gamesXml.Descendants(ns + "Game");
var outputDirectory = @"C:\Users\James\Projects\GitHub\PsnTrophies\games\";

XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";

foreach (var game in games)
{
	var gameId = game.Element(ns + "Id").Value;

	var newGameXml = new XDocument(
		new XElement("PlayStationGame",
			new XAttribute(XNamespace.Xmlns + "xsi", xsi.ToString()),
			new XAttribute(xsi + "noNamespaceSchemaLocation", "../PlayStationGame.xsd"),
			new XElement("Game",
				new XElement("Id", game.Element(ns + "Id").Value),
				new XElement("IdEurope", game.Element(ns + "IdGameEurope").Value),
				new XElement("Title", game.Element(ns + "Title").Value),
				new XElement("Image", game.Element(ns + "Image").Value),
				new XElement("TrophiesCount",
					new XAttribute("Total", game.Element(ns + "TotalTrophies").Value),
					new XAttribute("Bronze", ""),
					new XAttribute("Silver", ""),
					new XAttribute("Gold", ""),
					new XAttribute("Platinum", "")
				),
				new XElement("TotalPoints", game.Element(ns + "TotalPoints").Value),
				new XElement("Platform", game.Element(ns + "Platform").Value)
			),
			new XElement("Trophies")
		)
	);
	
	var trophyXml = XDocument.Load(string.Format("{0}trophies-{1}.xml", path, game.Element(ns + "Id").Value)).Root;
	var trophiesBronze = 0;
	var trophiesSilver = 0;
	var trophiesGold = 0;
	var trophiesPlatinum = 0;
	foreach (var trophy in trophyXml.Elements(ns + "Trophy"))
	{
		switch (trophy.Element(ns + "TrophyType").Value)
		{
			case "BRONZE":
				trophiesBronze += 1;
				break;
			case "SILVER":
				trophiesSilver += 1;
				break;
			case "GOLD":
				trophiesGold += 1;
				break;
			case "PLATINUM":
				trophiesPlatinum += 1;
				break;
			default:
				break;
		}
	
		var trophyElement = new XElement("Trophy",
			new XElement("Id", trophy.Element(ns + "IdTrophy").Value),
			new XElement("GameId", trophy.Element(ns + "GameId").Value),
			new XElement("Title", trophy.Element(ns + "Title").Value),
			new XElement("Image", trophy.Element(ns + "Image").Value),
			new XElement("Description", trophy.Element(ns + "Description").Value),
			new XElement("Type", trophy.Element(ns + "TrophyType").Value),
			new XElement("Hidden", trophy.Element(ns + "Hidden").Value)
		);
		newGameXml.Root.Element("Trophies").Add(trophyElement);
	}
	
	newGameXml.Root.Element("Game").Element("TrophiesCount").Attribute("Bronze").Value = trophiesBronze.ToString();
	newGameXml.Root.Element("Game").Element("TrophiesCount").Attribute("Silver").Value = trophiesSilver.ToString();
	newGameXml.Root.Element("Game").Element("TrophiesCount").Attribute("Gold").Value = trophiesGold.ToString();
	newGameXml.Root.Element("Game").Element("TrophiesCount").Attribute("Platinum").Value = trophiesPlatinum.ToString();
	
	newGameXml.Save(outputDirectory + gameId + ".xml");
	
	//newGameXml.Dump();
	//break;
}
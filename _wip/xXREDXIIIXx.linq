<Query Kind="Statements">
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

var trophyRepoDirectory = @"C:\Users\James\Projects\GitHub\PsnTrophies\";
var existingXmlDirectory = new DirectoryInfo(trophyRepoDirectory + @"games\");
var newXmlDirectory = new DirectoryInfo(trophyRepoDirectory + @"_wip\xXREDXIIIXx\");
var testDirectory = trophyRepoDirectory + @"_wip\";

var existingXmlFiles = existingXmlDirectory.GetFiles("*.xml");
var existingXmlFileNames = existingXmlFiles.Select(f => f.Name).ToList();
var newXmlFiles = newXmlDirectory.GetFiles("*.xml");
var newXmlFileNames = newXmlFiles.Select(f => f.Name).ToList();

//existingXmlFiles.Dump();

var count = 0;
foreach (var newXmlFile in newXmlFiles)
{
	if (count > 3) {
		//break;
	}
	try
	{
		var newGameXml = XDocument.Load(newXmlFile.FullName).Root;
		// Create a new game from the new XML.
		var playStationGame = new PlayStationGame(newGameXml);
		if (existingXmlFileNames.Contains(newXmlFile.Name))
		{
			// File already exists, so we need to check the existing file.
			count++;
			//var gameId = Path.GetFileNameWithoutExtension(newXmlFile.Name);
			//gameId.Dump();
			//var existingGameXml = XDocument.Load(existingXmlDirectory + newXmlFile.Name).Root;
			//newGameXml.Dump();
			//existingGameXml.Dump();
			//playStationGame.Save();
		}
		else
		{
			// This is a new game. Save it.

			// PHP export is different enough that .NET can't just deserialize it as-is.
			/*var deserializer = new XmlSerializer(typeof(PlayStationGame));
			var newPlayStationGame = new PlayStationGame();
			using (StreamReader reader = new StreamReader(newXmlFile.FullName)) {
				newPlayStationGame = (PlayStationGame)deserializer.Deserialize(reader);
			}
			newPlayStationGame.Dump();*/
			
			playStationGame.Save();
		}
	}
	catch (Exception ex)
	{
		newXmlFile.Name.Dump();
		//ex.Dump();
	}
}

/*var serializer = new XmlSerializer(typeof(PlayStationGame));
using (StreamWriter writer = new StreamWriter(testDirectory + "test.xml")) {
	var game = new PlayStationGame();
	game.Trophies.Add(new Trophy());
	serializer.Serialize(writer, game);
}*/




}

public class PlayStationGame {
	[XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
	public string Schema = "../PlayStationGame.xsd";
	public Game Game { get; set; }
	public List<Trophy> Trophies { get; set; }

	public PlayStationGame() {
		this.Game = new Game();
		this.Trophies = new List<Trophy>();
	}
	
	public PlayStationGame(XElement newGameXml) : this() {
		var playStationGame = new PlayStationGame();
		this.Game.Id = newGameXml.Element("Game").Element("Id").Value;
		this.Game.IdEurope = newGameXml.Element("Game").Element("IdEurope") != null
			? newGameXml.Element("Game").Element("IdEurope").Value
			: null;
		this.Game.Title = newGameXml.Element("Game").Element("Title").Value;
		this.Game.Image = newGameXml.Element("Game").Element("Image").Value;
		this.Game.TrophiesCount.Bronze = int.Parse(newGameXml.Element("Game").Element("TrophiesCount").Attribute("Bronze").Value);
		this.Game.TrophiesCount.Silver = int.Parse(newGameXml.Element("Game").Element("TrophiesCount").Attribute("Silver").Value);
		this.Game.TrophiesCount.Gold = int.Parse(newGameXml.Element("Game").Element("TrophiesCount").Attribute("Gold").Value);
		this.Game.TrophiesCount.Platinum = int.Parse(newGameXml.Element("Game").Element("TrophiesCount").Attribute("Platinum").Value);
		this.Game.TrophiesCount.Total = int.Parse(newGameXml.Element("Game").Element("TrophiesCount").Attribute("Total").Value);
		this.Game.UpdateTotalPoints();
		this.Game.Platform = newGameXml.Element("Game").Element("Platform").Value;
		
		foreach (var trophyItem in newGameXml.Descendants("Trophy"))
		{
			var trophy = new Trophy();
			trophy.Id = trophyItem.Element("Id").Value;
			trophy.GameId = trophyItem.Element("GameId").Value;
			trophy.Title = trophyItem.Element("Title").Value;
			trophy.Image = trophyItem.Element("Image").Value;
			trophy.Description = trophyItem.Element("Description").Value;
			trophy.Type = trophyItem.Element("Type").Value;
			trophy.Hidden = trophyItem.Element("Hidden") != null && !string.IsNullOrWhiteSpace(trophyItem.Element("Hidden").Value)
				? (bool?)bool.Parse(trophyItem.Element("Hidden").Value)
				: null;
			
			if (!this.Trophies.Any(t => t.Id == trophy.Id))
			{
				this.Trophies.Add(trophy);
			}
			else
			{
				// TODO, this could probably be better.
				("Duplicate trophy " + trophy.Id + " for game " + trophy.GameId).Dump();
			}
		}
	}
	
	public void Save() {
		// Create XDocument and save.
		XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
		var outputDirectory = @"C:\Users\James\Projects\GitHub\PsnTrophies\_wip\";

		var xml = new XDocument(
			new XElement("PlayStationGame",
				new XAttribute(XNamespace.Xmlns + "xsi", xsi.ToString()),
				new XAttribute(xsi + "noNamespaceSchemaLocation", "../PlayStationGame.xsd"),
				new XElement("Game",
					new XElement("Id", this.Game.Id),
					new XElement("IdEurope", this.Game.IdEurope),
					new XElement("Title", this.Game.Title),
					new XElement("Image", this.Game.Image),
					new XElement("TrophiesCount",
						new XAttribute("Total", this.Game.TrophiesCount.Total),
						new XAttribute("Bronze", this.Game.TrophiesCount.Bronze),
						new XAttribute("Silver", this.Game.TrophiesCount.Silver),
						new XAttribute("Gold", this.Game.TrophiesCount.Gold),
						new XAttribute("Platinum", this.Game.TrophiesCount.Platinum)
					),
					new XElement("TotalPoints", this.Game.TotalPoints),
					new XElement("Platform", this.Game.Platform)
				),
				new XElement("Trophies")
			)
		);
		
		foreach (var trophy in this.Trophies)
		{
			var trophyElement = new XElement("Trophy",
				new XElement("Id", trophy.Id),
				new XElement("GameId", trophy.GameId),
				new XElement("Title", trophy.Title),
				new XElement("Image", trophy.Image),
				new XElement("Description", trophy.Description),
				new XElement("Type", trophy.Type),
				new XElement("Hidden", trophy.Hidden)
			);
			if (!trophy.Hidden.HasValue) {
				trophyElement.Element("Hidden").Add(new XAttribute(xsi + "nil", true));
			}
			xml.Root.Element("Trophies").Add(trophyElement);
		}
		
		//xml.Dump();
	
		/*
		var newGameXml = new XDocument(
			new XElement("PlayStationGame",
				// ...
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
	
		*/
		//newGameXml.Root.Element("Game").Element("TrophiesCount").Attribute("Bronze").Value = trophiesBronze.ToString();
		//newGameXml.Root.Element("Game").Element("TrophiesCount").Attribute("Silver").Value = trophiesSilver.ToString();
		//newGameXml.Root.Element("Game").Element("TrophiesCount").Attribute("Gold").Value = trophiesGold.ToString();
		//newGameXml.Root.Element("Game").Element("TrophiesCount").Attribute("Platinum").Value = trophiesPlatinum.ToString();
	
		xml.Save(outputDirectory + this.Game.Id + ".xml");
	}
}

public class Game {
	public string Id { get; set; }
	public string IdEurope { get; set; }
	public string Title { get; set; }
	public string Image { get; set; }
	public TrophiesCount TrophiesCount { get; set; }
	public int? TotalPoints { get; set; }
	public string Platform { get; set; }

	public Game() {
		this.TrophiesCount = new TrophiesCount();
	}
	
	public void UpdateTotalPoints() {
		if (this.TrophiesCount != null
			&& this.TrophiesCount.HasAllCounts) {
			this.TotalPoints = (15 * this.TrophiesCount.Bronze)
				+ (30 *  this.TrophiesCount.Silver)
				+ (90 * this.TrophiesCount.Gold)
				+ (0 * this.TrophiesCount.Platinum);
		}
	}
}

public class TrophiesCount {
	public int? Total { get; set; }
	public int? Bronze { get; set; }
	public int? Silver { get; set; }
	public int? Gold { get; set; }
	public int? Platinum { get; set; }
	public bool HasAllCounts {
		get {
			return this.Bronze.HasValue
				&& this.Silver.HasValue
				&& this.Gold.HasValue
				&& this.Platinum.HasValue;
		}
	}
	
	public void UpdateTotal() {
		this.Total = !this.HasAllCounts ? null : (int?)(this.Bronze.Value + this.Silver.Value + this.Gold.Value + this.Platinum.Value);
	}

	public TrophiesCount() {
	}
	
	public TrophiesCount(int? bronze, int? silver, int? gold, int? platinum) {
		this.Bronze = bronze;
		this.Silver = silver;
		this.Gold = gold;
		this.Platinum = platinum;
		this.Total = this.Bronze.Value + this.Silver.Value + this.Gold.Value + this.Platinum.Value;
	}
}

public class Trophy {
	public string Id { get; set; }
	public string GameId { get; set; }
	public string Title { get; set; }
	public string Image { get; set; }
	public string Description { get; set; }
	public string Type { get; set; }
	public bool? Hidden { get; set; }

	public Trophy() {
	}
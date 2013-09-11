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
		break;
	}
	try
	{
		var newGameXml = XDocument.Load(newXmlFile.FullName).Root;
		if (existingXmlFileNames.Contains(newXmlFile.Name))
		{
			// File already exists, so we need to check the existing file.
			count++;
			var gameId = Path.GetFileNameWithoutExtension(newXmlFile.Name);
			gameId.Dump();
			//newGameXml.Dump();
		}
		else
		{
			/*var deserializer = new XmlSerializer(typeof(PlayStationGame));
			var newPlayStationGame = new PlayStationGame();
			using (StreamReader reader = new StreamReader(newXmlFile.FullName)) {
				newPlayStationGame = (PlayStationGame)deserializer.Deserialize(reader);
			}
			newPlayStationGame.Dump();*/
		
			// This is a new game. We'll make sure it's all good.
			var playStationGame = new PlayStationGame();
			playStationGame.Game.Id = newGameXml.Element("Game").Element("Id").Value;
			playStationGame.Game.IdEurope = newGameXml.Element("Game").Element("IdEurope") != null ? newGameXml.Element("Game").Element("IdEurope").Value : null;
			playStationGame.Game.Title = newGameXml.Element("Game").Element("Title").Value;
			playStationGame.Game.Image = newGameXml.Element("Game").Element("Image").Value;
			playStationGame.Game.TrophiesCount.Bronze = int.Parse(newGameXml.Element("Game").Element("TrophiesCount").Attribute("Bronze").Value);
			playStationGame.Game.TrophiesCount.Silver = int.Parse(newGameXml.Element("Game").Element("TrophiesCount").Attribute("Silver").Value);
			playStationGame.Game.TrophiesCount.Gold = int.Parse(newGameXml.Element("Game").Element("TrophiesCount").Attribute("Gold").Value);
			playStationGame.Game.TrophiesCount.Platinum = int.Parse(newGameXml.Element("Game").Element("TrophiesCount").Attribute("Platinum").Value);
			playStationGame.Game.TrophiesCount.Total = int.Parse(newGameXml.Element("Game").Element("TrophiesCount").Attribute("Total").Value);
			playStationGame.Game.UpdateTotalPoints();
			newGameXml.Dump();

			var serializer = new XmlSerializer(typeof(PlayStationGame));
			using (StreamWriter writer = new StreamWriter(testDirectory + newXmlFile.Name)) {
				serializer.Serialize(writer, playStationGame);
			}
			break;
		}
	}
	catch (Exception ex)
	{
		ex.Dump();
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
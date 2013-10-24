<Query Kind="Statements" />

var trophiesDirectory = System.IO.Directory.GetParent(Path.GetDirectoryName(Util.CurrentQueryPath)) + Path.DirectorySeparatorChar.ToString() + "games";
trophiesDirectory.Dump();

var trophyFiles = Directory.GetFiles(trophiesDirectory);
foreach (var trophyFile in trophyFiles.Where (f => f.EndsWith(".xml")))
{
	var xml = XDocument.Load(trophyFile).Root;
	// Check trophy count.
	if (!ValidTrophyCountSum(xml.Element("Game")))
	{
		string.Format("Trophy count sum incorrect for " + trophyFile).Dump();
	}
	if (!ValidTotalPoints(xml.Element("Game")))
	{
		string.Format("Total points incorrect for " + trophyFile).Dump();
	}
	//xml.Dump();
	//break;
}

}

public bool ValidTrophyCountSum(XElement gameElement) {
	var trophiesCount = gameElement.Element("TrophiesCount");
	try
	{	        
		var total = int.Parse(trophiesCount.Attribute("Total").Value);
		var bronze = int.Parse(trophiesCount.Attribute("Bronze").Value);
		var silver = int.Parse(trophiesCount.Attribute("Silver").Value);
		var gold = int.Parse(trophiesCount.Attribute("Gold").Value);
		var platinum = int.Parse(trophiesCount.Attribute("Platinum").Value);
		
		return (bronze + silver + gold + platinum == total);
	}
	catch (Exception ex)
	{
		return false;
	}
}

public bool ValidTotalPoints(XElement gameElement) {
	var trophiesCount = gameElement.Element("TrophiesCount");
	try
	{	        
		var totalPoints = int.Parse(gameElement.Element("TotalPoints").Value);
		var bronze = int.Parse(trophiesCount.Attribute("Bronze").Value);
		var silver = int.Parse(trophiesCount.Attribute("Silver").Value);
		var gold = int.Parse(trophiesCount.Attribute("Gold").Value);
		var platinum = int.Parse(trophiesCount.Attribute("Platinum").Value);
		
		return ((bronze * 15) + (silver * 30) + (gold * 90) + (platinum * 0) == totalPoints);
	}
	catch (Exception ex)
	{
		return false;
	}
}




private void Nothing() {
// No closing brace required.
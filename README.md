PsnTrophies
===========

XML files with information about PlayStation 3 trophies and games.

Purpose
====

With the lose of psnapi.com.ar, a lot of us have lost game/trophy information.

To offset that I purpose creating a repository of information about PlayStation 3/Vita/Network/etc games and their trophies.

This data can then be both shared and updated by the community.

Status
====

8/23/2013, morning: I have 156 games that I have information for. I'll work on creating XML files for those 156 games, as well as a schema (XSD) we can follow.

If you have information that you can provide about all the games, I'm willing to do the heavy lifting of converting that, in whatever format you have it, to XML.


Comments from https://psnapi.codeplex.com/discussions/453867
=====

In regards to an alternative, I unfortunately only have the data for 156 of the games (the ones that I have/play) from the psnapi.com.ar service. It looks like Nathan might have all the games?

@mbrookes, what do you have?

In order to supplement the data we can pull from the quasi-official services we'd need the following information:

- Game title
- Game id (NP...)
- Total trophies (and totals for each type)
- for each trophy:
- - Trophy id
- - Trophy title
- - Trophy description
- - Trophy image URL
- - Whether it's hidden

User data, like the number of trophies they've earned, can be pulled from the quasi-official services. Dates the trophies were earned can also be pulled from that. As can platforms the games are on and when the game was last played.

I've started https://github.com/JamesSkemp/PsnTrophies to store the information.

I'll work on creating an XSD for the XML files tonight, as well as get all the games I have in there.

Nathan, it looks like you were looking for additional information. Do you have any of that information currently for the games?

Would anyone be willing to provide a dump of the data they've collected? I'd be more than willing to go through it to create the initial files.

I don't yet know how we'll do new games, though. What springs to mind is the PS3Trophies.org/Xbox360Achievements.org methods of getting lists from users.

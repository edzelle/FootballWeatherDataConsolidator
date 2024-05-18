Getting Started:

SQL Scripts:

I looked to investigate the following hypothesis using the data accumulated with this application:
**Home teams will tend to outperform opponents when adverse weather impacts outdoor games and opponents need to travel far either north or south**
I am using the location of each team's home stadium as the basis for calculating distance traveled.

The first metric to calculate here is the average, and standard deviation of the difference lattitude between home stadiums.

The following query produces a dataset of all games, with home and away teams, followed by the difference in the absolute value in the lattitudes between home stadiums.
 ```
 select home.*, 
		away.*,
		abs(home.HomeLat - away.awayLat) 'latt difference'
from (select g.id, 
		t.Name 'Home Team Name', 
		s.Lattitude HomeLat
	from games g, 
		teams t, 
		TeamPlaysInStadium tps,
		stadiums s
	where g.HomeTeam =  t.Name
	and t.Id = tps.TeamId
	and tps.StadiumId = s.id) home,
 (select g.Id, 
		t.Name 'Away Team Name', 
		s.Lattitude AwayLat
	from games g, 
		teams t, 
		TeamPlaysInStadium tps,
		stadiums s
	where g.AwayTeam =  t.Name
	and t.Id = tps.TeamId
	and tps.StadiumId = s.id) away,
 Games
 where home.id = away.id
 and games.Id = home.id
 order by games.id; 
 ```

From this data, I can calculate the average and standard deviation for the `Latt Difference` column.
One quick note, SQLite does not support a stadard deviation function, so I calculated that value using excel.

The values are as follows: 
`Avg Lattitude Difference: 5.64, Standard Deviation Lattitude Difference: 4.40`


Now using these values, we can caluclate winning percentage of home teams in adverse weather games where the away teams need to travel more than one standard deviation in lattitude.

I am defining adverse weather games with the following criteria:

	1. Games played with an average apparent temperature played below freezing (32 degrees F)
	2. Games played with an average apparent temperature played below freezing (32 degrees F) and in rain or snow
	3. Games played with an average humidity above 90% 
	4. Games played with an average humidity above 90% and average apparent temperature above 70 degrees F


A quick baseline to take into account, the home team winning percentage can be shown with the following query:
```
select HomeTeamWins, count(*) from(select home.*, away.*,
	case 
		when games.HomeTeamScore > AwayTeamScore
		then true
		else false 
	end HomeTeamWins,
	abs(home.HomeLat - away.awayLat) 'lattDifference'
	 from (select g.id, t.Name 'Home Team Name', s.Lattitude HomeLat
from games g, 
	teams t, 
	TeamPlaysInStadium tps,
	stadiums s
 where g.HomeTeam =  t.Name
 and t.Id = tps.TeamId
 and g.GMTOffset < -1
 and g.HomeTeamScore is not null
 and tps.StadiumId = s.id) home join
 (select g.Id, t.Name 'Away Team Name', s.Lattitude AwayLat
from games g, 
	teams t, 
	TeamPlaysInStadium tps,
	stadiums s
 where g.AwayTeam =  t.Name
 and t.Id = tps.TeamId
 and tps.StadiumId = s.id) away
 on home.id = away.id,
 Games
 where games.Id = home.id
 order by games.id) g 
	group by homeTeamWins;
```
Produces this result:

	HomeTeamWins	count(*)
	0		363
	1		432

The home team won 432 of 795 games (54.3%).


The following query gives the set of all games played between the 2021 and 2023 seasons where the away team traveled more than one standard deviation from their home stadium to playan outdoor game, excluding games played outside the USA and games that did not finish.

```
select * from(select home.*, away.*,
	abs(home.HomeLat - away.awayLat) 'lattDifference'
	 from (select g.id, t.Name 'Home Team Name', s.Lattitude HomeLat
from games g, 
	teams t, 
	TeamPlaysInStadium tps,
	stadiums s
 where g.HomeTeam =  t.Name
 and t.Id = tps.TeamId
 and s.roofType = 'Open'
 and g.GMTOffset < -1
 and g.HomeTeamScore is not null
 and tps.StadiumId = s.id) home join
 (select g.Id, t.Name 'Away Team Name', s.Lattitude AwayLat
from games g, 
	teams t, 
	TeamPlaysInStadium tps,
	stadiums s
 where g.AwayTeam =  t.Name
 and t.Id = tps.TeamId
 and tps.StadiumId = s.id) away
 on home.id = away.id,
 Games
 where games.Id = home.id
 order by games.id)
 where lattDifference > 4.4; 
```

Using this query as a base, we can compute the home team winning percentage in the four cases outlined above. 
These computations are done below:

Case 1:
```
select HomeTeamWins, count(*) from(select home.*, away.*,
	case 
		when games.HomeTeamScore > AwayTeamScore
		then true
		else false 
	end HomeTeamWins,
	abs(home.HomeLat - away.awayLat) 'lattDifference'
	 from (select g.id, t.Name 'Home Team Name', s.Lattitude HomeLatfrom games g, 
	teams t, 
	TeamPlaysInStadium tps,
	stadiums s
 where g.HomeTeam =  t.Name
 and t.Id = tps.TeamId
 and s.roofType = 'Open'
 and g.GMTOffset < -1
 and g.HomeTeamScore is not null
 and tps.StadiumId = s.id) home join
 (select g.Id, t.Name 'Away Team Name', s.Lattitude AwayLat
from games g, 
	teams t, 
	TeamPlaysInStadium tps,
	stadiums s
 where g.AwayTeam =  t.Name
 and t.Id = tps.TeamId
 and tps.StadiumId = s.id) away
 on home.id = away.id,
 Games
 where games.Id = home.id
 order by games.id) g join
 gameWeather gw
 on g.id = gw.GameId
 where g.lattDifference > 4.4
	and gw.AverageApparentTempurature < 32
	group by homeTeamWins; 
```

Produces this result:

	HomeTeamWins	count(*)
	0		11
	1		17

The home team won 17 of 28 games (60.7%).

Case 2:

```
select HomeTeamWins, count(*) from(select home.*, away.*,
	case 
		when games.HomeTeamScore > AwayTeamScore
		then true
		else false 
	end HomeTeamWins,
	abs(home.HomeLat - away.awayLat) 'lattDifference'
	 from (select g.id, t.Name 'Home Team Name', s.Lattitude HomeLatfrom games g, 
	teams t, 
	TeamPlaysInStadium tps,
	stadiums s
 where g.HomeTeam =  t.Name
 and t.Id = tps.TeamId
 and s.roofType = 'Open'
 and g.GMTOffset < -1
 and g.HomeTeamScore is not null
 and tps.StadiumId = s.id) home join
 (select g.Id, t.Name 'Away Team Name', s.Lattitude AwayLat
from games g, 
	teams t, 
	TeamPlaysInStadium tps,
	stadiums s
 where g.AwayTeam =  t.Name
 and t.Id = tps.TeamId
 and tps.StadiumId = s.id) away
 on home.id = away.id,
 Games
 where games.Id = home.id
 order by games.id) g join
 gameWeather gw
 on g.id = gw.GameId
 where g.lattDifference > 4.4
	and gw.AverageApparentTempurature < 32
	and (gw.AverageSnowfall > 0.0 or gw.AverageRain >0.0)
	group by homeTeamWins; 
```

Produces this result: 

	HomeTeamWins	count(*)
	0		3
	1		5

The home team won 5 of 8 games (62.5%).

Case 3:

```
select HomeTeamWins, count(*) from(select home.*, away.*,
	case 
		when games.HomeTeamScore > AwayTeamScore
		then true
		else false 
	end HomeTeamWins,
	abs(home.HomeLat - away.awayLat) 'lattDifference'
	 from (select g.id, t.Name 'Home Team Name', s.Lattitude HomeLat
from games g, 
	teams t, 
	TeamPlaysInStadium tps,
	stadiums s
 where g.HomeTeam =  t.Name
 and t.Id = tps.TeamId
 and s.roofType = 'Open'
 and g.GMTOffset < -1
 and g.HomeTeamScore is not null
 and tps.StadiumId = s.id) home join
 (select g.Id, t.Name 'Away Team Name', s.Lattitude AwayLat
from games g, 
	teams t, 
	TeamPlaysInStadium tps,
	stadiums s
 where g.AwayTeam =  t.Name
 and t.Id = tps.TeamId
 and tps.StadiumId = s.id) away
 on home.id = away.id,
 Games
 where games.Id = home.id
 order by games.id) g join
 gameWeather gw
 on g.id = gw.GameId
 where g.lattDifference > 4.4
	and gw.AverageRelativeHumitidty2M > 90
	group by homeTeamWins;select HomeTeamWins, count(*) from(select home.*, away.*,
	case 
		when games.HomeTeamScore > AwayTeamScore
		then true
		else false 
	end HomeTeamWins,
	abs(home.HomeLat - away.awayLat) 'lattDifference'
	 from (select g.id, t.Name 'Home Team Name', s.Lattitude HomeLat
from games g, 
	teams t, 
	TeamPlaysInStadium tps,
	stadiums s
 where g.HomeTeam =  t.Name
 and t.Id = tps.TeamId
 and s.roofType = 'Open'
 and g.GMTOffset < -1
 and g.HomeTeamScore is not null
 and tps.StadiumId = s.id) home join
 (select g.Id, t.Name 'Away Team Name', s.Lattitude AwayLat
from games g, 
	teams t, 
	TeamPlaysInStadium tps,
	stadiums s
 where g.AwayTeam =  t.Name
 and t.Id = tps.TeamId
 and tps.StadiumId = s.id) away
 on home.id = away.id,
 Games
 where games.Id = home.id
 order by games.id) g join
 gameWeather gw
 on g.id = gw.GameId
 where g.lattDifference > 4.4
	and gw.AverageRelativeHumitidty2M > 90
	group by homeTeamWins; 
```

Produces this result: 

	HomeTeamWins	count(*)
	0		10
	1		10

The home team won 10 of 20 games (50%).

Case 4:

```
select HomeTeamWins, count(*) from(select home.*, away.*,
	case 
		when games.HomeTeamScore > AwayTeamScore
		then true
		else false 
	end HomeTeamWins,
	abs(home.HomeLat - away.awayLat) 'lattDifference'
	 from (select g.id, t.Name 'Home Team Name', s.Lattitude HomeLat
from games g, 
	teams t, 
	TeamPlaysInStadium tps,
	stadiums s
 where g.HomeTeam =  t.Name
 and t.Id = tps.TeamId
 and s.roofType = 'Open'
 and g.GMTOffset < -1
 and g.HomeTeamScore is not null
 and tps.StadiumId = s.id) home join
 (select g.Id, t.Name 'Away Team Name', s.Lattitude AwayLat
from games g, 
	teams t, 
	TeamPlaysInStadium tps,
	stadiums s
 where g.AwayTeam =  t.Name
 and t.Id = tps.TeamId
 and tps.StadiumId = s.id) away
 on home.id = away.id,
 Games
 where games.Id = home.id
 order by games.id) g join
 gameWeather gw
 on g.id = gw.GameId
 where g.lattDifference > 4.4
	and gw.AverageRelativeHumitidty2M > 90
	and gw.AverageApparentTempurature > 70
	group by homeTeamWins;
```

Produces this result: 

	HomeTeamWins	count(*)
	0		2
	1		4

The home team won 4 of 6 games (66.7%).

From these results, it appears that adverse weather does seem give an advantage to the home team when games are played outdoors and the away team needs to travel far along the north/south axis.
Three of the four inquiries shows the home team has a higher winning percentage than the baseline home win percentage.
More historic data should be used to corraborate these results as the datasets in this analysis have a small sample size.
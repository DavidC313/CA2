-- Insert Teams
INSERT INTO Teams (Name, League, Country, FoundedYear, Stadium, Manager)
VALUES 
('Arsenal', 'Premier League', 'England', 1886, 'Emirates Stadium', 'Mikel Arteta'),
('Manchester City', 'Premier League', 'England', 1880, 'Etihad Stadium', 'Pep Guardiola'),
('Liverpool', 'Premier League', 'England', 1892, 'Anfield', 'Jürgen Klopp'),
('Real Madrid', 'La Liga', 'Spain', 1902, 'Santiago Bernabéu', 'Carlo Ancelotti'),
('Bayern Munich', 'Bundesliga', 'Germany', 1900, 'Allianz Arena', 'Thomas Tuchel'),
('Paris Saint-Germain', 'Ligue 1', 'France', 1970, 'Parc des Princes', 'Luis Enrique'),
('Inter Milan', 'Serie A', 'Italy', 1908, 'San Siro', 'Simone Inzaghi');

-- Insert Players for Arsenal
INSERT INTO Players (Name, Age, Position, Goals, Assists, Appearances, Nationality, TeamId)
SELECT 'Bukayo Saka', 22, 'Forward', 14, 8, 35, 'England', TeamId FROM Teams WHERE Name = 'Arsenal'
UNION ALL
SELECT 'Martin Ødegaard', 25, 'Midfielder', 8, 10, 32, 'Norway', TeamId FROM Teams WHERE Name = 'Arsenal'
UNION ALL
SELECT 'Declan Rice', 25, 'Midfielder', 6, 7, 34, 'England', TeamId FROM Teams WHERE Name = 'Arsenal'
UNION ALL
SELECT 'William Saliba', 23, 'Defender', 2, 1, 27, 'France', TeamId FROM Teams WHERE Name = 'Arsenal'
UNION ALL
SELECT 'Gabriel Jesus', 26, 'Forward', 4, 3, 20, 'Brazil', TeamId FROM Teams WHERE Name = 'Arsenal';

-- Insert Players for Manchester City
INSERT INTO Players (Name, Age, Position, Goals, Assists, Appearances, Nationality, TeamId)
SELECT 'Erling Haaland', 23, 'Forward', 25, 5, 30, 'Norway', TeamId FROM Teams WHERE Name = 'Manchester City'
UNION ALL
SELECT 'Kevin De Bruyne', 32, 'Midfielder', 4, 14, 18, 'Belgium', TeamId FROM Teams WHERE Name = 'Manchester City'
UNION ALL
SELECT 'Phil Foden', 23, 'Midfielder', 16, 7, 34, 'England', TeamId FROM Teams WHERE Name = 'Manchester City'
UNION ALL
SELECT 'Rodri', 27, 'Midfielder', 7, 6, 33, 'Spain', TeamId FROM Teams WHERE Name = 'Manchester City'
UNION ALL
SELECT 'Rúben Dias', 26, 'Defender', 1, 1, 30, 'Portugal', TeamId FROM Teams WHERE Name = 'Manchester City'; 
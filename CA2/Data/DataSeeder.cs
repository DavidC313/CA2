using CA2.Models;
using Microsoft.EntityFrameworkCore;

namespace CA2.Data
{
    public static class DataSeeder
    {
        public static void SeedData(FootballContext context)
        {
            // Check if we already have data
            if (context.Teams.Any())
            {
                return; // Database has been seeded
            }

            var teams = new List<Team>
            {
                new Team
                {
                    Name = "Arsenal",
                    League = "Premier League",
                    Country = "England",
                    FoundedYear = 1886,
                    Stadium = "Emirates Stadium",
                    Manager = "Mikel Arteta",
                    Players = new List<Player>
                    {
                        new Player { Name = "Bukayo Saka", Age = 22, Position = "Forward", Goals = 14, Assists = 8, Appearances = 35, Nationality = "England" },
                        new Player { Name = "Martin Ødegaard", Age = 25, Position = "Midfielder", Goals = 8, Assists = 10, Appearances = 32, Nationality = "Norway" },
                        new Player { Name = "Declan Rice", Age = 25, Position = "Midfielder", Goals = 6, Assists = 7, Appearances = 34, Nationality = "England" },
                        new Player { Name = "William Saliba", Age = 23, Position = "Defender", Goals = 2, Assists = 1, Appearances = 27, Nationality = "France" },
                        new Player { Name = "Gabriel Jesus", Age = 26, Position = "Forward", Goals = 4, Assists = 3, Appearances = 20, Nationality = "Brazil" }
                    }
                },
                new Team
                {
                    Name = "Manchester City",
                    League = "Premier League",
                    Country = "England",
                    FoundedYear = 1880,
                    Stadium = "Etihad Stadium",
                    Manager = "Pep Guardiola",
                    Players = new List<Player>
                    {
                        new Player { Name = "Erling Haaland", Age = 23, Position = "Forward", Goals = 25, Assists = 5, Appearances = 30, Nationality = "Norway" },
                        new Player { Name = "Kevin De Bruyne", Age = 32, Position = "Midfielder", Goals = 4, Assists = 14, Appearances = 18, Nationality = "Belgium" },
                        new Player { Name = "Phil Foden", Age = 23, Position = "Midfielder", Goals = 16, Assists = 7, Appearances = 34, Nationality = "England" },
                        new Player { Name = "Rodri", Age = 27, Position = "Midfielder", Goals = 7, Assists = 6, Appearances = 33, Nationality = "Spain" },
                        new Player { Name = "Rúben Dias", Age = 26, Position = "Defender", Goals = 1, Assists = 1, Appearances = 30, Nationality = "Portugal" }
                    }
                },
                new Team
                {
                    Name = "Liverpool",
                    League = "Premier League",
                    Country = "England",
                    FoundedYear = 1892,
                    Stadium = "Anfield",
                    Manager = "Jürgen Klopp",
                    Players = new List<Player>
                    {
                        new Player { Name = "Mohamed Salah", Age = 31, Position = "Forward", Goals = 18, Assists = 9, Appearances = 32, Nationality = "Egypt" },
                        new Player { Name = "Darwin Núñez", Age = 24, Position = "Forward", Goals = 11, Assists = 8, Appearances = 33, Nationality = "Uruguay" },
                        new Player { Name = "Virgil van Dijk", Age = 32, Position = "Defender", Goals = 2, Assists = 2, Appearances = 34, Nationality = "Netherlands" },
                        new Player { Name = "Trent Alexander-Arnold", Age = 25, Position = "Defender", Goals = 2, Assists = 9, Appearances = 28, Nationality = "England" },
                        new Player { Name = "Alexis Mac Allister", Age = 25, Position = "Midfielder", Goals = 4, Assists = 5, Appearances = 30, Nationality = "Argentina" }
                    }
                },
                new Team
                {
                    Name = "Real Madrid",
                    League = "La Liga",
                    Country = "Spain",
                    FoundedYear = 1902,
                    Stadium = "Santiago Bernabéu",
                    Manager = "Carlo Ancelotti",
                    Players = new List<Player>
                    {
                        new Player { Name = "Jude Bellingham", Age = 20, Position = "Midfielder", Goals = 19, Assists = 6, Appearances = 28, Nationality = "England" },
                        new Player { Name = "Vinicius Junior", Age = 23, Position = "Forward", Goals = 13, Assists = 5, Appearances = 24, Nationality = "Brazil" },
                        new Player { Name = "Rodrygo", Age = 23, Position = "Forward", Goals = 10, Assists = 5, Appearances = 31, Nationality = "Brazil" },
                        new Player { Name = "Federico Valverde", Age = 25, Position = "Midfielder", Goals = 1, Assists = 6, Appearances = 32, Nationality = "Uruguay" },
                        new Player { Name = "Antonio Rüdiger", Age = 30, Position = "Defender", Goals = 1, Assists = 1, Appearances = 30, Nationality = "Germany" }
                    }
                },
                new Team
                {
                    Name = "Bayern Munich",
                    League = "Bundesliga",
                    Country = "Germany",
                    FoundedYear = 1900,
                    Stadium = "Allianz Arena",
                    Manager = "Thomas Tuchel",
                    Players = new List<Player>
                    {
                        new Player { Name = "Harry Kane", Age = 30, Position = "Forward", Goals = 35, Assists = 8, Appearances = 32, Nationality = "England" },
                        new Player { Name = "Jamal Musiala", Age = 21, Position = "Midfielder", Goals = 10, Assists = 6, Appearances = 24, Nationality = "Germany" },
                        new Player { Name = "Leroy Sané", Age = 28, Position = "Forward", Goals = 8, Assists = 11, Appearances = 28, Nationality = "Germany" },
                        new Player { Name = "Joshua Kimmich", Age = 29, Position = "Midfielder", Goals = 2, Assists = 10, Appearances = 30, Nationality = "Germany" },
                        new Player { Name = "Manuel Neuer", Age = 38, Position = "Goalkeeper", Goals = 0, Assists = 0, Appearances = 20, Nationality = "Germany" }
                    }
                },
                new Team
                {
                    Name = "Paris Saint-Germain",
                    League = "Ligue 1",
                    Country = "France",
                    FoundedYear = 1970,
                    Stadium = "Parc des Princes",
                    Manager = "Luis Enrique",
                    Players = new List<Player>
                    {
                        new Player { Name = "Kylian Mbappé", Age = 25, Position = "Forward", Goals = 27, Assists = 7, Appearances = 29, Nationality = "France" },
                        new Player { Name = "Ousmane Dembélé", Age = 26, Position = "Forward", Goals = 1, Assists = 8, Appearances = 24, Nationality = "France" },
                        new Player { Name = "Vitinha", Age = 24, Position = "Midfielder", Goals = 7, Assists = 4, Appearances = 30, Nationality = "Portugal" },
                        new Player { Name = "Marquinhos", Age = 29, Position = "Defender", Goals = 2, Assists = 1, Appearances = 28, Nationality = "Brazil" },
                        new Player { Name = "Gianluigi Donnarumma", Age = 25, Position = "Goalkeeper", Goals = 0, Assists = 0, Appearances = 30, Nationality = "Italy" }
                    }
                },
                new Team
                {
                    Name = "Inter Milan",
                    League = "Serie A",
                    Country = "Italy",
                    FoundedYear = 1908,
                    Stadium = "San Siro",
                    Manager = "Simone Inzaghi",
                    Players = new List<Player>
                    {
                        new Player { Name = "Lautaro Martínez", Age = 26, Position = "Forward", Goals = 23, Assists = 2, Appearances = 30, Nationality = "Argentina" },
                        new Player { Name = "Marcus Thuram", Age = 26, Position = "Forward", Goals = 10, Assists = 7, Appearances = 28, Nationality = "France" },
                        new Player { Name = "Hakan Çalhanoğlu", Age = 30, Position = "Midfielder", Goals = 11, Assists = 3, Appearances = 29, Nationality = "Turkey" },
                        new Player { Name = "Alessandro Bastoni", Age = 24, Position = "Defender", Goals = 1, Assists = 3, Appearances = 27, Nationality = "Italy" },
                        new Player { Name = "Yann Sommer", Age = 35, Position = "Goalkeeper", Goals = 0, Assists = 0, Appearances = 30, Nationality = "Switzerland" }
                    }
                }
            };

            // Set the Team property for each player
            foreach (var team in teams)
            {
                foreach (var player in team.Players)
                {
                    player.Team = team;
                }
            }

            context.Teams.AddRange(teams);
            context.SaveChanges();
        }
    }
} 
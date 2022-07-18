using StackExchange.Redis;
using FootballData.Models;

namespace FootballData
{
    class Program
    {
        private static string commands = "\nadd [player|team|playerInTeam] - добавление новых данных о игроке/команде/игроке в команде\n" +
                                         "exit - выход из программы\n";


        static void Main(string[] args)
        {
            PrintHeader();
            using (var db = new FootballDbContext())
            {
                while (true)
                {
                    Console.Write("> ");
                    string? command = Console.ReadLine();

                    if (String.IsNullOrEmpty(command)) continue;

                    string[] commandParts = command.Split(" ");

                    if (commandParts[0] == "exit") break;

                    else if (commandParts[0] == "help") Console.WriteLine(commands);

                    else if (commandParts.Length < 2)
                    {
                        Console.WriteLine("Команда не найдена!");
                    }

                    else if (commandParts[0] == "add")
                    {
                        if (commandParts[1] == "player")
                        {
                            // Ввод данных об игроке
                            // Ввод имени игрока
                            Console.WriteLine("Введите имя игрока:");
                            string? name = Console.ReadLine();

                            // Ввод возраста игрока
                            Console.WriteLine("Введите возраст игрока:");
                            string? ageString = Console.ReadLine();
                            int age; int.TryParse(ageString, out age);

                            // Добавление игрока в бд
                            var player = db.Players.Add(new Player { Name = name, Age = age });
                            db.SaveChanges();
                            Console.WriteLine($"Id нового игрока: {player.Entity.Id}");
                        }
                        else if (commandParts[1] == "team")
                        {
                            // Ввод данных о команде
                            Console.WriteLine("Введите название команды:");
                            string? name = Console.ReadLine();
                            var team = db.Teams.Add(new Team { Name = name ?? String.Empty });

                            // Добавление команды в бд
                            db.SaveChanges();
                            Console.WriteLine($"Id новой команды: {team.Entity.Id}");
                        }
                        else if (commandParts[1] == "playerInTeam")
                        {
                            // Ввод Id игрока
                            PrintAllPlayers(db);
                            Console.WriteLine("Введите id игрока:");
                            string? input = Console.ReadLine();
                            int playerId; int.TryParse(input, out playerId);

                            // Ввод Id команды
                            PrintAllTeams(db);
                            Console.WriteLine("Введите id команды:");
                            input = Console.ReadLine();
                            int teamId; int.TryParse(input, out teamId);

                            // Ввод зарплаты игрока в команде
                            Console.WriteLine("Введите зарплату игрока в команде:");
                            input = Console.ReadLine();
                            double salary; double.TryParse(input, out salary);

                            // Добавление в бд
                            var playerInTeam = db.PlayersInTeams.Add(new PlayersInTeams { PlayerId = playerId, TeamId = teamId, Salary = salary });
                            db.SaveChanges();
                            Console.WriteLine($"Игрок #{playerInTeam.Entity.PlayerId} добавлен в команду #{playerInTeam.Entity.TeamId}");
                        }
                        else
                        {
                            Console.WriteLine("Неверные аргументы для команды add!");
                        }

                    }
                    else
                    {
                        Console.WriteLine("Команда не распознана!");
                    }
                }
            }

        }
        private static void PrintHeader()
        {
            Console.WriteLine("УПРАВЛЕНИЕ ДАННЫМИ О ФУТБОЛЬНЫХ КОМАНДАХ И ИГРОКАХ");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Введите команду 'help' для отображения доступных комманд");
        }

        private static void PrintAllPlayers(FootballDbContext db)
        {
            Console.WriteLine("Список игроков:");
            foreach (var player in db.Players)
            {
                Console.WriteLine($"\t Id - {player.Id}, Name - {player.Name}, Age - {player.Age}");
            }
        }

        private static void PrintAllTeams(FootballDbContext db)
        {
            Console.WriteLine("Список игроков:");
            foreach (var team in db.Teams)
            {
                Console.WriteLine($"\t Id - {team.Id}, Name - {team.Name}");
            }
        }
    }
}
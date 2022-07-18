using StackExchange.Redis;
using FootballData.Models;

namespace FootballData
{
    class Program
    {
        private static string commands = "\nadd [player|team|playerInTeam] - добавление новых данных о игроке/команде/игроке в команде\n" +
                                         "get [playerCount|totalExpenses] - получение информации о количестве игроков/суммарных расходах на игроков\n" +
                                         "info - выводит информацию о имеющихся записях\n" +
                                         "exit - выход из программы\n";


        static void Main(string[] args)
        {
            try
            {
                // Подключение к Redis
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
                IDatabase redisDb = redis.GetDatabase();

                PrintHeader();
                using (var db = new FootballDbContext())
                {
                    while (true)
                    {
                        // Ввод команды
                        Console.Write("> ");
                        string? command = Console.ReadLine();
                        if (String.IsNullOrEmpty(command)) continue;

                        // Определение и выполнение команды
                        string[] commandParts = command.Split(" ");
                        if (commandParts[0] == "exit") break;   // выход из программы
                        else if (commandParts[0] == "help")     // вывод списка доступных команд
                            Console.WriteLine(commands);
                        else if (commandParts[0] == "info")     // вывод информации о всех записях в бд
                        {
                            PrintAllPlayers(db);
                            PrintAllTeams(db);
                            PrintAllPlayersInTeams(db);
                        }
                        else if (commandParts.Length >= 2 && commandParts[0] == "add" && commandParts[1] == "player")   // добавление игрока
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

                            redisDb.StringIncrement("playerCount");
                        }
                        else if (commandParts.Length >= 2 && commandParts[0] == "add" && commandParts[1] == "team")     // добавление команды
                        {
                            // Ввод данных о команде
                            Console.WriteLine("Введите название команды:");
                            string? name = Console.ReadLine();
                            var team = db.Teams.Add(new Team { Name = name ?? String.Empty });

                            // Добавление команды в бд
                            db.SaveChanges();
                            Console.WriteLine($"Id новой команды: {team.Entity.Id}");
                        }
                        else if (commandParts.Length >= 2 && commandParts[0] == "add" && commandParts[1] == "playerInTeam")     // добавление игрока в команду
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

                            redisDb.StringIncrement("totalExpenses", salary);
                        }
                        else if (commandParts.Length >= 2 && commandParts[0] == "get" && commandParts[1] == "playerCount")      // вывод общего кол-ва игроков
                        {
                            string? value = redisDb.StringGet("playerCount");
                            Console.WriteLine($"Общее кол-во игроков: {value ?? "0"}");
                        }
                        else if (commandParts.Length >= 2 && commandParts[0] == "get" && commandParts[1] == "totalExpenses")    // вывод суммарных расходов на игроков
                        {
                            string? value = redisDb.StringGet("totalExpenses");
                            Console.WriteLine($"Суммарные расходы на игроков: {value ?? "0"}");
                        }
                        else
                        {
                            Console.WriteLine("Команда не распознана!");
                        }
                    }
                }
            }
            catch (Npgsql.PostgresException)
            {
                Console.WriteLine("Не удалось подключиться к PostgreSQL!");
                return;
            }
            catch (RedisException)
            {
                Console.WriteLine("Не удалось подключиться к Redis Server!");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            Console.WriteLine("Список команд:");
            foreach (var team in db.Teams)
            {
                Console.WriteLine($"\t Id - {team.Id}, Name - {team.Name}");
            }
        }

        private static void PrintAllPlayersInTeams(FootballDbContext db)
        {
            Console.WriteLine("Список игроков в командах:");
            foreach (var playerInTeam in db.PlayersInTeams)
            {
                Console.WriteLine($"\t Игрок #{playerInTeam.PlayerId} находится в команде #{playerInTeam.TeamId} с зарплатой {playerInTeam.Salary}");
            }
        }
    }
}
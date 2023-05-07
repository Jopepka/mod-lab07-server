using Lab07;

namespace ShanukLab7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double Lambda = 5;
            double Mu = 1;
            int countPool = 2;

            string pathToSave = "AnsStatistic.txt";

            Statistic statistic = new Statistic(Lambda, Mu, countPool);

            string ansStatistic = statistic.Start();
            Console.WriteLine("\n" + ansStatistic);

            FileSave(pathToSave, ansStatistic);
            Console.WriteLine("\nСтатистика сохранена в файл " + pathToSave);
        }

        static void FileSave(string path, string text)
        {
            if (!File.Exists(path))
            {
                var file = File.Create(path);
                file.Close();
            }

            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine(text);
            sw.Close();
        }
    }
}
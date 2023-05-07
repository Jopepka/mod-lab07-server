using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Lab07
{
    public class Statistic
    {
        double Lambda { get; } //Lambda интенсивность поступления
        double Mu { get; } //Mu интенсивность обслуживание
        double UnitTime { get; } = 1000;
        int CountApplications { get; }

        int CountPool { get; }

        public Statistic(double lambda, double mu, int countPool, int countApplications = 10)
        {
            Lambda = lambda;
            Mu = mu;
            CountPool = countPool;
            CountApplications = countApplications;
        }

        public string Start()
        {
            Server server = new Server(CountPool, Mu);
            Client clients = new Client(server);
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < CountApplications; i++)
            {
                clients.Start(i);
                Thread.Sleep((int)(1 / Lambda * UnitTime));
            }

            while (server.IsNotWork())
                ;

            sw.Stop();

            double timeWork = sw.Elapsed.TotalSeconds;

            return ResultStatisticStr(server, timeWork);
        }

        string ResultStatisticStr(Server server, double timeWork)
        {
            string ans = "Параметры системы: ";
            ans += "\nИнтенсивность поступления: " + Lambda;
            ans += "\nИнтенсивность обслуживания: " + Mu;
            ans += "\nКоличество каналов сервера: " + CountPool;

            ans += "\n\nСервер принял: " + server.RequestCount;
            ans += "\nСервер обслужил: " + server.ProcessedCount;
            ans += "\nСервер отказал: " + server.RejectedCount;

            ans += "\n\nТеоретические данные: ";
            ans += "\n" + StatisticStr(Lambda, Mu);

            ans += "\n\nФактические данные:";
            ans += "\n" + StatisticStr(server.RequestCount / timeWork, server.ProcessedCount / timeWork);

            return ans;
        }

        string StatisticStr(double lambda, double mu)
        {
            string ans = "";
            ans += "Приведенная интенсивность: " + Ro(lambda, mu);
            ans += "\nВероятность простоя системы: " + ProbabilityDowntime(lambda, mu);
            ans += "\nВероятность отказа системы: " + ProbabilityFailure(lambda, mu);
            ans += "\nОтносительная пропускная способность: " + RelativeThroughput(lambda, mu);
            ans += "\nАбсолютная пропускная способность: " + AbsoluteThroughput(lambda, mu);
            ans += "\nСреднее число занятых каналов: " + AverageEmployed(lambda, mu);

            return ans;
        }

        double Ro(double lambda, double mu)
        {
            return lambda / mu;
        }

        double ProbabilityDowntime(double lambda, double mu)
        {
            double P = 0;

            for (int i = 0; i <= CountPool; i++)
                P += Math.Pow(Ro(lambda, mu), i) / Factorial(i);

            return 1 / P;
        }

        double ProbabilityFailure(double lambda, double mu)
        {
            return Math.Pow(Ro(lambda, mu), CountPool) * ProbabilityDowntime(lambda, mu) / Factorial(CountPool);
        }

        double RelativeThroughput(double lambda, double mu)
        {
            return 1 - ProbabilityFailure(lambda, mu);
        }

        double AbsoluteThroughput(double lambda, double mu)
        {
            return lambda * RelativeThroughput(lambda, mu);
        }

        double AverageEmployed(double lambda, double mu)
        {
            return AbsoluteThroughput(lambda, mu) / mu;
        }

        static double Factorial(int n)
        {
            double ans = 1;

            if (n == 0)
                return 1;

            for (int i = 1; i <= n; i++)
                ans *= i;

            return ans;
        }
    }
}

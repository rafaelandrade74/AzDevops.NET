using AzDevOps.NET.Config;
using AzDevOps.NET.WorkItemQL;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AzDevOpsConfig devOpsConfig = new AzDevOpsConfig("TOKEN", "NOME DO PROJETO");

            Wiql wiql = new Wiql(devOpsConfig, "NOME DO TIME QUE CONTEM A QUERY");
            var retorno = wiql.Get("ID DA QUERY").Result;

            var retorno2 = wiql.GetDetailsDataTable(retorno).Result;
        }
    }
}

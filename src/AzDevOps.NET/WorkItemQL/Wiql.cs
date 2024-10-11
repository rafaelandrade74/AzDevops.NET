using AzDevOps.NET.Config;
using AzDevOps.NET.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzDevOps.NET.WorkItemQL
{
    public class Wiql
    {
        private AzDevOpsConfig azDevOpsConfig;
        private readonly string project;

        public Wiql(AzDevOpsConfig azDevOpsConfig, string project)
        {
            this.azDevOpsConfig = azDevOpsConfig ?? throw new System.ArgumentNullException(nameof(azDevOpsConfig), "Campo nulo");
            this.project = project;
        }

        public async Task<JObject> Get(string queryId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Encoding.ASCII.GetBytes($":{azDevOpsConfig.personalAccessToken}")));

                    var requestUri = $"{azDevOpsConfig.url}/{azDevOpsConfig.organization}/{project}/_apis/wit/wiql/{queryId}?api-version={azDevOpsConfig.version}";

                    using (HttpResponseMessage response = await client.GetAsync(requestUri))
                    {
                        response.EnsureSuccessStatusCode();
                        var responseBody = await response.Content.ReadAsStringAsync();
                        JObject jsonResponse = JObject.Parse(responseBody);
                        return jsonResponse;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<WorkItem> GetDetails(JObject jobject)
        {
            if (jobject == null) throw new ArgumentNullException(nameof(jobject));

            var workItem = new WorkItem();

            // Verifica se o JSON tem as chaves necessárias
            if (jobject.TryGetValue("workItems", out JToken workItems)
                && jobject.TryGetValue("columns", out JToken workItemsColumns))
            {
                // Cria um cliente HTTP para buscar detalhes adicionais
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Encoding.ASCII.GetBytes($":{azDevOpsConfig.personalAccessToken}")));

                    foreach (var wki in workItemsColumns)
                    {
                        workItem.Columns.Add(new WorkItemColumn
                        {
                            Name = wki["name"].ToString(),
                            ColumnName = wki["referenceName"].ToString()
                        });
                    }

                    // Itera pelos work items
                    foreach (var jworkItem in workItems)
                    {
                        string workItemUrl = jworkItem["url"].ToString();

                        // Faz a requisição para pegar os detalhes do work item
                        using (HttpResponseMessage response = await client.GetAsync(workItemUrl))
                        {
                            response.EnsureSuccessStatusCode();
                            var detailContent = await response.Content.ReadAsStringAsync();

                            // Converte a resposta do detalhe em JSON
                            var workItemDetailJson = JObject.Parse(detailContent);

                            // Pegar os campos 
                            if (!workItemDetailJson.TryGetValue("fields", out JToken fieldsJson)) continue;

                            var linha = workItem.Rows.Add("System.Id", jworkItem["id"].ToString());
                                                        
                            foreach (var wki in workItem.Columns)
                            {
                                var refName = fieldsJson[wki.ColumnName];
                                if (refName == null || refName.ToString() == string.Empty) continue;
                                linha[wki.ColumnName] = refName.ToString();
                            }
                            workItem.Rows.Add(linha);
                        }
                    }
                }
            }

            return workItem;
        }
        public async Task<DataTable> GetDetailsDataTable(JObject jobject)
        {
            if (jobject == null) throw new ArgumentNullException(nameof(jobject));

            // Cria um DataTable para armazenar os dados
            DataTable dataTable = new DataTable();

            // Verifica se o JSON tem as chaves necessárias
            if (jobject.TryGetValue("workItems", out JToken workItems)
                && jobject.TryGetValue("columns", out JToken workItemsColumns))
            {
                // Adiciona colunas ao DataTable
                foreach (var wki in workItemsColumns)
                {
                    string columnName = wki["referenceName"].ToString();
                    dataTable.Columns.Add(columnName, typeof(string));
                }

                // Cria um cliente HTTP para buscar detalhes adicionais
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Encoding.ASCII.GetBytes($":{azDevOpsConfig.personalAccessToken}")));

                    // Itera pelos work items
                    foreach (var jworkItem in workItems)
                    {
                        string workItemUrl = jworkItem["url"].ToString();

                        // Faz a requisição para pegar os detalhes do work item
                        using (HttpResponseMessage response = await client.GetAsync(workItemUrl))
                        {
                            response.EnsureSuccessStatusCode();
                            var detailContent = await response.Content.ReadAsStringAsync();

                            // Converte a resposta do detalhe em JSON
                            var workItemDetailJson = JObject.Parse(detailContent);

                            // Pegar os campos 
                            if (!workItemDetailJson.TryGetValue("fields", out JToken fieldsJson)) continue;

                            // Cria uma nova linha no DataTable
                            DataRow linha = dataTable.NewRow();

                            // Preenche a coluna "System.Id"
                            linha["System.Id"] = jworkItem["id"].ToString();

                            // Preenche as outras colunas com os campos correspondentes
                            foreach (var wki in workItemsColumns)
                            {
                                string columnName = wki["referenceName"].ToString();
                                var refName = fieldsJson[columnName];
                                if (refName != null && refName.ToString() != string.Empty)
                                {
                                    linha[columnName] = refName.ToString();
                                }
                            }

                            // Adiciona a linha ao DataTable
                            dataTable.Rows.Add(linha);
                        }
                    }
                }
            }

            return dataTable;
        }


    }
}

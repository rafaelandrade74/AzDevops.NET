using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AzDevOps.NET.Model
{
    public class WorkItemRow : IEnumerable<WorkItemRow>
    {
        public Dictionary<string, string> Items { get; internal set; } = new Dictionary<string, string>();
        

        public WorkItemRow(string key, string value)
        {
            Items.Add(key, value);
        }

        // Indexador que permite acessar ou atribuir valores pelo nome da coluna
        public string this[string columnName]
        {
            get
            {
                if (Items.ContainsKey(columnName))
                {
                    return Items[columnName];
                }
                throw new ArgumentException($"Coluna '{columnName}' não encontrada.");
            }
            set
            {
                Items[columnName] = value;
            }
        }
        // Método para adicionar uma nova coluna e valor
        public void AddColumn(string columnName, string value)
        {
            if (Items.ContainsKey(columnName))
            {
                throw new ArgumentException($"Coluna '{columnName}' já existe.");
            }
            Items.Add(columnName, value);
        }

        // Método para remover uma coluna
        public bool RemoveColumn(string columnName)
        {
            return Items.Remove(columnName);
        }

        // Método para verificar se uma coluna existe
        public bool ContainsColumn(string columnName)
        {
            return Items.ContainsKey(columnName);
        }

        // Método para obter a lista de colunas
        public IEnumerable<string> GetColumns()
        {
            return Items.Keys;
        }

        // Método para limpar todos os dados da linha
        public void Clear()
        {
            Items.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator<WorkItemRow> IEnumerable<WorkItemRow>.GetEnumerator()
        {
            foreach (var item in Items)
            {
                yield return new WorkItemRow(item.Key, item.Value);
            }
        }

        // Método para obter o número de colunas
        public int ColumnCount => Items.Count;

        // Método para retornar os valores em formato de array
        public string[] ItemArray
        {
            get
            {
                var values = new List<string>();
                foreach (var value in Items.Values)
                {
                    values.Add(value);
                }
                return values.ToArray();
            }
            set
            {
                if (value.Length != Items.Count)
                {
                    throw new ArgumentException("O número de valores fornecidos não corresponde ao número de colunas.");
                }

                int i = 0;
                foreach (var key in Items.Keys)
                {
                    Items[key] = value[i++];
                }
            }
        }
    }
}

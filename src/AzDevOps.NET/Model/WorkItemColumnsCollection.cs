using System.Collections;
using System.Collections.Generic;

namespace AzDevOps.NET.Model
{
    public class WorkItemColumnsCollection : ICollection<WorkItemColumn>
    {
        // Armazenamento interno dos itens WorkItemColumns
        private readonly List<WorkItemColumn> _items = new List<WorkItemColumn>();

        public WorkItemColumnsCollection()
        {
        }

        // Construtor que pode inicializar com uma lista de WorkItemColumns
        public WorkItemColumnsCollection(IEnumerable<WorkItemColumn> items)
        {
            _items = new List<WorkItemColumn>(items);
        }

        // Propriedade Count retorna a contagem de itens na coleção
        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public void Add(WorkItemColumn item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items?.Clear();
        }

        public bool Contains(WorkItemColumn item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(WorkItemColumn[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        // Implementação do método GetEnumerator
        public IEnumerator<WorkItemColumn> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool Remove(WorkItemColumn item)
        {
            return _items.Remove(item);
        }

        // Implementação do método GetEnumerator do IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

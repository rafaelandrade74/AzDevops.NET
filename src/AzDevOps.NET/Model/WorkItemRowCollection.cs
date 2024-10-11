using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AzDevOps.NET.Model
{
    public class WorkItemRowCollection : ICollection
    {
        // Armazenamento interno dos itens WorkItemRows
        private readonly List<WorkItemRow> _items = new List<WorkItemRow>();
        public IEnumerable<WorkItemRow> Rows { get { return _items; } }

        public WorkItemRowCollection()
        {
        }
        
        // Construtor que pode inicializar com uma lista de WorkItemRows
        public WorkItemRowCollection(IEnumerable<WorkItemRow> items)
        {
            foreach (var item in items)
            {
                // Supondo que cada WorkItemRow tenha uma chave associada (ou você precisa gerar de outra forma)
                _items.Add(item);
            }
        }


        // Propriedade Count retorna a contagem de itens na coleção
        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        // Adiciona um item WorkItemRow à coleção (via chave e valor)
        public WorkItemRow Add(string name, string value)
        {
            var wk = new WorkItemRow(name,value);
            _items.Add(wk);
            return wk;
        }
        public void Add(IEnumerable<WorkItemRow> values)
        {
            foreach (var value in values)
            {
                _items.Add(value);
            }
        }

        public void Add(WorkItemRow item)
        {
            _items.Add(item);
        }

        // Limpa todos os itens da coleção
        public void Clear()
        {
            _items.Clear();
        }

        // Verifica se um WorkItemRow específico está presente na coleção
        public bool Contains(WorkItemRow item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(WorkItemRow[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool Remove(WorkItemRow item)
        {
            return _items.Remove(item);
        }

        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Índice deve ser não negativo.");
            }
            if ((array.Length - index) < _items.Count)
            {
                throw new ArgumentException("O array fornecido não tem espaço suficiente.");
            }

            for (int i = 0; i < _items.Count; i++)
            {
                array.SetValue(_items[i], index + i);
            }
        }
    }

}

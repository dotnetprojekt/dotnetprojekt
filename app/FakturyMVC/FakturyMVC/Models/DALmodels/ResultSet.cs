using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FakturyMVC.Models.DALmodels
{
    public class ResultSet<T>
    {
        private List<T> _items;
        private bool _isLastPage;

        public ResultSet()
        {
            _items = new List<T>();
            _isLastPage = false;
        }

        public List<T> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public bool IsLastPage
        {
            get { return _isLastPage; }
            set { _isLastPage = value; }
        }

        public int Count
        {
            get { return _items.Count; }
        }
    
        public void Add(T item)
        {
            _items.Add(item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        public void Clear()
        {
            _items.Clear();
            _isLastPage = false;
        }

        public T this[int key]
        {
            get { return _items[key]; }
            set { _items[key] = value; }
        }

        public T First()
        {
            return _items[0];
        }
    }
}
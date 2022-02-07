using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Bitango_.Models
{
    public class Menu : IEnumerable<Category>
    {
        List<Category> categories = new List<Category>();

        public Menu()
        {

        }
        public void Add(Category category) => categories.Add(category);
        public IEnumerator<Category> GetEnumerator()
        {
            return ((IEnumerable<Category>)categories).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Category>)categories).GetEnumerator();
        }
        public ObservableCollection<Category> Categories => new ObservableCollection<Category>(categories);
    }
}

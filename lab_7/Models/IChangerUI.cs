using System.Collections.Specialized;
using System.ComponentModel;

namespace lab_7.Models
{
    interface IChangerUI
    {
        public void ChangeProperty(object sender, PropertyChangedEventArgs e);
        public void ChangeCollection(object sender, NotifyCollectionChangedEventArgs e);
    }
}

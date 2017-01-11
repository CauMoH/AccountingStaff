using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class BaseDataCollection<TItem> : ObservableCollection<TItem>
    {
        public BaseDataCollection()
        {

        }

        public BaseDataCollection(IEnumerable<TItem> items)
        {
            AddRange(items);
        }

        public event Action<TItem> OnItemAdd;
        public event Action<TItem> OnItemRemove;

        public void AddRange(IEnumerable<TItem> items)
        {
            ExecuteNotifyOnce(() =>
            {
                foreach (var item in items)
                {
                    Add(item);
                }
            });
        }

        public void SetRange(IEnumerable<TItem> items)
        {
            ExecuteNotifyOnce(() =>
            {
                Clear();
                foreach (var item in items)
                {
                    Add(item);
                }
            });
        }

        public void RaiseCollectionChanged()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool SuppressNotification { get; set; }

        private void ExecuteNotifyOnce(Action action)
        {
            var prevSuppress = SuppressNotification;
            SuppressNotification = true;
            try
            {
                action();
            }
            finally
            {
                SuppressNotification = prevSuppress;
            }

            RaiseCollectionChanged();
        }

        protected override void ClearItems()
        {
            if (OnItemRemove != null)
            {
                foreach (var item in this)
                {
                    OnItemRemove?.Invoke(item);
                }
            }

            base.ClearItems();
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (OnItemRemove != null && e.OldItems != null)
            {
                foreach (var item in e.OldItems.Cast<TItem>())
                {
                    OnItemRemove?.Invoke(item);
                }
            }

            if (OnItemAdd != null && e.NewItems != null)
            {
                foreach (var item in e.NewItems.Cast<TItem>())
                {
                    OnItemAdd?.Invoke(item);
                }
            }

            if (!SuppressNotification)
            {
                base.OnCollectionChanged(e);
            }
        }
    }
}

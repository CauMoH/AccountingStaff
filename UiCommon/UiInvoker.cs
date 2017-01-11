using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UiCommon
{
    public static class UiInvoker
    {
        public static void Invoke(Action action)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(action);
            }
        }

        public static TResult Invoke<TResult>(Func<TResult> func)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                return func();
            }
            else
            {
                return Application.Current.Dispatcher.Invoke(func);
            }
        }

        public static void InvokeAsync(Action action)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.InvokeAsync(action);
            }
        }
    }
}

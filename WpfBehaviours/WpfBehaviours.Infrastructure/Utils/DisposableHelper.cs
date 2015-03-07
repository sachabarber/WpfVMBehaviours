using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace WpfBehaviours.Infrastructure.Utils
{
    public static class DisposableHelper
    {
        /// <summary>
        /// We need to create a new CompositeDisposable, as once a CompositeDisposable is Disposed
        /// it is useless, and we must recreate it
        /// </summary>
        public static void CreateNewCompositeDisposable(ref CompositeDisposable compositeDisposable)
        {
            compositeDisposable.Dispose();
            compositeDisposable = new CompositeDisposable();
        }

    }
}

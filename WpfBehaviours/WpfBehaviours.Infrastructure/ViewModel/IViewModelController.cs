using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBehaviours.Infrastructure.ViewModel
{
    public interface IViewModelController : IDisposable
    {
        /// <summary>
        /// controllers that are expecting to receieve a ViewModel should use this start method
        /// to do any initial RX hookups with the ViewModel that they get given
        /// </summary>
        void Start();
    }
}

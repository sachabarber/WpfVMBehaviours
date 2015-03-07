using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfBehaviours.Infrastructure.Services
{
    public interface IDispatcherService
    {
        /// <summary>
        /// Gets the current dispatcher.
        /// <para />
        /// Internally, this property uses the <see cref="DispatcherHelper"/>, but can be overriden if required.
        /// </summary>
        Dispatcher CurrentDispatcher { get; }

        /// <summary>
        /// Executes the specified action with the specified arguments synchronously on the thread the Dispatcher is associated with.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> is <c>null</c>.</exception>
        /// <remarks>
        /// For target frameworks where the <see cref="Dispatcher"/> class does not contain the <c>Invoke</c> method, the <c>BeginInvoke</c>
        /// method will be used instead.
        /// </remarks>
        void Invoke(Action action);

        /// <summary>
        /// Executes the specified delegate with the specified arguments synchronously on the thread the Dispatcher is associated with.
        /// </summary>
        /// <param name="method">A delegate to a method that takes parameters specified in args, which is pushed onto the Dispatcher event queue.</param>
        /// <param name="args">An array of objects to pass as arguments to the given method. Can be <c>null</c>.</param>
        /// <remarks>
        /// For target frameworks where the <see cref="Dispatcher"/> class does not contain the <c>Invoke</c> method, the <c>BeginInvoke</c>
        /// method will be used instead.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="method"/> is <c>null</c>.</exception>
        void Invoke(Delegate method, params object[] args);

        /// <summary>
        /// Executes the specified action asynchronously with the specified arguments on the thread that the Dispatcher was created on.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> is <c>null</c>.</exception>
        /// <remarks>
        /// For target frameworks where the <see cref="Dispatcher"/> class does not contain the <c>Invoke</c> method, the <c>BeginInvoke</c>
        /// method will be used instead.
        /// </remarks>
        void BeginInvoke(Action action);

        /// <summary>
        /// Executes the specified delegate asynchronously with the specified arguments on the thread that the Dispatcher was created on.
        /// </summary>
        /// <param name="method">A delegate to a method that takes parameters specified in args, which is pushed onto the Dispatcher event queue.</param>
        /// <param name="args">An array of objects to pass as arguments to the given method. Can be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="method"/> is <c>null</c>.</exception>
        void BeginInvoke(Delegate method, params object[] args);

        /// <summary>
        /// Executes the specified action asynchronously with the specified arguments on the thread that the Dispatcher was created on if required.
        /// <para />
        /// To check whether this is necessary, it will check whether the current thread has access to the dispatcher.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> is <c>null</c>.</exception>
        /// <remarks>
        /// For target frameworks where the <see cref="Dispatcher"/> class does not contain the <c>Invoke</c> method, the <c>BeginInvoke</c>
        /// method will be used instead.
        /// </remarks>
        void BeginInvokeIfRequired(Action action);

        /// <summary>
        /// Executes the specified delegate asynchronously with the specified arguments on the thread that the Dispatcher was created on if required.
        /// <para />
        /// To check whether this is necessary, it will check whether the current thread has access to the dispatcher.
        /// </summary>
        /// <param name="method">A delegate to a method that takes parameters specified in args, which is pushed onto the Dispatcher event queue.</param>
        /// <param name="args">An array of objects to pass as arguments to the given method. Can be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="method"/> is <c>null</c>.</exception>
        void BeginInvokeIfRequired(Delegate method, params object[] args);

        /// <summary>
        /// Executes the specified delegate asynchronously with the specified arguments on the thread that the Dispatcher was created on.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="onlyBeginInvokeWhenNoAccess">if set to <c>true</c>, the action will be executed directly if possible. Otherwise, 
        /// <c>Dispatcher.BeginInvoke</c> will be used.</param>
        void BeginInvoke(Action action, bool onlyBeginInvokeWhenNoAccess);
    }

}

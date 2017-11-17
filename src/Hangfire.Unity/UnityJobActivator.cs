using System;
using Unity;

namespace Hangfire
{
    /// <summary>
    /// A job activator which use unity to create job instance
    /// </summary>
    public class UnityJobActivator : JobActivator
    {
        private readonly IUnityContainer container;

        /// <summary>
        /// Initialize a new instance of the <see cref="T:UnityJobActivator"/> class
        /// </summary>
        /// <param name="container">The unity container to be used</param>
        public UnityJobActivator(IUnityContainer container)
        {
	        this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <inheritdoc />
        public override object ActivateJob(Type jobType)
        {
            return container.Resolve(jobType);
        }

        [Obsolete("Please implement/use the BeginScope(JobActivatorContext) method instead. Will be removed in 2.0.0.")]
        public override JobActivatorScope BeginScope()
        {
            return new UnityScope(container.CreateChildContainer());
        }

        class UnityScope : JobActivatorScope
        {
            private readonly IUnityContainer container;

            public UnityScope(IUnityContainer container)
            {
                this.container = container;
            }

            public override object Resolve(Type type)
            {
                return container.Resolve(type);
            }

            public override void DisposeScope()
            {
                container.Dispose();
            }
        }
    }
}

namespace LoveBank.Common
{
    public interface IServiceInjector
    {
        /// <summary>
        /// Injects the matching dependences.
        /// </summary>
        /// <param name="instance">The instance.</param>
        void Inject(object instance);
    }
}

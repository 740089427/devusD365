using System;

namespace devusD365
{
    public enum PluginAssemblyIsolationEnum
    {
        None = 1,
        Sandbox
    }
    public static class PluginAssemblyIsolationModel
    {
        public static readonly int None = Convert.ToInt32(PluginAssemblyIsolationEnum.None);
        public static readonly int Sandbox = Convert.ToInt32(PluginAssemblyIsolationEnum.Sandbox);
    }
    public class ThreadSafeLazy<T>
    {
        private readonly object Mutex = new object();
        private readonly Func<T> factory;
        private T value;
        private bool valueCreaded;
        public T Value
        {
            get
            {
                if (!valueCreaded)
                {
                    object obj = this.Mutex;
                    lock (obj)
                    {
                        if (!this.valueCreaded)
                        {
                            this.CreateValue();
                        }
                    }
                }
                return this.value;

            }
        }
        public ThreadSafeLazy(Func<T> factory)
        {
            this.factory = factory;
            this.valueCreaded = false;
            this.Mutex = new object();
        }

        private void CreateValue()
        {
            this.value = this.factory();
            this.valueCreaded = true;
        }
    }
}
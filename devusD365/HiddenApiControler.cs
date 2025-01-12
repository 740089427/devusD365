namespace devusD365
{
    public abstract class HiddenApiControler: IHiddenApiControler
    {
        public PLuginContext PLugin_context
        {
            get; private set;
        }
        public void InitializeContext(PLuginContext pLugin_context)
        {
            this.PLugin_context = pLugin_context;
        }
    }
}
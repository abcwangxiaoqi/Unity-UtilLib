namespace ScriptNodeFlow
{
    public abstract class RouterCondition
    {
        protected SharedData shareData;
        public RouterCondition(SharedData data)
        {
            shareData = data;
        }

        public abstract bool justify();
    }
}
namespace NodeTool
{
    public abstract class BaseCondition
    {
        protected SharedData shareData;
        public BaseCondition(SharedData data)
        {
            shareData = data;
        }

        public abstract bool justify();
    }
}
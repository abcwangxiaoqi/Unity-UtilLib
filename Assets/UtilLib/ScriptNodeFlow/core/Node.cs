
namespace ScriptNodeFlow
{
    public abstract class Node
    {
        public EntityState State { get; private set; }

        protected SharedData shareData;
        public Node(SharedData data)
        {
            shareData = data;
        }

        public void run()
        {
            State = EntityState.Wait;
            execute();
        }

        //reset state when finish
        //be called by NodeController when state turn finished
        public void reset()
        {
            State = EntityState.Idle;
        }

        protected abstract void execute();

        //be called when flow is broken
        public virtual void stop()
        { }

        //you must call this when you're sure the execute method is finished completely,
        //then the current node move to the next one
        //
        //why be designed like this? 
        //cause maybe your execute method includes some asyn operations
        protected void finish()
        {
            State = EntityState.Finished;
        }
    }
}
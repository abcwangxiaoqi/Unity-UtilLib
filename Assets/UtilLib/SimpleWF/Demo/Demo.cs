using UnityEngine;
using SimpleWF;

namespace SimpleWF
{
    public class Demo : MonoBehaviour
    {
        public static bool allow4 = false;
        public static int N = 0;

        void Start()
        {
            WorkFlow flow = new WorkFlow();

            WFEntity sp1 = new Step1();
            WFEntity sp2 = new Step2();
            WFEntity sp3 = new Step3();
            WFRouter router1 = new WFRouter();
            WFRouter router2 = new WFRouter();

            flow.StartWith(sp1);
            sp1.Router(router1);
            router1.If(() => { return N <= 5; }).Then(sp1);
            router1.Default(sp2);
            sp2.Router(router2);
            router2.If(() => { return N <= 20; }).Then(sp2);
            router2.Default(sp3);
            sp3.End("finshhshshsh");
            flow.Register(end);
            flow.Run();

            flow.Stop();

            flow.Run();
        }

        void end(bool success, string msg)
        {
            Debug.Log("msg=" + msg);
        }
    }

    public class Step1 : WFEntity
    {
        public override void execute()
        {
            Demo.N += 1;
            Debug.Log("Step 1");
            notify();
        }

        public override void stop()
        {
            base.stop();
        }
    }

    public class Step2 : WFEntity
    {
        public override void execute()
        {
            Demo.N += 2;
            Debug.Log("Step 2");
            notify();
        }
    }

    public class Step3 : WFEntity
    {
        public override void execute()
        {
            Debug.Log("Step 3");
            notify();
        }
    }
}
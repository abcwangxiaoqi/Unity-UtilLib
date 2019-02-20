using ParadoxNotion.Design;

namespace FlowCanvas.Nodes{

	[Description("Perform a for loop")]
	[Category("Flow Controllers/Iterators")]
	[ContextDefinedInputs(typeof(int))]
	[ContextDefinedOutputs(typeof(int))]
	public class ForLoop : FlowControlNode {
		
		private int current;
		private bool broken;

		ValueInput<int> first;
		ValueInput<int> last;
		ValueInput<int> step;

		protected override void RegisterPorts(){
			first = AddValueInput<int>("First");
			last = AddValueInput<int>("Last", "Loops");
			step = AddValueInput<int>("Step").SetDefaultAndSerializedValue(1);
			
			AddValueOutput<int>("Index", ()=> {return current;});
			var fCurrent = AddFlowOutput("Do");
			var fFinish = AddFlowOutput("Done");
			AddFlowInput("In", (f)=>
			{
				current = 0;
				broken = false;
				f.breakCall = ()=>{ broken = true; };
				var increment = UnityEngine.Mathf.Max(step.value, 1);
				for (var i = first.value; i < last.value; i += increment){
					if (broken){
						break;
					}
					current = i;
					fCurrent.Call(f);
				}
				f.breakCall = null;
				fFinish.Call(f);
			});

			AddFlowInput("Break", (f)=>{ broken = true; });
		}		
	}
}
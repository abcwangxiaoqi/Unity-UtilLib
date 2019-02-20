using ParadoxNotion;
using ParadoxNotion.Design;
using Logger = ParadoxNotion.Services.Logger;

namespace FlowCanvas.Nodes
{
	[Description("Should always be used to return out of a Custom Function. The return value is only required if the Custom Function returns a value as well.")]
	[Category("Functions/Custom")]
	[ContextDefinedInputs(typeof(object))]
	public class Return : FlowControlNode{

		[GatherPortsCallback]
		public bool useReturnValue = true;

		private ValueInput<object> returnPort;

		protected override void RegisterPorts(){
			if (useReturnValue){
				returnPort = AddValueInput<object>("Value");
			}
			AddFlowInput(" ", (f)=>
			{
				var returnData = f.PopReturnData();
				if (returnData.returnCall == null){
					Fail("The 'Return' node should be called as part of a Custom Function node.");
					return;
				}

				if (useReturnValue){
					var returnValue = returnPort.value;
					if ( returnData.returnType == null){
						if (returnValue != null){
							Logger.LogWarning("Function Returns a value, but no value is required", null, this);
						}
						returnData.returnCall(returnValue);
						return;
					}

					var returnType = returnValue != null? returnValue.GetType() : null;
					if ( (returnType == null && returnData.returnType.RTIsValueType() ) || (returnType != null && !returnData.returnType.RTIsAssignableFrom(returnType) ) ){
						Fail(string.Format("Return Value is not of expected type '{0}'.", returnData.returnType.FriendlyName() ) );
						return;
					}

					returnData.returnCall(returnValue);
				} else {
					returnData.returnCall(null);
				}
			});
		}
	}

/*
	///----------------------------------------------------------------------------------------------

	[Description("Should always be used to return out of a Custom Function. The return value is only required if the Custom Function returns a value as well.")]
	[Category("Functions/Custom")]
	[ContextDefinedInputs(typeof(object))]
	[ExposeAsDefinition]
	public class Return<T> : FlowControlNode{
		protected override void RegisterPorts(){
			var returnPort = AddValueInput<T>("Value");
			AddFlowInput(" ", (f)=>
			{
				var returnData = f.PopReturnData();
				if (returnData.returnCall == null){
					Fail("The 'Return' node should be called as part of a Custom Function node.");
					return;
				}

				var returnValue = returnPort.value;
				if ( returnData.returnType == null){
					if (returnValue != null){
						Logger.LogWarning("Function Returns a value, but no value is required", null, this);
					}
					returnData.returnCall(returnValue);
					return;
				}

				var returnType = returnValue != null? returnValue.GetType() : null;
				if ( (returnType == null && returnData.returnType.RTIsValueType() ) || (returnType != null && !returnData.returnType.RTIsAssignableFrom(returnType) ) ){
					Fail(string.Format("Return Value is not of expected type '{0}'.", returnData.returnType.FriendlyName() ) );
					return;
				}

				returnData.returnCall(returnValue);
			});
		}
	}	
*/

}
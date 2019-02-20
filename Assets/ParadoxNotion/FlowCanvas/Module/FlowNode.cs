﻿#define DO_EDITOR_BINDING //comment this out to test the real performance without editor binding specifics

#if UNITY_EDITOR
using UnityEditor;
using NodeCanvas.Editor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;
using Logger = ParadoxNotion.Services.Logger;

namespace FlowCanvas {

    ///The base node class for FlowGraph systems
    abstract public class FlowNode : Node, ISerializationCallbackReceiver {

		///----------------------------------------------------------------------------------------------

		[AttributeUsage(AttributeTargets.Class)] ///Helper attribute for context menu
		public class ContextDefinedInputsAttribute : Attribute{
			readonly public Type[] types;
			public ContextDefinedInputsAttribute(params Type[] types){
				this.types = types;
			}
		}

		[AttributeUsage(AttributeTargets.Class)] ///Helper attribute for context menu
		public class ContextDefinedOutputsAttribute : Attribute{
			readonly public Type[] types;
			public ContextDefinedOutputsAttribute(params Type[] types){
				this.types = types;
			}
		}

		[AttributeUsage(AttributeTargets.Class)] ///Helper attribute to show refresh button
		public class HasRefreshButtonAttribute : Attribute{}

		[AttributeUsage(AttributeTargets.Field)] ///When field change, ports will be gathered
		public class GatherPortsCallbackAttribute : CallbackAttribute{
			public GatherPortsCallbackAttribute() : base("GatherPorts"){}
		}

		///----------------------------------------------------------------------------------------------

		[SerializeField] //the user defined input port values
		private Dictionary<string, object> _inputPortValues;

		[NonSerialized] //all input ports
		private Dictionary<string, Port> inputPorts;
		[NonSerialized] //all output ports
		private Dictionary<string, Port> outputPorts;

		///----------------------------------------------------------------------------------------------

		sealed public override int maxInConnections{ get {return -1;} }
		sealed public override int maxOutConnections{ get {return -1;} }
		sealed public override bool allowAsPrime{ get {return false;} }
		sealed public override Type outConnectionType{ get {return typeof(BinderConnection);} }
		sealed public override Alignment2x2 commentsAlignment{ get {return Alignment2x2.Bottom;} }
		public override Alignment2x2 iconAlignment{ get {return Alignment2x2.Left;} }
		public FlowGraph flowGraph{ get {return (FlowGraph)graph;} }

		///----------------------------------------------------------------------------------------------

		///Store the changed input port values.
		void ISerializationCallbackReceiver.OnBeforeSerialize(){
			if (inputPorts != null){
				var valueInputs = inputPorts.Values.OfType<ValueInput>().ToArray();
				if (valueInputs.Length > 0){
					_inputPortValues = new Dictionary<string, object>( StringComparer.Ordinal );
					foreach(var port in valueInputs){
						if (!port.isConnected && !port.isDefaultValue){
							_inputPortValues[port.ID] = port.serializedValue;
						}
					}
				}
			}

			// try
			// {
			// 	//keep connections in same order as their respective ports for consistency?
			// 	inConnections = inConnections.OfType<BinderConnection>().OrderBy(c => inputPorts.Values.ToList().IndexOf(c.targetPort) ).Cast<Connection>().ToList();
			// 	outConnections = outConnections.OfType<BinderConnection>().OrderBy(c => outputPorts.Values.ToList().IndexOf(c.sourcePort) ).Cast<Connection>().ToList();
			// }
			// catch (Exception e) { Logger.LogException(e); }
		}

		//Nothing... Instead, deserialize input port value AFTER GatherPorts and Validation. We can't call GatherPorts here.
		void ISerializationCallbackReceiver.OnAfterDeserialize(){}

		///----------------------------------------------------------------------------------------------

		///This is called when the node is created, duplicated or otherwise needs validation
		sealed public override void OnValidate(Graph flowGraph){
			GatherPorts();
		}

		//Callback on connection
		sealed public override void OnParentConnected(int i){
			if (i < inConnections.Count){
				var connection = inConnections[i] as BinderConnection;
				if (connection != null) {
					TryHandleWildPortConnection(connection.targetPort, connection.sourcePort);
					OnPortConnected(connection.targetPort, connection.sourcePort);
				}
			}
		}
		
		//Callback on connection
		sealed public override void OnChildConnected(int i){
			if (i < outConnections.Count){
				var connection = outConnections[i] as BinderConnection;
				if (connection != null) {
					TryHandleWildPortConnection(connection.sourcePort, connection.targetPort);					
					OnPortConnected(connection.sourcePort, connection.targetPort);
				}
			}
		}

		//Callback on connection
		sealed public override void OnParentDisconnected(int i){
			if (i < inConnections.Count){
				var connection = inConnections[i] as BinderConnection;
				if (connection != null) { OnPortDisconnected(connection.targetPort, connection.sourcePort); }
			}
		}

		//Callback on connection
		sealed public override void OnChildDisconnected(int i){
			if (i < outConnections.Count){
				var connection = outConnections[i] as BinderConnection;
				if (connection != null) { OnPortDisconnected(connection.sourcePort, connection.targetPort); }
			}			
		}

		///---------------------------------------------------------------------------------------------

		virtual public void OnPortConnected(Port port, Port otherPort){}
		virtual public void OnPortDisconnected(Port port, Port otherPort){}

		///---------------------------------------------------------------------------------------------

		///Bind the port delegates. Called at runtime
		public void BindPorts(){
			for (var i = 0; i < outConnections.Count; i++){
				(outConnections[i] as BinderConnection).Bind();
			}
		}

		///Unbind the ports
		public void UnBindPorts(){
			for (var i = 0; i < outConnections.Count; i++){
				(outConnections[i] as BinderConnection).UnBind();
			}
		}

		///Gets an input Port by it's ID name which commonly is the same as it's name
		public Port GetInputPort(string ID){
			Port input = null;
			if (inputPorts != null){
				if (!inputPorts.TryGetValue(ID, out input)){
					// #if UNITY_EDITOR //update from previous version
					input = inputPorts.Values.FirstOrDefault(p => CheckReverseIDEquality(p, ID) );
					// #endif
				}
			}
			return input;
		}

		///Gets an output Port by it's ID name which commonly is the same as it's name
		public Port GetOutputPort(string ID){
			Port output = null;
			if (outputPorts != null){
				if (!outputPorts.TryGetValue(ID, out output)){
					// #if UNITY_EDITOR //update from previous version
					output = outputPorts.Values.FirstOrDefault(p => CheckReverseIDEquality(p, ID) );
					// #endif
				}
			}
			return output;
		}

		///----------------------------------------------------------------------------------------------

		///Returns all Flow Output Ports
		public FlowOutput[] GetOutputFlowPorts(){
			return outputPorts.Values.OfType<FlowOutput>().ToArray();
		}

		///Returns all Value Output Ports
		public ValueOutput[] GetOutputValuePorts(){
			return outputPorts.Values.OfType<ValueOutput>().ToArray();
		}

		///Returns all Flow Input Ports
		public FlowInput[] GetInputFlowPorts(){
			return inputPorts.Values.OfType<FlowInput>().ToArray();
		}

		///Returns all Value Input Ports
		public ValueInput[] GetInputValuePorts(){
			return inputPorts.Values.OfType<ValueInput>().ToArray();
		}

		///----------------------------------------------------------------------------------------------

		///Gets the BinderConnection of an Input port based on it's ID
		public BinderConnection GetInputConnectionForPortID(string ID){
			return inConnections.OfType<BinderConnection>().FirstOrDefault(c => c.targetPortID == ID);
		}

		///Gets the BinderConnection of an Output port based on it's ID
		public BinderConnection GetOutputConnectionForPortID(string ID){
			return outConnections.OfType<BinderConnection>().FirstOrDefault(c => c.sourcePortID == ID);
		}

		///Returns the first input port assignable to the type provided
		public Port GetFirstInputOfType(Type type){
			return inputPorts.Values.OrderBy(p => p is FlowInput? 0 : 1 ).FirstOrDefault(p => p.type.RTIsAssignableFrom(type) );
		}

		///Returns the first output port of a type assignable to the port
		public Port GetFirstOutputOfType(Type type){
			return outputPorts.Values.OrderBy(p => p is FlowInput? 0 : 1 ).FirstOrDefault(p => type.RTIsAssignableFrom(p.type) );
		}

		///----------------------------------------------------------------------------------------------
		
		///Set the Component or GameObject instance input port to Owner if not connected and not already set to another reference.
		///By convention, the instance port is always considered to be the first.
		///Called from Graph when started.
		public void AssignSelfInstancePort(){
			var instanceInput = inputPorts.Values.OfType<ValueInput>().FirstOrDefault();
			if (instanceInput != null && !instanceInput.isConnected && instanceInput.isDefaultValue){
				var instance = flowGraph.GetAgentComponent(instanceInput.type);
				if (instance != null){
					instanceInput.serializedValue = instance;
				}
			}
		}

		///Gather and register the node ports.
		public void GatherPorts(){
			inputPorts = new Dictionary<string, Port>( StringComparer.Ordinal );
			outputPorts = new Dictionary<string, Port>( StringComparer.Ordinal );
			RegisterPorts();
			DeserializeInputPortValues();
			
			#if UNITY_EDITOR && DO_EDITOR_BINDING
			OnPortsGatheredInEditor();
			ValidateConnections();
			#endif
		}

		///Override for registration/definition of ports.
		abstract protected void RegisterPorts();

		///Restore the serialized input port values
		void DeserializeInputPortValues(){

			if (_inputPortValues == null){
				return;
			}

			foreach (var pair in _inputPortValues){
				Port inputPort = null;
				if ( !inputPorts.TryGetValue(pair.Key, out inputPort) ){
					// #if UNITY_EDITOR //update from previous version
					inputPort = inputPorts.Values.FirstOrDefault(p => CheckReverseIDEquality(p, pair.Key) );
					// #endif
				}

				if (inputPort is ValueInput && pair.Value != null && inputPort.type.RTIsAssignableFrom(pair.Value.GetType())){
					(inputPort as ValueInput).serializedValue = pair.Value;
				}
			}
		}

		//Validate ports for connections
		//This is done seperately for Source and Target since we don't get control of when GatherPorts will be called on each node apart from in order of list (and we dont care)
		//So basicaly each node validates it's own inputs and outputs seperately.
		void ValidateConnections(){

			foreach (var cOut in outConnections.ToArray()){ //ToArray because connection might remove itself if invalid
				if (cOut is BinderConnection){
					(cOut as BinderConnection).GatherAndValidateSourcePort();
				}
			}

			foreach (var cIn in inConnections.ToArray()){ //ToArray because connection might remove itself if invalid
				if (cIn is BinderConnection){
					(cIn as BinderConnection).GatherAndValidateTargetPort();
				}
			}
		}

		///---------------------------------------------------------------------------------------------
		//Port registration/definition methods, to be used within RegisterPorts override

		///Add a new FlowInput with name and pointer. Pointer is the method to run when the flow port is called. Returns the new FlowInput object.
		public FlowInput AddFlowInput(string name, string ID, FlowHandler pointer){ return AddFlowInput(name, pointer, ID); }
		public FlowInput AddFlowInput(string name, FlowHandler pointer, string ID = ""){
			QualifyPortNameAndID(ref name, ref ID, inputPorts);
			return (FlowInput) (inputPorts[ID] = new FlowInput(this, name, ID, pointer) );
		}

		///Add a new FlowOutput with name. Returns the new FlowOutput object.
		public FlowOutput AddFlowOutput(string name, string ID = ""){
			QualifyPortNameAndID(ref name, ref ID, outputPorts);
			return (FlowOutput) (outputPorts[ID] = new FlowOutput(this, name, ID) );
		}

		///Recommended. Add a ValueInput of type T. Returns the new ValueInput<T> object.
		public ValueInput<T> AddValueInput<T>(string name, string ID = ""){
			QualifyPortNameAndID(ref name, ref ID, inputPorts);
			return (ValueInput<T>) (inputPorts[ID] = new ValueInput<T>(this, name, ID) );
		}

		///Recommended. Add a ValueOutput of type T. getter is the function to get the value from. Returns the new ValueOutput<T> object.
		public ValueOutput<T> AddValueOutput<T>(string name, string ID, ValueHandler<T> getter){ return AddValueOutput<T>(name, getter, ID); }
		public ValueOutput<T> AddValueOutput<T>(string name, ValueHandler<T> getter, string ID = ""){
			QualifyPortNameAndID(ref name, ref ID, outputPorts);
			return (ValueOutput<T>) (outputPorts[ID] = new ValueOutput<T>(this, name, ID, getter) );
		}

		///Add an object port of unkown runtime type. getter is a function to get the port value from. Returns the new ValueOutput object.
		///It is always recommended to use the generic versions to avoid value boxing/unboxing!
		public ValueInput AddValueInput(string name, string ID, Type type){ return AddValueInput(name, ID, type); }
		public ValueInput AddValueInput(string name, Type type, string ID = ""){
			QualifyPortNameAndID(ref name, ref ID, inputPorts);
			return (ValueInput) (inputPorts[ID] = ValueInput.CreateInstance(type, this, name, ID) );
		}
		
		///Add an object port of unkown runtime type. getter is a function to get the port value from. Returns the new ValueOutput object.
		///It is always recommended to use the generic versions to avoid value boxing/unboxing!
		public ValueOutput AddValueOutput(string name, string ID, Type type, ValueHandlerObject getter){ return AddValueOutput(name, type, getter, ID); }
		public ValueOutput AddValueOutput(string name, Type type, ValueHandlerObject getter, string ID = ""){
			QualifyPortNameAndID(ref name, ref ID, outputPorts);
			return (ValueOutput) (outputPorts[ID] = ValueOutput.CreateInstance(type, this, name, ID, getter) );
		}

		///Helper used above
		void QualifyPortNameAndID(ref string name, ref string ID, IDictionary dict){
			if (string.IsNullOrEmpty(ID)) ID = name;
			if (string.IsNullOrEmpty(ID)){
				ID = " ";
				while (dict.Contains(ID)){
					ID += " ";
				}
			}
		}

		//Check whether port can qualify for requested ID
		//This is only for upgrading from previous versions.
		bool CheckReverseIDEquality(Port port, string requestedID){
			if (port.ID.Trim() == requestedID.Trim()){ return true; }
			if (port.name.Trim() == requestedID.Trim()){ return true; }
			if (port.name.SplitCamelCase().Trim() == requestedID.Trim()){ return true; }
			return false;
		}

		///----------------------------------------------------------------------------------------------

		///Reflection based registration for target object. Nowhere used by default.
		void TryAddReflectionBasedRegistrationForObject(object instance){
			//FlowInputs. All void methods with one Flow parameter.
			foreach (var method in instance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)){
				TryAddMethodFlowInput(method, instance);
			}

			//ValueOutputs. All readable public properties.
			foreach (var prop in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)){
				TryAddPropertyValueOutput(prop, instance);
			}

			//Search for delegates fields
			foreach (var field in instance.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)){
				TryAddFieldDelegateFlowOutput(field, instance);
				TryAddFieldDelegateValueInput(field, instance);
			}
		}

		///Register a MethodInfo as FlowInput. Used only in reflection based registration.
		public FlowInput TryAddMethodFlowInput(MethodInfo method, object instance){
			var parameters = method.GetParameters();
			if (method.ReturnType == typeof(void) && parameters.Length == 1 && parameters[0].ParameterType == typeof(Flow)){
				var nameAtt = method.RTGetAttribute<NameAttribute>(false);
				var name = nameAtt != null? nameAtt.name : method.Name;
				var pointer = method.RTCreateDelegate<FlowHandler>(instance);
				return AddFlowInput(name, pointer);
			}
			return null;
		}

		///Register a FieldInfo Delegate (FlowHandler) as FlowOutput. Used only in reflection based registration.
		public FlowOutput TryAddFieldDelegateFlowOutput(FieldInfo field, object instance){
			if ( field.FieldType == typeof(FlowHandler) ){
				var nameAtt = field.RTGetAttribute<NameAttribute>(false);
				var name = nameAtt != null? nameAtt.name : field.Name;
				var flowOut = AddFlowOutput(name);
				field.SetValue(instance, (FlowHandler)flowOut.Call);
				return flowOut;
			}
			return null;
		}

		///Register a FieldInfo Delegate (ValueHandler<T>) as ValueInput. Used only in reflection based registration.
		public ValueInput TryAddFieldDelegateValueInput(FieldInfo field, object instance){
			if ( typeof(Delegate).RTIsAssignableFrom(field.FieldType) ){
				var invokeMethod = field.FieldType.GetMethod("Invoke");
				var parameters = invokeMethod.GetParameters();
				if (invokeMethod.ReturnType != typeof(void) && parameters.Length == 0){
					var nameAtt = field.RTGetAttribute<NameAttribute>(false);
					var name = nameAtt != null? nameAtt.name : field.Name;
					var delType = invokeMethod.ReturnType;
					var portType = typeof(ValueInput<>).RTMakeGenericType( delType );
					var port = (ValueInput)Activator.CreateInstance(portType, new object[]{ instance, name, name });
					var getterType = typeof(ValueHandler<>).RTMakeGenericType( delType );
					var getter = port.GetType().GetMethod("get_value").RTCreateDelegate(getterType, port);
					field.SetValue(instance, getter);
					inputPorts[name] = port;
					return port;
				}
			}
			return null;
		}

		///Register a PropertyInfo as ValueOutput. Used only in reflection based registration.
		public ValueOutput TryAddPropertyValueOutput(PropertyInfo prop, object instance){
			if (prop.CanRead){
				var nameAtt = prop.RTGetAttribute<NameAttribute>(false);
				var name = nameAtt != null? nameAtt.name : prop.Name;
				var getterType = typeof(ValueHandler<>).RTMakeGenericType( prop.PropertyType );
				var getter = prop.RTGetGetMethod().RTCreateDelegate(getterType, instance);
				var portType = typeof(ValueOutput<>).RTMakeGenericType( prop.PropertyType );
				var port = (ValueOutput)Activator.CreateInstance(portType, new object[]{ this, name, name, getter });
				return (ValueOutput) (outputPorts[name] = port);
			}
			return null;
		}

		///----------------------------------------------------------------------------------------------

		///Replace with another type.
		//1) Because SetTarget, SetSource also fires OnPortConnected, Wild ports are handled automatically.
		//2) GatherPorts is also firing connections validation.
		//3) Because connections are validated, changing connection types to correct types is also automatic.
		//So for example if we change to a new Wild type, OnPortConnected will re-set the type to closed.
		public FlowNode ReplaceWith(System.Type t){
			var newNode = graph.AddNode(t, this.position) as FlowNode;
			if (newNode == null) return null;
			foreach(var c in inConnections.ToArray()){
				c.SetTarget(newNode);
			}
			foreach(var c in outConnections.ToArray()){
				c.SetSource(newNode);
			}
			if (this._inputPortValues != null){
				newNode._inputPortValues = this._inputPortValues.ToDictionary(k => k.Key, v => v.Value);
			}
			newNode.GatherPorts();
			graph.RemoveNode(this);
			return (FlowNode)newNode;
		}

		///----------------------------------------------------------------------------------------------

		///Should return the base wild definition type with which new generic version can be made.
		virtual public Type GetNodeWildDefinitionType(){
			return this.GetType().GetFirstGenericParameterConstraintType();
		}

		///Handles connecting to a wild port and changing generic version to that new connection
		void TryHandleWildPortConnection(Port port, Port otherPort){
			TryHandleWildPortConnection(port.type, otherPort.type);
		}
		
		///Handles connecting to a wild port and changing generic version to that new connection
		void TryHandleWildPortConnection(Type currentType, Type targetType){
			var wildType = GetNodeWildDefinitionType();
			var content = this.GetType();
			var newType = TryGetNewGenericTypeForWild(wildType, currentType, targetType, content, null);
			if (newType != null){
				this.ReplaceWith(newType);
			}
		}

		///Give a wildType and two types, will try and return a closed type for wild definition of content
		public static Type TryGetNewGenericTypeForWild(Type wildType, Type currentType, Type targetType, Type content, Type context){
			if (wildType == null || !content.IsGenericType){
				return null;
			}
			var args = content.GetGenericArguments();
			var arg1 = args.FirstOrDefault();
			if (arg1 != wildType && arg1.IsGenericType){
				return TryGetNewGenericTypeForWild(wildType, currentType, targetType, arg1, content);
			}
			//Current arg type is wild type
			if (args.Length == 1 && arg1 == wildType){
				var otherPortElementType = targetType.GetEnumerableElementType();
				var portElementType = currentType.GetEnumerableElementType();
				var areBothEnumerable = otherPortElementType != null && portElementType != null;
				currentType = areBothEnumerable? portElementType : currentType;
				targetType = areBothEnumerable? otherPortElementType : targetType;
				//currentType is wild type, but only proceed if the target type is not
				if (currentType == wildType && targetType != currentType){
					content = content.GetGenericTypeDefinition();
					arg1 = content.GetGenericArguments().First();
					if (targetType.IsAllowedByGenericArgument(arg1)){
						var newType = content.MakeGenericType(targetType);
						if (context != null && context.IsGenericType){
							newType = context.GetGenericTypeDefinition().MakeGenericType(newType);
						}
						return newType;
					}
				}
			}
			return null;
		}
	
		///Give a wildType and two ports, will try and return a closed method for wild definition of content
		public static MethodInfo TryGetNewGenericMethodForWild(Type wildType, Type currentType, Type targetType, MethodInfo content){
			if (wildType == null || !content.IsGenericMethod){
				return null;
			}
			var args = content.GetGenericArguments();
			var arg1 = args.FirstOrDefault();
			if (args.Length == 1 && arg1 == wildType){
				var otherPortElementType = targetType.GetEnumerableElementType();
				var portElementType = currentType.GetEnumerableElementType();
				var areBothEnumerable = otherPortElementType != null && portElementType != null;
				currentType = areBothEnumerable? portElementType : currentType;
				targetType = areBothEnumerable? otherPortElementType : targetType;
				if (currentType == wildType && targetType != currentType){
					content = content.GetGenericMethodDefinition();
					arg1 = content.GetGenericArguments().First();
					if (targetType.IsAllowedByGenericArgument(arg1)){
						var newMethod = content.MakeGenericMethod( targetType );
						return newMethod;
					}
				}
			}
			return null;
		}




		///----------------------------------------------------------------------------------------------
		///---------------------------------------UNITY EDITOR-------------------------------------------
		///----------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		
		private static Port clickedPort;
		private static int dragDropMisses;

		private Port[] orderedInputs;
		private Port[] orderedOutputs;
		private ValueInput firstValuePort;
		private bool portsLayoutHasDrawn;


		//when gathering ports and we are in Unity Editor
		//gather the ordered inputs and outputs
		void OnPortsGatheredInEditor(){
			portsLayoutHasDrawn = false; //set flag to false whenever ports are gather
			orderedInputs = inputPorts.Values.OrderBy(p => p is FlowInput? 0 : 1 ).ToArray();
			orderedOutputs = outputPorts.Values.OrderBy(p => p is FlowOutput || p.IsDelegate()? 0 : 1 ).ToArray();
			firstValuePort = orderedInputs.OfType<ValueInput>().FirstOrDefault();
		}

		//Get all output Connections of a port. Used only for when removing
		BinderConnection[] GetOutPortConnections(Port port){
			return outConnections.OfType<BinderConnection>().Where(c => c.sourcePort == port ).ToArray();
		}

		//Get all input Connections of a port. Used only for when removing
		BinderConnection[] GetInPortConnections(Port port){
			return inConnections.OfType<BinderConnection>().Where(c => c.targetPort == port ).ToArray();
		}

		///A position on the right side of the node header for ports
		Vector2 GetHeaderPortPosOut(){
			return new Vector2( rect.xMax, rect.yMin + 16 );
		}

		///A position on the left side of the node header for ports
		Vector2 GetHeaderPortPosIn(){
			return new Vector2( rect.xMin, rect.yMin + 16 );
		}

		//Seal it...
		sealed protected override void DrawNodeConnections(Rect drawCanvas, bool fullDrawPass, Vector2 canvasMousePos, float zoomFactor){

			var e = Event.current;

			//Port container graphics
			if (fullDrawPass || drawCanvas.Overlaps(rect)){
				GUI.Box(new Rect(rect.x - 8, rect.y + 2, 10, rect.height), string.Empty, CanvasStyles.nodePortContainer);
				GUI.Box(new Rect(rect.xMax - 2, rect.y + 2, 10, rect.height), string.Empty, CanvasStyles.nodePortContainer);
			}
			///

			if ( fullDrawPass || drawCanvas.Overlaps(rect) ){

				var portRect = new Rect(0,0,10,10);

				//INPUT Ports
				if (orderedInputs != null){
					Port instancePort = null;
					for (var i = 0; i < orderedInputs.Length; i++){
						var port = orderedInputs[i];
						var canConnect = true;
						if (clickedPort != null){
							if ((port == clickedPort) ||
								(clickedPort.parent == port.parent) ||
								(clickedPort.IsInputPort()) ||
								(port.isConnected && port is ValueInput) ||
								(!TypeConverter.HasConvertion(clickedPort.type, port.type)) )
							{
								canConnect = false;
							}
						}

						port.pos = new Vector2(rect.xMin - 5, rect.y + port.posOffsetY);
						portRect.width = port.isConnected? 12:10;
						portRect.height = portRect.width;
						portRect.center = port.pos;

						//first gameobject or component port is considered to be the 'instance' port by convention
						if (port == firstValuePort){
							if ( port.IsUnitySceneObject() ){
								instancePort = port;
							}
						}

						//Port graphic
						if (clickedPort != null && !canConnect && clickedPort != port){
							GUI.color = new Color(1,1,1,0.3f);
						}
						GUI.Box(portRect, string.Empty, port.isConnected? CanvasStyles.nodePortConnected : CanvasStyles.nodePortEmpty);
						GUI.color = Color.white;

						//Tooltip
						if (portRect.Contains(e.mousePosition)){
							
							var labelString = (canConnect || port.isConnected || port == clickedPort)? port.type.FriendlyName() : "Can't Connect Here";
							var size = CanvasStyles.box.CalcSize(new GUIContent(labelString));
							var rect = new Rect(0, 0, size.x + 10, size.y + 5);
							rect.x = portRect.x - size.x - 10;
							rect.y = portRect.y - size.y/2;
							GUI.Box(rect, labelString, CanvasStyles.box);
						
						//Or value
						} else {

							if ( port is ValueInput && !port.isConnected){
								//Only these types are shown their value
								if ( port.type.IsValueType || port.type == typeof(Type) || port.type == typeof(string) || port.IsUnityObject() ){
									var value = (port as ValueInput).serializedValue;
									string labelString = null;
									if (!(port as ValueInput).isDefaultValue ){
										if (value is UnityEngine.Object && value as UnityEngine.Object != null){
											labelString = string.Format("<b><color=#66ff33>{0}</color></b>", (value as UnityEngine.Object).name);
										} else {
											labelString = value.ToStringAdvanced();
										}
									} else if ( port == instancePort ){
										var exists = true;
										if (graphAgent != null && typeof(Component).IsAssignableFrom(port.type) ){
											exists = graphAgent.GetComponent(port.type) != null;
										}
										var color = exists? "66ff33" : "ff3300";
										labelString = string.Format("<color=#{0}><b>♟ <i>Self</i></b></color>", color);
									} else {
										GUI.color = new Color(1,1,1,0.15f);
										labelString = value.ToStringAdvanced();
									}
									if (port.type == typeof(string)){
										labelString = labelString.CapLength(20);
									}
									var size = CanvasStyles.label.CalcSize(new GUIContent(labelString));
									var rect = new Rect(0, 0, size.x, size.y);
									rect.x = portRect.x - size.x - 5;
									rect.y = portRect.y - size.y * 0.3f; //*0.3? something's wrong here. FIX
									GUI.Label(rect, labelString, CanvasStyles.label);
									GUI.color = Color.white;
								}
							}
						}

						if (GraphEditorUtility.allowClick){
							//Right click removes connections
							if (port.isConnected && e.type == EventType.ContextClick && portRect.Contains(e.mousePosition)){
								foreach(var c in GetInPortConnections(port)){
									graph.RemoveConnection(c);
								}
								e.Use();
								return;
							}

							//Click initialize new drag & drop connection
							if (e.type == EventType.MouseDown && e.button == 0 && portRect.Contains(e.mousePosition)){
								if (port.CanAcceptConnections() ){
									dragDropMisses = 0;
									clickedPort = port;
									e.Use();
								}
							}

							//Drop on creates connection
							if (e.type == EventType.MouseUp && e.button == 0 && clickedPort != null){
								if (portRect.Contains(e.mousePosition) && port.CanAcceptConnections() ){
									BinderConnection.Create(clickedPort, port);
									clickedPort = null;
									e.Use();
								}
							}
						}

					}
				}

				//OUTPUT Ports
				if (orderedOutputs != null){
					for (var i = 0; i < orderedOutputs.Length; i++){
						var port = orderedOutputs[i];
						var canConnect = true;
						if (clickedPort != null){
							if ((port == clickedPort) ||
								(clickedPort.parent == port.parent) ||
								(clickedPort.IsOutputPort()) ||
								(port.isConnected && port is FlowOutput) ||
								(!TypeConverter.HasConvertion(port.type, clickedPort.type)) )
							{
								canConnect = false;
							}
						}

						port.pos = new Vector2(rect.xMax + 5, rect.y + port.posOffsetY);
						portRect.width = port.isConnected? 12:10;
						portRect.height = portRect.width;
						portRect.center = port.pos;

						//Port graphic
						if (clickedPort != null && !canConnect && clickedPort != port){
							GUI.color = new Color(1,1,1,0.3f);
						}
						GUI.Box(portRect, string.Empty, port.isConnected? CanvasStyles.nodePortConnected : CanvasStyles.nodePortEmpty);
						GUI.color = Color.white;

						//Tooltip
						if (portRect.Contains(e.mousePosition)){
							var labelString = (canConnect || port.isConnected || port == clickedPort)? port.type.FriendlyName() : "Can't Connect Here";
							var size = CanvasStyles.box.CalcSize(new GUIContent(labelString));
							var rect = new Rect(0, 0, size.x + 10, size.y + 5);
							rect.x = portRect.x + 15;
							rect.y = portRect.y - portRect.height/2;
							GUI.Box(rect, labelString, CanvasStyles.box);
						}

						if (GraphEditorUtility.allowClick){
							//Right click removes connections
							if (e.type == EventType.ContextClick && portRect.Contains(e.mousePosition)){
								foreach(var c in GetOutPortConnections(port)){
									graph.RemoveConnection(c);
								}
								e.Use();
								return;
							}

							//Click initialize new drag & drop connection
							if (e.type == EventType.MouseDown && e.button == 0 && portRect.Contains(e.mousePosition)){
								if (port.CanAcceptConnections() ){
									dragDropMisses = 0;
									clickedPort = port;
									e.Use();
								}
							}

							//Drop on creates connection
							if (e.type == EventType.MouseUp && e.button == 0 && clickedPort != null){
								if (portRect.Contains(e.mousePosition) && port.CanAcceptConnections() ){
									BinderConnection.Create(port, clickedPort);
									clickedPort = null;
									e.Use();
								}
							}
						}
						
					}
				}
			}

			///ACCEPT CONNECTION
			if (clickedPort != null && e.type == EventType.MouseUp){

				///ON NODE
				if (rect.Contains(e.mousePosition)){
					var cachePort = clickedPort;
					var menu = new GenericMenu();
					if (cachePort is ValueOutput || cachePort is FlowOutput){
						if (orderedInputs != null){
							foreach (var _port in orderedInputs.Where(p => p.CanAcceptConnections() && TypeConverter.HasConvertion(cachePort.type, p.type) )){
								var port = _port;
								menu.AddItem(new GUIContent(string.Format("To: '{0}'", port.name) ), false, ()=> { BinderConnection.Create(cachePort, port); } );
							}
						}
					} else {
						if (orderedOutputs != null){
							foreach (var _port in orderedOutputs.Where(p => p.CanAcceptConnections() && TypeConverter.HasConvertion(p.type, cachePort.type) )){
								var port = _port;
								menu.AddItem(new GUIContent(string.Format("From: '{0}'", port.name) ), false, ()=> { BinderConnection.Create(port, cachePort); } );
							}
						}
					} 

					//append menu items
					menu = OnDragAndDropPortContextMenu(menu, cachePort);

					//if there is only 1 option, just do it
					if (menu.GetItemCount() == 1){
						EditorUtils.GetMenuItems(menu)[0].func();
					} else {
						GraphEditorUtility.PostGUI += ()=> { menu.ShowAsContext(); };
					}

					clickedPort = null;
					e.Use();
					///

				///ON CANVAS
				} else {

					dragDropMisses ++;
					if (dragDropMisses == graph.allNodes.Count && clickedPort != null){
						var cachePort = clickedPort;
						clickedPort = null;
						DoContextPortConnectionMenu(cachePort, e.mousePosition, zoomFactor);
						e.Use();
					}
				}
			}

			//Temp connection line when linking
			if (clickedPort != null && clickedPort.parent == this){
				var from = clickedPort.pos;
				var to = e.mousePosition;
				var xDiff = (from.x - to.x) * 0.8f;
				xDiff = to.x > from.x? xDiff : -xDiff;
				var tangA = clickedPort is FlowInput || clickedPort is ValueInput? new Vector2(xDiff, 0) : new Vector2(-xDiff, 0);
				var tangB = tangA * -1;
				Handles.DrawBezier(from, to, from + tangA , to + tangB, new Color(0.5f,0.5f,0.8f,0.8f), null, 3);
			}

			//Actualy draw the existing connections
			for (var i = 0; i < outConnections.Count; i++){
				var binder = outConnections[i] as BinderConnection;
				if (binder != null){ //for in case it's MissingConnection
					var sourcePort = binder.sourcePort;
					var targetPort = binder.targetPort;
					if (sourcePort != null && targetPort != null){
						if (fullDrawPass || drawCanvas.Overlaps(RectUtils.GetBoundRect(sourcePort.pos, targetPort.pos) ) ){
							binder.DrawConnectionGUI(sourcePort.pos, targetPort.pos);
						}
					}
				}
			}
		}

		///Let nodes handle ports draged on top of them
		virtual protected GenericMenu OnDragAndDropPortContextMenu(GenericMenu menu, Port port){
			return menu;
		}

		///Context menu for when dragging a connection on empty canvas
		void DoContextPortConnectionMenu(Port clickedPort, Vector2 mousePos, float zoomFactor){
			var menu = new GenericMenu();
			if (clickedPort is ValueInput || clickedPort is ValueOutput){
				menu = flowGraph.AppendTypeReflectionNodesMenu(menu, clickedPort.type, "", mousePos, clickedPort, null);
			}
			menu = flowGraph.AppendFlowNodesMenu(menu, "", mousePos, clickedPort, null);
			menu = flowGraph.AppendSimplexNodesMenu(menu, "Functions/Implemented", mousePos, clickedPort, null);
			menu = flowGraph.AppendAllReflectionNodesMenu(menu, "Functions/Reflected", mousePos, clickedPort, null);
			menu = flowGraph.AppendVariableNodesMenu(menu, "Variables", mousePos, clickedPort, null);
			menu = flowGraph.AppendMacroNodesMenu(menu, "MACROS", mousePos, clickedPort, null);
			menu = flowGraph.AppendMenuCallbackReceivers(menu, "", mousePos, clickedPort, null);

			if (zoomFactor == 1){
				menu.ShowAsBrowser(string.Format("Add & Connect (Type of {0})", clickedPort.type.FriendlyName() ), graph.baseNodeType );
			} else {
				GraphEditorUtility.PostGUI += ()=> { menu.ShowAsContext(); };
			}
			Event.current.Use();
		}

		//GUI within the node
		protected override void OnNodeGUI(){
			// ShowWildTypeSelectionButton();
			DrawNodePorts();
		}

		//..
		void ShowWildTypeSelectionButton(){
			var wildType = GetNodeWildDefinitionType();
			var content = this.GetType();
			var args = content.GetGenericArguments();
			var arg1 = args.FirstOrDefault();
			if (arg1 == wildType){
				EditorUtils.ButtonTypePopup("", wildType, (t)=>
				{
					var newType = TryGetNewGenericTypeForWild(wildType, wildType, t, content, null);
					if (newType != null){
						this.ReplaceWith(newType);
					}
				});
			}
		}

		//Draw all ports in order
		void DrawNodePorts(){
			GUILayout.BeginHorizontal();
			{
				GUILayout.BeginVertical();
				if (orderedInputs != null && orderedInputs.Length > 0){
					for (var i = 0; i < orderedInputs.Length; i++){
						DrawNodePort(orderedInputs[i], Styles.leftLabel);
					}
					GUILayout.Space(2);
				}
				GUILayout.EndVertical();

				GUILayout.BeginVertical();
				if (orderedOutputs != null && orderedOutputs.Length > 0){
					for (var i = 0; i < orderedOutputs.Length; i++){
						DrawNodePort(orderedOutputs[i], Styles.rightLabel);
					}
					GUILayout.Space(2);
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();

			if (Event.current.type == EventType.Repaint){
				portsLayoutHasDrawn = true;
			}
		}

		//Draw a port
		void DrawNodePort(Port port, GUIStyle style){
			port.EnsureCachedGUIContent();
			GUILayout.Label(port.displayContent, style, GUILayout.MaxHeight(16));
			GUI.color = Color.white;
			if (portsLayoutHasDrawn == false && Event.current.type == EventType.Repaint){
				port.posOffsetY = GUILayoutUtility.GetLastRect().center.y;
				var x = port.IsInputPort()? rect.xMin - 5 : rect.xMax + 5;
				port.pos = new Vector2(x, rect.y + port.posOffsetY);
			}
		}

		//The inspector panel
		protected override void OnNodeInspectorGUI(){
			if (this.GetType().RTIsDefined<HasRefreshButtonAttribute>(true)){
				if (GUILayout.Button("Refresh")){
					GatherPorts();
				}
			}
			DrawDefaultInspector();
			EditorUtils.Separator();
			DrawValueInputsGUI();
		}

		//Show the serialized input port values if any
		protected void DrawValueInputsGUI(){
			foreach (var input in inputPorts.Values.OfType<ValueInput>() ){
				if (input.isConnected){
					EditorGUILayout.LabelField(input.displayName, "[CONNECTED]");
					continue;
				}
				input.serializedValue = EditorUtils.ReflectedFieldInspector(input.displayName, input.serializedValue, input.type, null);
			}
		}

		//Override of right click node context menu
		protected override GenericMenu OnContextMenu(GenericMenu menu){
			menu = base.OnContextMenu(menu);
			if (outputPorts.Values.Any(p => p is FlowOutput)){ //breakpoints only work with FlowOutputs
				menu.AddItem(new GUIContent("Breakpoint"), isBreakpoint, ()=>{ isBreakpoint = !isBreakpoint; });
			}

			var type = this.GetType();
			if (type.IsGenericType){
				menu = EditorUtils.GetPreferedTypesSelectionMenu(type.GetGenericTypeDefinition(), (t)=>{ this.ReplaceWith(t); }, menu, "Change Generic Type");
			}

			return menu;
		}

		#endif
		///----------------------------------------------------------------------------------------------

	}
}
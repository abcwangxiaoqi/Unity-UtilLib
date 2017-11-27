public delegate void CallBack();
public delegate void CallBackWithParams<T>(T t);
public delegate void CallBackWithParams<T, U>(T t, U u);
public delegate void CallBackWithParams<T, U, V>(T t, U u, V v);

public delegate R CallBackWithReturn<R>();
public delegate R CallBackWithParamsReturn<T, R>(T t);
public delegate R CallBackWithParamsReturn<T, U, R>(T t, U u);
public delegate R CallBackWithParamsReturn<T, U, V, R>(T t, U u, V v);
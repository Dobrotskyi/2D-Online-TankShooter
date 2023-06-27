using System;
public interface IUseLoading
{
    public event Action StartLoading;
    public event Action EndLoading;
}

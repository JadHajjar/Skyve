namespace Skyve.Domain;
public interface IDTO<TData, TObj>
{
	public TObj Convert(TData? data);
}

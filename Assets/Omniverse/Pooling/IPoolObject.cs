namespace Omniverse
{
	public interface IPoolObject<T>
	{
		T Prefab { get; set; }
		
		void Cleanup();
	}
}

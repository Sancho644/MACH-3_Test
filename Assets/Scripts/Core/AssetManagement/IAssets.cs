using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.AssetManagement
{
    public interface IAssets
    {
        public Task<GameObject> Instantiate(string address);
        public Task<GameObject> Instantiate(string address, Vector3 at);
        public Task<GameObject> Instantiate(string address, Transform under);
        public Task<T> Load<T>(AssetReference assetReference) where T : class;
        public Task<T> Load<T>(string address) where T : class;
        public void CleanUp();
        public void Instantiate();
    }
}
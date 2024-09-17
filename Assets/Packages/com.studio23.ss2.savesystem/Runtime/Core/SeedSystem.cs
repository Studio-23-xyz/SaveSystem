using Studio23.SS2.SaveSystem.Utilities;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Core
{
    [RequireComponent(typeof(FileProcessor))]
    public class SeedSystem : MonoBehaviour
    {
        [SerializeField]
        internal FileProcessor _fileProcessor;
        [SerializeField]
        internal SeedGeneratorBase _seedGenerator;

        [SerializeField] private string _seed;

        public string Seed => _seed;

        public void Initialize()
        {
            _fileProcessor = GetComponent<FileProcessor>();
            if (_seedGenerator == null)
            {
                Debug.LogError("Seed Generator Scriptable Object is null,Did you forget to assign it?");
                return;
            }

            _seed =_seedGenerator.GenerateSeed();
        }


    }
}
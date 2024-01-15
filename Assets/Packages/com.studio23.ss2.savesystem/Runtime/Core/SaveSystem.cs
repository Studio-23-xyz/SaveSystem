
using System.Runtime.CompilerServices;
using UnityEngine;


[assembly: InternalsVisibleTo("com.studio23.ss2.savesystem.playmodetest")]
[assembly: InternalsVisibleTo("com.studio23.ss2.savesystem.editor")]
namespace Studio23.SS2.SaveSystem.Core
{

    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem Instance;

        [SerializeField] internal SaveSlotProcessor _slotProcessor;

        private void Awake()
        {
            Instance = this;
        }


        [ContextMenu("Save")]
        public async void Save()
        {
           await _slotProcessor.SaveAllSavable();
        }

        [ContextMenu("Load")]
        public async void Load()
        {
            await _slotProcessor.LoadAllSavable();
        }


    }
}
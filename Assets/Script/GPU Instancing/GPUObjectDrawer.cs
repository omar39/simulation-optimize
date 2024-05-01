using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GPUObjectDrawer : MonoBehaviour
{
    [SerializeField] private int _instances;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    private List<List<ObjectData>> _batches = new List<List<ObjectData>>();
    private void Awake() 
    {
        int batchIndex = 0;
        List<ObjectData> currBatch = new List<ObjectData>();
        for(int i = 0;i < _instances;i++)
        {
            AddObject(currBatch, i);
            batchIndex++;
            if(batchIndex >= 1000)
            {
                _batches.Add(currBatch);
                currBatch = new List<ObjectData>();
                batchIndex = 0;
            }
        }
    }
    private void AddObject(List<ObjectData> currBatch, int i)
    {
        Vector3 pos = Random.insideUnitSphere * 100f;
        currBatch.Add(new ObjectData() { position = pos, rotation = Quaternion.identity, scale = Vector3.one });
    }
    
    public void UpdateBatches()
    {
        foreach(var batch in _batches)
        {
            Graphics.DrawMeshInstanced(_mesh, 0, _material, batch
                .Select((a) => a.matrix).ToList());
        }
    }
    private void Update() 
    {
        UpdateBatches();    
    }
    public List<List<ObjectData>> GetBatches() => _batches;

}

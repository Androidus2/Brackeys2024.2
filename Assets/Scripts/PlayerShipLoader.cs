using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipLoader : MonoBehaviour
{

    [SerializeField]
    private MeshFilter healthMeshFilter;
    [SerializeField]
    private Mesh[] healthMeshes;
    [SerializeField]
    private MeshRenderer healthMeshRenderer;

    [SerializeField]
    private MeshFilter radarMeshFilter;
    [SerializeField]
    private Mesh[] radarMeshes;
    [SerializeField]
    private MeshRenderer radarMeshRenderer;

    [SerializeField]
    private MeshFilter storageMeshFilter;
    [SerializeField]
    private Mesh[] storageMeshes;
    [SerializeField]
    private MeshRenderer storageMeshRenderer;

    [SerializeField]
    private Material[] speedMaterials;

    [SerializeField]
    private Material[] glassMaterials;

    private void Awake()
    {
        LoadShip();
    }

    private void LoadShip()
    {
        healthMeshFilter.mesh = healthMeshes[GameMaster.HealthLevel - 1];
        storageMeshFilter.mesh = storageMeshes[GameMaster.StorageLevel - 1];
        radarMeshFilter.mesh = radarMeshes[GameMaster.RadarLevel - 1];

        List<Material> tmp = new List<Material>();
        healthMeshRenderer.GetMaterials(tmp);
        tmp[1] = speedMaterials[GameMaster.SpeedLevel - 1];
        if(GameMaster.HealthLevel <= 2)
            tmp[2] = glassMaterials[0];
        else
            tmp[2] = glassMaterials[1];
        healthMeshRenderer.SetMaterials(tmp);
        tmp.Clear();

        radarMeshRenderer.GetMaterials(tmp);
        tmp[1] = speedMaterials[GameMaster.SpeedLevel - 1];
        radarMeshRenderer.SetMaterials(tmp);
        tmp.Clear();

        storageMeshRenderer.GetMaterials(tmp);
        tmp[1] = speedMaterials[GameMaster.SpeedLevel - 1];
        storageMeshRenderer.SetMaterials(tmp);
        tmp.Clear();
    }

}

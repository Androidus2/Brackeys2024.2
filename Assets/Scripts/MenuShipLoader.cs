using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShipLoader : MonoBehaviour
{

    [SerializeField]
    private MeshRenderer[] calmLevels;

    [SerializeField]
    private MeshRenderer[] radarLevels;

    [SerializeField]
    private MeshRenderer[] storageLevels;

    [SerializeField]
    private Material[] speedLevels;

    private void Start()
    {
        LoadShip();
    }

    public void LoadShip()
    {
        // Enable all the levels
        for (int i = 0; i < calmLevels.Length; i++)
            calmLevels[i].gameObject.SetActive(true);

        for (int i = 0; i < radarLevels.Length; i++)
            radarLevels[i].gameObject.SetActive(true);

        for (int i = 0; i < storageLevels.Length; i++)
            storageLevels[i].gameObject.SetActive(true);


        MeshRenderer calmMeshFilter = calmLevels[GameMaster.HealthLevel - 1];
        MeshRenderer radarMeshFilter = radarLevels[GameMaster.RadarLevel - 1];
        MeshRenderer storageMeshFilter = storageLevels[GameMaster.StorageLevel - 1];

        List<Material> tmp = new List<Material>();
        calmMeshFilter.GetMaterials(tmp);
        tmp[1] = speedLevels[GameMaster.SpeedLevel - 1];
        calmMeshFilter.SetMaterials(tmp);
        tmp.Clear();

        radarMeshFilter.GetMaterials(tmp);
        tmp[1] = speedLevels[GameMaster.SpeedLevel - 1];
        radarMeshFilter.SetMaterials(tmp);
        tmp.Clear();

        storageMeshFilter.GetMaterials(tmp);
        tmp[1] = speedLevels[GameMaster.SpeedLevel - 1];
        storageMeshFilter.SetMaterials(tmp);
        tmp.Clear();

        // Disable the other levels
        for (int i = 0; i < calmLevels.Length; i++)
        {
            if (i != GameMaster.HealthLevel - 1)
            {
                calmLevels[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < radarLevels.Length; i++)
        {
            if (i != GameMaster.RadarLevel - 1)
            {
                radarLevels[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < storageLevels.Length; i++)
        {
            if (i != GameMaster.StorageLevel - 1)
            {
                storageLevels[i].gameObject.SetActive(false);
            }
        }
    }

}

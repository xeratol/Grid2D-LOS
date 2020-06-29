using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainImporter : MonoBehaviour
{
    [SerializeField]
    private TerrainInfo _terrainInfo = null;

    void Start()
    {
        Debug.Assert(_terrainInfo, "Terrain Info not set", this);
    }

    #region Preset Maps
    private void LoadMap(int rows, int cols, System.Func<IEnumerator> more)
    {
        _terrainInfo.InitWalls(rows, cols);
        StartCoroutine(more());
    }

    public void LoadMap01()
    {
        LoadMap(20, 20, LoadMap01More);
    }

    private IEnumerator LoadMap01More()
    {
        yield return 0; // Renderer component is not ready on the same frame

        _terrainInfo.SetWall(4, 6);
        _terrainInfo.SetWall(4, 6);
        _terrainInfo.SetWall(4, 7);
        _terrainInfo.SetWall(4, 8);
        _terrainInfo.SetWall(4, 9);
        _terrainInfo.SetWall(4, 10);
        _terrainInfo.SetWall(4, 11);
        _terrainInfo.SetWall(4, 12);
        _terrainInfo.SetWall(4, 13);
        _terrainInfo.SetWall(5, 6);
        _terrainInfo.SetWall(6, 6);
        _terrainInfo.SetWall(7, 6);
        _terrainInfo.SetWall(8, 6);
        _terrainInfo.SetWall(8, 13);
        _terrainInfo.SetWall(9, 6);
        _terrainInfo.SetWall(9, 13);
        _terrainInfo.SetWall(10, 6);
        _terrainInfo.SetWall(10, 13);
        _terrainInfo.SetWall(11, 6);
        _terrainInfo.SetWall(11, 13);
        _terrainInfo.SetWall(12, 13);
        _terrainInfo.SetWall(13, 13);
        _terrainInfo.SetWall(14, 13);
        _terrainInfo.SetWall(15, 6);
        _terrainInfo.SetWall(15, 7);
        _terrainInfo.SetWall(15, 8);
        _terrainInfo.SetWall(15, 9);
        _terrainInfo.SetWall(15, 10);
        _terrainInfo.SetWall(15, 11);
        _terrainInfo.SetWall(15, 12);
        _terrainInfo.SetWall(15, 13);
    }

    public void LoadMap02()
    {
        LoadMap(40, 40, LoadMap02More);
    }

    private IEnumerator LoadMap02More()
    {
        yield return 0; // Renderer component is not ready on the same frame

        _terrainInfo.SetWall(0, 10);
        _terrainInfo.SetWall(0, 17);
        _terrainInfo.SetWall(0, 27);
        _terrainInfo.SetWall(1, 10);
        _terrainInfo.SetWall(1, 17);
        _terrainInfo.SetWall(1, 27);
        _terrainInfo.SetWall(2, 10);
        _terrainInfo.SetWall(2, 17);
        _terrainInfo.SetWall(2, 27);
        _terrainInfo.SetWall(3, 10);
        _terrainInfo.SetWall(3, 17);
        _terrainInfo.SetWall(3, 27);
        _terrainInfo.SetWall(4, 10);
        _terrainInfo.SetWall(4, 27);
        _terrainInfo.SetWall(5, 10);
        _terrainInfo.SetWall(5, 17);
        _terrainInfo.SetWall(5, 27);
        _terrainInfo.SetWall(6, 10);
        _terrainInfo.SetWall(6, 17);
        _terrainInfo.SetWall(6, 27);
        _terrainInfo.SetWall(7, 10);
        _terrainInfo.SetWall(7, 17);
        _terrainInfo.SetWall(7, 27);
        _terrainInfo.SetWall(8, 10);
        _terrainInfo.SetWall(8, 17);
        _terrainInfo.SetWall(8, 27);
        _terrainInfo.SetWall(9, 10);
        _terrainInfo.SetWall(9, 17);
        _terrainInfo.SetWall(9, 27);
        _terrainInfo.SetWall(10, 17);
        _terrainInfo.SetWall(10, 27);
        _terrainInfo.SetWall(11, 10);
        _terrainInfo.SetWall(11, 17);
        _terrainInfo.SetWall(12, 10);
        _terrainInfo.SetWall(12, 17);
        _terrainInfo.SetWall(12, 27);
        _terrainInfo.SetWall(13, 10);
        _terrainInfo.SetWall(13, 17);
        _terrainInfo.SetWall(13, 27);
        _terrainInfo.SetWall(14, 10);
        _terrainInfo.SetWall(14, 17);
        _terrainInfo.SetWall(14, 27);
        _terrainInfo.SetWall(15, 10);
        _terrainInfo.SetWall(15, 11);
        _terrainInfo.SetWall(15, 12);
        _terrainInfo.SetWall(15, 13);
        _terrainInfo.SetWall(15, 14);
        _terrainInfo.SetWall(15, 15);
        _terrainInfo.SetWall(15, 16);
        _terrainInfo.SetWall(15, 17);
        _terrainInfo.SetWall(15, 18);
        _terrainInfo.SetWall(15, 19);
        _terrainInfo.SetWall(15, 20);
        _terrainInfo.SetWall(15, 21);
        _terrainInfo.SetWall(15, 22);
        _terrainInfo.SetWall(15, 23);
        _terrainInfo.SetWall(15, 24);
        _terrainInfo.SetWall(15, 25);
        _terrainInfo.SetWall(15, 26);
        _terrainInfo.SetWall(15, 27);
        _terrainInfo.SetWall(15, 28);
        _terrainInfo.SetWall(15, 29);
        _terrainInfo.SetWall(15, 30);
        _terrainInfo.SetWall(15, 31);
        _terrainInfo.SetWall(15, 33);
        _terrainInfo.SetWall(15, 34);
        _terrainInfo.SetWall(16, 14);
        _terrainInfo.SetWall(16, 21);
        _terrainInfo.SetWall(16, 34);
        _terrainInfo.SetWall(17, 14);
        _terrainInfo.SetWall(17, 21);
        _terrainInfo.SetWall(17, 34);
        _terrainInfo.SetWall(18, 21);
        _terrainInfo.SetWall(18, 34);
        _terrainInfo.SetWall(19, 14);
        _terrainInfo.SetWall(19, 21);
        _terrainInfo.SetWall(19, 34);
        _terrainInfo.SetWall(20, 14);
        _terrainInfo.SetWall(20, 21);
        _terrainInfo.SetWall(20, 34);
        _terrainInfo.SetWall(21, 14);
        _terrainInfo.SetWall(21, 21);
        _terrainInfo.SetWall(21, 34);
        _terrainInfo.SetWall(22, 14);
        _terrainInfo.SetWall(22, 21);
        _terrainInfo.SetWall(22, 34);
        _terrainInfo.SetWall(23, 14);
        _terrainInfo.SetWall(23, 21);
        _terrainInfo.SetWall(23, 34);
        _terrainInfo.SetWall(24, 14);
        _terrainInfo.SetWall(24, 21);
        _terrainInfo.SetWall(24, 34);
        _terrainInfo.SetWall(25, 14);
        _terrainInfo.SetWall(25, 21);
        _terrainInfo.SetWall(25, 22);
        _terrainInfo.SetWall(25, 23);
        _terrainInfo.SetWall(25, 24);
        _terrainInfo.SetWall(25, 25);
        _terrainInfo.SetWall(25, 26);
        _terrainInfo.SetWall(25, 27);
        _terrainInfo.SetWall(25, 28);
        _terrainInfo.SetWall(25, 30);
        _terrainInfo.SetWall(25, 31);
        _terrainInfo.SetWall(25, 32);
        _terrainInfo.SetWall(25, 33);
        _terrainInfo.SetWall(25, 34);
        _terrainInfo.SetWall(26, 14);
        _terrainInfo.SetWall(26, 21);
        _terrainInfo.SetWall(27, 14);
        _terrainInfo.SetWall(27, 21);
        _terrainInfo.SetWall(28, 14);
        _terrainInfo.SetWall(28, 21);
        _terrainInfo.SetWall(29, 14);
        _terrainInfo.SetWall(29, 21);
        _terrainInfo.SetWall(30, 14);
        _terrainInfo.SetWall(30, 21);
        _terrainInfo.SetWall(31, 14);
        _terrainInfo.SetWall(32, 14);
        _terrainInfo.SetWall(32, 21);
        _terrainInfo.SetWall(33, 14);
        _terrainInfo.SetWall(33, 21);
        _terrainInfo.SetWall(34, 14);
        _terrainInfo.SetWall(34, 21);
        _terrainInfo.SetWall(35, 14);
        _terrainInfo.SetWall(35, 21);
        _terrainInfo.SetWall(36, 14);
        _terrainInfo.SetWall(36, 21);
        _terrainInfo.SetWall(37, 14);
        _terrainInfo.SetWall(37, 21);
        _terrainInfo.SetWall(38, 14);
        _terrainInfo.SetWall(38, 21);
        _terrainInfo.SetWall(39, 14);
        _terrainInfo.SetWall(39, 21);
    }

    public void LoadMap03()
    {
        LoadMap(40, 40, LoadMap03More);
    }

    private IEnumerator LoadMap03More()
    {
        yield return 0; // Renderer component is not ready on the same frame

        _terrainInfo.SetWall(0, 27);
        _terrainInfo.SetWall(0, 32);
        _terrainInfo.SetWall(0, 39);
        _terrainInfo.SetWall(1, 4);
        _terrainInfo.SetWall(1, 12);
        _terrainInfo.SetWall(1, 18);
        _terrainInfo.SetWall(1, 24);
        _terrainInfo.SetWall(1, 29);
        _terrainInfo.SetWall(1, 35);
        _terrainInfo.SetWall(1, 37);
        _terrainInfo.SetWall(2, 9);
        _terrainInfo.SetWall(2, 16);
        _terrainInfo.SetWall(2, 27);
        _terrainInfo.SetWall(2, 30);
        _terrainInfo.SetWall(2, 34);
        _terrainInfo.SetWall(3, 21);
        _terrainInfo.SetWall(3, 38);
        _terrainInfo.SetWall(4, 26);
        _terrainInfo.SetWall(4, 28);
        _terrainInfo.SetWall(4, 31);
        _terrainInfo.SetWall(4, 36);
        _terrainInfo.SetWall(5, 4);
        _terrainInfo.SetWall(5, 8);
        _terrainInfo.SetWall(5, 12);
        _terrainInfo.SetWall(5, 18);
        _terrainInfo.SetWall(5, 23);
        _terrainInfo.SetWall(5, 33);
        _terrainInfo.SetWall(5, 39);
        _terrainInfo.SetWall(6, 1);
        _terrainInfo.SetWall(6, 30);
        _terrainInfo.SetWall(6, 36);
        _terrainInfo.SetWall(7, 15);
        _terrainInfo.SetWall(7, 27);
        _terrainInfo.SetWall(7, 35);
        _terrainInfo.SetWall(7, 38);
        _terrainInfo.SetWall(8, 21);
        _terrainInfo.SetWall(8, 30);
        _terrainInfo.SetWall(8, 33);
        _terrainInfo.SetWall(8, 39);
        _terrainInfo.SetWall(9, 7);
        _terrainInfo.SetWall(9, 11);
        _terrainInfo.SetWall(9, 25);
        _terrainInfo.SetWall(9, 36);
        _terrainInfo.SetWall(10, 15);
        _terrainInfo.SetWall(10, 19);
        _terrainInfo.SetWall(10, 38);
        _terrainInfo.SetWall(11, 25);
        _terrainInfo.SetWall(11, 27);
        _terrainInfo.SetWall(12, 23);
        _terrainInfo.SetWall(12, 32);
        _terrainInfo.SetWall(12, 35);
        _terrainInfo.SetWall(12, 37);
        _terrainInfo.SetWall(13, 30);
        _terrainInfo.SetWall(13, 38);
        _terrainInfo.SetWall(14, 6);
        _terrainInfo.SetWall(14, 15);
        _terrainInfo.SetWall(14, 20);
        _terrainInfo.SetWall(15, 24);
        _terrainInfo.SetWall(15, 33);
        _terrainInfo.SetWall(16, 29);
        _terrainInfo.SetWall(16, 36);
        _terrainInfo.SetWall(16, 38);
        _terrainInfo.SetWall(17, 2);
        _terrainInfo.SetWall(18, 19);
        _terrainInfo.SetWall(18, 24);
        _terrainInfo.SetWall(18, 39);
        _terrainInfo.SetWall(19, 10);
        _terrainInfo.SetWall(19, 30);
        _terrainInfo.SetWall(19, 32);
        _terrainInfo.SetWall(19, 35);
        _terrainInfo.SetWall(20, 37);
        _terrainInfo.SetWall(21, 21);
        _terrainInfo.SetWall(21, 26);
        _terrainInfo.SetWall(21, 33);
        _terrainInfo.SetWall(22, 1);
        _terrainInfo.SetWall(23, 7);
        _terrainInfo.SetWall(23, 28);
        _terrainInfo.SetWall(23, 36);
        _terrainInfo.SetWall(23, 38);
        _terrainInfo.SetWall(25, 15);
        _terrainInfo.SetWall(26, 23);
        _terrainInfo.SetWall(26, 28);
        _terrainInfo.SetWall(26, 34);
        _terrainInfo.SetWall(27, 3);
        _terrainInfo.SetWall(27, 8);
        _terrainInfo.SetWall(27, 36);
        _terrainInfo.SetWall(28, 38);
        _terrainInfo.SetWall(29, 33);
        _terrainInfo.SetWall(30, 20);
        _terrainInfo.SetWall(31, 7);
        _terrainInfo.SetWall(31, 37);
        _terrainInfo.SetWall(32, 28);
        _terrainInfo.SetWall(33, 14);
        _terrainInfo.SetWall(33, 34);
        _terrainInfo.SetWall(36, 4);
        _terrainInfo.SetWall(36, 23);
        _terrainInfo.SetWall(37, 28);
        _terrainInfo.SetWall(37, 34);
        _terrainInfo.SetWall(38, 15);
    }
    #endregion
}

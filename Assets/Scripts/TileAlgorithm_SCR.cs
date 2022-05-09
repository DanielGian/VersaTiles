using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class TileAlgorithm_SCR : MonoBehaviour
{
    [SerializeField] private bool showGridSequence = true;
    private float timeVal = 2f;
    private const int GridSize = 64;
    private const int numOfGrids = 4;
    private int PlusTile_Count = 0;
    private int NonPlusTile_Count = 0;
    private int ATKPlus1_Count = 0;
    private int ATKPlus2_Count = 0;
    private int ATKPlus3_Count = 0;
    private int ATKPlus4_Count = 0;
    private int MulATKPlus1_Count = 0;
    private int MulATKPlus2_Count = 0;
    private int Guard_Count = 0;
    private int HPPlus5_Count = 0;
    private int CPPlus1_Count = 0;
    private int CPPlus2_Count = 0;
    private int CmdItem_Count = 0;
    private int Mul_Count = 0;
    private int ElementSwap_Count = 0;

    public enum Tile
    {
        ATKPlus1,       // Attack +1
        ATKPlus2,       // Attack +2
        ATKPlus3,       // Attack +3
        ATKPlus4,       // Attack +4
        MulATKPlus1,    // Multi-Attack +1
        MulATKPlus2,    // Multi-Attack +2
        Guard,          // Guard
        HPPlus5,        // Heart Points +5
        CPPlus1,        // Command Points +1
        CPPlus2,        // Command Points +2
        CmdItem,        // Command Item
        Mul2,           // x2
        Mul3,           // x3
        ElementSwap     // Element Swap
    };
    [SerializeField]
    private readonly List<Tile> Tiles = new List<Tile>()
    {
        Tile.ATKPlus1, Tile.ATKPlus1, Tile.ATKPlus1, Tile.ATKPlus1, Tile.ATKPlus1, Tile.ATKPlus1, Tile.ATKPlus1, Tile.ATKPlus1, Tile.ATKPlus1, Tile.ATKPlus1, Tile.ATKPlus1, Tile.ATKPlus1, Tile.ATKPlus1, Tile.ATKPlus1,
        Tile.ATKPlus2, Tile.ATKPlus2, Tile.ATKPlus2, Tile.ATKPlus2, Tile.ATKPlus2, Tile.ATKPlus2, Tile.ATKPlus2, Tile.ATKPlus2, Tile.ATKPlus2, Tile.ATKPlus2, Tile.ATKPlus2, Tile.ATKPlus2, Tile.ATKPlus2, Tile.ATKPlus2,
        Tile.ATKPlus3, Tile.ATKPlus3, Tile.ATKPlus3, Tile.ATKPlus3,
        Tile.ATKPlus4, Tile.ATKPlus4, Tile.ATKPlus4, Tile.ATKPlus4,
        Tile.MulATKPlus1, Tile.MulATKPlus1, Tile.MulATKPlus1, Tile.MulATKPlus2, Tile.MulATKPlus2, Tile.MulATKPlus2,
        Tile.Guard, Tile.Guard,
        Tile.HPPlus5, Tile.HPPlus5, Tile.HPPlus5, Tile.HPPlus5,
        Tile.CPPlus1, Tile.CPPlus1, Tile.CPPlus1, Tile.CPPlus1, Tile.CPPlus1, Tile.CPPlus1,
        Tile.CPPlus2, Tile.CPPlus2, Tile.CPPlus2,
        Tile.CmdItem, Tile.CmdItem, Tile.CmdItem,
        Tile.Mul2,
        Tile.Mul2,
        Tile.ElementSwap, Tile.ElementSwap
    };
    private List<List<Tile>> GridSequence = new List<List<Tile>>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeVal -= 1f * Time.deltaTime;
        if (timeVal <= 0) { MakeGridSequence(); timeVal = 2f; }
    }

    public List<Tile> GetGrid() { return GridSequence[0]; }
    public void RemoveGridFromSequence()
    {
        GridSequence.RemoveAt(0);
        if (GridSequence.Count == 0) { MakeGridSequence(); }
    }

    //Creates a new grid sequence
    public void MakeGridSequence()
    {
        List<Tile> gridTiles = new List<Tile>();
        gridTiles.AddRange(Tiles);
        ShuffleAllTiles(ref gridTiles);

        int ATKPlus1_MAX = 7;       //Max # of Attack +1 Tiles
        int ATKPlus2_MAX = 7;       //Max # of Attack +2 Tiles
        int ATKPlus3_MAX = 2;       //Max # of Attack +3 Tiles
        int ATKPlus4_MAX = 2;       //Max # of Attack +4 Tiles
        int MulATKPlus1_MAX = 2;    //Max # of Multi-Attack +1 Tiles
        int MulATKPlus2_MAX = 2;    //Max # of Multi-Attack +2 Tiles
        int Guard_MAX = 1;          //Max # of Guard Tiles
        int HPPlus5_MAX = 2;        //Max # of HP +5 Tiles
        int CPPlus1_MAX = 3;        //Max # of CP +1 Tiles
        int CPPlus2_MAX = 2;        //Max # of CP +2 Tiles
        int CmdItem_MAX = 2;        //Max # of Command Tiles
        int Mul_MAX = 1;            //Max # of Multiplier Tiles
        int ElementSwap_MAX = 1;    //Max # of Element Swap Tiles

        //Creates 4 grids (16 tiles per grid)
        for (int i = 0; i < numOfGrids; i++)
        {
            List<Tile> grid = new List<Tile>();
            while (grid.Count < 16)
            {
                Tile firstTile = gridTiles[0];
                bool ViolatedRule = false;

                //Tile is Attack +1 Tile
                if (firstTile == Tile.ATKPlus1) { ViolatedRule = CheckViolatedRule(ref ATKPlus1_Count, ref PlusTile_Count, ATKPlus1_MAX, true); }
                //Tile is Attack +2 Tile
                else if (firstTile == Tile.ATKPlus2) { ViolatedRule = CheckViolatedRule(ref ATKPlus2_Count, ref PlusTile_Count, ATKPlus2_MAX, true); }
                //Tile is Attack +3 Tile
                else if (firstTile == Tile.ATKPlus3) { ViolatedRule = CheckViolatedRule(ref ATKPlus3_Count, ref PlusTile_Count, ATKPlus3_MAX, true); }
                //Tile is Attack +4 Tile
                else if (firstTile == Tile.ATKPlus4) { ViolatedRule = CheckViolatedRule(ref ATKPlus4_Count, ref PlusTile_Count, ATKPlus4_MAX, true); }
                //Tile is Multi-Attack +1 Tile
                else if (firstTile == Tile.MulATKPlus1) { ViolatedRule = CheckViolatedRule(ref MulATKPlus1_Count, ref NonPlusTile_Count, MulATKPlus1_MAX, false); }
                //Tile is Multi-Attack +2 Tile
                else if (firstTile == Tile.MulATKPlus2) { ViolatedRule = CheckViolatedRule(ref MulATKPlus2_Count, ref NonPlusTile_Count, MulATKPlus2_MAX, false); }
                //Tile is Guard Tile
                else if (firstTile == Tile.Guard) { ViolatedRule = CheckViolatedRule(ref Guard_Count, ref NonPlusTile_Count, Guard_MAX, false); }
                //Tile is HP +5 Tile
                else if (firstTile == Tile.HPPlus5) { ViolatedRule = CheckViolatedRule(ref HPPlus5_Count, ref NonPlusTile_Count, HPPlus5_MAX, false); }
                //Tile is CP +1 Tile
                else if (firstTile == Tile.CPPlus1) { ViolatedRule = CheckViolatedRule(ref CPPlus1_Count, ref NonPlusTile_Count, CPPlus1_MAX, false); }
                //Tile is CP +2 Tile
                else if (firstTile == Tile.CPPlus2) { ViolatedRule = CheckViolatedRule(ref CPPlus2_Count, ref NonPlusTile_Count, CPPlus2_MAX, false); }
                //Tile is Command Tile
                else if (firstTile == Tile.CmdItem) { ViolatedRule = CheckViolatedRule(ref CmdItem_Count, ref NonPlusTile_Count, CmdItem_MAX, false); }
                //Tile is x2 Tile
                else if (firstTile == Tile.Mul2) { ViolatedRule = CheckViolatedRule(ref Mul_Count, ref NonPlusTile_Count, Mul_MAX, false); }
                //Tile is x3 Tile
                else if (firstTile == Tile.Mul3) { ViolatedRule = CheckViolatedRule(ref Mul_Count, ref NonPlusTile_Count, Mul_MAX, false); }
                //Tile is Element Swap Tile
                else if (firstTile == Tile.ElementSwap) { ViolatedRule = CheckViolatedRule(ref ElementSwap_Count, ref NonPlusTile_Count, ElementSwap_MAX, false); }

                //If rule is violated, pop tile and push it to the end of the list
                if (ViolatedRule)
                {
                    gridTiles.RemoveAt(0);
                    gridTiles.Add(firstTile);
                    continue;
                }

                //Add tile to grid
                grid.Add(firstTile);
                gridTiles.RemoveAt(0);
            }

            //Before the last iteration, check the leftover tiles and make any necessary replacements to avoid infinite loop
            if (i == 2)
            {
                int ATKPlus1_Leftovers = 0;
                int ATKPlus2_Leftovers = 0;
                int ATKPlus3_Leftovers = 0;
                int ATKPlus4_Leftovers = 0;
                int MulATKPlus1_Leftovers = 0;
                int MulATKPlus2_Leftovers = 0;
                int Guard_Leftovers = 0;
                int HPPlus5_Leftovers = 0;
                int CPPlus1_Leftovers = 0;
                int CPPlus2_Leftovers = 0;
                int CmdItem_Leftovers = 0;
                int Mul_Leftovers = 0;
                int ElementSwap_Leftovers = 0;

                do
                {
                    ATKPlus1_Leftovers = 0;
                    ATKPlus2_Leftovers = 0;
                    ATKPlus3_Leftovers = 0;
                    ATKPlus4_Leftovers = 0;
                    MulATKPlus1_Leftovers = 0;
                    MulATKPlus2_Leftovers = 0;
                    Guard_Leftovers = 0;
                    HPPlus5_Leftovers = 0;
                    CPPlus1_Leftovers = 0;
                    CPPlus2_Leftovers = 0;
                    CmdItem_Leftovers = 0;
                    Mul_Leftovers = 0;
                    ElementSwap_Leftovers = 0;

                    foreach (Tile tile in gridTiles)
                    {
                        if (tile == Tile.ATKPlus1) { ATKPlus1_Leftovers++; }
                        else if (tile == Tile.ATKPlus2) { ATKPlus2_Leftovers++; }
                        else if (tile == Tile.ATKPlus3) { ATKPlus3_Leftovers++; }
                        else if (tile == Tile.ATKPlus4) { ATKPlus4_Leftovers++; }
                        else if (tile == Tile.MulATKPlus1) { MulATKPlus1_Leftovers++; }
                        else if (tile == Tile.MulATKPlus2) { MulATKPlus2_Leftovers++; }
                        else if (tile == Tile.Guard) { Guard_Leftovers++; }
                        else if (tile == Tile.HPPlus5) { HPPlus5_Leftovers++; }
                        else if (tile == Tile.CPPlus1) { CPPlus1_Leftovers++; }
                        else if (tile == Tile.CPPlus2) { CPPlus2_Leftovers++; }
                        else if (tile == Tile.CmdItem) { CmdItem_Leftovers++; }
                        else if (tile == Tile.Mul2) { Mul_Leftovers++; }
                        else if (tile == Tile.Mul3) { Mul_Leftovers++; }
                        else if (tile == Tile.ElementSwap) { ElementSwap_Leftovers++; }
                    }

                    //Replaces a tile with an Attack +1 Tile if there are more than 6 tiles of this type in setup list
                    while (ATKPlus1_Leftovers > ATKPlus1_MAX)
                    {
                        for (int n = 0; n < 16; n++)
                        {
                            if (grid[n] == Tile.ATKPlus2 || grid[n] == Tile.ATKPlus3 || grid[n] == Tile.ATKPlus4)
                            {
                                Tile replacementTile = gridTiles[gridTiles.IndexOf(Tile.ATKPlus1)];

                                if (grid[n] == Tile.ATKPlus2) { ATKPlus2_Leftovers++; }
                                if (grid[n] == Tile.ATKPlus3) { ATKPlus3_Leftovers++; }
                                if (grid[n] == Tile.ATKPlus4) { ATKPlus4_Leftovers++; }

                                grid.Add(replacementTile);
                                gridTiles.Add(grid[n]);
                                grid.RemoveAt(n);
                                gridTiles.RemoveAt(gridTiles.IndexOf(Tile.ATKPlus1));
                                ATKPlus1_Leftovers--;
                            }
                            else { continue; }

                            break;
                        }
                    }
                    //Replaces a tile with an Attack +2 Tile if there are more than 6 tiles of this type in setup list
                    while (ATKPlus2_Leftovers > ATKPlus2_MAX)
                    {
                        for (int n = 0; n < 16; n++)
                        {
                            if (grid[n] == Tile.ATKPlus1 || grid[n] == Tile.ATKPlus3 || grid[n] == Tile.ATKPlus4)
                            {
                                Tile replacementTile = gridTiles[gridTiles.IndexOf(Tile.ATKPlus2)];

                                if (grid[n] == Tile.ATKPlus1) { ATKPlus1_Leftovers++; }
                                if (grid[n] == Tile.ATKPlus3) { ATKPlus3_Leftovers++; }
                                if (grid[n] == Tile.ATKPlus4) { ATKPlus4_Leftovers++; }

                                grid.Add(replacementTile);
                                gridTiles.Add(grid[n]);
                                grid.RemoveAt(n);
                                gridTiles.RemoveAt(gridTiles.IndexOf(Tile.ATKPlus2));
                                ATKPlus2_Leftovers--;
                            }
                            else { continue; }

                            break;
                        }
                    }
                    //Replaces a tile with an Attack +3 Tile if there are more than 6 tiles of this type in setup list
                    while (ATKPlus3_Leftovers > ATKPlus3_MAX)
                    {
                        for (int n = 0; n < 16; n++)
                        {
                            if (grid[n] == Tile.ATKPlus1 || grid[n] == Tile.ATKPlus2 || grid[n] == Tile.ATKPlus4)
                            {
                                Tile replacementTile = gridTiles[gridTiles.IndexOf(Tile.ATKPlus3)];

                                if (grid[n] == Tile.ATKPlus1) { ATKPlus1_Leftovers++; }
                                if (grid[n] == Tile.ATKPlus2) { ATKPlus2_Leftovers++; }
                                if (grid[n] == Tile.ATKPlus4) { ATKPlus4_Leftovers++; }

                                grid.Add(replacementTile);
                                gridTiles.Add(grid[n]);
                                grid.RemoveAt(n);
                                gridTiles.RemoveAt(gridTiles.IndexOf(Tile.ATKPlus3));
                                ATKPlus3_Leftovers--;
                            }
                            else { continue; }

                            break;
                        }
                    }
                    //Replaces a tile with an Attack +4 Tile if there are more than 6 tiles of this type in setup list
                    while (ATKPlus4_Leftovers > ATKPlus4_MAX)
                    {
                        for (int n = 0; n < 16; n++)
                        {
                            if (grid[n] == Tile.ATKPlus1 || grid[n] == Tile.ATKPlus2 || grid[n] == Tile.ATKPlus3)
                            {
                                Tile replacementTile = gridTiles[gridTiles.IndexOf(Tile.ATKPlus4)];

                                if (grid[n] == Tile.ATKPlus1) { ATKPlus1_Leftovers++; }
                                if (grid[n] == Tile.ATKPlus2) { ATKPlus2_Leftovers++; }
                                if (grid[n] == Tile.ATKPlus3) { ATKPlus3_Leftovers++; }

                                grid.Add(replacementTile);
                                gridTiles.Add(grid[n]);
                                grid.RemoveAt(n);
                                gridTiles.RemoveAt(gridTiles.IndexOf(Tile.ATKPlus4));
                                ATKPlus4_Leftovers--;
                            }
                            else { continue; }

                            break;
                        }
                    }
                    //Replaces a tile with an Multi-Attack +1 Tile if there are more than 2 tiles of this type in setup list
                    while (MulATKPlus1_Leftovers > MulATKPlus1_MAX)
                    {
                        for (int n = 0; n < 16; n++)
                        {
                            if (grid[n] == Tile.MulATKPlus2 || grid[n] == Tile.Guard || grid[n] == Tile.HPPlus5 || grid[n] == Tile.CPPlus1 || grid[n] == Tile.CPPlus2 || grid[n] == Tile.CmdItem || grid[n] == Tile.Mul2 || grid[n] == Tile.Mul3 || grid[n] == Tile.ElementSwap)
                            {
                                Tile replacementTile = gridTiles[gridTiles.IndexOf(Tile.MulATKPlus1)];

                                if (grid[n] == Tile.MulATKPlus2) { MulATKPlus2_Leftovers++; }
                                if (grid[n] == Tile.Guard) { Guard_Leftovers++; }
                                if (grid[n] == Tile.HPPlus5) { HPPlus5_Leftovers++; }
                                if (grid[n] == Tile.CPPlus1) { CPPlus1_Leftovers++; }
                                if (grid[n] == Tile.CPPlus2) { CPPlus2_Leftovers++; }
                                if (grid[n] == Tile.CmdItem) { CmdItem_Leftovers++; }
                                if (grid[n] == Tile.Mul2) { Mul_Leftovers++; }
                                if (grid[n] == Tile.Mul3) { Mul_Leftovers++; }
                                if (grid[n] == Tile.ElementSwap) { ElementSwap_Leftovers++; }

                                grid.Add(replacementTile);
                                gridTiles.Add(grid[n]);
                                grid.RemoveAt(n);
                                gridTiles.RemoveAt(gridTiles.IndexOf(Tile.MulATKPlus1));
                                MulATKPlus1_Leftovers--;
                            }
                            else { continue; }

                            break;
                        }
                    }
                    //Replaces a tile with an Multi-Attack +2 Tile if there are more than 2 tiles of this type in setup list
                    while (MulATKPlus2_Leftovers > MulATKPlus2_MAX)
                    {
                        for (int n = 0; n < 16; n++)
                        {
                            if (grid[n] == Tile.MulATKPlus1 || grid[n] == Tile.Guard || grid[n] == Tile.HPPlus5 || grid[n] == Tile.CPPlus1 || grid[n] == Tile.CPPlus2 || grid[n] == Tile.CmdItem || grid[n] == Tile.Mul2 || grid[n] == Tile.Mul3 || grid[n] == Tile.ElementSwap)
                            {
                                Tile replacementTile = gridTiles[gridTiles.IndexOf(Tile.MulATKPlus2)];

                                if (grid[n] == Tile.MulATKPlus1) { MulATKPlus1_Leftovers++; }
                                if (grid[n] == Tile.Guard) { Guard_Leftovers++; }
                                if (grid[n] == Tile.HPPlus5) { HPPlus5_Leftovers++; }
                                if (grid[n] == Tile.CPPlus1) { CPPlus1_Leftovers++; }
                                if (grid[n] == Tile.CPPlus2) { CPPlus2_Leftovers++; }
                                if (grid[n] == Tile.CmdItem) { CmdItem_Leftovers++; }
                                if (grid[n] == Tile.Mul2) { Mul_Leftovers++; }
                                if (grid[n] == Tile.Mul3) { Mul_Leftovers++; }
                                if (grid[n] == Tile.ElementSwap) { ElementSwap_Leftovers++; }

                                grid.Add(replacementTile);
                                gridTiles.Add(grid[n]);
                                grid.RemoveAt(n);
                                gridTiles.RemoveAt(gridTiles.IndexOf(Tile.MulATKPlus2));
                                MulATKPlus2_Leftovers--;
                            }
                            else { continue; }

                            break;
                        }
                    }
                    //Replaces a tile with a Guard Tile if there are more than 2 tiles of this type in setup list
                    while (Guard_Leftovers > Guard_MAX)
                    {
                        for (int n = 0; n < 16; n++)
                        {
                            if (grid[n] == Tile.MulATKPlus1 || grid[n] == Tile.MulATKPlus2 || grid[n] == Tile.CPPlus1 || grid[n] == Tile.CPPlus2 || grid[n] == Tile.CmdItem || grid[n] == Tile.Mul2 || grid[n] == Tile.Mul3 || grid[n] == Tile.ElementSwap)
                            {
                                Tile replacementTile = gridTiles[gridTiles.IndexOf(Tile.Guard)];

                                if (grid[n] == Tile.MulATKPlus1) { MulATKPlus1_Leftovers++; }
                                if (grid[n] == Tile.MulATKPlus2) { MulATKPlus2_Leftovers++; }
                                if (grid[n] == Tile.HPPlus5) { HPPlus5_Leftovers++; }
                                if (grid[n] == Tile.CPPlus1) { CPPlus1_Leftovers++; }
                                if (grid[n] == Tile.CPPlus2) { CPPlus2_Leftovers++; }
                                if (grid[n] == Tile.CmdItem) { CmdItem_Leftovers++; }
                                if (grid[n] == Tile.Mul2) { Mul_Leftovers++; }
                                if (grid[n] == Tile.Mul3) { Mul_Leftovers++; }
                                if (grid[n] == Tile.ElementSwap) { ElementSwap_Leftovers++; }

                                grid.Add(replacementTile);
                                gridTiles.Add(grid[n]);
                                grid.RemoveAt(n);
                                gridTiles.RemoveAt(gridTiles.IndexOf(Tile.Guard));
                                Guard_Leftovers--;
                            }
                            else { continue; }

                            break;
                        }
                    }
                    //Replaces a tile with a HP +5 Tile if there are more than 2 tiles of this type in setup list
                    while (HPPlus5_Leftovers > HPPlus5_MAX)
                    {
                        for (int n = 0; n < 16; n++)
                        {
                            if (grid[n] == Tile.MulATKPlus1 || grid[n] == Tile.MulATKPlus2 || grid[n] == Tile.Guard || grid[n] == Tile.CPPlus1 || grid[n] == Tile.CPPlus2 || grid[n] == Tile.CmdItem || grid[n] == Tile.Mul2 || grid[n] == Tile.Mul3 || grid[n] == Tile.ElementSwap)
                            {
                                Tile replacementTile = gridTiles[gridTiles.IndexOf(Tile.HPPlus5)];

                                if (grid[n] == Tile.MulATKPlus1) { MulATKPlus1_Leftovers++; }
                                if (grid[n] == Tile.MulATKPlus2) { MulATKPlus2_Leftovers++; }
                                if (grid[n] == Tile.Guard) { Guard_Leftovers++; }
                                if (grid[n] == Tile.CPPlus1) { CPPlus1_Leftovers++; }
                                if (grid[n] == Tile.CPPlus2) { CPPlus2_Leftovers++; }
                                if (grid[n] == Tile.CmdItem) { CmdItem_Leftovers++; }
                                if (grid[n] == Tile.Mul2) { Mul_Leftovers++; }
                                if (grid[n] == Tile.Mul3) { Mul_Leftovers++; }
                                if (grid[n] == Tile.ElementSwap) { ElementSwap_Leftovers++; }

                                grid.Add(replacementTile);
                                gridTiles.Add(grid[n]);
                                grid.RemoveAt(n);
                                gridTiles.RemoveAt(gridTiles.IndexOf(Tile.HPPlus5));
                                HPPlus5_Leftovers--;
                            }
                            else { continue; }

                            break;
                        }
                    }
                    //Replaces a tile with an CP +1 Tile if there are more than 2 tiles of this type in setup list
                    while (CPPlus1_Leftovers > CPPlus1_MAX)
                    {
                        for (int n = 0; n < 16; n++)
                        {
                            if (grid[n] == Tile.MulATKPlus1 || grid[n] == Tile.MulATKPlus2 || grid[n] == Tile.Guard || grid[n] == Tile.HPPlus5 || grid[n] == Tile.CPPlus2 || grid[n] == Tile.CmdItem || grid[n] == Tile.Mul2 || grid[n] == Tile.Mul3 || grid[n] == Tile.ElementSwap)
                            {
                                Tile replacementTile = gridTiles[gridTiles.IndexOf(Tile.CPPlus1)];

                                if (grid[n] == Tile.MulATKPlus1) { MulATKPlus1_Leftovers++; }
                                if (grid[n] == Tile.MulATKPlus2) { MulATKPlus2_Leftovers++; }
                                if (grid[n] == Tile.Guard) { Guard_Leftovers++; }
                                if (grid[n] == Tile.HPPlus5) { HPPlus5_Leftovers++; }
                                if (grid[n] == Tile.CPPlus2) { CPPlus2_Leftovers++; }
                                if (grid[n] == Tile.CmdItem) { CmdItem_Leftovers++; }
                                if (grid[n] == Tile.Mul2) { Mul_Leftovers++; }
                                if (grid[n] == Tile.Mul3) { Mul_Leftovers++; }
                                if (grid[n] == Tile.ElementSwap) { ElementSwap_Leftovers++; }

                                grid.Add(replacementTile);
                                gridTiles.Add(grid[n]);
                                grid.RemoveAt(n);
                                gridTiles.RemoveAt(gridTiles.IndexOf(Tile.CPPlus1));
                                CPPlus1_Leftovers--;
                            }
                            else { continue; }

                            break;
                        }
                    }
                    //Replaces a tile with an CP +2 Tile if there are more than 2 tiles of this type in setup list
                    while (CPPlus2_Leftovers > CPPlus2_MAX)
                    {
                        for (int n = 0; n < 16; n++)
                        {
                            if (grid[n] == Tile.MulATKPlus1 || grid[n] == Tile.MulATKPlus2 || grid[n] == Tile.Guard || grid[n] == Tile.HPPlus5 || grid[n] == Tile.CPPlus1 || grid[n] == Tile.CmdItem || grid[n] == Tile.Mul2 || grid[n] == Tile.Mul3 || grid[n] == Tile.ElementSwap)
                            {
                                Tile replacementTile = gridTiles[gridTiles.IndexOf(Tile.CPPlus2)];

                                if (grid[n] == Tile.MulATKPlus1) { MulATKPlus1_Leftovers++; }
                                if (grid[n] == Tile.MulATKPlus2) { MulATKPlus2_Leftovers++; }
                                if (grid[n] == Tile.Guard) { Guard_Leftovers++; }
                                if (grid[n] == Tile.HPPlus5) { HPPlus5_Leftovers++; }
                                if (grid[n] == Tile.CPPlus1) { CPPlus1_Leftovers++; }
                                if (grid[n] == Tile.CmdItem) { CmdItem_Leftovers++; }
                                if (grid[n] == Tile.Mul2) { Mul_Leftovers++; }
                                if (grid[n] == Tile.Mul3) { Mul_Leftovers++; }
                                if (grid[n] == Tile.ElementSwap) { ElementSwap_Leftovers++; }

                                grid.Add(replacementTile);
                                gridTiles.Add(grid[n]);
                                grid.RemoveAt(n);
                                gridTiles.RemoveAt(gridTiles.IndexOf(Tile.CPPlus2));
                                CPPlus2_Leftovers--;
                            }
                            else { continue; }

                            break;
                        }
                    }
                    //Replaces a tile with an Command Tile if there are more than 1 tile of this type in setup list
                    while (CmdItem_Leftovers > CmdItem_MAX)
                    {
                        for (int n = 0; n < 16; n++)
                        {
                            if (grid[n] == Tile.MulATKPlus1 || grid[n] == Tile.MulATKPlus2 || grid[n] == Tile.Guard || grid[n] == Tile.HPPlus5 || grid[n] == Tile.CPPlus1 || grid[n] == Tile.CPPlus2 || grid[n] == Tile.Mul2 || grid[n] == Tile.Mul3 || grid[n] == Tile.ElementSwap)
                            {
                                Tile replacementTile = gridTiles[gridTiles.IndexOf(Tile.CmdItem)];

                                if (grid[n] == Tile.MulATKPlus1) { MulATKPlus1_Leftovers++; }
                                if (grid[n] == Tile.MulATKPlus2) { MulATKPlus2_Leftovers++; }
                                if (grid[n] == Tile.Guard) { Guard_Leftovers++; }
                                if (grid[n] == Tile.HPPlus5) { HPPlus5_Leftovers++; }
                                if (grid[n] == Tile.CPPlus1) { CPPlus1_Leftovers++; }
                                if (grid[n] == Tile.CPPlus2) { CPPlus2_Leftovers++; }
                                if (grid[n] == Tile.Mul2) { Mul_Leftovers++; }
                                if (grid[n] == Tile.Mul3) { Mul_Leftovers++; }
                                if (grid[n] == Tile.ElementSwap) { ElementSwap_Leftovers++; }

                                grid.Add(replacementTile);
                                gridTiles.Add(grid[n]);
                                grid.RemoveAt(n);
                                gridTiles.RemoveAt(gridTiles.IndexOf(Tile.CmdItem));
                                CmdItem_Leftovers--;
                            }
                            else { continue; }

                            break;
                        }
                    }
                    //Replaces a tile with an x2 Tile if there are more than 1 tile of this type in setup list
                    while (Mul_Leftovers > Mul_MAX)
                    {
                        for (int n = 0; n < 16; n++)
                        {
                            if (grid[n] == Tile.MulATKPlus1 || grid[n] == Tile.MulATKPlus2 || grid[n] == Tile.Guard || grid[n] == Tile.HPPlus5 || grid[n] == Tile.CPPlus1 || grid[n] == Tile.CPPlus2 || grid[n] == Tile.CmdItem || grid[n] == Tile.ElementSwap)
                            {
                                int searchIndex;
                                if (gridTiles.IndexOf(Tile.Mul2) != -1) { searchIndex = gridTiles.IndexOf(Tile.Mul2); }
                                else { searchIndex = gridTiles.IndexOf(Tile.Mul3); }
                                Tile replacementTile = gridTiles[searchIndex];

                                if (grid[n] == Tile.MulATKPlus1) { MulATKPlus1_Leftovers++; }
                                if (grid[n] == Tile.MulATKPlus2) { MulATKPlus2_Leftovers++; }
                                if (grid[n] == Tile.Guard) { Guard_Leftovers++; }
                                if (grid[n] == Tile.HPPlus5) { HPPlus5_Leftovers++; }
                                if (grid[n] == Tile.CPPlus1) { CPPlus1_Leftovers++; }
                                if (grid[n] == Tile.CPPlus2) { CPPlus2_Leftovers++; }
                                if (grid[n] == Tile.CmdItem) { CmdItem_Leftovers++; }
                                if (grid[n] == Tile.ElementSwap) { ElementSwap_Leftovers++; }

                                grid.Add(replacementTile);
                                gridTiles.Add(grid[n]);
                                grid.RemoveAt(n);
                                gridTiles.RemoveAt(gridTiles.IndexOf(Tile.Mul2));
                                Mul_Leftovers--;
                            }
                            else { continue; }

                            break;
                        }
                    }
                    //Replaces a tile with an Element Swap Tile if there are more than 1 tile of this type in setup list
                    while (ElementSwap_Leftovers > ElementSwap_MAX)
                    {
                        for (int n = 0; n < 16; n++)
                        {
                            if (grid[n] == Tile.MulATKPlus1 || grid[n] == Tile.MulATKPlus2 || grid[n] == Tile.Guard || grid[n] == Tile.HPPlus5 || grid[n] == Tile.CPPlus1 || grid[n] == Tile.CPPlus2 || grid[n] == Tile.CmdItem || grid[n] == Tile.Mul2 || grid[n] == Tile.Mul3)
                            {
                                Tile replacementTile = gridTiles[gridTiles.IndexOf(Tile.ElementSwap)];

                                if (grid[n] == Tile.MulATKPlus1) { MulATKPlus1_Leftovers++; }
                                if (grid[n] == Tile.MulATKPlus2) { MulATKPlus2_Leftovers++; }
                                if (grid[n] == Tile.Guard) { Guard_Leftovers++; }
                                if (grid[n] == Tile.HPPlus5) { HPPlus5_Leftovers++; }
                                if (grid[n] == Tile.CPPlus1) { CPPlus1_Leftovers++; }
                                if (grid[n] == Tile.CPPlus2) { CPPlus2_Leftovers++; }
                                if (grid[n] == Tile.CmdItem) { CmdItem_Leftovers++; }
                                if (grid[n] == Tile.Mul2) { Mul_Leftovers++; }
                                if (grid[n] == Tile.Mul3) { Mul_Leftovers++; }

                                grid.Add(replacementTile);
                                gridTiles.Add(grid[n]);
                                grid.RemoveAt(n);
                                gridTiles.RemoveAt(gridTiles.IndexOf(Tile.ElementSwap));
                                ElementSwap_Leftovers--;
                            }
                            else { continue; }

                            break;
                        }
                    }
                }
                while (ATKPlus1_Leftovers > ATKPlus1_MAX || ATKPlus2_Leftovers > ATKPlus2_MAX || ATKPlus3_Leftovers > ATKPlus3_MAX || ATKPlus4_Leftovers > ATKPlus4_MAX
                        || MulATKPlus1_Leftovers > MulATKPlus1_MAX || MulATKPlus2_Leftovers > MulATKPlus2_MAX || Guard_Leftovers > Guard_MAX || HPPlus5_Leftovers > HPPlus5_MAX
                        || CPPlus1_Leftovers > CPPlus1_MAX || CPPlus2_Leftovers > CPPlus2_MAX || CmdItem_Leftovers > CmdItem_MAX || Mul_Leftovers > Mul_MAX || ElementSwap_Leftovers > ElementSwap_MAX);
            }

            ShuffleTilesInGrid(ref grid);
            GridSequence.Add(grid);
            ResetCounters();

            if (showGridSequence)
            {
                Debug.Log($"Grid: [{grid[0]}, {grid[1]}, {grid[2]}, {grid[3]}, {grid[4]}, {grid[5]}, {grid[6]}, {grid[7]}," +
                    $"{grid[8]}, {grid[9]}, {grid[10]}, {grid[11]}, {grid[12]}, {grid[13]}, {grid[14]}, {grid[15]}]");
            }
        }
    }

    //Resets all counters to 0
    private void ResetCounters()
    {
        PlusTile_Count = 0;
        NonPlusTile_Count = 0;
        ATKPlus1_Count = 0;
        ATKPlus2_Count = 0;
        ATKPlus3_Count = 0;
        ATKPlus4_Count = 0;
        MulATKPlus1_Count = 0;
        MulATKPlus2_Count = 0;
        Guard_Count = 0;
        HPPlus5_Count = 0;
        CPPlus1_Count = 0;
        CPPlus2_Count = 0;
        CmdItem_Count = 0;
        Mul_Count = 0;
        ElementSwap_Count = 0;
    }

    //Check if the tile being inserted violated the algorithmic rule
    private bool CheckViolatedRule(ref int count, ref int typeCount, int max, bool isPlusTile)
    {
        //Ensures that for each individual grid, there are only 9 Attack Plus Tiles and 7 other tiles
        int typeCountMax = (isPlusTile ? 9 : 7);
        if (count < max && typeCount < typeCountMax) { count++; typeCount++; return false; }
        return true;
    }

    //Shuffles all tiles from the grid
    private void ShuffleAllTiles(ref List<Tile> grid)
    {
        for (int i = GridSize - 1; i > 0; i--)
        {
            // might have a range error based random number
            int j = Mathf.FloorToInt((Random.value * (i + 1)) % GridSize);
            Tile temp = grid[i];
            grid[i] = grid[j];
            grid[j] = temp;
        }
    }

    //Shuffles all tiles from the grid
    private void ShuffleTilesInGrid(ref List<Tile> grid)
    {
        for (int i = 16 - 1; i > 0; i--)
        {
            // might have a range error based random number
            int j = Mathf.FloorToInt((Random.value * (i + 1)) % 16);
            Tile temp = grid[i];
            grid[i] = grid[j];
            grid[j] = temp;
        }
    }
}


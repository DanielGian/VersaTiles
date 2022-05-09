using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EaseFunctions;
using TMPro;

public class Command_SCR : MonoBehaviour
{
    public enum Rarity
    {
        Rare = 5,
        VeryUncommon = 25,
        Uncommon = 40,
        Average = 50,
        Common = 60,
    }
    public enum Element
    {
        Null,
        Fire,
        Water,
        Electric,
        Wood,
        Light,
        Dark
    }
    public enum Command
    {
        None = 0,
        Heal,               // Common
        SuperHeal,          // Uncommon
        FullHeal,           // Very Uncommon
        HPRegen,            // Average
        CPRegen,            // Average
        PowerUp,            // Common
        DefenseUp,          // Common
        Cure,               // Average
        Shuffle,            // Common
        SpreadAttack,       // Common
        PiercingBlow,       // Average
        Confusion,          // Average
        GetRevenge,         // Uncommon
        Slap,               // Common
        Struggle,           // Uncommon
        TouchDamage,        // Average
        Burn,               // Average (Fire)
        Fever,              // Uncommon (Fire)
        TripleFireballs,    // Average (Fire)
        BigBurst,           // Very Uncommon (Fire)
        RainDance,          // Average (Water)
        AquaShield,         // Uncommon (Water)
        Freeze,             // Uncommon (Water)
        TidalWave,          // Very Uncommon (Water)
        Paralyze,           // Common (Electric)
        E_Strike,           // Average (Electric)
        ThunderRage,        // Very Uncommon (Electric)
        ThinNeedles,        // Common (Wood)
        Absorb,             // Average (Wood)
        SleepPowder,        // Average (Wood)
        NewLeaf,            // Very Uncommon (Wood)
        LightSpears,        // Average (Light)
        Reflect,            // Average (Light)
        Poison,             // Common (Dark)
        LuckyBoost,         // Uncommon
        InstantKO,          // Rare
        LuckyCharm,         // Very Uncommon
        LuckySevenStreak,   // Rare
        Stop,               // Uncommon
        Gravity,            // Rare
        Immunity,           // Very Uncommon
    }

    private GameObject Obj;
    private GameObject BM;
    private GameObject Name_Text;
    private GameObject Cost_Text;
    [SerializeField] private Command command = Command.None;
    private SpriteRenderer SPR;
    private string cmdName = "";
    [SerializeField] private int CP_Cost = 0;
    [SerializeField] private string cmdDescription;
    private string cmdAffectStr;
    private string cmdRarityStr;
    private bool isAssigned = false;
    private bool startVibrating = false;
    private float vibration = 0f;
    private Vector3 staticPos;

    private void Awake()
    {
        Obj = this.gameObject;
        staticPos = Obj.transform.position;
        SPR = Obj.GetComponent<SpriteRenderer>();
        BM = GameObject.Find("BattleManager");
        Name_Text = Obj.transform.Find("Canvas/Name Text").gameObject;
        Cost_Text = Obj.transform.Find("Canvas/CP_Cost Text").gameObject;
        Name_Text.transform.position = Obj.transform.position + new Vector3(16f, 0f, -1f);
        Cost_Text.transform.position = Obj.transform.position + new Vector3(260f, 0f, -1f);
        SPR.color = new Color(1f, 1f, 1f, 0.25f);
        Obj.transform.Find("Canvas").GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startVibrating)
        {
            float randX = Random.Range(-vibration, vibration);
            float randY = Random.Range(-vibration, vibration);
            Obj.transform.position = staticPos + new Vector3(randX, randY, 0f);

            if (vibration > 0f) { vibration -= 0.25f; }
            else { vibration = 0; startVibrating = false; }
        }
        Name_Text.transform.position = Obj.transform.position + new Vector3(16f, 0f, -1f);
        Cost_Text.transform.position = Obj.transform.position + new Vector3(260f, 0f, -1f);
    }

    public string GetName() { return Name_Text.GetComponent<TMP_Text>().text; }
    public int GetCost() { return CP_Cost; }
    public string GetDescription() { return cmdDescription; }
    public string GetAffect() { return cmdAffectStr; }
    public string GetRarity() { return cmdRarityStr; }
    public Color GetColor() { return SPR.color; }
    public void Vibrate() { startVibrating = true; vibration = 15f; }

    private Command GetCommand(int elem)
    {
        int failSafeCounter = 0;
        Command cmd = Command.None;
        Element playerElement = (Element)elem;
        List<Command> CommandList = new List<Command>();

        do
        {
            if (RandomChance((int)Rarity.Common))
            {
                CommandList.AddRange(new Command[] { Command.Heal, Command.Shuffle, Command.SpreadAttack, Command.Slap });
                AddElementalCommands(ref CommandList, playerElement, 1);
            }
            else if (RandomChance((int)Rarity.Average))
            {
                CommandList.AddRange(new Command[] { Command.PowerUp, Command.DefenseUp, Command.HPRegen, Command.CPRegen, Command.PiercingBlow, Command.Confusion,
                                                        Command.TouchDamage });
                AddElementalCommands(ref CommandList, playerElement, 2);
            }
            else if (RandomChance((int)Rarity.Uncommon))
            {
                CommandList.AddRange(new Command[] { Command.SuperHeal, Command.Cure, Command.LuckyBoost, Command.Stop, Command.GetRevenge });
                AddElementalCommands(ref CommandList, playerElement, 3);
            }
            else if (RandomChance((int)Rarity.VeryUncommon))
            {
                CommandList.AddRange(new Command[] { Command.FullHeal, Command.LuckyCharm, Command.LuckySevenStreak });
                AddElementalCommands(ref CommandList, playerElement, 4);
            }
            else if (RandomChance((int)Rarity.Rare))
            {
                CommandList.AddRange(new Command[] {Command.InstantKO, Command.Gravity });
                AddElementalCommands(ref CommandList, playerElement, 5);
            }
            failSafeCounter++;
            if (failSafeCounter == 1) { CommandList.AddRange(new Command[] { Command.Shuffle }); }
        }
        while (CommandList.Count == 0);

        cmd = GetRandomCommand(CommandList);
        return cmd;
    }

    private void AddElementalCommands(ref List<Command> cmdList, Element playerElement, int rarityNum)
    {
        //switch (playerElement)
        //{
        //    case Element.Fire:
        //        {
        //            if (rarityNum == 2) { cmdList.AddRange(new Command[] { Command.Burn, Command.TripleFireballs }); }
        //            if (rarityNum == 5) { cmdList.Add(Command.BigBurst); }
        //            break;
        //        }
        //    case Element.Electric:
        //        {
        //            if (rarityNum == 1) { cmdList.Add(Command.Paralyze); }
        //            if (rarityNum == 2) { cmdList.Add(Command.E_Strike); }
        //            if (rarityNum == 5) { cmdList.Add(Command.ThunderRage); }
        //            break;
        //        }
        //    case Element.Wood:
        //        {
        //            if (rarityNum == 1) { cmdList.Add(Command.Paralyze); }
        //            if (rarityNum == 2) { cmdList.Add(Command.E_Strike); }
        //            if (rarityNum == 5) { cmdList.Add(Command.ThunderRage); }
        //            break;
        //        }
        //}
    }

    private bool RandomChance(int percentage) { return (Random.Range(0, 100) < percentage); }
    private Command GetRandomCommand(List<Command> commands) { return commands[Random.Range(0, commands.Count)]; }
    private bool MatchElement(Element firstElement, Element secondElement) { return (firstElement == secondElement); }

    public bool CommandIsAssigned() { return isAssigned; }
    public void AssignCommand()
    {
        isAssigned = true;
        command = GetCommand(BM.GetComponent<BattleManager_SCR>().GetPlayerElement());

        switch (command)
        {
            case Command.None: { AssignStats("Dummy", 5, "N/A", "N/A", new Color(0.5f, 0.5f, 0.5f), "Does nothing."); break; }
            case Command.Heal: { AssignStats("Heal", 2, "Common", "You", new Color(1f, 81f/255f, 184f/255f), "Heals 10 HP."); break; }
            case Command.SuperHeal: { AssignStats("Super Heal", 6, "Uncommon", "You", new Color(1f, 81f / 255f, 184f / 255f), "Heals 30 HP."); break; }
            case Command.FullHeal: { AssignStats("Full Heal", 12, "Very Uncommon", "You", new Color(1f, 81f / 255f, 184f / 255f), "Fully heals your HP."); break; }
            case Command.HPRegen: { AssignStats("HP Regen", 5, "Average", "You", new Color(1f, 81f / 255f, 184f / 255f), "Slowly heals 3 HP for 4 turns."); break; }
            case Command.CPRegen: { AssignStats("CP Regen", 5, "Average", "You", new Color(6f / 255f, 232f / 255f, 150f / 255f), "Slowly heals 2 CP for 4 turns."); break; }
            case Command.Shuffle: { AssignStats("Shuffle", 1, "Common", "You", new Color(0f, 214f / 255f, 32f / 255f), "Shuffles all tiles in your grid."); break; }
            case Command.SpreadAttack: { AssignStats("Spread Attack", 3, "Common", "All Enemies", new Color(252f / 255f, 186f / 255f, 3f / 255f), "Deals 3 damage to all enemies."); break; }
            case Command.PiercingBlow: { AssignStats("Piercing Blow", 5, "Average", "Enemy", new Color(1f, 132f / 255f, 0f), "Strong attack that negates the target enemy's defense."); break; }
            case Command.Confusion: { AssignStats("Confusion", 4, "Average", "Enemy", new Color(141f / 255f, 210f / 255f, 60f / 255f), "Confuses the target enemy. Enemy may attack itself."); break; }
            case Command.GetRevenge: { AssignStats("Get Revenge", 8, "Uncommon", "You", new Color(1f, 0f, 72f / 255f), "When attacked, all attackers receive 1/2 the damage you received."); break; }
            case Command.Slap: { AssignStats("Slap", 1, "Common", "Enemy", new Color(223f / 255f, 164f / 255f, 121f / 255f), "Flinches the enemy. Can only be used once per enemy."); break; }
            case Command.TouchDamage: { AssignStats("Touch Damage", 3, "Average", "You", new Color(1f, 110f / 255f, 74f / 255f), "When attacked, all attackers receive 1 damage for each hit."); break; }
            case Command.PowerUp: { AssignStats("Power Up", 4, "Average", "You", new Color(1f, 0f, 0f), "Boosts your Attack by 2."); break; }
            case Command.DefenseUp: { AssignStats("Defense Up", 4, "Average", "You", new Color(0f, 84f / 255f, 1f), "Boosts your Defense by 2."); break; }
            case Command.Cure: { AssignStats("Cure", 6, "Uncommon", "You", new Color(0f, 214f / 255f, 196f / 255f), "Cures all negative status ailments."); break; }
            case Command.LuckyBoost: { AssignStats("Lucky Boost", 3, "Uncommon", "You", new Color(1f, 184f / 255f, 83f / 255f), "Boosts the chance of landing Lucky hits by 1 stage."); break; }
            case Command.LuckyCharm: { AssignStats("Lucky Charm", 9, "Very Uncommon", "You", new Color(1f, 184f / 255f, 83f / 255f), "Fully maxes your chance of landing Lucky hits."); break; }
            case Command.LuckySevenStreak: { AssignStats("Lucky 7 Streak", 7, "Very Uncommon", "Enemy", new Color(1f, 33f / 255f, 78f / 255f), "Deals 7 damage to target enemy 1-7 time(s) per turn. Negates the target enemy's defense."); break; }
            case Command.InstantKO: { AssignStats("Instant KO", 9, "Rare", "Enemy", new Color(0.2f, 0.2f, 0.2f), "Instantly KOes the target enemy."); break; }
            case Command.Stop: { AssignStats("Stop", 6, "Uncommon", "Enemy", new Color(183f / 255f, 143f / 255f, 116f / 255f), "Stops the target enemy in place for 2 turns."); break; }
            case Command.Gravity: { AssignStats("Gravity", 9, "Rare", "You", new Color(100f / 255f, 51f / 255f, 245f / 255f), "Deals damage equal to 1/4 of the target enemy's current HP."); break; }
            case Command.Burn: { AssignStats("Burn", 3, "Average", "Enemy", new Color(1f, 0f, 0f), ""); break; }
            case Command.Fever: { AssignStats("Fever", 3, "Average", "Enemy", new Color(1f, 0f, 0f), "Lowers the target enemy's Attack."); break; }
            case Command.TripleFireballs: { AssignStats("Triple Fireball", 3, "Average", "Enemy", new Color(1f, 0f, 0f), ""); break; }
            case Command.BigBurst: { AssignStats("Big Burst", 3, "Very Uncommon", "Enemy", new Color(1f, 0f, 0f), ""); break; }
        }
        StartCoroutine(EasingFunctions.ColorChangeFromRGBA(Obj, new Color(SPR.color.r, SPR.color.g, SPR.color.b, 1f), 0.25f, Format.Scalar));
    }
    private void WriteMessage(string msg, bool typewriterMode) { BM.GetComponent<BattleManager_SCR>().WriteMessage(msg, typewriterMode); }
    private void ClearMessage() { BM.GetComponent<BattleManager_SCR>().ClearMessage(); }

    public float ActivateCommand()
    {
        WriteMessage($"You used {cmdName}!", true);
        switch (command)
        {
            case Command.None: { StartCoroutine(Dummy()); return 2f; }
            case Command.Heal: { StartCoroutine(Heal()); return 2f; }
            case Command.SuperHeal: { StartCoroutine(SuperHeal()); return 2f; }
            case Command.FullHeal: { StartCoroutine(FullHeal()); return 2f; }
            case Command.HPRegen: { StartCoroutine(HPRegen()); return 2.5f; }
            case Command.CPRegen: { StartCoroutine(CPRegen()); return 2.5f; }
            case Command.Shuffle: { StartCoroutine(Shuffle()); return 2.5f; }
            case Command.SpreadAttack: { StartCoroutine(SpreadAttack()); return 2.5f; }
            case Command.PiercingBlow: { StartCoroutine(PiercingBlow()); return 2.5f; }
            case Command.Confusion: { StartCoroutine(Confusion()); return 4f; }
            case Command.GetRevenge: { StartCoroutine(GetRevenge()); return 2.5f; }
            case Command.Slap: { StartCoroutine(Slap()); return 2.5f; }
            case Command.TouchDamage: { StartCoroutine(TouchDamage()); return 2.5f; }
            case Command.PowerUp: { StartCoroutine(PowerUp()); return 2.5f; }
            case Command.DefenseUp: { StartCoroutine(DefenseUp()); return 2.5f; }
            case Command.Cure: { StartCoroutine(Cure()); return 2.5f; }
            case Command.LuckyBoost: { StartCoroutine(LuckyBoost()); return 2.5f; }
            case Command.LuckyCharm: { StartCoroutine(LuckyCharm()); return 2.5f; }
            case Command.LuckySevenStreak:
                {
                    int numOfHits = 0;
                    int chance = 49;
                    for (int i = 0; i < 7; i++)
                    {
                        if (numOfHits == 0) { numOfHits = 1; chance -= 7; continue; }
                        if (RandomChance(chance)) { numOfHits++; chance -= 7; }
                        else { break; }
                    }
                    StartCoroutine(LuckySevenStreak(numOfHits));
                    return 2f + (0.2f * numOfHits);
                }
            case Command.InstantKO: { StartCoroutine(InstantKO()); return 2.5f; }
            case Command.Stop: { StartCoroutine(Stop()); return 2.5f; }
            case Command.Gravity: { StartCoroutine(Gravity()); return 2.5f; }
        }
        return 2f;
    }

    private IEnumerator Dummy()
    {
        yield return new WaitForSeconds(1f);
        WriteMessage("It does nothing!", true);
    }

    private IEnumerator Heal()
    {
        yield return new WaitForSeconds(1f);
        BM.GetComponent<BattleManager_SCR>().FadeOutTiles();
        BM.GetComponent<BattleManager_SCR>().AddHPToPlayer(10);
    }
    private IEnumerator SuperHeal()
    {
        yield return new WaitForSeconds(1f);
        BM.GetComponent<BattleManager_SCR>().FadeOutTiles();
        BM.GetComponent<BattleManager_SCR>().AddHPToPlayer(30);
    }
    private IEnumerator FullHeal()
    {
        int currentHP = BM.GetComponent<BattleManager_SCR>().GetPlayer().HP;
        int maxHP = BM.GetComponent<BattleManager_SCR>().GetPlayer().MaxHP;
        yield return new WaitForSeconds(1f);
        BM.GetComponent<BattleManager_SCR>().FadeOutTiles();
        BM.GetComponent<BattleManager_SCR>().AddHPToPlayer(maxHP - currentHP);
    }
    private IEnumerator HPRegen()
    {
        yield return new WaitForSeconds(1f);
        BM.GetComponent<BattleManager_SCR>().FadeOutTiles();
        BM.GetComponent<BattleManager_SCR>().GetPlayer().AddStatusAilment(BattleManager_SCR.MainPlayer.StatusAilment.HP_Regen, 4);
        WriteMessage($"Your HP will slowly recover for a brief period!", true);
    }
    private IEnumerator CPRegen()
    {
        yield return new WaitForSeconds(1f);
        BM.GetComponent<BattleManager_SCR>().FadeOutTiles();
        BM.GetComponent<BattleManager_SCR>().GetPlayer().AddStatusAilment(BattleManager_SCR.MainPlayer.StatusAilment.CP_Regen, 4);
        WriteMessage($"Your CP will slowly recover for a brief period!", true);
    }
    private IEnumerator Shuffle()
    {
        GameObject[] tempGrid = BM.GetComponent<BattleManager_SCR>().GetPlayerGrid();
        Playtile_SCR.Tile[] tileTypes = new Playtile_SCR.Tile[16];
        Vector3[] tilePositions = new Vector3[16];
        for (int i = 0; i < tilePositions.Length; i++)
        {
            tilePositions[i] = tempGrid[i].transform.position;
            tileTypes[i] = tempGrid[i].GetComponent<Playtile_SCR>().GetTileType();
        }
        int frameIndex = 0;
        for (int i = 0; i < tempGrid.Length; i++)
        {
            if (tempGrid[i].GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.ElementSwap)
            {
                frameIndex = tempGrid[i].GetComponent<Playtile_SCR>().GetFrameIndex();
                Debug.Log($"frameIndex: {frameIndex}");
            }
        }
        for (int i = tileTypes.Length - 1; i > 0; i--)
        {
            // might have a range error based random number
            int j = Mathf.FloorToInt((Random.value * (i + 1)) % tileTypes.Length);
            Playtile_SCR.Tile temp = tileTypes[i];
            tileTypes[i] = tileTypes[j];
            tileTypes[j] = temp;
        }

        yield return new WaitForSeconds(1f);
        for (int i = 0; i < tempGrid.Length; i++) { StartCoroutine(EasingFunctions.TranslateTo(tempGrid[i], new Vector3(454f, 467f, 0f), 0.5f, 3, Easing.EaseOut)); }
        yield return new WaitForSeconds(0.75f);
        for (int i = 0; i < tempGrid.Length; i++)
        {
            BM.GetComponent<BattleManager_SCR>().SetPlaytile(i, (int)tileTypes[i], frameIndex);
            StartCoroutine(EasingFunctions.TranslateTo(tempGrid[i], tilePositions[i], 0.5f, 3, Easing.EaseOut));
        }
    }
    private IEnumerator SpreadAttack()
    {
        List<GameObject> enemies = new List<GameObject>();
        BM.GetComponent<BattleManager_SCR>().GetEnemies(ref enemies);

        yield return new WaitForSeconds(0.8f);
        ClearMessage();
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies != null)
            {
                int punchNum = UnityEngine.Random.Range(20, 23);
                GameManager_SCR.PlaySound(punchNum);
                Debug.Log($"Enemy {i+1}");
                OtherFunctions.CreateObjectFromResource("Prefabs/Punch_PFX", enemies[i].transform.position);
                yield return new WaitForSeconds(0.2f);
                enemies[i].GetComponent<Enemy_SCR>().ReceiveDamage(3);
            }
            else { continue; }
        }
    }
    private IEnumerator PiercingBlow()
    {
        yield return new WaitForSeconds(0.8f);
        int punchNum = UnityEngine.Random.Range(20, 23);
        GameManager_SCR.PlaySound(punchNum);
        ClearMessage();
        GameObject EnemyTarget = BM.GetComponent<BattleManager_SCR>().GetEnemyTarget();
        OtherFunctions.CreateObjectFromResource("Prefabs/Punch_PFX", EnemyTarget.transform.position);
        yield return new WaitForSeconds(0.2f);
        EnemyTarget.GetComponent<Enemy_SCR>().ReceiveDamage(5, true);
    }
    private IEnumerator Confusion()
    {
        yield return new WaitForSeconds(1f);
        GameManager_SCR.PlaySound(23);
        ClearMessage();
        GameObject EnemyTarget = BM.GetComponent<BattleManager_SCR>().GetEnemyTarget();
        StartCoroutine(EasingFunctions.RelRotateCycles(EnemyTarget, 8, Axis.Z, 2f, false, 2, Easing.EaseInOut));
        yield return new WaitForSeconds(2f);
        EnemyTarget.GetComponent<Enemy_SCR>().AddStatusAilment(Enemy_SCR.StatusAilment.Confused, 3);
        WriteMessage($"The {EnemyTarget.GetComponent<Enemy_SCR>().GetName()} is confused! It may attack itself.", true);
    }
    private IEnumerator GetRevenge()
    {
        yield return new WaitForSeconds(1f);
        BM.GetComponent<BattleManager_SCR>().GetPlayer().AddStatusAilment(BattleManager_SCR.MainPlayer.StatusAilment.Revenge, 3);
        BM.GetComponent<BattleManager_SCR>().FadeOutTiles();
        WriteMessage($"When hit, all enemies receive 1/2 the damage you receive.", true);
    }
    private IEnumerator Slap()
    {
        yield return new WaitForSeconds(1f);
        ClearMessage();
        GameObject EnemyTarget = BM.GetComponent<BattleManager_SCR>().GetEnemyTarget();
        GameObject HandSlap = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", EnemyTarget.transform.position + new Vector3(-128f, 64f, -1f));
        HandSlap.AddComponent<SlapAnimation_SCR>();
    }
    private IEnumerator TouchDamage()
    {
        yield return new WaitForSeconds(1f);
        BM.GetComponent<BattleManager_SCR>().GetPlayer().AddStatusAilment(BattleManager_SCR.MainPlayer.StatusAilment.TouchDamage, 3);
        BM.GetComponent<BattleManager_SCR>().FadeOutTiles();
        WriteMessage($"When hit, all attackers receive 1 damage for each hit.", true);
    }
    private IEnumerator PowerUp()
    {
        yield return new WaitForSeconds(1f);
        BM.GetComponent<BattleManager_SCR>().AddATKToPlayer();
        BM.GetComponent<BattleManager_SCR>().FadeOutTiles();
        int playerATKLvl = BM.GetComponent<BattleManager_SCR>().GetPlayer().ATK_LevelNum;
        WriteMessage($"Your ATK stat was increased by 2!", true);
    }
    private IEnumerator DefenseUp()
    {
        yield return new WaitForSeconds(1f);
        BM.GetComponent<BattleManager_SCR>().AddDEFToPlayer();
        BM.GetComponent<BattleManager_SCR>().FadeOutTiles();
        int playerDEFLvl = BM.GetComponent<BattleManager_SCR>().GetPlayer().DEF_LevelNum;
        WriteMessage($"Your DEF stat was increased by 2!", true);
    }
    private IEnumerator Cure()
    {
        yield return new WaitForSeconds(1f);
        ClearMessage();
        BM.GetComponent<BattleManager_SCR>().GetPlayer().CureNegativeStatusAilments();
        WriteMessage($"All negative status ailments were cured!", true);
    }
    private IEnumerator LuckyBoost()
    {
        yield return new WaitForSeconds(1f);
        BM.GetComponent<BattleManager_SCR>().AddLUCKToPlayer();
        BM.GetComponent<BattleManager_SCR>().FadeOutTiles();
        int playerLUCKLvl = BM.GetComponent<BattleManager_SCR>().GetPlayer().LUCK_LevelNum;
        WriteMessage($"Your LUCK stat was increased!", true);
    }
    private IEnumerator LuckyCharm()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 3; i++) { BM.GetComponent<BattleManager_SCR>().AddLUCKToPlayer(); }
        BM.GetComponent<BattleManager_SCR>().FadeOutTiles();
        int playerLUCKLvl = BM.GetComponent<BattleManager_SCR>().GetPlayer().LUCK_LevelNum;
        WriteMessage($"Your LUCK stat was increased!", true);
    }
    private IEnumerator LuckySevenStreak(int numOfHits)
    {
        yield return new WaitForSeconds(1f);
        GameObject EnemyTarget = BM.GetComponent<BattleManager_SCR>().GetEnemyTarget();
        for (int tries = 0; tries < numOfHits; tries++)
        {
            EnemyTarget.GetComponent<Enemy_SCR>().ReceiveDamage(7, true);
            yield return new WaitForSeconds(0.2f);
        }
        if (numOfHits == 7) { WriteMessage($"AMAZING!!!!!!! You landed {numOfHits} hits!!!!!!!", true); }
        else { WriteMessage($"You landed {numOfHits} hits!", true); }
    }
    private IEnumerator InstantKO()
    {
        yield return new WaitForSeconds(0.8f);
        ClearMessage();
        int punchNum = UnityEngine.Random.Range(20, 23);
        GameManager_SCR.PlaySound(punchNum);
        GameObject EnemyTarget = BM.GetComponent<BattleManager_SCR>().GetEnemyTarget();
        OtherFunctions.CreateObjectFromResource("Prefabs/Punch_PFX", EnemyTarget.transform.position);
        yield return new WaitForSeconds(0.2f);
        EnemyTarget.GetComponent<Enemy_SCR>().ReceiveDamage(999, true);
    }
    private IEnumerator Stop()
    {
        yield return new WaitForSeconds(0.8f);
        ClearMessage();
        GameManager_SCR.PlaySound(25);
        GameObject EnemyTarget = BM.GetComponent<BattleManager_SCR>().GetEnemyTarget();
        string enemyName = EnemyTarget.GetComponent<Enemy_SCR>().GetName();
        EnemyTarget.GetComponent<Enemy_SCR>().AddStatusAilment(Enemy_SCR.StatusAilment.Stop, 2);
        WriteMessage($"The {enemyName} is frozen in time!", true);
    }
    private IEnumerator Gravity()
    {
        yield return new WaitForSeconds(0.8f);
        ClearMessage();
        int punchNum = UnityEngine.Random.Range(20, 23);
        GameManager_SCR.PlaySound(punchNum);
        GameObject EnemyTarget = BM.GetComponent<BattleManager_SCR>().GetEnemyTarget();
        int damage = Mathf.Max(1, (int)(EnemyTarget.GetComponent<Enemy_SCR>().GetHP() / 4f));
        OtherFunctions.CreateObjectFromResource("Prefabs/Punch_PFX", EnemyTarget.transform.position);
        yield return new WaitForSeconds(0.2f);
        EnemyTarget.GetComponent<Enemy_SCR>().ReceiveDamage(damage, true);
    }

    private void AssignStats(string name, int cp_cost, string rarity, string affectsWho, Color col, string desc)
    {
        cmdName = name;
        Name_Text.GetComponent<TMP_Text>().text = name;
        CP_Cost = cp_cost;
        Cost_Text.GetComponent<TMP_Text>().text = $"{CP_Cost} CP";
        SPR.color = col;
        cmdDescription = desc;
        cmdAffectStr = affectsWho;
        cmdRarityStr = rarity;
    }

    public void EraseStats()
    {
        isAssigned = false;
        command = Command.None;
        cmdName = "";
        Name_Text.GetComponent<TMP_Text>().text = "";
        CP_Cost = 0;
        Cost_Text.GetComponent<TMP_Text>().text = "";
        SPR.color = new Color(1f, 1f, 1f, 0.25f);
        cmdDescription = "";
        cmdAffectStr = "";
        cmdRarityStr = "";
    }
}

<h1 align="center">
    Outward Loot Manager
</h1>
<br/>
<div align="center">
  <img src="https://raw.githubusercontent.com/GymMed/Outward-Loot-Manager/refs/heads/main/preview/images/Logo.png" alt="Logo"/>
</div>

<div align="center">
	<a href="https://thunderstore.io/c/outward/p/GymMed/Loot_Manager/">
		<img src="https://img.shields.io/thunderstore/dt/GymMed/Loot_Manager" alt="Thunderstore Downloads">
	</a>
	<a href="https://github.com/GymMed/Outward-Loot-Manager/releases/latest">
		<img src="https://img.shields.io/thunderstore/v/GymMed/Loot_Manager" alt="Thunderstore Version">
	</a>
	<a href="https://github.com/GymMed/Outward-Mods-Communicator/releases/latest">
		<img src="https://img.shields.io/badge/Mods_Communicator-v1.2.0-D4BD00" alt="Mods Communicator Version">
	</a>
</div>

Outward mod that allows you to assign loots to enemies using [Mods Communicator](https://github.com/GymMed/Outward-Mods-Communicator) to send events.

<details>
    <summary>Deeper Explanation</summary>
This mod hooks into the <code>Lootable.OnDeath</code> method and evaluates the
provided <code>LootRules</code>. These rules determine whether your custom loot
should be applied to a dying enemy.

The mod acts as a central <strong>Loot Manager</strong> that stores simple,
abstracted logic and handles more complex game mechanics internally. Its state
can be saved or loaded using XML.

Other mods can inspect, modify, or react to the <code>LootManager</code>’s
dynamic state and see how different mods interact with it.
</details>

## How to use it

Firstly, install [Mods
Communicator](https://github.com/GymMed/Outward-Mods-Communicator)
After that, you can publish and subscribe to <code>LootManager</code> events.

All events are registered and visible in Mods Communicator’s logging system.
However, it is still recommended to read the event descriptions below and
review the examples. Many event fields are optional and give you extra control,
but they are not required.

If the total drop chance of your items exceeds 100, the system automatically
switches to weight-based loot instead of percentages. Providing
<code>maxDiceValue</code> also forces weight mode.

In weight mode, <code>LootManager</code> delegates all weight calculations to
Outward's original system, which assigns <code>minDiceRollValue</code> and
<code>maxDiceRollValue</code> to each item automatically.

You can control drop behavior using <code>emptyDropChance</code> and
<code>maxDiceValue</code>.

### Events

Check the parameter tables to understand which values are required. Event
descriptions below are intentionally abstract — always refer to the tables for
the exact field requirements.

<details>
    <summary>Publish</summary>
Mod Namespace: <code>gymmed.loot_manager_*</code>
Make sure to publish only after <code>LootManager</code> has initialized. I
recommend adding it as a dependency in your mod, or publishing it before/after
<code>ResourcesPrefabManager.Load</code> has finished.
<details>
    <summary>Add Loot</summary>
Event Name: <code>AddLoot</code>
Requires at least one of the following:

<ul>
<li>
<code>item</code>
</li>

<li>
<code>itemDropChance</code>
</li>

<li>
<code>ListItemDropChance</code>
</li>
</ul>

Also requires one or more of the filtering fields: enemy / faction / area / uniqueness.

This event is the most flexible one — you can do everything with it.
Other events exist only for convenience.

<details>
    <summary>Examples</summary>

<details>
    <summary>Guaranteed Loot</summary>
<pre><code>using OutwardModsCommunicator;
...
int myItemId = 4000360; // Dreamers root
var payload = new EventPayload
{
    ["itemId"] = myItemId,
    ["enemyName"] = "Hyena", //Character.Name
};
EventBus.Publish("gymmed.loot_manager_*", "AddLoot", payload);</code></pre>
</details>

<details>
    <summary>Chance Loot</summary>
Providing enemy id ignores other enemy requirements.<br>
This only works for enemy id, other requirements can be combined for
flexibility. By providing <code>dropChance</code> to item it will internally
make <code>emptyDropChance</code>
<pre><code>using OutwardModsCommunicator;
...
int myItemId = 4000360; // Dreamers root
string myEnemyId = "eCz766tEIEOWfK81om19wg"; // Calixa Boss
var payload = new EventPayload
{
    ["itemId"] = myItemId,
    ["dropChance"] = 50,
    ["enemyId"] = myEnemyId,
};
EventBus.Publish("gymmed.loot_manager_*", "AddLoot", payload);</code></pre>
</details>

<details>
    <summary>Faction Loot</summary>
Additionally added how many items should be dropped(<code>LootManager</code>
tries to build <code>ItemDropChance</code> internally). 
<pre><code>using OutwardModsCommunicator;
...
int myItemId = 4000360; // Dreamers root
var payload = new EventPayload
{
    ["itemId"] = myItemId,
    ["minDropCount"] = 1,
    ["maxDropCount"] = 3,
    ["faction"] = Character.Factions.Bandits,
};
EventBus.Publish("gymmed.loot_manager_*", "AddLoot", payload);</code></pre>
</details>

<details>
    <summary>Area Loot</summary>
<pre><code>using OutwardModsCommunicator;
...
int myItemId = 4000360; // Dreamers root
var payload = new EventPayload
{
    ["itemId"] = myItemId,
    ["area"] = AreaManager.AreaEnum.Abrassar,
};
EventBus.Publish("gymmed.loot_manager_*", "AddLoot", payload);</code></pre>
</details>

<details>
    <summary>Area Family Loot</summary>
I recommend to make a helper method of finding your <code>AreaFamily</code> by enum/name.<br>
You can find them in <code>AreaManager.AreaFamilies</code>.
<pre><code>using OutwardModsCommunicator;
...
int myItemId = 4000360; // Dreamers root
var payload = new EventPayload
{
    ["itemId"] = myItemId,
    ["areaFamily"] = AreaManager.AreaFamilies[2], //Levant Region includes abrassar and all dungeons
    // you can combine requirements
    ["faction"] = Character.Factions.Bandits,
    ["listExceptNames"] = new List&lt;string&gt;() { "Hyena" },
};
EventBus.Publish("gymmed.loot_manager_*", "AddLoot", payload);</code></pre>
</details>
</details>
</details>

<details>
    <summary>Add Loot By Enemy Name</summary>
Event Name: <code>AddLootByEnemyName</code><br>
Compares Character.Name to your provided string and if they match provides loot<br>
Required one of the item/itemDropChance/ListItemDropChance information.<br>
Required enemyName.<br>

Only <code>listExceptIds</code> work.

<details>
    <summary>Example</summary>
<pre><code>using OutwardModsCommunicator;
...
ItemDropChance myItem = new ItemDropChance();
myItem.ItemID = 4000360; // Dreamers root
myItem.DropChance = 30;
var payload = new EventPayload
{
    ["itemDropChance"] = myItem,
    ["enemyName"] = "Hyena", //Character.Name
    ["listExceptIds"] = new List&lt;string&gt;() { "yourEnemyId" },
};
EventBus.Publish("gymmed.loot_manager_*", "AddLootByEnemyName", payload);</code></pre>
</details>
</details>

<details>
    <summary>Add Loot By Enemy Id</summary>
Event Name: <code>AddLootByEnemyId</code><br>
Required one of the item/itemDropChance/ListItemDropChance information.<br>
Required enemyId.

<details>
    <summary>Example</summary>
<pre><code>using OutwardModsCommunicator;
...
List&lt;ItemDropChance&gt; drops = new();
ItemDropChance myFirstItem = new ItemDropChance();
myFirstItem.ItemID = 4000360; // Dreamers root
myFirstItem.DropChance = 30;
ItemDropChance mySecondItem = new ItemDropChance();
mySecondItem.ItemID = 6000170; // Purifying quartz
mySecondItem.DropChance = 30;
drops.Add(myFirstItem);
drops.Add(mySecondItem);
var payload = new EventPayload
{
    ["listOfItemDropChances"] = drops,
    ["enemyId"] = "JmeufMpL_E6eYnqCYP2r3w", // Elite Burning Man
};
EventBus.Publish("gymmed.loot_manager_*", "AddLootByEnemyId", payload);</code></pre>
</details>
</details>

<details>
    <summary>Add Loot For Uniques</summary>
Makes bosses lootable.<br>
Event Name: <code>AddLootForUniques</code><br>
Required one of the item/itemDropChance/ListItemDropChance information.<br>
Required one of the <code>isForBosses</code>/<code>isForBossPawns</code>/
<code>isForStoryBosses</code>/<code>isForUniqueArenaBosses</code>/
<code>isForUniqueArenaBosses</code> parameters.

<details>
    <summary>Example</summary>
<pre><code>using OutwardModsCommunicator;
...
int myItemId = 4000360; // Dreamers root
var payload = new EventPayload
{
    ["itemId"] = myItemId,
    ["isForBosses"] = true, // is for all bosses
};
EventBus.Publish("gymmed.loot_manager_*", "AddLootForUniques", payload);</code></pre>
</details>
</details>

<details>
    <summary>Load Loots XML</summary>
Loads your custom loot-rules XML document into the mod.<br>
Event Name: <code>LootRulesSerializer@LoadCustomLoots</code><br> 
Requires the full path to the XML file, provided as the <code>filePath</code> parameter.<br>

<details>
    <summary>Example</summary>
<pre><code>using OutwardModsCommunicator;
...
var payload = new EventPayload
{
    ["filePath"] = "assemblyLocation/filePath.xml",
};
EventBus.Publish("gymmed.loot_manager", "LootRulesSerializer@SaveLootRulesToXml", payload);</code></pre>
</details>
</details>

<details>
    <summary>Store Loots to XML</summary>
Stores the current <code>LootManager</code> loot rules into an XML document.<br>
Event Name: <code>LootRulesSerializer@SaveLootRulesToXml</code><br> 
Optional: you may provide a full XML file path using the <code>filePath</code>
parameter. If omitted, the file will be stored at:
<code>BepInEx/config/gymmed.Mods_Communicator/Loot_Manager/LootRules-date.xml</code>

<details>
    <summary>Example</summary>
<pre><code>using OutwardModsCommunicator;
...
var payload = new EventPayload
{
    //["filePath"] = "",
};
EventBus.Publish("gymmed.loot_manager", "LootRulesSerializer@SaveLootRulesToXml", payload);</code></pre>
</details>
</details>

</details>

<details>
    <summary>Subscribe</summary>
Mod Namespace: <code>gymmed.loot_manager</code><br>
You can listen then mod methods a triggered.

<details>
    <summary>Append Loot Rule</summary>
Event Name: <code>LootRuleRegistryManager@AppendLootRule</code><br>
On published and added loot rule gets triggered.

<details>
    <summary>Example</summary>
<pre><code>using OutwardModsCommunicator;
...
public awake()
{
...
EventBus.Subscribe(
"gymmed.loot_manager",
"LootRuleRegistryManager@AppendLootRule", 
YourMethod
);
}
...
public static void YourMethod(EventPayload payload)
{
    if (payload == null) return;
    int lootRuleId = payload.Get&lt;int&gt;("lootRuleId", null);
    // You would compare it with your registered loot rule id that you can provide
    // Your code...
}</code></pre>
</details>
</details>

<details>
    <summary>Remove Loot Rule</summary>
Event Name: <code>LootRuleRegistryManager@RemoveLootRule</code><br>
On published and removed loot rule gets triggered.

<details>
    <summary>Example</summary>
<pre><code>using OutwardModsCommunicator;
...
public awake()
{
...
EventBus.Subscribe(
"gymmed.loot_manager",
"LootRuleRegistryManager@RemoveLootRule", 
YourMethod
);
}
...
public static void YourMethod(EventPayload payload)
{
    if (payload == null) return;
    int lootRuleId = payload.Get&lt;int&gt;("lootRuleId", null);
    // You would compare it with your registered loot rule id that you can provide
    // Your code...
}</code></pre>
</details>
</details>

</details>

<details>
    <summary>Unique Enemies</summary>
If you going to provide only <code>enemyName</code> and that enemy is unique
make sure to provide one of <code>isForBosses</code>,
<code>isForBossPawns</code>, <code>isForStoryBosses</code>,
<code>isForUniqueArenaBosses</code>, <code>isForUniqueEnemies</code> parameters.<br>
Data collected using <a href="https://thunderstore.io/c/outward/p/GymMed/Scene_Tester/">Outward Scene Tester</a>

<details>
    <summary>Bosses</summary>
Below are all included in <code>isForBosses</code> parameter.
<details>
    <summary>Story Bosses</summary>
All enemies included in <code>isForStoryBosses</code>. They are all compared by id.
<table>
    <thead>
        <tr>
            <th>
                Picture
            </th>
            <th>
                Character.Name
            </th>
            <th>
                Character.m_name
            </th>
            <th>
                Character.m_nameLoc
            </th>
            <th>
                Character.UID
            </th>
            <th>
                Wiki location
            </th>
            <th>
                Game location
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Crimson_Avatar">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/d/d1/Crimson_Avatar.png/70px-Crimson_Avatar.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="85"
                    alt="Crimson Avatar"
                    />
                </a>
            </td>
            <td>
                Burning Man
            </td>
            <td>
                CrimsonAvatar (1)
            </td>
            <td>
                Undead_BurningMan
            </td>
            <td>
                4eeggsSn2Eyah4IjjqvpYQ
            </td>
            <td>
                Scarlet Sanctuary
            </td>
            <td>
                Scarlet Sanctuary
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Djinn">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/c/c4/Djinn.png/70px-Djinn.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="88"
                    alt="Djinn"
                    />
                </a>
            </td>
            <td>
                Gold Lich
            </td>
            <td>
                LichGold
            </td>
            <td>
                Undead_LichGold
            </td>
            <td>
                EwoPQ0iVwkK-XtNuaVPf3g
            </td>
            <td>
                Oil Refinery
            </td>
            <td>
                Oil Refinery
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Forge_Master">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/d/dd/Rust_Lich.png/70px-Rust_Lich.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="99"
                    alt="Forge Master"
                    />
                </a>
            </td>
            <td>
                Jade Lich
            </td>
            <td>
                LichRust
            </td>
            <td>
                Undead_LichJade
            </td>
            <td>
                shyc5M7b-UGVHBZsJMdP4Q
            </td>
            <td>
                Forgotten Research Laboratory
            </td>
            <td>
                Forgotten Research Laboratory
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Light_Mender">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/7/7c/Gold_Lich_-_LichGold.png/70px-Gold_Lich_-_LichGold.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="61"
                    alt="Light Mender"
                    />
                </a>
            </td>
            <td>
                Gold Lich
            </td>
            <td>
                LichGold
            </td>
            <td>
                Undead_LichGold
            </td>
            <td>
                EwoPQ0iVwkK-XtNuaVPf3g
            </td>
            <td>
                Spire of Light
            </td>
            <td>
                Spire of Light
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Plague_Doctor">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/d/d2/Jade_Lich_-_LichJade.png/70px-Jade_Lich_-_LichJade.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="81"
                    alt="Plague Doctor"
                    />
                </a>
            </td>
            <td>
                Jade Lich
            </td>
            <td>
                LichJade
            </td>
            <td>
                Undead_LichJade
            </td>
            <td>
                8sjFFBPMvkuJcrcyIYs-KA
            </td>
            <td>
                Dark Ziggurat
            </td>
            <td>
                Dark Ziggurat Interior
            </td>
        </tr>
    </tbody>
</table>
</details>

<details>
    <summary>Boss Pawns</summary>
All enemies included in <code>isForBossPawns</code>. They are all compared by id.
<table>
    <thead>
        <tr>
            <th>
                Picture
            </th>
            <th>
                Character.Name
            </th>
            <th>
                Character.m_name
            </th>
            <th>
                Character.m_nameLoc
            </th>
            <th>
                Character.UID
            </th>
            <th>
                Wiki location
            </th>
            <th>
                Game location
            </th>
            <th>
                Scene name
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Elite_Krypteia_Warrior">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/e/e3/Elite_Krypteia_Warrior.png/70px-Elite_Krypteia_Warrior.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="92"
                    alt="Elite Krypteia Warrior"
                    />
                </a>
            </td>
            <td>
                Balira
            </td>
            <td>
                KrypteiaGuard
            </td>
            <td>
                name_unpc_balira_01
            </td>
            <td>
                AbvgKMnPLUiffB6LzjaguQ
            </td>
            <td>
                Tower of Regrets Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                CalderaDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Elite_Krypteia_Witch">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/c/cd/Elite_Krypteia_Witch.png/70px-Elite_Krypteia_Witch.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="72"
                    alt="Elite Krypteia Witch"
                    />
                </a>
            </td>
            <td>
                Balira
            </td>
            <td>
                KrypteiaMage
            </td>
            <td>
                name_unpc_balira_01
            </td>
            <td>
                MfBjNPYsvkODdyLjYrlXXw
            </td>
            <td>
                Tower of Regrets Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                CalderaDungeonsBosses
            </td>
        </tr>
        <tr>
            <td rowspan="2">
                <a href="https://outward.fandom.com/wiki/Elite_Obsidian_Elemental">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/7/79/Elite_Obsidian_Elemental.png/70px-Elite_Obsidian_Elemental.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="82"
                    alt="Elite Obsidian Elemental"
                    />
                </a>
            </td>
            <td>
                Obsidian Elemental
            </td>
            <td rowspan="2">
                ObsidianElemental
            </td>
            <td rowspan="2">
                Wildlife_ObsidianElemental
            </td>
            <td>
                RM13rq4JTEqbuANnncMCKA
            </td>
            <td rowspan="2">
                Burning Tree Arena
            </td>
            <td rowspan="2">
                Unknown Arena
            </td>
            <td rowspan="2">
                EmercarDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                Obsidian Elemental (1)
            </td>
            <td>
                Qrq3e4nUpkS8CH3yd8J-ow
            </td>
        </tr>
    </tbody>
</table>
</details>

<details>
    <summary>Unique Arena Bosses</summary>
All enemies included in <code>isForUniqueArenaBosses</code>. They are all compared by id.
<table>
    <thead>
        <tr>
            <th>
                Picture
            </th>
            <th>
                Character.Name
            </th>
            <th>
                Character.m_name
            </th>
            <th>
                Character.m_nameLoc
            </th>
            <th>
                Character.UID
            </th>
            <th>
                Wiki location
            </th>
            <th>
                Game location
            </th>
            <th>
                Scene name
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Ash_Giant_Highmonk">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/5/51/Ash_Giant_Highmonk.png/70px-Ash_Giant_Highmonk.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="72"
                    alt="Ash Giant Highmonk"
                    />
                </a>
            </td>
            <td>
                Ash Giant Priest
            </td>
            <td>
                EliteAshGiantPriest
            </td>
            <td>
                Giant_Priest
            </td>
            <td>
                UnIXpnDMzUSfBu4S-ZDgsA
            </td>
            <td>
                Giant's Village Arena (south)
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                HallowedDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Brand_Squire">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/f/fe/Brand_Squire.png/70px-Brand_Squire.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="88"
                    alt="Brand Squire"
                    />
                </a>
            </td>
            <td>
                Desert Bandit
            </td>
            <td>
                EliteBrandSquire
            </td>
            <td>
                Bandit_Desert_Basic
            </td>
            <td>
                sb0TOkOPS06jhp56AOYJCw
            </td>
            <td>
                Conflux Mountain Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                ChersoneseDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Breath_of_Darkness">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/7/76/Breath_of_Darkness.png/70px-Breath_of_Darkness.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="87"
                    alt="Breath of Darkness"
                    />
                </a>
            </td>
            <td>
                Breath of Darkness
            </td>
            <td>
                AncientDwellerDark
            </td>
            <td>
                Elite_Dweller
            </td>
            <td>
                JmeufMpL_E6eYnqCYP2r3w
            </td>
            <td>
                The Vault of Stone
            </td>
            <td>
                The Vault of Stone
            </td>
            <td>
                Caldera_Dungeon8
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Calixa_(boss)">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/b/bf/Calixa_2.png/70px-Calixa_2.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="74"
                    alt="Calixa (boss)"
                    />
                </a>
            </td>
            <td>
                Cyrene
            </td>
            <td>
                EliteCalixa
            </td>
            <td>
                Cyrene
            </td>
            <td>
                eCz766tEIEOWfK81om19wg
            </td>
            <td>
                Levant Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                AbrassarDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Concealed_Knight:_%3F%3F%3F">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/2/2a/ConcealedKnight2.png/70px-ConcealedKnight2.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="92"
                    alt="Concealed Knight: ???"
                    />
                </a>
            </td>
            <td>
                ???
            </td>
            <td>
                NewBandit
            </td>
            <td>
                name_unpc_unknown_01
            </td>
            <td>
                XVuyIaCAVkatv89kId9Uqw
            </td>
            <td>
                Shipwreck (Castaway)
            </td>
            <td>
                CierzoTutorial
            </td>
            <td>
                CierzoTutorial
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Elite_Alpha_Tuanosaur">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/2/2f/Elite_Alpha_Tuanosaur.png/70px-Elite_Alpha_Tuanosaur.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="85"
                    alt="Elite Alpha Tuanosaur"
                    />
                </a>
            </td>
            <td>
                Alpha Tuanosaur
            </td>
            <td>
                EliteTuanosaurAlpha
            </td>
            <td>
                Wildlife_TuanosaurAlpha
            </td>
            <td>
                El8bA54i4E6vZraXsVZMow
            </td>
            <td>
                Ziggurat Passage Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                HallowedDungeonsBosses
            </td>
        </tr>
        <tr>
            <td rowspan="3">
                <a href="https://outward.fandom.com/wiki/Elite_Ash_Giant">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/b/bc/Elite_Ash_Giants.png/70px-Elite_Ash_Giants.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="44"
                    alt="Elite Ash Giant"
                    />
                </a>
            </td>
            <td rowspan="3">
                Ash Giant
            </td>
            <td>
                EliteAshGiantPaf
            </td>
            <td rowspan="3">
                Giant_Guard
            </td>
            <td>
                3vXChaIK90qgq03PmsHFCg
            </td>
            <td rowspan="3">
                Unknown Arena
            </td>
            <td rowspan="3">
                Unknown Arena
            </td>
            <td rowspan="3">
                HallowedDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                EliteAshGiantPif
            </td>
            <td>
                851czvFVDUaB42CgVzfKdg
            </td>
        </tr>
        <tr>
            <td>
                EliteAshGiantPouf
            </td>
            <td>
                kNmmOHZzKU-82F3OoX9NXw
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Elite_Beast_Golem">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/1/1d/Elite_Beast_Golem.png/70px-Elite_Beast_Golem.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="62"
                    alt="Elite Beast Golem"
                    />
                </a>
            </td>
            <td>
                Beast Golem
            </td>
            <td>
                EliteBeastGolem
            </td>
            <td>
                Golem_Beast
            </td>
            <td>
                n83g2QJhwUyUrN469WC4jA
            </td>
            <td>
                Parched Shipwrecks Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                AbrassarDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Elite_Boozu">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/d/d1/Elite_Boozu.png/70px-Elite_Boozu.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="45"
                    alt="Elite Boozu"
                    />
                </a>
            </td>
            <td>
                Blade Dancer
            </td>
            <td>
                BoozuProudBeast
            </td>
            <td>
                Wildlife_BladeDancer
            </td>
            <td>
                2Ef5z9OfYkev7M7Oi9GN-A
            </td>
            <td>
                Mana Lake Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                AntiqueFieldDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Elite_Burning_Man">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/d/d8/Elite_Burning_Man_2.png/70px-Elite_Burning_Man_2.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="77"
                    alt="Elite Burning Man"
                    />
                </a>
            </td>
            <td>
                Burning Man
            </td>
            <td>
                EliteBurningMan
            </td>
            <td>
                Undead_BurningMan
            </td>
            <td>
                JmeufMpL_E6eYnqCYP2r3w
            </td>
            <td>
                Burning Tree Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                EmercarDungeonsBosses
            </td>
        </tr>
        <tr>
            <td rowspan="3">
                <a href="https://outward.fandom.com/wiki/Elite_Crescent_Shark">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/f/f8/Elite_Crescent_Shark.png/70px-Elite_Crescent_Shark.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="70"
                    alt="Elite Crescent Shark"
                    />
                </a>
            </td>
            <td rowspan="3">
                Crescent Shark
            </td>
            <td>
                EliteCrescentShark
            </td>
            <td rowspan="3">
                Wildlife_CrescentShark
            </td>
            <td>
                RM13rq4JTEqbuANnncMCKA
            </td>
            <td rowspan="3">
                Electric Lab Arena
            </td>
            <td rowspan="3">
                Unknown Arena
            </td>
            <td rowspan="3">
                AbrassarDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                EliteCrescentShark (1)
            </td>
            <td>
                ElDi5-rvqEqJKcXhEdgwBQ
            </td>
        </tr>
        <tr>
            <td>
                EliteCrescentShark (2)
            </td>
            <td>
                z3sfjJtqQEmUZ_S6g2RPIg
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Elite_Crimson_Avatar">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/2/28/Elite_Crimson_Avatar.png/70px-Elite_Crimson_Avatar.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="82"
                    alt="Elite Crimson Avatar"
                    />
                </a>
            </td>
            <td>
                Burning Man
            </td>
            <td>
                CrimsonAvatarElite (1)
            </td>
            <td>
                Undead_BurningMan
            </td>
            <td>
                JmeufMpL_E6eYnqCYP2r3w
            </td>
            <td>
                Vault of Stone Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                CalderaDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Elite_Gargoyle_Alchemist">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/f/f1/Elite_Gargoyle_Alchemist.png/70px-Elite_Gargoyle_Alchemist.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="74"
                    alt="Elite Gargoyle Alchemist"
                    />
                </a>
            </td>
            <td>
                Shell Horror
            </td>
            <td>
                GargoyleBossMelee (1)
            </td>
            <td>
                Horror_Shell
            </td>
            <td>
                k75QmVu5t0e_zIjHnUFbIQ
            </td>
            <td>
                New Sirocco Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                CalderaDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Elite_Gargoyle_Mage">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/4/49/Elite_Gargoyle_Mage.png/70px-Elite_Gargoyle_Mage.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="69"
                    alt="Elite Gargoyle Mage"
                    />
                </a>
            </td>
            <td>
                Shell Horror
            </td>
            <td>
                GargoyleBossMelee (1)
            </td>
            <td>
                Horror_Shell
            </td>
            <td>
                AKBYHSSMJUaH9ddLiz_SZA
            </td>
            <td>
                New Sirocco Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                CalderaDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Elite_Gargoyle_Warrior">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/b/b4/Elite_Gargoyle_Warrior.png/70px-Elite_Gargoyle_Warrior.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="76"
                    alt="Elite Gargoyle Warrior"
                    />
                </a>
            </td>
            <td>
                Shell Horror
            </td>
            <td>
                GargoyleBossMelee (1)
            </td>
            <td>
                Horror_Shell
            </td>
            <td>
                Z6yTTWK4u0GjDPfZ9X332A
            </td>
            <td>
                New Sirocco Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                CalderaDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Elite_Mantis_Shrimp">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/f/f9/Elite_Mantis_Shrimp.png/70px-Elite_Mantis_Shrimp.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="51"
                    alt="Elite Mantis Shrimp"
                    />
                </a>
            </td>
            <td>
                Mantis Shrimp
            </td>
            <td>
                EliteMantisShrimp
            </td>
            <td>
                Wildlife_Shrimp
            </td>
            <td>
                RM13rq4JTEqbuANnncMCKA
            </td>
            <td>
                Voltaic Hatchery Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                ChersoneseDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Elite_Sublime_Shell">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/5/53/Elite_Sublime_Shell.png/70px-Elite_Sublime_Shell.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="74"
                    alt="Elite Sublime Shell"
                    />
                </a>
            </td>
            <td>
                Nicolas
            </td>
            <td>
                CageArmorBoss (1)
            </td>
            <td>
                Nicolas
            </td>
            <td>
                X-dfltOoGUm7YlCE_Li1zQ
            </td>
            <td>
                Isolated Windmill Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                AntiqueFieldDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Elite_Torcrab">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/d/d2/Elite_Torcrab.png/70px-Elite_Torcrab.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="62"
                    alt="Elite Torcrab"
                    />
                </a>
            </td>
            <td>
                Wildlife_Torcrab
            </td>
            <td>
                TorcrabGiant (1)
            </td>
            <td>
                Wildlife_Torcrab
            </td>
            <td>
                gQDvpLQh3kimgwMmvXJc4g
            </td>
            <td>
                River of Red Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                CalderaDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Grandmother">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/6/66/Grandmother.png/70px-Grandmother.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="79"
                    alt="Grandmother"
                    />
                </a>
            </td>
            <td>
                Ghost
            </td>
            <td>
                Grandmother
            </td>
            <td>
                Undead_Ghost
            </td>
            <td>
                7G5APgUksEGdQrBxKXr04g
            </td>
            <td>
                Tower of Regrets Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                CalderaDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Immaculate_Dreamer">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/2/2d/Elite_Immaculate.png/70px-Elite_Immaculate.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="81"
                    alt="Immaculate Dreamer"
                    />
                </a>
            </td>
            <td>
                Immaculate
            </td>
            <td>
                EliteImmaculate
            </td>
            <td>
                Horror_Immaculate
            </td>
            <td>
                9jsiejBtHkOzeo4tOyyweg
            </td>
            <td>
                Cabal of Wind Temple Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                EmercarDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Immaculate%27s_Bird">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/6/64/Immaculate%27s_Bird.png/70px-Immaculate%27s_Bird.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="48"
                    alt="Immaculate's Bird"
                    />
                </a>
            </td>
            <td>
                Immaculate
            </td>
            <td>
                EliteSupremeShell (1)
            </td>
            <td>
                Horror_Immaculate
            </td>
            <td>
                JsyOv_Cwu0K0HlXyZInRQQ
            </td>
            <td>
                Immaculate's Camp Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                AntiqueFieldDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Light_Mender">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/7/7c/Gold_Lich_-_LichGold.png/70px-Gold_Lich_-_LichGold.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="61"
                    alt="Light Mender"
                    />
                </a>
            </td>
            <td>
                Gold Lich
            </td>
            <td>
                LichGold (1)
            </td>
            <td>
                Undead_LichGold
            </td>
            <td>
                v9mN1u1uMkaxsncBXhIM9A
            </td>
            <td>
                Spire of Light
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                EmercarDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Plague_Doctor">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/d/d2/Jade_Lich_-_LichJade.png/70px-Jade_Lich_-_LichJade.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="81"
                    alt="Plague Doctor"
                    />
                </a>
            </td>
            <td>
                Jade Lich
            </td>
            <td>
                LichJade (1)
            </td>
            <td>
                Undead_LichJade
            </td>
            <td>
                GfWl16_MZ0uS7UYIKpS5Lg
            </td>
            <td>
                Dark Ziggurat
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                EmercarDungeonsBosses
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/wiki/Troglodyte_Queen">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/6/64/Trog_Queen_2.png/70px-Trog_Queen_2.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="65"
                    alt="Troglodyte Queen"
                    />
                </a>
            </td>
            <td>
                Mana Troglodyte
            </td>
            <td>
                TroglodyteMana (1)
            </td>
            <td>
                Troglodyte_Mana
            </td>
            <td>
                pcQlY_whLUCC-FZel18VMg
            </td>
            <td>
                Blister Burrow Arena
            </td>
            <td>
                Unknown Arena
            </td>
            <td>
                ChersoneseDungeonsBosses
            </td>
        </tr>
    </tbody>
</table>
</details>

</details>

<details>
    <summary>Definitive Edition Unique Enemies</summary>
All enemies included in <code>isForUniqueEnemies</code>. They are all compared by id.
<table>
    <thead>
        <tr>
            <th>
                Picture
            </th>
            <th>
                Character.Name
            </th>
            <th>
                Character.m_name
            </th>
            <th>
                Character.m_nameLoc
            </th>
            <th>
                Character.UID
            </th>
            <th>
                Wiki location
            </th>
            <th>
                Game location
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Accursed_Wendigo">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/3/3f/Accursed_Wendigo.png/70px-Accursed_Wendigo.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="57"
                    alt="Accursed Wendigo"
                    />
                </a>
            </td>
            <td>
                Accursed Wendigo
            </td>
            <td>
                WendigoAccursed
            </td>
            <td>
                Unique_DefEd_Wendigo
            </td>
            <td>
                ElxncPIfuEuWkexSKkqFXg
            </td>
            <td>
                Corrupted Tombs
            </td>
            <td>
                Corrupted Tombs
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Altered_Gargoyle">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/2/20/Altered_Gargoyle.png/70px-Altered_Gargoyle.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="59"
                    alt="Altered Gargoyle"
                    />
                </a>
            </td>
            <td>
                Altered Gargoyle
            </td>
            <td>
                GargoyleAltered
            </td>
            <td>
                Unique_DefEd_AlteredGargoyle
            </td>
            <td>
                dhQVMNRU5kCIWsWRFYpDdw
            </td>
            <td>
                Ziggurat Passage
            </td>
            <td>
                Ziggurat Passage
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Ancestral_General">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/b/b8/Ancestral_General.png/70px-Ancestral_General.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="86"
                    alt="Ancestral General"
                    />
                </a>
            </td>
            <td>
                Ancestral General
            </td>
            <td>
                SkeletonBig (1)
            </td>
            <td>
                Unique_DefEd_AncestorBig
            </td>
            <td>
                XVuyIaCAVkatv89kId9Uqw
            </td>
            <td>
                Necropolis
            </td>
            <td>
                Necropolis
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Ancestral_Soldier">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/1/10/Ancestral_Soldier.png/70px-Ancestral_Soldier.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="73"
                    alt="Ancestral Soldier"
                    />
                </a>
            </td>
            <td>
                Ancestral Soldier
            </td>
            <td>
                SkeletonSmall (1)
            </td>
            <td>
                Unique_DefEd_AncestorSmall
            </td>
            <td>
                IwZYxBIQZkaXXMWT9HC5nA
            </td>
            <td>
                Necropolis
            </td>
            <td>
                Necropolis
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Bloody_Alexis">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/e/ec/Bloody_Alexis.png/70px-Bloody_Alexis.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="65"
                    alt="Bloody Alexis"
                    />
                </a>
            </td>
            <td>
                Bloody Alexis
            </td>
            <td>
                HumanArmoredThug
            </td>
            <td>
                Unique_DefEd_ArmoredThug
            </td>
            <td>
                VfYPPb4wcESdDVSiEq4UhA
            </td>
            <td>
                Undercity Passage
            </td>
            <td>
                Undercity Passage
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Calygrey_Hero">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/6/6d/Calygrey_Hero.png/70px-Calygrey_Hero.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="74"
                    alt="Calygrey Hero"
                    />
                </a>
            </td>
            <td>
                Calygrey Hero
            </td>
            <td>
                LionmanElite (1)
            </td>
            <td>
                Elite_Calygrey
            </td>
            <td>
                pMfhK69Stky7MvE9Ro0XMQ
            </td>
            <td>
                Steam Bath Tunnels
            </td>
            <td>
                Steam Bath Tunnels
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Chromatic_Arcane_Elemental">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/4/44/Chromatic_Elemental.png/70px-Chromatic_Elemental.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="52"
                    alt="Chromatic Arcane Elemental"
                    />
                </a>
            </td>
            <td>
                Chromatic Arcane Elemental
            </td>
            <td>
                ElementalChromatic
            </td>
            <td>
                Unique_DefEd_ChromaElemental
            </td>
            <td>
                RM13rq4JTEqbuANnncMCKA
            </td>
            <td>
                Compromised Mana Transfer Station
            </td>
            <td>
                Compromised Mana Transfer Station
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Cracked_Gargoyle">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/f/fd/Cracked_Gargoyle.png/70px-Cracked_Gargoyle.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="77"
                    alt="Cracked Gargoyle"
                    />
                </a>
            </td>
            <td>
                Cracked Gargoyle
            </td>
            <td>
                GargoyleCracked
            </td>
            <td>
                Unique_DefEd_CrackedGargoyle
            </td>
            <td>
                -McLNdZsNEa3itw-ny7YBw
            </td>
            <td>
                Ark of the Exiled
            </td>
            <td>
                Ark of the Exiled
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Elemental_Parasite">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/d/d4/Elemental_Parasite.png/70px-Elemental_Parasite.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="69"
                    alt="Elemental Parasite"
                    />
                </a>
            </td>
            <td>
                Crescent Shark
            </td>
            <td>
                ElementalParasite
            </td>
            <td>
                Wildlife_CrescentShark
            </td>
            <td>
                YDPy9S-An0-qGuvrJAM8yA
            </td>
            <td>
                Crumbling Loading Docks
            </td>
            <td>
                Crumbling Loading Docks
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Executioner_Bug">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/b/b4/Executioner_Bug.png/70px-Executioner_Bug.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="49"
                    alt="Executioner Bug"
                    />
                </a>
            </td>
            <td>
                Executioner Bug
            </td>
            <td>
                ExecutionerBug
            </td>
            <td>
                Unique_DefEd_ExecutionerBug
            </td>
            <td>
                MWplmyrxokSXELL63oWcAg
            </td>
            <td>
                The Slide
            </td>
            <td>
                The Slide
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Ghost_of_Vanasse">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/7/77/Ghost_of_Vanasse_-_GhostOfVanasse.png/70px-Ghost_of_Vanasse_-_GhostOfVanasse.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="105"
                    alt="Ghost of Vanasse"
                    />
                </a>
            </td>
            <td>
                Ghost of Vanasse
            </td>
            <td>
                GhostOfVanasse
            </td>
            <td>
                Undead_GhostVanasse
            </td>
            <td>
                R5UngwGS5EGWCH13toZO8Q
            </td>
            <td>
                Chersonese
            </td>
            <td>
                Chersonese
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Giant_Horror">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/e/e3/Giant_Horror.png/70px-Giant_Horror.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="80"
                    alt="Giant Horror"
                    />
                </a>
            </td>
            <td>
                Giant_Horror
            </td>
            <td>
                GiantHorror
            </td>
            <td>
                Giant_Horror
            </td>
            <td>
                SntMM-EzuE-ptgmqp4qkYQ
            </td>
            <td>
                Crumbling Loading Docks
            </td>
            <td>
                Crumbling Loading Docks
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Glacial_Tuanosaur">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/f/f8/Glacial_Tuanosaur.png/70px-Glacial_Tuanosaur.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="71"
                    alt="Glacial Tuanosaur"
                    />
                </a>
            </td>
            <td>
                Glacial Tuanosaur
            </td>
            <td>
                TuanosaurIce
            </td>
            <td>
                Unique_DefEd_GlacialTuano
            </td>
            <td>
                1IKBT9DYc0yIkESwKtU40g
            </td>
            <td>
                Conflux Chambers
            </td>
            <td>
                Conflux Chambers
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Golden_Matriarch">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/c/c9/Golden_Matriarch.png/70px-Golden_Matriarch.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="56"
                    alt="Golden Matriarch"
                    />
                </a>
            </td>
            <td>
                Golden Matriarch
            </td>
            <td>
                SpecterMeleeMatriarch
            </td>
            <td>
                Unique_DefEd_GoldenMatriarch
            </td>
            <td>
                GI-aE4Ry7UOIyAYYk7emFg
            </td>
            <td>
                Voltaic Hatchery
            </td>
            <td>
                Voltaic Hatchery
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Grandmother_Medyse">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/1/1d/Grandmother_Medyse.png/70px-Grandmother_Medyse.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="84"
                    alt="Grandmother Medyse"
                    />
                </a>
            </td>
            <td>
                Grandmother Medyse
            </td>
            <td>
                JellyFishMother
            </td>
            <td>
                Unique_DefEd_GrandmotherMedyse
            </td>
            <td>
                kp9R4kaoG02YfLdS9ROM4w
            </td>
            <td>
                Sulphuric Caverns
            </td>
            <td>
                Sulphuric Caverns
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Greater_Grotesque">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/0/05/Greater_Grotesque.png/70px-Greater_Grotesque.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="70"
                    alt="Greater Grotesque"
                    />
                </a>
            </td>
            <td>
                Greater Grotesque
            </td>
            <td>
                ImmaculateHorrorGreater
            </td>
            <td>
                Unique_DefEd_GreaterGrotestue
            </td>
            <td>
                JmeufMpL_E6eYnqCYP2r3w
            </td>
            <td>
                Lost Golem Manufacturing Facility
            </td>
            <td>
                Lost Golem Manufacturing Facility
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Guardian_of_the_Compass">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/5/5d/Guardian_of_the_Compass_-_Golem_Boss.png/70px-Guardian_of_the_Compass_-_Golem_Boss.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="91"
                    alt="Guardian of the Compass"
                    />
                </a>
            </td>
            <td>
                Guardian of the Compass
            </td>
            <td>
                GolemBoss
            </td>
            <td>
                Golem_Basic2
            </td>
            <td>
                BINT--E9xUaCgp4onBM54g
            </td>
            <td>
                The Walled Garden
            </td>
            <td>
                Abrassar
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Kazite_Admiral">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/6/6c/Kazite_Admiral.png/70px-Kazite_Admiral.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="69"
                    alt="Kazite Admiral"
                    />
                </a>
            </td>
            <td>
                Kazite Admiral
            </td>
            <td>
                HumanKaziteCaptain
            </td>
            <td>
                Unique_DefEd_KaziteCaptain
            </td>
            <td>
                XVuyIaCAVkatv89kId9Uqw
            </td>
            <td>
                Abandoned Living Quarters
            </td>
            <td>
                Abandoned Living Quarters
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Lightning_Dancer">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/1/1b/Lightning_Dancer.png/70px-Lightning_Dancer.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="58"
                    alt="Lightning Dancer"
                    />
                </a>
            </td>
            <td>
                Lightning Dancer
            </td>
            <td>
                BladeDancerLight
            </td>
            <td>
                Unique_DefEd_LightningDancer
            </td>
            <td>
                QRzc3AYY10CWyXOMgrIQTg
            </td>
            <td>
                Ancient Foundry
            </td>
            <td>
                Ancient Foundry
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Liquid-Cooled_Golem">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/3/3e/Liquid-Cooled_Golem.png/70px-Liquid-Cooled_Golem.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="77"
                    alt="Liquid-Cooled Golem"
                    />
                </a>
            </td>
            <td>
                Liquid-Cooled Golem
            </td>
            <td>
                GolemShieldedIce
            </td>
            <td>
                Unique_DefEd_LiquidGolem
            </td>
            <td>
                8ztut4_yiEmK0-NFLa-XNQ
            </td>
            <td>
                Destroyed Test Chambers
            </td>
            <td>
                Destroyed Test Chambers
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Luke_the_Pearlescent">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/4/44/Luke_the_Pearlescent_-_NewBanditEquip_WhiteScavengerCaptainBoss_A.png/70px-Luke_the_Pearlescent_-_NewBanditEquip_WhiteScavengerCaptainBoss_A.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="88"
                    alt="Luke the Pearlescent"
                    />
                </a>
            </td>
            <td>
                Luke the Pearlescent
            </td>
            <td>
                NewBanditEquip_WhiteScavengerCaptainBoss_A (1)
            </td>
            <td>
                Bandit_Standard_Captain2
            </td>
            <td>
                XVuyIaCAVkatv89kId9Uqw
            </td>
            <td>
                Ruins of Old Levant
            </td>
            <td>
                Abrassar
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Mad_Captain%27s_Bones">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/6/6b/Mad_Captain%27s_Bones.png/70px-Mad_Captain%27s_Bones.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="88"
                    alt="Mad Captain's Bones"
                    />
                </a>
            </td>
            <td>
                Mad Captain’s Bones
            </td>
            <td>
                SkeletFighter
            </td>
            <td>
                Undead_Skeleton2
            </td>
            <td>
                JM_HjGXMlkq7a1Yb6gijgQ
            </td>
            <td>
                Pirates' Hideout
            </td>
            <td>
                Chersonese Misc. Dungeons
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Matriarch_Myrmitaur">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/f/f0/Matriarch_Myrmitaur.png/70px-Matriarch_Myrmitaur.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="83"
                    alt="Matriarch Myrmitaur"
                    />
                </a>
            </td>
            <td>
                Matriarch Myrmitaur
            </td>
            <td>
                MyrmElite (1)
            </td>
            <td>
                Elite_Myrm
            </td>
            <td>
                6sB4_5lOJU2bWuMHnOL4Ww
            </td>
            <td>
                Myrmitaur’s Haven
            </td>
            <td>
                Myrmitaur’s Haven
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Quartz_Elemental">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/2/22/Quartz_Elemental.png/70px-Quartz_Elemental.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="77"
                    alt="Quartz Elemental"
                    />
                </a>
            </td>
            <td>
                Quartz Elemental
            </td>
            <td>
                ObsidianElementalQuartz
            </td>
            <td>
                Unique_DefEd_QuartzElemental
            </td>
            <td>
                LhhpSt8BO0aRN5mbeSuDrw
            </td>
            <td>
                The Grotto of Chalcedony
            </td>
            <td>
                The Grotto of Chalcedony
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Razorhorn_Stekosaur">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/e/e6/Razorhorn_Stekosaur.png/70px-Razorhorn_Stekosaur.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="69"
                    alt="Razorhorn Stekosaur"
                    />
                </a>
            </td>
            <td>
                Razorhorn Stekosaur
            </td>
            <td>
                SteakosaurBlack (1)
            </td>
            <td>
                Unique_DefEd_BlackSteko
            </td>
            <td>
                03dSXwJMRUuzGu8s3faATQ
            </td>
            <td>
                Reptilian Lair
            </td>
            <td>
                Reptilian Lair
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Royal_Manticore">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/0/0e/The_Royal_Manticore_Close-up.png/70px-The_Royal_Manticore_Close-up.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="39"
                    alt="Royal Manticore"
                    />
                </a>
            </td>
            <td>
                The Royal Manticore
            </td>
            <td>
                RoyalManticore
            </td>
            <td>
                Wildlife_Manticore2
            </td>
            <td>
                RM13rq4JTEqbuANnncMCKA
            </td>
            <td>
                Enmerkar Forest
            </td>
            <td>
                Enmerkar Forest
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Rusted_Enforcer">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/a/a4/Rusted_Enforcer.png/70px-Rusted_Enforcer.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="73"
                    alt="Rusted Enforcer"
                    />
                </a>
            </td>
            <td>
                Rusted Enforcer
            </td>
            <td>
                GolemRusted (1)
            </td>
            <td>
                Unique_DefEd_RustyGolem
            </td>
            <td>
                Ed2bzrgz5k-cRx3bUYTfmg
            </td>
            <td>
                Ghost Pass
            </td>
            <td>
                Ghost Pass
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Sandrose_Horror">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/2/27/Sandrose_Horror.png/70px-Sandrose_Horror.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="60"
                    alt="Sandrose Horror"
                    />
                </a>
            </td>
            <td>
                Sandrose Horror
            </td>
            <td>
                ShelledHorrorBurning
            </td>
            <td>
                Unique_DefEd_SandroseHorror
            </td>
            <td>
                H7HoCKhBl0mC1j9UOECDrQ
            </td>
            <td>
                Sand Rose Cave
            </td>
            <td>
                Sand Rose Cave
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/She_Who_Speaks">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/7/74/She_Who_Speaks.png/70px-She_Who_Speaks.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="63"
                    alt="She Who Speaks"
                    />
                </a>
            </td>
            <td>
                She Who Speaks
            </td>
            <td>
                AncientDwellerSpeak
            </td>
            <td>
                Unique_DefEd_BossDweller
            </td>
            <td>
                MBooN38mU0GPjQJGRuJ95g
            </td>
            <td>
                The Vault of Stone
            </td>
            <td>
                The Vault of Stone
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/That_Annoying_Troglodyte">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/1/1c/That_Annoying_Troglodyte.png/70px-That_Annoying_Troglodyte.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="61"
                    alt="That Annoying Troglodyte"
                    />
                </a>
            </td>
            <td>
                That Annoying Troglodyte
            </td>
            <td>
                TroglodyteAnnoying
            </td>
            <td>
                Unique_DefEd_AnnoyingTrog
            </td>
            <td>
                no-Z4ibpcEWbNntm_wRwZA
            </td>
            <td>
                Jade Quarry
            </td>
            <td>
                Jade Quarry
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/The_Crusher">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/b/bc/The_Crusher.png/70px-The_Crusher.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="72"
                    alt="The Crusher"
                    />
                </a>
            </td>
            <td>
                The Crusher
            </td>
            <td>
                HumanCrusher (1)
            </td>
            <td>
                Unique_DefEd_DesertCrusher
            </td>
            <td>
                AZL-EjXmhkOYB1obj0VkTw
            </td>
            <td>
                Ancestor’s Resting Place
            </td>
            <td>
                Ancestor’s Resting Place
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/The_First_Cannibal">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/b/bd/The_First_Cannibal2.png/70px-The_First_Cannibal2.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="64"
                    alt="The First Cannibal"
                    />
                </a>
            </td>
            <td>
                The First Cannibal
            </td>
            <td>
                WendigoCanibal
            </td>
            <td>
                Wildlife_Wendigo2
            </td>
            <td>
                wrYHXXh8J0KMhwoV8AC59w
            </td>
            <td>
                Face of the Ancients
            </td>
            <td>
                Face of the Ancients
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/The_Last_Acolyte">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/3/3b/The_Last_Acolyte.png/70px-The_Last_Acolyte.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="75"
                    alt="The Last Acolyte"
                    />
                </a>
            </td>
            <td>
                The Last Acolyte
            </td>
            <td>
                HumanAcolyte (1)
            </td>
            <td>
                Unique_DefEd_LastAcolyte
            </td>
            <td>
                YeYzQP-gYUmSivlk5JCJew
            </td>
            <td>
                Stone Titan Caves
            </td>
            <td>
                Stone Titan Caves
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Thunderbolt_Golem">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/4/46/Thunderbolt_Golem.png/70px-Thunderbolt_Golem.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="64"
                    alt="Thunderbolt Golem"
                    />
                </a>
            </td>
            <td>
                Troglodyte Archmage
            </td>
            <td>
                TroglodyteArcMageDefEd (1)
            </td>
            <td>
                Unique_DefEd_TrogMage
            </td>
            <td>
                syKWNGT3QUO3nXxPt1WEcQ
            </td>
            <td>
                Blister Burrow
            </td>
            <td>
                Blister Burrow
            </td>
        </tr>
        <tr rowspan="3">
            <td rowspan="3">
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Titanic_Guardian_Mk-7">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/4/4e/Titanic_Guardian_Mk-7.png/70px-Titanic_Guardian_Mk-7.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="69"
                    alt="Titanic Guardian Mk-7"
                    />
                </a>
            </td>
            <td rowspan="3">
                Jade Lich
            </td>
            <td>
                TitanGolemHalberd
            </td>
            <td rowspan="3">
                Undead_LichJade
            </td>
            <td>
                65aI6XT89kmHa1bwJz5PGQ
            </td>
            <td rowspan="3">
                Ruined Warehouse
            </td>
            <td rowspan="3">
                Ruined Warehouse
            </td>
        </tr>
        <tr>
            <td>
                TitanGolemHammer
            </td>
            <td>
                G_Q0oH1ttkWAZXCMuaAHjA
            </td>
        </tr>
        <tr>
            <td>
                TitanGolemSword
            </td>
            <td>
                wj3frikyIkqwVv7myrc5gw
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Tyrant_of_the_Hive">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/3/32/Hive_Lord_-_HiveLord_1AI_D-.png/70px-Hive_Lord_-_HiveLord_1AI_D-.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="71"
                    alt="Tyrant of the Hive"
                    />
                </a>
            </td>
            <td>
                Tyrant of the Hive
            </td>
            <td>
                HiveLord1AID+
            </td>
            <td>
                Undead_Hivelord2
            </td>
            <td>
                yOo-iKN3-0mAtZ2pG16pyw
            </td>
            <td>
                Forest Hives
            </td>
            <td>
                Forest Hives
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Vile_Illuminator">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/6/61/Vile_Illuminator.png/70px-Vile_Illuminator.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="61"
                    alt="Vile Illuminator"
                    />
                </a>
            </td>
            <td>
                Vile Illuminator
            </td>
            <td>
                IlluminatorHorrorVile
            </td>
            <td>
                Unique_DefEd_VileIlluminator
            </td>
            <td>
                l5ignQfsE0Cv4imB9DZJ5w
            </td>
            <td>
                Cabal of Wind Temple
            </td>
            <td>
                Cabal of Wind Temple
            </td>
        </tr>
        <tr>
            <td>
                <a href="https://outward.fandom.com/https://outward.fandom.com/wiki/Virulent_Hiveman">
                    <img
                    src="https://static.wikia.nocookie.net/outward_gamepedia/images/thumb/3/3c/Virulent_Hiveman.png/70px-Virulent_Hiveman.png"
                    loading="lazy"
                    decoding="async"
                    width="70"
                    height="68"
                    alt="Virulent Hiveman"
                    />
                </a>
            </td>
            <td>
                Virulent Hiveman
            </td>
            <td>
                HiveManVirulent
            </td>
            <td>
                Unique_DefEd_VirulentHiveman
            </td>
            <td>
                v1PnLFpcxEmm_IrZaP-eyg
            </td>
            <td>
                Ancient Hive
            </td>
            <td>
                Ancient Hive
            </td>
        </tr>
    </tbody>
</table>
</details>
</details>

<details>
    <summary>All Parameters</summary>

<table>
    <thead>
        <tr>
            <th colspan="2">Required atleast one of type</th> <th>Parameter</th> <th>Type</th> <th>Description</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td rowspan="8" align="center">Item</td> <td rowspan="6" align="center">Builds ItemDropChance Internally</td> <td>itemId</td> <td>int</td> <td>Required if itemDropChance/listOfItemDropChances is not provided. Loot item ID.</td>
        </tr>
        <tr>
            <td>dropChance</td> <td>int</td> <td>Optional. Default is 10. Determines chance of dropping item. You can provide ItemDropChance instead if you like.</td>
        </tr>
        <tr>
            <td>minDropCount</td> <td>int</td> <td>Optional. Default is 1. Provides minimum amount of items could be dropped. You can provide ItemDropChance instead if you like.</td>
        </tr>
        <tr>
            <td>dropChance</td> <td>int</td> <td>Optional. Default is 1. Provides maximum amount of items could be dropped. You can provide ItemDropChance instead if you like.</td>
        </tr>
        <tr>
            <td>minDiceRollValue</td> <td>int</td> <td>Optional. Default is 0. Sets the lowest dice roll value at which item drop chances begin to count. Use together with 'maxDiceRollValue' and 'maxDiceValue'. You can provide ItemDropChance instead if you like.</td>
        </tr>
        <tr>
            <td>maxDiceRollValue</td> <td>int</td> <td>Optional. Default is 0. Sets the highest dice roll value considered when calculating item drop chances. Use together with 'minDiceRollValue' and 'maxDiceValue'. You can provide ItemDropChance instead if you like.</td>
        </tr>
        <tr>
            <td colspawn="2" rowspan="2" align="center">Choice to provide instead</td> <td>listOfItemDropChances</td> <td>List&lt;string&gt;</td> <td>Optional. Default null. Provide your created list of your ItemDropChance instances to be dropped.</td>
        </tr>
        <tr>
            <td>itemDropChance</td> <td>ItemDropChance</td> <td>Optional. Default null. Provide your created ItemDropChance instance to be dropped.</td>
        </tr>
        <tr>
            <td colspan="2" rowspan="10" align="center">Enemy</td> <td>enemyId</td> <td>string</td> <td>Default null. Determines if drop should be appliead for enemy. You can get this from UnityExplorer mod or logs.</td>
        </tr>
        <tr>
            <td>enemyName</td> <td>string</td> <td>Default null. Determines if drop should be appliead for enemy. You can get this from UnityExplorer mod or logs.</td>
        </tr>
        <tr>
            <td>area</td> <td>AreaManager.AreaEnum?</td> <td>Optional. Default nullable. Determines if drop should be appliead for specific area. You can get this from AreaManager.AreaEnum enum.</td>
        </tr>
        <tr>
            <td>areaFamily</td> <td>AreaFamily</td> <td>Optional. Default null. Determines if drop should be appliead for specific area family(region). You can get this from AreaManager.AreaFamilies variable.</td>
        </tr>
        <tr>
            <td>faction</td> <td>Character.Factions?</td> <td>Optional. Default nullable. Determines if drop should be appliead for specific faction. You can get this from Character.Factions enum.</td>
        </tr>
        <tr>
            <td>isForBosses</td> <td>bool</td> <td>Optional. Default false. Determines if drop should be appliead for all game bosses and pawns.</td>
        </tr>
        <tr>
            <td>isForBossPawns</td> <td>bool</td> <td>Optional. Default false. Should drop be applied for bosses pawns?</td>
        </tr>
        <tr>
            <td>isForStoryBosses</td> <td>bool</td> <td>Optional. Default false. Should drop be applied for story bosses?</td>
        </tr>
        <tr>
            <td>isForUniqueArenaBosses</td> <td>bool</td> <td>Optional. Default false. Should drop be applied for unique arena bosses?</td>
        </tr>
        <tr>
            <td>isForUniqueEnemies</td> <td>bool</td> <td>Optional. Default false. Should drop be applied for unique enemies?</td>
        </tr>
        <tr>
            <td colspan="2" rowspan="6" align="center">Not Required</td> <td>lootId</td> <td>string</td> <td>Optional. You will need loot id if you planning to remove loot later.</td>
        </tr>
        <tr>
            <td>listExceptIds</td> <td>List&lt;string&gt;</td> <td>Optional. Default null. List of enemy ids that will not receive loot. You can get this from Character.UID.Value .</td>
        </tr>
        <tr>
            <td>listExceptNames</td> <td>List&lt;string&gt;</td> <td>Optional. Default null. List of enemy names that will not receive loot. You can get this from Character.Name .</td>
        </tr>
        <tr>
            <td>minNumberOfDrops</td> <td>int</td> <td>Optional. Default is 1. Determines minimum amout of drops for same provided items(ItemDropChance).</td>
        </tr>
        <tr>
            <td>maxNumberOfDrops</td> <td>int</td> <td>Optional. Default is 1. Determines maximum amout of drops for same provided items(ItemDropChance).</td>
        </tr>
        <tr>
            <td>emptyDropChance</td> <td>int</td> <td>Optional. Default is 0. Defines the percentage chance for a drop to be empty. Used together with 'maxDiceValue'.</td>
        </tr>
    </tbody>
</table>


<table>
    <thead>
        <tr>
            <th colspan="3">
                Parameters used for rules control
            </th>
        </tr>
        <tr>
            <th>Parameter</th> <th>Type</th> <th>Description</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>filePath</td> <td>string</td> <td>Required. Used for loading custom loots from xml file.</td>
        </tr>
        <tr>
            <td>filePath</td> <td>string</td> <td>Required. Used for loading custom loots from xml file.</td>
        </tr>
        <tr>
            <td>lootRuleId</td> <td>string</td> <td>Provides loot rule id.</td>
        </tr>
    </tbody>
</table>

</details>

<details>
    <summary>Known Bugs</summary>
<details>
    <summary>Unique Enemies</summary>
The unique arena boss <i>Grandmother</i> and its pawn enemies, the <i>Kryptia
Warriors</i>, do not drop loot because they never trigger the
<code>Lootable.OnDeath</code> event. Not a priority for me right now to fix it.
</details>
</details>

## Can I find full project example how to use this?

You can view [outward enchantments balancer pack here](https://github.com/GymMed/Outward-Enchantments-Balancer-Pack).

## How to set up

To manually set up, do the following

1. Create the directory: `Outward\BepInEx\plugins\OutwardLootManager\`.
2. Extract the archive into any directory(recommend empty).
3. Move the contents of the plugins\ directory from the archive into the `BepInEx\plugins\OutwardLootManager\` directory you created.
4. It should look like `Outward\BepInEx\plugins\OutwardSceneTester\OutwardLootManager.dll`
   Launch the game.

### If you liked the mod leave a star on [GitHub](https://github.com/GymMed/Outward-Loot-Manager) it's free

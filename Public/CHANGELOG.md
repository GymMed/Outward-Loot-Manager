## Changelog

### Release 0.0.6 Version

#### Fixed

-   Fixed README document by adding <code>maxDiceValue</code> to parameters
    table, added events column for rules control table, fixed load xml example.

### Release 0.0.5 Version

#### Fixed

-   Fixed <code>listOfItemDropChances</code> parameter description. Instead of
    providing type <code>List&lt;string&gt;</code> now it is declared that
    <code>List&lt;ItemDropChance&gt;</code> type should be used.

### Release 0.0.4 Version

#### Fixed

-   Changed Unique enemies validation. Character.UID seems to be not unique
    because most likely game developers copied prefabs instead of using code to
    generate and assign it to enemies. Now to validate <code>isForBosses</code>,
    <code>isForBossPawns</code>, <code>isForStoryBosses</code>,
    <code>isForUniqueArenaBosses</code>, <code>isForUniqueEnemies</code> 
    parameters mod uses
    <code>UID + _ + Area.GetName().Trim().Replace(' ', '_')</code>

### Release 0.0.3 Version

#### Fixed

-   Changed readme descriptions. Parameters fixes.

### Release 0.0.2 Version

#### Fixed

-   Changed readme descriptions. Table fixes and example parameters.

### Release 0.0.1 Version

-   Initial release.

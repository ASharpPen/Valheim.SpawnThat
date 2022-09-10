# Config Format

All `.cfg` from Spawn That (apart from `spawn_that.cfg`) are loaded using a custom parser.

This results in some different behaviour than you would normally see in valheim config files.
- Detection of when settings are missing and when settings are empty.
- Warnings while parsing incorrect config files.
- Faster load time.
- No automatic comments for settings. Gotta go to this wiki now. Sorry.

## Sections
- `[]` are sections, that will contain all the lines following them, until the next section is reached.
    - Eg., The section `SomeSectionName` will get the settings `Line1` and `Line2` assigned to it. 
    ```INI 
    [SomeSectionName]
    Line1 = Value1
    Line2 = Value2

    [SomeOtherSectionName]
    ```

## Section Nesting

Sections can be "nested", by adding dots (`.`) to the section name. This will indicate the hierarchy they are in.
This is also how Spawn That makes lists.

Eg.,
```INI 
[TopLevel]
Line1 = Value1

[TopLevel.LowerLevel]
Line1 = Value1
```
Means that TopLevel will have a single line, but also an extra section associated, which has its own line.

Whether or not a nested section can be used, and which names are valid, depends on the specic configuration file in question.

## Section lines

Section lines always consist of a setting name, followed by a value. 
Eg.,

```toml
Setting1 = SettingValue`
```

If a setting is *not* added, Spawn That will select a value for that setting in the following order:

- Other configurations (eg. ones done by other mods), if any are present.
- The original value for the entity being modified, if any is being modified.
- The default value for that setting 

If a setting value is *not* specified, but the setting is:
    
```toml
Setting1 = 
```

Spawn That will read that as a desire to revert existing configurations to the original.
Eg., if a mod is configuring Spawn That to have increased spawn time, adding a corresponding line without setting the value, will make Spawn That remove that increased spawn time and use the original. 

## Decimal Numbers

Decimal numbers are always expected separated by `.`.
That means 1/10 should be written as `0.1`.

## Comments

Comment lines can be added by starting with any of the following identifiers:
- `//`
- `#` 
- `--` 

Whitespace before it is ignored.

Eg., 
```toml
    # Some Comment
Setting=Value
```

Lines starting with any of these identifiers are skipped during parsing.
Comments in same line as normal settings is not supported.

Eg., ```Setting1=Value # Some Comment``` will attempt to read the setting as `Value # Some Comment`, and not just `Value`.


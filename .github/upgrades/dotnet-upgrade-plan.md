# .NET 8 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 8 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 8 upgrade.
3. Upgrade Encryption Demo\Encryption Demo.csproj
4. Upgrade Player Demo\Player Demo.csproj

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name                                   | Description                 |
|:-----------------------------------------------|:---------------------------:|

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                        | Current Version | New Version | Description                         |
|:------------------------------------|:---------------:|:-----------:|:------------------------------------|

### Project upgrade details
This section contains details about each project upgrade and modifications that need to be done in the project.

#### Encryption Demo\Encryption Demo.csproj modifications

Project properties changes:
  - Target framework should be changed from `net48` to `net8.0-windows`

NuGet packages changes:
  - <none>

Feature upgrades:
  - Convert project file to SDK-style format (ruleId Project.0001)

Other changes:
  - Review and update any Windows-specific APIs if necessary for net8.0-windows

#### Player Demo\Player Demo.csproj modifications

Project properties changes:
  - Target framework should be changed from `net48` to `net8.0-windows`

NuGet packages changes:
  - <none>

Feature upgrades:
  - Convert project file to SDK-style format (ruleId Project.0001)

Other changes:
  - Review and update any Windows-specific APIs if necessary for net8.0-windows

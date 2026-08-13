param(
    [string] $ServerInstance = "localhost",
    [string] $DatabaseName = "SelqiDb",
    [string] $User,
    [string] $Password = $env:SELQI_DB_PASSWORD
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$createScript = Join-Path $repoRoot "database\000_create_database.sql"
$migrationScript = Join-Path $repoRoot "database\migrations\001_complete_schema_and_seed.sql"

if ($User -and -not $Password) {
    throw "Set SELQI_DB_PASSWORD or pass -Password when using SQL authentication."
}

function Invoke-SqlcmdFile {
    param(
        [string] $ScriptPath,
        [string] $Database
    )

    $arguments = @("-S", $ServerInstance, "-i", $ScriptPath, "-b", "-C")

    if ($Database) {
        $arguments += @("-d", $Database)
    }

    if ($User -and $Password) {
        $arguments += @("-U", $User, "-P", $Password)
    }
    else {
        $arguments += "-E"
    }

    if ($ScriptPath -eq $createScript) {
        $arguments += @("-v", "DatabaseName=$DatabaseName")
    }

    & sqlcmd @arguments
}

Invoke-SqlcmdFile -ScriptPath $createScript
Invoke-SqlcmdFile -ScriptPath $migrationScript -Database $DatabaseName

Write-Host "Selqi database '$DatabaseName' is initialized on '$ServerInstance'."

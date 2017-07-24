function __exec($cmd) {
  $cmdName = [IO.Path]::GetFileName($cmd)
  Write-Host -ForegroundColor Cyan "> $cmdName $args"
  $originalErrorPref = $ErrorActionPreference
  $ErrorActionPreference = 'Continue'
  & $cmd @args
  $exitCode = $LASTEXITCODE
  $ErrorActionPreference = $originalErrorPref
  if($exitCode -ne 0) {
      throw "'$cmdName $args' failed with exit code: $exitCode"
  }
}

__exec dotnet pack .\src\YoloDev.AspNetCore.Assets\YoloDev.AspNetCore.Assets.csproj --include-symbols

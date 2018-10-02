# Taken from psake https://github.com/psake/psake

<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}


if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

exec { & dotnet restore }

$tag = $(git tag -l --points-at HEAD)
$revision = @{ $true = "{0:00000}" -f [convert]::ToInt32("0" + $env:APPVEYOR_BUILD_NUMBER, 10); $false = "local" }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$suffix = @{ $true = ""; $false = "ci-$revision"}[$tag -ne $NULL -and $revision -ne "local"]
$commitHash = $(git rev-parse --short HEAD)
$buildSuffix = @{ $true = "$($suffix)-$($commitHash)"; $false = "$($branch)-$($commitHash)" }[$suffix -ne ""]

echo "build: Tag is $tag"
echo "build: Package version suffix is $suffix"
echo "build: Build version suffix is $buildSuffix" 


exec { & dotnet build tvision2.sln -c Release --version-suffix=$buildSuffix -v q /nologo }


#echo "running tests"

#try {
#
#	Push-Location -Path .\tests
#	exec { & dotnet test }
#} finally {
#	Pop-Location
#}


try {
    Push-Location -Path .\src
    if ($suffix -eq "") {
        exec { & dotnet pack .\Tvision2.ConsoleDriver\Tvision2.ConsoleDriver.csproj -c Release -o ..\..\artifacts --include-symbols --no-build }
        exec { & dotnet pack .\Tvision2.Controls\Tvision2.Controls.csproj -c Release -o ..\..\artifacts --include-symbols --no-build }
        exec { & dotnet pack .\Tvision2.Controls.Styles.Mc\Tvision2.Controls.Styles.Mc.csproj -c Release -o ..\..\artifacts --include-symbols --no-build }
        exec { & dotnet pack .\Tvision2.Core\Tvision2.Core.csproj -c Release -o ..\..\artifacts --include-symbols --no-build }
        exec { & dotnet pack .\Tvision2.Debug\Tvision2.Debug.csproj -c Release -o ..\..\artifacts --include-symbols --no-build }
        exec { & dotnet pack .\Tvision2.Layouts\Tvision2.Layouts.csproj -c Release -o ..\..\artifacts --include-symbols --no-build }
        exec { & dotnet pack .\Tvision2.Statex\Tvision2.Statex.csproj -c Release -o ..\..\artifacts --include-symbols --no-build }
        exec { & dotnet pack .\Tvision2.Statex.Controls\Tvision2.Statex.Controls.csproj -c Release -o ..\..\artifacts --include-symbols --no-build }
        exec { & dotnet pack .\Tvision2.Viewports\Tvision2.Viewports.csproj -c Release -o ..\..\artifacts --include-symbols --no-build }
        exec { & dotnet pack .\Tvision2.Dialogs\Tvision2.Dialogs.csproj -c Release -o ..\..\artifacts --include-symbols --no-build }
    } else {
        exec { & dotnet pack .\Tvision2.ConsoleDriver\Tvision2.ConsoleDriver.csproj -c Release -o ..\..\artifacts --include-symbols --no-build -version-suffix=$suffix  }
        exec { & dotnet pack .\Tvision2.Controls\Tvision2.Controls.csproj -c Release -o ..\..\artifacts --include-symbols --no-build -version-suffix=$suffix }
        exec { & dotnet pack .\Tvision2.Controls.Styles.Mc\Tvision2.Controls.Styles.Mc.csproj -c Release -o ..\..\artifacts --include-symbols --no-build -version-suffix=$suffix }
        exec { & dotnet pack .\Tvision2.Core\Tvision2.Core.csproj -c Release -o ..\..\artifacts --include-symbols --no-build -version-suffix=$suffix }
        exec { & dotnet pack .\Tvision2.Debug\Tvision2.Debug.csproj -c Release -o ..\..\artifacts --include-symbols --no-build -version-suffix=$suffix }
        exec { & dotnet pack .\Tvision2.Layouts\Tvision2.Layouts.csproj -c Release -o ..\..\artifacts --include-symbols --no-build -version-suffix=$suffix }
        exec { & dotnet pack .\Tvision2.Statex\Tvision2.Statex.csproj -c Release -o ..\..\artifacts --include-symbols --no-build -version-suffix=$suffix }
        exec { & dotnet pack .\Tvision2.Statex.Controls\Tvision2.Statex.Controls.csproj -c Release -o ..\..\artifacts --include-symbols --no-build -version-suffix=$suffix }
        exec { & dotnet pack .\Tvision2.Viewports\Tvision2.Viewports.csproj -c Release -o ..\..\artifacts --include-symbols --no-build -version-suffix=$suffix }
        exec { & dotnet pack .\Tvision2.Dialogs\Tvision2.Dialogs.csproj -c Release -o ..\..\artifacts --include-symbols --no-build -version-suffix=$suffix }
    }
}
finally {
    Pop-Location
}


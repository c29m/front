# This script was derived from the Rhino.Mocks buildscript written by Ayende Rahien.
include .\psake_ext.ps1

properties {
    $config = 'debug'
    $showtestresult = $FALSE
    $base_dir = resolve-path .
    $lib_dir = "$base_dir\lib\"
    $build_dir = "$base_dir\build\" 
    $release_dir = "$base_dir\release\"
    $source_dir = "$base_dir"
    $version = Get-Git-Version
}

task default -depends Release

task Clean {
    remove-item -force -recurse $build_dir -ErrorAction SilentlyContinue 
    remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue 
}

task Init -depends Clean {
    Write-Host $version
    Generate-Assembly-Info `
        -file "$source_dir\front.Core\Properties\AssemblyInfo.cs" `
        -title "front $version" `
        -description "CommonJS transport for .net" `
        -company "KBK" `
        -product "front" `
        -version $version `
        -copyright "Copyright © kbk 2011" `
        -partial $True
    Generate-Assembly-Info `
        -file "$source_dir\front.Tests\Properties\AssemblyInfo.cs" `
        -title "front Tests $version" `
        -description "CommonJS transport for .net" `
        -company "KBK" `
        -product "front" `
        -version $version `
        -copyright "Copyright © kbk 2011"
    Generate-Assembly-Info `
        -file "$source_dir\front.Web\Properties\AssemblyInfo.cs" `
        -title "front example web $version" `
        -description "CommonJS transport for .net" `
        -company "KBK" `
        -product "front" `
        -version $version `
        -copyright "Copyright © kbk 2011"
        
    new-item $build_dir -itemType directory
    new-item $release_dir -itemType directory
    
}

task Build -depends Init {
    msbuild $source_dir\front.Core\front.Core.csproj /p:OutDir=$build_dir /p:Configuration=$config
    if ($lastExitCode -ne 0) {
        throw "Error: compile failed"
    }
}

task Test -depends Build {
    msbuild $source_dir\front.Tests\front.Tests.csproj /p:OutDir=$build_dir /p:Configuration=$config
    if ($lastExitCode -ne 0) {
        throw "Error: Test compile failed"
    }
    $old = pwd
    cd $build_dir
    & $lib_dir\NUnit\nunit-console-x86.exe $build_dir\front.Tests.dll 
    if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute tests"
        if ($showtestresult)
        {
            start $build_dir\TestResult.xml
        }
    }
    cd $old
}

task Merge -depends Build {
    $old = pwd
    cd $build_dir
    
    $filename = "front.dll"
    Remove-Item $filename-partial.dll -ErrorAction SilentlyContinue
    Rename-Item $filename $filename-partial.dll
    & $lib_dir\ilmerge\ILMerge.exe $filename-partial.dll `
        front.Core.dll `
        Microsoft.Practices.ServiceLocation.dll `
        /out:$filename `
        /internalize `
        /keyfile:../kbk-open-source.snk `
        /t:exe
    if ($lastExitCode -ne 0) {
        throw "Error: Failed to merge compiler assemblies"
    }
	
	cd $old
}

task Release-NoTest -depends Build {
    $commit = Get-Git-Commit
    $filename = "front.core"
    & $lib_dir\7zip\7z.exe a $release_dir\front-$commit.zip `
    $build_dir\$filename.dll `
    $build_dir\$filename.pdb `
    #$build_dir\Testresult.xml `
    
    
    Write-Host -ForegroundColor Yellow "Please note that no tests where run during release process!"
    Write-host "-----------------------------"
    Write-Host "front $version was successfully compiled and packaged."
    Write-Host "The release bits can be found in ""$release_dir"""
    Write-Host -ForegroundColor Cyan "Thank you for using front!"
}

task Release -depends NuGetPackage {
    $commit = Get-Git-Commit
    $filename = "front.core"
    & $lib_dir\7zip\7z.exe a $release_dir\front-$commit.zip `
    $build_dir\$filename.dll `
    $build_dir\$filename.pdb `
    
    Move-Item $build_dir\*.nupkg $release_dir\
    
    
    Write-host "-----------------------------"
    Write-Host "dfront $version was successfully compiled and packaged."
    Write-Host "The release bits can be found in ""$release_dir"""
    Write-Host -ForegroundColor Cyan "Thank you for using front!"
}


task NuGetPackage -depends Build {
    $target = "$build_dir\NuGet\"
    remove-item -force -recurse $target -ErrorAction SilentlyContinue     
    New-Item $target -ItemType directory
    New-Item $target\lib -ItemType directory
    New-Item $target\tool -ItemType directory
    New-Item $target\content -ItemType directory
    
    Copy-Item $source_dir\front.nuspec $target
    #Copy-Item $source_dir\web.config.transform $target\content\
    Copy-Item $build_dir\front.Core.dll $target\lib\
        
    .\lib\NuGet.exe pack $target\front.nuspec -o $build_dir
}
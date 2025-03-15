dotnet new uninstall "Feature-Full"
dotnet new uninstall "Feature-NoRequest"
dotnet new uninstall "Feature-NoResponse"

dotnet new install "Feature-Full"
dotnet new install "Feature-NoRequest"
dotnet new install "Feature-NoResponse"

Write-Host "All feature templates installed successfully!"
Read-Host "Press Enter to exit"
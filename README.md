# Shopping Cart
* A simple shopping cart API. It includes a rest api to interact with shopping carts and items.
* A proxy api framework to communicate with the api from C# applications
* A DesktopClient to showcase how to use the proxy framework.

# Run
Run xUnit Tests:

`dotnet build && ( cd ./src/Shopping.UnitTest/ && dotnet xunit )`

Run WebApi:

`dotnet build && (cd ./src/Shopping.Webapi/ && dotnet run)`

Run DesktopClient:

`dotnet build && (cd ./src/Shopping.DesktopClient/ && dotnet run)`

# TODO
* Unit tests for proxy project
* Cleanup ProxyHttpClient
* Extract configs for project to appsettings.json
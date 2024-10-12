
dotnet-ef database drop -f -c AccountsDbContext -p .\src\Accounts\AnimalAllies.Accounts.Infrastructure\ -s .\src\AnimalAllies.Web\
dotnet-ef database drop -f -c WriteDbContext -p .\src\PetManagement\AnimalAllies.Volunteer.Infrastructure\ -s .\src\AnimalAllies.Web\
dotnet-ef database drop -f -c WriteDbContext -p .\src\BreedManagement\AnimalAllies.Species.Infrastructure\ -s .\src\AnimalAllies.Web\


dotnet-ef migrations remove -c AccountsDbContext -p .\src\Accounts\AnimalAllies.Accounts.Infrastructure\ -s .\src\AnimalAllies.Web\
dotnet-ef migrations remove -c WriteDbContext -p .\src\PetManagement\AnimalAllies.Volunteer.Infrastructure\ -s .\src\AnimalAllies.Web\
dotnet-ef migrations remove -c WriteDbContext -p .\src\BreedManagement\AnimalAllies.Species.Infrastructure\ -s .\src\AnimalAllies.Web\


dotnet-ef migrations add init -c AccountsDbContext -p .\src\Accounts\AnimalAllies.Accounts.Infrastructure\ -s .\src\AnimalAllies.Web\
dotnet-ef migrations add init -c WriteDbContext -p .\src\PetManagement\AnimalAllies.Volunteer.Infrastructure\ -s .\src\AnimalAllies.Web\
dotnet-ef migrations add init -c WriteDbContext -p .\src\BreedManagement\AnimalAllies.Species.Infrastructure\ -s .\src\AnimalAllies.Web\

dotnet-ef database update -c AccountsDbContext -p .\src\Accounts\AnimalAllies.Accounts.Infrastructure\ -s .\src\AnimalAllies.Web\
dotnet-ef database update -c WriteDbContext -p .\src\PetManagement\AnimalAllies.Volunteer.Infrastructure\ -s .\src\AnimalAllies.Web\
dotnet-ef database update -c WriteDbContext -p .\src\BreedManagement\AnimalAllies.Species.Infrastructure\ -s .\src\AnimalAllies.Web\
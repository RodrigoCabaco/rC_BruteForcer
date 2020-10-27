read -p "Username:" user
read -p "Password List:" list
read -p "Threads:" threads 
dotnet run $user $list twitter $threads
#!/bin/bash

[ -d Luban ] && rm -rf Luban

dotnet build ../Luban_Plugins/Luban.Plugins/Luban.Runner/Luban.Runner.csproj -c Release -o Luban

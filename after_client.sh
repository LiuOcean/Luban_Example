#!/bin/bash

[ -f C#/Client/ALocalizeConfig.cs ] && rm C#/Client/ALocalizeConfig.cs
rm -rf C#/Client/LocalizeConfig_*

rm -rf Unity_Example/Assets/Scripts/Model/Config/Gen/*
rm -rf Unity_Example/Assets/Res/Configs/*

cp -rf C#/Client/* Unity_Example/Assets/Scripts/Model/Config/Gen/
cp -rf Json/Client/* Unity_Example/Assets/Res/Configs/
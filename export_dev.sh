#!/bin/bash

PAUSE=${1:-1}

cp -rf .githooks/pre-commit .git/hooks

GEN_CLIENT=Tools/Luban.ClientServer/Luban.ClientServer.dll

dotnet ${GEN_CLIENT} -t Tpls -j cfg --\
 -d Data/Defines/Root.xml \
 --input_data_dir Data/Excels \
 --output_data_dir Json/Server \
 --output_code_dir C#/Server \
 --gen_types code_cs_dotnet_json,data_json2 \
 --naming_convention:bean_member under_scores \
 --cs:use_unity_vector \
 --output:exclude_tags test \
 -s server

if [ $? -ne 0 ]; then
    echo "生成失败, 请检查错误"
	[ $PAUSE -eq 1 ] && read
    exit 1
fi

dotnet ${GEN_CLIENT} -t Tpls -j cfg --\
 -d Data/Defines/Root.xml \
 --input_data_dir Data/Excels \
 --output_data_dir Json/Client \
 --output_code_dir C#/Client \
 --gen_types code_cs_unity_json,data_json2,data_resources \
 --naming_convention:bean_member under_scores \
 --cs:use_unity_vector \
 --validate_root_dir Unity_Example/ \
 --output:data:resource_list_file Json/resources.txt \
 --output:exclude_tags test \
 --l10n:input_text_files Data/Excels/I18N/LocalizeConfig.csv \
 --l10n:text_field_name text_cn \
 --l10n:output_not_translated_text_file Json/NotLocalized.txt \
 -s client 

if [ $? -ne 0 ]; then
    echo "生成失败, 请检查错误"
	[ $PAUSE -eq 1 ] && read
    exit 1
fi

[ -f C#/Client/ALocalizeConfig.cs ] && rm C#/Client/ALocalizeConfig.cs
rm -rf C#/Client/LocalizeConfig_*

rm -rf Unity_Example/Assets/Scripts/Model/Config/Gen/*
rm -rf Unity_Example/Assets/Res/Configs/*

cp -rf C#/Client/* Unity_Example/Assets/Scripts/Model/Config/Gen/
cp -rf Json/Client/* Unity_Example/Assets/Res/Configs/

[ $PAUSE -eq 1 ] && echo "输入 Enter 继续" && read

exit 0

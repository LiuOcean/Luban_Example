#!/bin/bash

cp -rf .githooks/pre-commit .git/hooks

GEN_CLIENT=Tools/Luban.ClientServer/Luban.ClientServer.dll

dotnet ${GEN_CLIENT} -t Tpls -j cfg -w Data/Excels --\
 -d Data/Defines/Root.xml \
 --input_data_dir Data/Excels \
 --output_data_dir Unity_Example/Assets/Res/Configs \
 --gen_types data_json2 \
 --naming_convention:bean_member under_scores \
 --cs:use_unity_vector \
 --validate_root_dir Unity_Example/ \
 -s client 

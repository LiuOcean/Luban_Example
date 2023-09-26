#!/bin/bash

PAUSE=${1:-1}

cp -rf .githooks/pre-commit .git/hooks

GEN_CLIENT=Luban/Luban.Runner.dll

[ -d C#/Server ] && rm -rf C#/Server

dotnet $GEN_CLIENT \
    --conf luban.conf \
    -t server \
    -c cs-simple-json \
    -d newtonjson \
    -i test \
    --customTemplateDir Tpls \
    --validationFailAsError \
    -x outputCodeDir=C#/Server \
    -x outputDataDir=Json/Server \
    -x newtonsoft.dll=Example

./after_server.sh

if [ $? -ne 0 ]; then
    echo "生成失败, 请检查错误"
	[ $PAUSE -eq 1 ] && read
    exit 1
fi

dotnet $GEN_CLIENT \
    --conf luban.conf \
    -t client \
    -c cs-simple-json \
    -d newtonjson \
    -i test \
    --customTemplateDir Tpls \
    --validationFailAsError \
    -x outputCodeDir=C#/Client \
    -x outputDataDir=Json/Client \
    -x newtonsoft.dll=Example

./after_client.sh

[ $PAUSE -eq 1 ] && echo "输入 Enter 继续" && read

exit 0
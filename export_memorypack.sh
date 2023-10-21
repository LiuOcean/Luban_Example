#!/bin/bash

PAUSE=${1:-1}

cp -rf .githooks/pre-commit .git/hooks

GEN_CLIENT=Luban/Luban.Runner.dll

dotnet $GEN_CLIENT \
    --conf luban.conf \
    -t client \
    -c cs-memorypack \
    -d memorypack \
    -e test \
    --customTemplateDir Tpls \
    --validationFailAsError \
    -x outputCodeDir=C#/Client \
    -x outputDataDir=Json/Client \
    -x newtonsoft.dll=Model\
    -x newtonsoft.namespace=Example \
    -x i18n.dir=I18N
    
./after_client.sh

[ $PAUSE -eq 1 ] && echo "输入 Enter 继续" && read

exit 0

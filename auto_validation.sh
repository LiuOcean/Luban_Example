#!/bin/bash

GEN_CLIENT=Luban/Luban.Runner.dll

dotnet $GEN_CLIENT \
    --conf luban.conf \
    -t client \
    -f \
    --validationFailAsError \
    -e test

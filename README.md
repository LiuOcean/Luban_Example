# Luban_Example

本仓库是 [Luban](https://github.com/focus-creative-games/luban) 的示例

[简单版文档](https://app.heptabase.com/w/514c9827e9627b063281903b68ed662773c45c845d90f8da1da04dd1e6fc08c4)
[Luban 官方文档](https://focus-creative-games.github.io/lubandoc/)

## 功能介绍

- 基于 `json` 的配表，使用 `NewtonSoft`
- 配表内存加密
- 本地化
- 与 [ET](https://github.com/egametang/ET) 中 `ConfigComponent` 风格一致
    - 但是剔除了 `ET` 相关的代码，如果希望使用 `ET` 需要自己修改继承 `Entity`
- 支持 `Map` 表，单例表，多主键表的自动生成
- 支持 `ref` 类型的代码生成及自动绑定
- 异步使用的是 `UniTask` 
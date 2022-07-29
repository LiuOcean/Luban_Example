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

## 有效性验证

在每个导出的脚步中， 都会自动将 `.githooks/pre-commit` 文件强制复制到 `.git/hooks` 文件下（hook 想要生效，至少需要运行一次导出脚本）

> 虽然 git 有直接修改 hookpath 的指令，但是经过测试，在部分 GUI 下这个是不工作的

`pre-commit` 文件中，首先会运行 `auto_validation.sh` 脚本，记录 `--generateonly` 中所有的日志，并对 "不存在" 和 "被过滤了" 关键字进行统计，如果存在任意错误，就会拒绝本次 commit

> "不存在" 和 "被过滤了" 主要为了解决，ref 关键字 ID 为空，path 关键字路径不存在，以及 `--output:exclude_tags` 中引用了如 `test` 下的 ID 的有效性验证

对于实际开发过程中，一些复杂的业务逻辑配表验证是 luban 现有的校验器无法校验的，此时 `pre-commit` 的第二步就会运行 `unit_test_client.sh` 脚本，对项目特化的逻辑进行单元测试

> 比如说，新手引导中可能会出现某个英雄相关的操作，不知道哪个策划手欠，给这个英雄 ID 给改了，直接导致新手引导裂开

基于上面两个防御手段，我们可以做到只要配表能 push 上去，就一定是对的

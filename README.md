# Luban_Example

[![license](http://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/MIT)

本仓库是 [Luban](https://github.com/focus-creative-games/luban) 的示例

详细文档可查看 [官方文档](https://luban.doc.code-philosophy.com/docs/intro)

## Luban.Plugins

当前仓库使用的 Luban 为 [Luban_Plugins](https://github.com/LiuOcean/Luban_Plugins) 请注意区分

主要对 Newtonsoft 在面对多态时的反序列化增加了对应的适配, 以及在面对被裁剪的数据行, 比如标记为 `test` 的行, 裁剪时标记为错误

## 功能介绍

- 基于 `json` 的配表，使用 `NewtonSoft`
- 配表内存加密
- 本地化
- 与 [ET](https://github.com/egametang/ET) 中 `ConfigComponent` 风格一致
    - 但是剔除了 `ET` 相关的代码，如果希望使用 `ET` 需要自己修改继承 `Entity`
- 支持 `Map` 表，单例表，多主键表的自动生成
- 支持 `ref` 类型的代码生成及自动绑定
- 异步使用的是 `UniTask` 

## 示例内容

使用 unity 打开 `Unity_Example` 目录，选择 `Scenes/SampleScene` 场景，运行即可查看配表示例

> 示例的代码均写在 `UsageExample` 脚本中

- YooAsset 初始化
- 加载配表
- 加载本地化
- 单例表示例
- 多主键表示例
- ref 绑定示例
- 本地化使用示例

## 有效性验证

在每个导出的脚步中， 都会自动将 `.githooks/pre-commit` 文件强制复制到 `.git/hooks` 文件下（hook 想要生效，至少需要运行一次导出脚本）

> 虽然 git 有直接修改 hookpath 的指令，但是经过测试，在部分 GUI 下这个是不工作的

`pre-commit` 文件中，会运行 `auto_validation.sh` 脚本，如果存在任意错误，就会拒绝本次 commit

对于实际开发过程中，一些复杂的业务逻辑配表验证是 luban 现有的校验器无法校验的，此时 `pre-commit` 的第二步就会运行 `unit_test_client.sh` 脚本，对项目特化的逻辑进行单元测试

> 比如说，新手引导中可能会出现某个英雄相关的操作，不知道哪个策划手欠，给这个英雄 ID 给改了，直接导致新手引导裂开

基于上面两个防御手段，我们可以做到只要配表能 push 上去，就一定是对的

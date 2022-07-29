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

### 有效性验证

示例中，为了解决策划胡乱推一个错误的配置到仓库，增加了对应的 git hook，因为部分错误在 luban 中是警告，不可以使用 `[ $? -ne 0]` 来判断是否存在错误，所以需要对 luban 生成的日志进行过滤

但在实际开发过程中，总会有一些复杂的业务逻辑配表验证是 luban 现有的校验器无法校验的，此时就必须通过单元测试完成，每次 git commit 时，都会运行 `auto_alivation.sh` 和 `unit_test_client.sh`

基于上面两个防御手段，我们可以做到只要配表能 push 上去，就一定是对的，甚至可以在公司内部说明，只要配表推上去有问题，那就一定是设计这个表的程序没有考虑清楚问题
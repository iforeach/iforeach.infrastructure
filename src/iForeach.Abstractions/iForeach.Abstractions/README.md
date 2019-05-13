### iForeach.Abstractions

#### 代码参考
* github 项目：[OrchardCore](https://github.com/OrchardCMS/OrchardCore)
* github 路径：[OrchardCore.Abstractions](https://github.com/OrchardCMS/OrchardCore/src/OrchardCore/OrchardCore.Abstractions)

### 基础类库
* netstandard：2.1+

### iForeach 修订
1. Abstractions化：除基础类库外，应该仅引用类 Abstractions 模块
2. 单一第三方模块引用：非必要仅引用以下第三方模块：
   1. 首选：Microsoft.Extensions.**，version：3.0 +
   2. 次选：Microsoft.AspNetCore.**，version：2.2 +
   3. 必要：Newtonsoft.Json，version：12.02 +
3. 增加命名服务 / 键化服务的支持：
   1. 命名服务： 
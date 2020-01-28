# Matrix UWP

A UWP Client for Matrix Course System.

## Environment
- Windows 10 1903+
- Windows 10 SDK 18362
- Visual Studio / Visual Studio Build Tools

## Build
开发版本构建:
```
msbuild MatrixUWP.sln /t:restore /m
msbuild MatrixUWP.sln /p:Configuration=Debug /p:AppxBundlePlatforms="x86|x64|ARM" /p:AppxPackageDir="AppxPackages" /p:AppxBundle=Always /p:UapAppxPackageBuildMode=StoreUpload /m /verbosity:m
```
生产版本构建 (含 .NET Native AOT 编译，因此编译速度很慢):
```
msbuild MatrixUWP.sln /t:restore /m
msbuild MatrixUWP.sln /p:Configuration=Release /p:AppxBundlePlatforms="x86|x64|ARM|ARM64" /p:AppxPackageDir="AppxPackages" /p:AppxBundle=Always /p:UapAppxPackageBuildMode=StoreUpload /m /verbosity:m
```

## Infrastructure
### Null-safety
Null 安全默认是不启用的，因此需要手动 opt-in，由于 UWP 目前不支持项目级别的 opt-in，因此需要在每个 `.cs` 文件头部添加一行 `#nullable enable`。  
### HTTP requests
全局使用同一个 `HttpClient` `AppModel.MatrixHttpClient`，因为在 `HttpClient` 中保存有 `cookies` 信息。  
所有的请求响应可使用 `ResponseExtensions` 中定义的扩展方法 `JsonAsync<T>`, `TextAsync`, `BlobAsync` 解析数据。  
对于响应可以使用 `ResponseModel<T>` 来进行包装，其中 `T` 为响应数据类型。  
一个请求例子：
```csharp
public static async ValueTask<ResponseModel<CourseInfoModel>> FetchCourseAsync(int courseId) =>
    await AppModel.MatrixHttpClient.GetAsync($"/api/courses/{courseId}")
        .JsonAsync<ResponseModel<CourseInfoModel>>();
```
### UserModel
用于用户数据和状态的保存和更新，通过 `UserModel` 中的 `CurrentUser` 和 `UpdateUserData` 来实现。

- `CurrentUser`: 用户数据，只读，该对象为单例对象，全局共用一个，对其中属性任何的更改都会触发 UI 变更通知
- `UpdateUserData`: 更新用户数据的委托，当需要替换原有的全局 `CurrentUser` 对象时调用此委托可更新全局的用户状态

### AppModel
用于应用数据和状态的保存和更新，如资源、主题、设置等。  
除此之外还提供了两个方法：

- `ShowMessage`: 用于在界面下方显示一段消息
- `NavigateToPage`: 导航到页面，参数分别是：目标的页面类型、`Parameter` 数据、页面导航动画信息 (可为 null)，具体使用参考下方关于页面导航的说明。

### Page navigation
页面之间的导航通过 `Layout.xaml` 中的 `Frame` 实现。  
其中每个页面都有一个虚方法 `protected virtual void OnNavigatedTo(NavigationEventArgs e)`，`e` 中包含了导航页面时传递的参数信息 `Parameter`，该方法会在页面被导航且在 `Load` 事件之前被调用。  
除此之外，还有 `protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)`，该方法在当前页面即将被切出时被调用， `e` 中包含目标页面的类型 `SourcePageType`。  
  
手动导航时，可调用 `parameter` 的 `NavigateToPage` 委托，并传入相关参数即可。  

例如从 `A` 页面导航到 `B` 页面：  
B 页面参数类型 ParametersB.cs
```csharp
class ParametersB
{
    public int CourseId { get; set; }
}
```

A 页面逻辑代码 PageA.xaml.cs
```csharp
private void Navigate_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e) // 点击某个按钮的事件处理函数
{
    parameters?.NavigateToPage(typeof(PageB), new ParametersB { CourseId = 123 }, null);
}
```

B 页面逻辑代码 PageB.xaml.cs
```csharp
private ParametersB? parameters;
protected override void OnNavigatedTo(NavigationEventArgs e)
{
    if (e.Parameter is ParameterB para) parameters = para; // 保存参数信息，以供后续使用
    Debug.WriteLine(para.CourseId); // 输出 123
}
```

如果需要保存该页面状态，即切换回该页面时为上一次加载该页面的状态，可以在该页面类型的构造函数第一行添加 `NavigationCacheMode = NavigationCacheMode.Required`，这样该页面的状态将会被缓存，下一次再切回该页面时将会恢复之前的状态。

### Data binding
如果可以不使用 `Binding`，一律使用 `x:Bind` 做强类型的数据绑定。  
绑定方式有三种，通过 `Mode` 指定，默认为 `OneTime`：
- `OneTime`：一次性绑定
- `OneWay`：单向绑定
- `TwoWay`：双向绑定

### Data conversion
如果需要进行数据转换，可以在 `Conveters` 下定义转换器，然后在 `App.xaml` 中将该转换器添加到资源字典，然后即可使用。  
例如一个对布尔值取反的转换器：  

NotConverter.cs
```csharp
class NotConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool b) return !b;
        throw new InvalidCastException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is bool b) return !b;
        throw new InvalidCastException();
    }
}
```

App.xaml
```xml
<cvt:NotConverter x:Key="NotConverter"/>
```

使用
```xml
<CheckBox IsChecked="{x:Bind isChecked, Converter={StaticResource NotConverter}}" />
```
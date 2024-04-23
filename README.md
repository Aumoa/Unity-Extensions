# Unity에서 사용 가능한 각종 플러그인을 포함합니다.

## Ayla.Inspector

Ayla.Inspector는 유니티의 Inspector 환경을 개선해줍니다. 여기에는 다음 기능들이 포함됩니다.

### NonSerialized 필드/프로퍼티/메서드에 대한 Inspector 드로잉 지원

기본적으로 NonSerialized 프로퍼티는 Inspector에 표시되지 않습니다. 하지만 Ayla Inspector를 이용하면 이를 Inspector에 표시하고, 편집할 수 있습니다.

![image](https://github.com/Aumoa/Unity-Extensions/assets/58451385/c515484e-a75c-4119-b9fd-9f08cd1d0d40)

ShowNativeMember는 Serialize 대상에 포함되지 않은 필드, 프로퍼티, 메서드에 대한 Inspector 표시를 제공합니다. 심지어 필드 또는 프로퍼티가 값 변경을 지원한다면 Inspector에서 편집할 수도 있습니다. 물론, 이는 Serialize 되어 애셋에 저장되지 않으므로 편집된 내용은 어셈블리 리로드 또는 인스턴스 재생성 시 휘발됩니다.

![image](https://github.com/Aumoa/Unity-Extensions/assets/58451385/413cefb8-498e-44fc-bb03-9e12e2cb40c9)

ShowNativeMember로 표시되는 개체에 대한 Drawer를 재정의할 경우 NativePropertyDrawer를 사용하세요. 이 규칙은 동일하게 모든 경우에 적용됩니다. 즉, 필드, 프로퍼티 및 메서드에 대해 동일한 규칙으로 작성할 수 있습니다. 단, List 개체에 대해 ShowNativeMember는 Reorder 등, 리스트의 모든 기능을 지원하지는 않습니다.

![image](https://github.com/Aumoa/Unity-Extensions/assets/58451385/730f3279-2f18-4f44-a000-e6d09f1b23ff)

물론, 프로퍼티가 SerializeField일 경우 모든 List의 기능을 정상 지원합니다.

![image](https://github.com/Aumoa/Unity-Extensions/assets/58451385/8b312775-e5cb-4648-8ed6-56d65c5df2b0)

### 다양한 Attribute에 대한 동일한 경험 제공

Ayla.Inspector에서는 모든 Attribute가 SerializedField, NonSerialize 뿐만 아니라, 메서드, 심지어는 클래스에도 작동합니다.

![image](https://github.com/Aumoa/Unity-Extensions/assets/58451385/eb1d4d8d-bc61-410d-be76-16d1e5a5467f)

Decorator 뿐만 아닌, 각종 Meta Attribute도 작동하는 것을 확인할 수 있습니다.

![image](https://github.com/Aumoa/Unity-Extensions/assets/58451385/96f6aff5-14e9-4ff0-919c-6d849a2bab08)

### 쉽게 커스터마이즈 가능한 CustomInspector 제공

CustomInspectorAttribute를 사용하면 특정 함수가 Custom Inspector 드로잉을 제공하도록 구현할 수 있습니다.

![image](https://github.com/Aumoa/Unity-Extensions/assets/58451385/a12016a9-9830-4021-8ca7-6bf4bfc749b6)

여기에도 마찬가지로, 기본 Meta 등 모든 것을 사용할 수 있습니다. Custom Inspector 함수 내에서는 EditorGUILayout 대신 CustomInspectorLayout을 사용합니다. 이 CustomInspectorLayout 클래스는 비-렌더링 모드에서는 레이아웃의 크기만을 조사하며, 렌더링 모드에서는 레이아웃을 그립니다.

CustomInspectorLayout을 사용하는 대신 EditorGUI 및 position을 직접 사용하려면 CustomInspectorLayout.Space를 사용하세요. 함수의 첫 번째 매개변수로 (Rect position)을 입력할 경우, 현재 렌더링 레이아웃의 사각형 영역이 전달됩니다. 이 영역을 이용하여 그려질 레이아웃을 직접 사용할 수 있습니다.

![image](https://github.com/Aumoa/Unity-Extensions/assets/58451385/886f0805-d911-4f25-bcfe-4cee7568edd5)

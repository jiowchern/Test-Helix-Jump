# Test-Helix-Jump

> 用 Unity 原生物理組出來的 Helix Jump 原型：沒有任何射線計算，物理相關的程式碼只有一支 28 行的腳本。
>
> A Helix Jump prototype built on Unity's built-in physics — zero raycasts, and only one 28-line script touches physics.

<!-- TODO: 在這裡放一張遊玩 GIF，例如 docs/demo.gif -->

## 背景

2020 年 1 月，我在有夢娛樂時，製作人請數位程式各自做出 [Helix Jump](https://play.google.com/store/apps/details?id=com.h8games.helixjump) 的原型，當作一次技術評測。

當時大部分人的直覺做法是**每個 frame 從球往下打射線**，自己判斷有沒有碰到平台、再自己算反彈。這種做法會碰到一連串問題：球速太快時穿透平台、反彈高度不穩定、斜面角度要另外算，最後陷入大量參數微調和修 bug。

我的想法剛好相反：**碰撞、重力、反彈本來就是物理引擎的工作，不需要再寫一遍。** 只要把場景和材質設定好，讓 PhysX 去處理，程式只負責物理引擎不會做的部分，也就是操作、鏡頭和關卡生成。

結果大約四天就做完，還加入了隨機關卡（見下方[開發時間軸](#開發時間軸)）。

## 兩種做法的對比

| | 逐 frame 射線計算 | 本專案：交給物理引擎 |
|---|---|---|
| 碰撞偵測 | 自己打射線、自己判斷命中 | Collider ＋ Rigidbody，由 PhysX 處理 |
| 反彈 | 自己算反射向量和速度 | Physic Material 的 `bounciness`，或在碰撞瞬間施加一次衝量 |
| 斜面／特殊平台 | 每種角度都要另外寫邏輯 | 在編輯器裡把物件轉個角度就好 |
| 高速穿透 | 要自己補連續偵測 | Rigidbody 開啟 Continuous Dynamic 即可 |
| 調整手感 | 改程式、重新編譯 | 在 Inspector 調材質數值 |

## 運作方式

### 物理設定

| 物件 | 設定 | 作用 |
|---|---|---|
| 球 `Ball` | Rigidbody（碰撞偵測設為 Continuous Dynamic）＋ Physic Material `Ball`（摩擦 0、彈性 0） | 球本身不帶任何反彈或摩擦，行為完全由它碰到的平台決定；連續碰撞偵測可避免高速穿透 |
| 螺旋平台片段 | 每一片的 MeshCollider 指定 Physic Material `Board`（摩擦 1）或 `Ceiling`（彈性 1），Combine 都設為 Maximum | 平台的彈性與摩擦直接由材質決定，Combine = Maximum 讓平台的設定蓋過球的 0 值。調手感只要改材質數值 |
| 反彈面（含斜板） | [`Reflect.cs`](Helix%20Jump/Assets/Project/Scrips/Reflect.cs) | 碰撞時把球的速度歸零，沿著物件的 `forward` 方向施加一次衝量，讓每次彈跳的高度固定。反彈方向由物件在場景中的擺放角度決定，不需要計算 |

`Reflect.cs` 是整個專案唯一跟物理有關的程式：

```csharp
private void OnCollisionEnter(Collision collision)
{
    if (_CollisionCooldown.Ticks < _CooldownTicks)   // 0.2 秒冷卻，避免一次碰撞觸發多次
        return;

    var rig = collision.gameObject.GetComponent<Rigidbody>();
    rig.velocity = Vector3.zero;
    rig.AddForce(transform.forward * Force, ForceMode.Impulse);

    _CollisionCooldown.Reset();
}
```

### 其他腳本

全部腳本加起來 244 行，物理以外的部分都是物理引擎本來就不會處理的事：

| 腳本 | 行數 | 用途 |
|---|---:|---|
| [`Reflect.cs`](Helix%20Jump/Assets/Project/Scrips/Reflect.cs) | 28 | 碰撞時施加反彈衝量（唯一的物理腳本） |
| [`Rotater.cs`](Helix%20Jump/Assets/Project/Scrips/Rotater.cs) | 38 | 拖曳旋轉螺旋塔，支援滑鼠和觸控 |
| [`LookAt.cs`](Helix%20Jump/Assets/Project/Scrips/LookAt.cs) | 59 | 鏡頭跟隨球，並設定死區（dead zone）避免畫面抖動 |
| [`PieSpawner.cs`](Helix%20Jump/Assets/Project/Scrips/PieSpawner.cs) | 85 | 依種子碼產生隨機關卡 |
| [`ControllBoard.cs`](Helix%20Jump/Assets/Project/Scrips/ControllBoard.cs) | 25 | 測試用 UI：輸入種子碼、層數、間距後重新產生關卡 |
| [`Obstacle.cs`](Helix%20Jump/Assets/Project/Scrips/Obstacle.cs) | 9 | 標記平台的類型（地板／擋板） |

### 隨機關卡

`PieSpawner` 以一個環狀的平台 prefab 為樣板，依指定的層數和間距往下堆疊。每一層會隨機關掉 1–7 塊地板形成缺口，再從剩下的擋板中隨機關掉一部分。

隨機數由**種子碼**決定，所以同一個種子碼一定會產生同一個關卡，方便重現問題，也方便跟企劃討論某一關的手感。

## 執行方式

1. 用 **Unity 2019.4.1f1**（或相容的 2019.4 LTS）開啟 `Helix Jump/` 資料夾。
2. 開啟場景 `Assets/Scenes/SampleScene.unity`，按 Play。
3. 用滑鼠（或手指）左右拖曳來旋轉螺旋塔。
4. 在畫面上的控制面板輸入種子碼、層數、間距，即可重新產生關卡；也可以按隨機按鈕換一個種子碼。

> 專案依賴 `Assets/Project/Plugins/Regulus.Utility.dll`（只用到其中的 `TimeCounter` 計時器），已經包含在 repo 裡。

## 開發時間軸

| 日期 | 內容 |
|---|---|
| 2020-01-05 | 建立專案，完成基本玩法 |
| 2020-01-06 | 降版 Unity，加入斜板 |
| 2020-01-08 | 隨機關卡、種子碼控制面板 |
| 2020-06-30 | 升級至 Unity 2019.4.1f1 |

## 想說明的事

這個專案的程式碼很少，但這正是它想展示的：**先弄清楚引擎已經幫你做好什麼，再決定要寫什麼程式。** 能用設定解決的問題，就不要寫成程式碼。寫得越少，要維護和除錯的東西也越少。

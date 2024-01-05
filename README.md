# 科教館微波爐小遊戲

# 重點程式碼架構解釋

這部分講解程式碼的架構以及未來銜接過程中能使用的Unity Events
## 程式碼架構
`System`: `System` Folder裡面的程式為主要遊戲"管理者"腳色，而之中`System.core` 掌管遊戲進行包括遊戲開始、結束、暫停等等，其餘則是掌管物件，例如`SwimmingPoolManager` 掌管感應區(水池)以及裡面的水分子。
## Unity Component Headers

 在Unity component 我對於每個參數進行分類
 - Configuration: 這裡的參數表示拿來調整遊戲設定
 - State: 物件內部的狀態，這些數值只能讀取不能調整
 - Dependency: 表示這物件所需要的子物件
 - Dual Dependency: 這比較特別，表示已謝物件和此物件互相依賴，是依賴性最強所以通常我們會把這些Component放在同一個Unity 物件之中。
 ## Scriptable Object Configuration
 我盡量把遊戲參數調整放入Scriptable Object中，這樣日後比較好設定，不用一直在Scene中找來找去，目前將設定放在`Configurations` folder中，例如: 水分子熱度等級時狀態的設定、泳池生成水分子概率。
 ## Microwave Game Manager
 `MicrowaveGameManager` Unity 物件是掌控遊戲邏輯主物件，在這物件中，有加入一些幫助測試的按鈕在Editor

在`MicrowaveGameManager` 可以控制開始以及結束遊戲:

 ![image](https://github.com/easonyu0203/amus-micro-gourmet/assets/62385417/98d65cd0-ac82-4727-9605-81cb2bdecbb2)


`Test Game Controller` 可以控制加熱那些水池

![image](https://github.com/easonyu0203/amus-micro-gourmet/assets/62385417/e574fe6f-04eb-4a9b-b628-0fd40f8d87d3)

## Unity Events
我有開幾個未來可能用到的Events,例如在`GameManager`中有遊戲開始結束的Events，`GameManager.SpwanManager`中有水分子死掉的Event(未來可做為觸發計分)

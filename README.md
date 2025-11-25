## 前端網頁部分
https://github.com/tohousanae/am3buger-vue

## 使用案例圖
<img width="1004" height="496" alt="image" src="https://github.com/user-attachments/assets/dcd1dde9-d22b-4665-b629-67e7f96455cb" />
<img width="1004" height="933" alt="image" src="https://github.com/user-attachments/assets/a871bf28-7a94-4883-8eca-7c759fcf448b" />
<img width="1004" height="414" alt="image" src="https://github.com/user-attachments/assets/1d3b5d3b-7470-448c-a64b-fdcf06b432f9" />
<img width="1004" height="496" alt="image" src="https://github.com/user-attachments/assets/ae232d77-3b93-431e-a65f-138ce153ba96" />
<img width="1004" height="735" alt="image" src="https://github.com/user-attachments/assets/f1373f19-09f5-4f36-9b80-f8cd253a8a56" />
<img width="1004" height="659" alt="image" src="https://github.com/user-attachments/assets/38f440ce-3b99-43d8-bd0d-4aeb21c86816" />
<img width="1004" height="559" alt="image" src="https://github.com/user-attachments/assets/ea5df24f-43ca-460a-b07d-edaabe5c8d3b" />
<img width="1004" height="738" alt="image" src="https://github.com/user-attachments/assets/33d0c7a2-d1af-4835-9891-f5e22b70d9fe" />
<img width="1004" height="738" alt="image" src="https://github.com/user-attachments/assets/b8bd4e72-2759-418f-a01c-91f1e7e9ddc7" />

## ER圖(請點開大圖觀看)
<img width="4844" height="2567" alt="Er圖-第一階段開發" src="https://github.com/user-attachments/assets/875ef389-1961-4ba7-84db-f51a89e88ba8" />

## 資料庫圖表(請點開大圖觀看)
<img width="2377" height="4905" alt="資料庫圖表" src="https://github.com/user-attachments/assets/69e9eacc-1762-4bce-a45f-d67771ae4aea" />

## API操作示範
### 會員功能
#### 登入
1. 完成會員信箱與手機驗證的會員，可正常登入，否則提醒會員未完成信箱或手機驗證&nbsp;  
<img width="1439" height="929" alt="image" src="https://github.com/user-attachments/assets/af9399e7-74d7-47a3-8b20-70abf978004e" />&nbsp;  
<img width="1440" height="965" alt="image" src="https://github.com/user-attachments/assets/312c664d-7e01-4a35-8986-60a69554eb8a" />&nbsp;  

#### 註冊
1. 建立帳號並發送註冊驗證信&nbsp;  
<img width="1488" height="728" alt="image" src="https://github.com/user-attachments/assets/c12c3590-5108-4548-8153-40eb788442e1" />&nbsp;  
<img width="1466" height="797" alt="image" src="https://github.com/user-attachments/assets/ebe1fbd1-3294-4164-835a-ac75476323fe" />&nbsp;  

2. 點擊信箱連結並驗證會員&nbsp;  
2.1 收到驗證信&nbsp;  
<img width="1230" height="486" alt="image" src="https://github.com/user-attachments/assets/6128624d-db05-4ab4-ba3d-48d0d4e118a1" />&nbsp;  
2.2 輸入從信件收到的token，另外，從連結取得token字串的程序由前端處理，後端只負責接收token值。&nbsp;  
<img width="1503" height="960" alt="image" src="https://github.com/user-attachments/assets/4836752d-3970-46b2-8275-e46a28c29086" />&nbsp;  
<img width="1464" height="738" alt="image" src="https://github.com/user-attachments/assets/35ad8e4f-b4de-4edb-9406-9fe51cd84066" />&nbsp;  

3. 驗證註冊會員手機號碼&nbsp;  
3.1 發送手機驗證簡訊&nbsp;  
<img width="1429" height="962" alt="1" src="https://github.com/user-attachments/assets/ad96210d-0d61-4e6f-8bed-810979572365" />
<img width="1510" height="888" alt="2" src="https://github.com/user-attachments/assets/6dba854c-92b0-43b5-8670-4b7c189d9073" />

3.2 輸入收到的簡訊驗證碼&nbsp;  
<img width="1448" height="881" alt="image" src="https://github.com/user-attachments/assets/2f6807f7-a504-4ffe-ae8d-74d96d0800a3" />&nbsp;  
<img width="1462" height="926" alt="image" src="https://github.com/user-attachments/assets/4952a262-e671-405a-81ff-bc4c38c97db1" />&nbsp;  

#### 忘記密碼
1. 發送忘記密碼驗證信&nbsp;  
<img width="1463" height="962" alt="image" src="https://github.com/user-attachments/assets/edfa5165-9219-4104-9dec-bc3b720d3441" />&nbsp;  
<img width="1512" height="835" alt="image" src="https://github.com/user-attachments/assets/76accb41-3ea6-47e6-89c0-d9d3bdf30fab" />&nbsp;  

1.1 收到忘記密碼驗證信&nbsp;  
<img width="973" height="263" alt="image" src="https://github.com/user-attachments/assets/a9bcf73f-e7b9-4c6d-ae29-1dd7c3e702aa" />&nbsp;  
1.2 輸入從信箱取得的token進行驗證，模擬點擊重設密碼連結時驗證連結是否有效與是否過期，從連結取得token字串的程序由前端處理，後端只負責接收token值。&nbsp;  
<img width="1455" height="951" alt="image" src="https://github.com/user-attachments/assets/a37c3601-ed9c-4054-89c0-bd99331dbdc5" />&nbsp;  
1.3 重設密碼，並在送出表單時再次驗證token是否有效。&nbsp;  
<img width="1539" height="869" alt="image" src="https://github.com/user-attachments/assets/078c4b83-85a2-4098-9b32-eff9985d8ec6" /><img width="1481" height="944" alt="image" src="https://github.com/user-attachments/assets/536c6f33-86cb-402b-8dfc-46c950f0a387" />&nbsp;  
<img width="1588" height="968" alt="image" src="https://github.com/user-attachments/assets/b94df07b-5b05-440f-9c98-1862baacb186" />&nbsp;  

### 商品相關功能

### 練團室與樂器租借相關功能

### 表演/活動預約相關功能

### 商家後臺相關功能


## 使用技術
1. .net core 8
2. c#
3. SQL Server

## 參考引用資料
1. https://ithelp.ithome.com.tw/articles/10307773
2. https://github.com/tohousanae/LifetimeLiveHouse-WebApi/blob/main/HatsuneMikuShopWebAPI/Areas/User/Controllers/HomeController.cs

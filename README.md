# MyEvernote
  ```javascript
Database'i kullanabilmek için Web.config dosyasnda yer alan;

  <connectionStrings>
                 <add name="DatabaseContext" providerName="System.Data.SqlClient" connectionString="Server=AHMET-ERKAN;     Database=MyEvernoteDB; Integrated Security=True"/>
  </connectionStrings> 

 Kod blogundaki connectionString içindeki server'a kendi database server adınızı yazmanız gerekmektedir.
    
 Onay mailini gönderebilmek için yine web.config'de yer alan 
    
    <add key ="MailHost" value="smtp.gmail.com"/>
    <add key="MailPort" value="587"/>
    <add key="MailUser" value="1ahmeterkan1@gmail.com"/>
    <add key="MailPass" value="sifre"/>
    <add key="SiteRootUrl" value="https://localhost:44384"/>
    
Kod blogunda MailUser'ın value'suna kendi mail adresinizi ve MailPass'in value'suna o mail adresinin şifresinizi yazmanız gerekmektedir.
Ayrıca mail adresinden gerekli izinleri açmanız gerekmektedir.


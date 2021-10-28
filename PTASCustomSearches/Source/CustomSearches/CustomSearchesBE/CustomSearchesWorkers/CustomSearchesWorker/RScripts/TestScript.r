library("RODBC") 

#Connection string
RODBC_connection <- odbcDriverConnect('driver={SQL Server};server=ptas-sbox-dbserver.database.windows.net;database=PTAS-SBOX-Database;Uid=dbadmin;Pwd=KUTcFrGRYJwS01')

# e.g. with a server called "Cliff" and a database called "Richard" your string would be: 
# driver={SQL Server};server=Cliff;database=Richard;trusted_connection=true')                            

#dt1 <- sqlFetch(channel=RODBC_connection, sqtable = "dbo.ScriptPOCTestData") 
#summary(dt1)

# Load data from SQL query 
dt2 <- sqlQuery(channel=RODBC_connection, query = "select * from dbo.ScriptPOCTestData") 

result <- (dt2$Sale_Price*dt2$AV_Ratio)
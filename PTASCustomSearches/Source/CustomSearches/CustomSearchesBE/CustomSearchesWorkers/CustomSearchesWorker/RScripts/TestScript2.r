library("RODBC") 

#Connection string
RODBC_connection <- odbcDriverConnect('driver={SQL Server};server=ptas-sbox-dbserver.database.windows.net;database=PTAS-SBOX-Database;Uid=dbadmin;Pwd=KUTcFrGRYJwS01')

# e.g. with a server called "Cliff" and a database called "Richard" your string would be: 
# driver={SQL Server};server=Cliff;database=Richard;trusted_connection=true')                            

#dt1 <- sqlFetch(channel=RODBC_connection, sqtable = "dbo.ScriptPOCTestData") 
#summary(dt1)

# Load data from SQL query 
dt2 <- sqlQuery(channel=RODBC_connection, query = "select * from dbo.ScriptPOCTestData") 

# Get the 2 columns 'AV Ratio' and 'Valuation Date Minus Sale Date'
input <- dt2[,c('AV_Ratio', 'Valuation_Date_Minus_Sale_date')]
# Create a local png file
png(file = "{outputFileName}")
# Draw the plot
plot(x = input$AV_Ratio, y=input$Valuation_Date_Minus_Sale_date, xlab="Date", ylab="Ratio")
# Save the image
dev.off()
# Done

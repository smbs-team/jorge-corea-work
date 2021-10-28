# libraries
library(ggplot2)
library(rmarkdown)

# Test Code Section ---------------------------------
# Any code added here will be removed by the server execution.

library(jsonlite)

executingFolder <- "C:/RScript/Tmp"
parametersFileName <- "parameters.json"
datasetFileName <- "dataset.csv"
datasetSeparatorChar <- ","
setwd(executingFolder)
paramJson <- fromJSON(parametersFileName) # Clean the dataset by removing the "removed"
dataset <- read.csv(file = datasetFileName, sep = datasetSeparatorChar)
dataset <- subset(dataset, dataset$Included.Or.Removed != "Removed") # Clean the dataset by removing the "removed"
jsonResultFileName <- "results.json"

# Server Code Section ---------------------------------
# This section will be executed by the server.
file.copy("AppraiserRatioReportTemplate.pdf", "AppraiserRatioReport.pdf")
# Prepare the result - The list structure allows to create relationships between each object
results <- list("result" = list( ),
                "fileResults" = list(
                    list("Type" = "Report", "Title" = "Appraiser Ratios Report", "FileName" = "AppraiserRatioReport.pdf", Description = "Appraiser Ratios Report (PDF)")))
                
# Export the JSON file to results.json
writeLines(toJSON(results, pretty=TRUE, auto_unbox=TRUE, digits=16), jsonResultFileName)

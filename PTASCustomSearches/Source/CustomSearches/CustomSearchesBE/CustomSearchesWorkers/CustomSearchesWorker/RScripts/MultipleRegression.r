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
formulaText <- paste0(paramJson$IndependentVariable, " ~ ", paramJson$DependentVariable)

print(paste0("Regression formula: ", formulaText))

# Calculate a simple linear regression

linearRegression <- lm(as.formula(formulaText), dataset, na.action = na.exclude)

# generate a chart
ggplot(dataset, aes(x = eval(as.name(paramJson$IndependentVariable)), y = eval(as.name(paramJson$DependentVariable)))) + 
  geom_point(pch=17, color="blue", size=2) +
  geom_smooth(method="lm", color = "red", linetype = 1) +
  scale_x_continuous(breaks=waiver()) +
  labs(title=paste0("Example of ", paramJson$DependentVariable), x = paramJson$IndependentVariable, y = paramJson$DependentVariable)
# save the chart
ggsave("linearChart.png")

# generate a second chart
ggplot(dataset, aes(x = eval(as.name(paramJson$IndependentVariable)), y = eval(as.name(paramJson$DependentVariable)))) + 
  geom_point(pch=17, color="green", size=1) +
  scale_x_continuous(breaks=waiver()) +
  labs(title=paste0("Example of ", paramJson$DependentVariable), x = paramJson$IndependentVariable, y = paramJson$DependentVariable)
# save the chart
ggsave("linearChart2.png")

# Generate the report for linear regression - first time to get the HTML format
render("linearRegression.rmd", output_format="html_document")
# Second time to get the Word format
render("linearRegression.rmd", output_format="word_document")

# Prepare the result - The list structure allows to create relationships between each object
results <- list("results" = list(
                    list(name="regression_results", "Intercept" = coefficients(linearRegression)[1],"Coefficient" = coefficients(linearRegression)[2] ),
                    list(name="regression_goodness_of_fit", "AdjustedRSquared" = 0.39, "RSquared" = 0.35 )),
                "fileResults" = list(
                    list("Type" = "Chart", "Title" = "Regression Chart", "FileName" = "linearChart.png", Description = "First Chart Example"),
                    list("Type" = "Chart", "Title" = "Regression Chart", "FileName" = "linearChart2.png", Description = "First Chart Example"),
                    list("Type" = "Report", "Title" = "Regression Report", "FileName" = "linearRegression.docx", Description = "Linear Regression Word Report"),
                    list("Type" = "Report", "Title" = "Regression Report", "FileName" = "linearRegression.html", Description = "Linear Regression HTML Report")))
                
# Export the JSON file to results.json
writeLines(toJSON(results, pretty=TRUE, auto_unbox=TRUE, digits=16), jsonResultFileName)

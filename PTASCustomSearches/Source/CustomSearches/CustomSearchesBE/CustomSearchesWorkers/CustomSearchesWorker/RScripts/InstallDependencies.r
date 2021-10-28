.libPaths( c( "{LibrariesPath}" , .libPaths() ) )
packages <- c(
    "RODBC",
    "ggplot2",
    "rmarkdown",
    "jsonlite",
    "tidyverse",
    "dplyr",
    "jtools",
    "kableExtra",
    "plotrix",
    "lubridate",
    "tibbletime",
    "xts",
    "anytime",
    "zoo",
    "plotly",
    "magrittr",
    "knitr",
    "MASS",
    "remotes",
    "clustMixType",
    "dbscan",
    "stats",
    "utils",
    "ccao",
    "covr",
    "devtools",
    "DT",
    "forcats",
    "lintr",
    "pkgdown",
    "scales",
    "sf",
    "testthat",
    "tidyr",
    "rcompanion",
    "DescTools",
    "hrbrthemes",
    "viridis",
    "mgcViz",
    "rgl",
    "data.table",
    "broom",
    "psych",
    "lme4",
    "nlme",
    "caret",
    "glmnet",
    "randomForest",
    "sp",
    "geoR",
    "gstat",
    "car",
    "cli", # Needs to install before tidymodels
    #"tidymodels",
    "RColorBrewer",
    "gganimate",
    "class",
    "cluster",
    "kknn",
    "rpart",
    "rgdal",
    "raster",
    "maps",
    "ggmap",
    "ISLR",
    "ElemStatLearn",
    "arm",
    "gridExtra",
    "ggthemes",
    "ggpubr",
    "GGally",
    "grid",
    "olsrr",
    "fBasics",
    "lmSubsets")

installedLib <- rownames(installed.packages(lib.loc = "{LibrariesPath}"))
installedGlobal <- rownames(installed.packages())
installed = c(installedLib, installedGlobal)

toInstall <- setdiff(packages, installed )

print(paste("Installed Packages: ", toString(installed)))
print(paste("Required Packages: ", toString(packages)))
print(paste("Packages to Install: ", toString(toInstall)))

if (length(toInstall) > 0)
{
	install.packages(toInstall, lib = "{LibrariesPath}")  	
}

Sys.setenv(RSTUDIO_PANDOC="{PandocPath}")
$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

az account set --subscription e2e5ecc4-5bf0-4dc0-9d85-fdbcaebd8740
az acr login --name dunphyacrtest3                      
docker build -t servercore/customsearchesworker-uat:1.51 "$($scriptPath)\..\..\Docker" 
docker tag servercore/customsearchesworker-uat:1.51 dunphyacrtest3.azurecr.io/servercore/customsearchesworker-uat:1.51
docker push dunphyacrtest3.azurecr.io/servercore/customsearchesworker-uat:1.51

kubectl apply -f .\Namespace.yaml --namespace ptas-uat
kubectl apply -f .\StorageSecret.yaml --namespace ptas-uat
kubectl apply -f .\AzureIdentity.yaml --namespace ptas-uat
kubectl apply -f .\AzureIdentityBinding.yaml --namespace ptas-uat
kubectl apply -f .\CustomSearchesWorker.yaml --namespace ptas-uat
kubectl apply -f .\MapServerService.yaml --namespace ptas-uat
kubectl apply -f .\MapServerRestartJob.yaml --namespace ptas-uat
kubectl apply -f .\CustomSearchesJob.yaml --namespace ptas-uat
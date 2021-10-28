$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

az account set --subscription e2e5ecc4-5bf0-4dc0-9d85-fdbcaebd8740
az acr login --name dunphyacrtest3                      
docker build -t servercore/customsearchesworker-sbox:1.70 "$($scriptPath)\..\..\Docker" 
docker tag servercore/customsearchesworker-sbox:1.70 dunphyacrtest3.azurecr.io/servercore/customsearchesworker-sbox:1.70
docker push dunphyacrtest3.azurecr.io/servercore/customsearchesworker-sbox:1.70

kubectl apply -f .\Namespace.yaml --namespace ptas-sbox
kubectl apply -f .\StorageSecret.yaml --namespace ptas-sbox
kubectl apply -f .\AzureIdentity.yaml --namespace ptas-sbox
kubectl apply -f .\AzureIdentityBinding.yaml --namespace ptas-sbox
kubectl apply -f .\CustomSearchesWorker.yaml --namespace ptas-sbox
kubectl apply -f .\MapServerService.yaml --namespace ptas-sbox
kubectl apply -f .\MapServerRestartJob.yaml --namespace ptas-sbox
kubectl apply -f .\CustomSearchesJob.yaml --namespace ptas-sbox
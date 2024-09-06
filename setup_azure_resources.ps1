# Azure Resource Setup Script

# Set these variables according to your preferences
$RESOURCE_GROUP = "SustainableMaterialsRG"
$LOCATION = "eastus"
$APP_NAME = "sustainable-materials-app"
$SQL_SERVER_NAME = "sustainable-materials-sql"
$SQL_DB_NAME = "SustainableMaterialsDB"
$REDIS_NAME = "sustainable-materials-redis"
$KEYVAULT_NAME = "sustainable-materials-kv"
$APP_INSIGHTS_NAME = "sustainable-materials-insights"

# Get connection strings
$SQL_CONNECTION_STRING = az sql db show-connection-string --server sustainable-materials-sql --name $SQL_DB_NAME --client ado.net --output tsv
$SQL_CONNECTION_STRING = $SQL_CONNECTION_STRING.Replace("<username>", "sqladmin").Replace("<password>", "P@ssw0rd123!")

$REDIS_CONNECTION_STRING = az redis list-keys --name $REDIS_NAME --resource-group $RESOURCE_GROUP --query primaryKey --output tsv

$APP_INSIGHTS_KEY = az monitor app-insights component show --app $APP_INSIGHTS_NAME --resource-group $RESOURCE_GROUP --query instrumentationKey --output tsv

# Store in Key Vault
az keyvault secret set --vault-name $KEYVAULT_NAME --name "SqlConnectionString" --value $SQL_CONNECTION_STRING
az keyvault secret set --vault-name $KEYVAULT_NAME --name "RedisConnectionString" --value $REDIS_CONNECTION_STRING
az keyvault secret set --vault-name $KEYVAULT_NAME --name "ApplicationInsightsKey" --value $APP_INSIGHTS_KEY

# Configure Web App settings
az webapp config appsettings set --name $APP_NAME --resource-group $RESOURCE_GROUP --settings AZURE_KEY_VAULT_ENDPOINT="https://${KEYVAULT_NAME}.vault.azure.net/" ASPNETCORE_ENVIRONMENT="Production"

# Enable managed identity for the Web App
az webapp identity assign --name $APP_NAME --resource-group $RESOURCE_GROUP

# Get the principal ID of the Web App's managed identity
$PRINCIPAL_ID = az webapp identity show --name $APP_NAME --resource-group $RESOURCE_GROUP --query principalId --output tsv

# Grant the Web App access to Key Vault
az keyvault set-policy --name $KEYVAULT_NAME --object-id $PRINCIPAL_ID --secret-permissions get list

Write-Host "Azure resources created and configured successfully!" -ForegroundColor Green
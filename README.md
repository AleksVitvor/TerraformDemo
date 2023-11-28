# SimpleDotnetWebAPI
## How to deploy infrastructure via Terraform

1. Open PowerShell in folder with terraform code
2. Create `terraform.tfvars` file and provide all required variables in this file (all variables described in `variables.tf`)
3. Apply 3 terraform commands:
- `terraform init` - initializes a working directory containing configuration files and installs plugins for required providers
- `terraform plan` - creates an execution plan, which lets you preview the changes that Terraform plans to make to your infrastructure
- `terraform apply` - executes the actions proposed in a Terraform plan to create, update, or destroy infrastructure

#### How to set up Azurerm provider

1. Use this [article](https://learn.microsoft.com/en-us/azure/developer/terraform/get-started-cloud-shell-bash) to get everything that you need for azurerm provider
2. Add `azure-subscription-id`, `azure-client-id`, `azure-client-secret`, `azure-tenant-id` to `terraform.tfvars` file

### How API will be deployed

1. Apply infrastructure changes
2. Get publish profile of created service app (possible Azure PowerShell command `Get-AzWebAppPublishingProfile`)
3. Use publish profile for deploy via `azure/webapps-deploy@v2`:
- you should provide `app-name`, `slot-name`, `publish-profile`, `package`
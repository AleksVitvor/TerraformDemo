variable "example-vnet-cidr" {
  type        = string
  description = "The CIDR of the VNET"
}
variable "example-db-subnet-cidr" {
  type        = string
  description = "The CIDR for the Backoffice subnet"
}
variable "example-private-dns" {
  type        = string
  description = "The private DNS name"
}
variable "example-dns-privatelink" {
  type        = string
  description = "SQL DNS Private Link"
}

variable "azure-subscription-id" {
  type = string
  description = "Azure Subscription ID"
}
variable "azure-client-id" {
  type = string
  description = "Azure Client ID"
}
variable "azure-client-secret" {
  type = string
  description = "Azure Client Secret"
}
variable "azure-tenant-id" {
  type = string
  description = "Azure Tenant ID"
}

variable "location" {
  type = string
  description = "Location for resource group"
}

variable "sql-server-login" {
  type = string
  description = "Login for sql server"
}

variable "sql-server-password" {
  type = string
  description = "Login for sql server"
}

variable "sql-server-sku" {
  type = string
  description = "SKU for SQL Server"
}

variable "service-plan-sku" {
  type = string
  description = "SKU for Azure Service plan"
}
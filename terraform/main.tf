provider "azurerm" {
  features {}
  subscription_id = var.azure-subscription-id
  client_id = var.azure-client-id
  client_secret = var.azure-client-secret
  tenant_id = var.azure-tenant-id
}

resource "azurerm_resource_group" "example" {
  name     = "example-resources"
  location = var.location
}

resource "azurerm_storage_account" "example" {
  name                     = "exampleav250101"
  resource_group_name      = azurerm_resource_group.example.name
  location                 = azurerm_resource_group.example.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_virtual_network" "example-vnet" {
  name = "example-sql-vnet"
  address_space = [var.example-vnet-cidr]
  location = azurerm_resource_group.example.location
  resource_group_name = azurerm_resource_group.example.name
}

resource "azurerm_subnet" "example-db-subnet" {
  name = "example-sql-db-subnet"
  address_prefixes = [var.example-db-subnet-cidr]
  virtual_network_name = azurerm_virtual_network.example-vnet.name
  resource_group_name = azurerm_resource_group.example.name
  enforce_private_link_endpoint_network_policies = true
}

resource "azurerm_private_dns_zone" "example-private-dns" {
  name = var.example-private-dns
  resource_group_name = azurerm_resource_group.example.name
}

resource "azurerm_private_dns_zone_virtual_network_link" "example-private-dns-link" {
  name = "example-vnet"
  resource_group_name = azurerm_resource_group.example.name
  private_dns_zone_name = azurerm_private_dns_zone.example-private-dns.name
  virtual_network_id = azurerm_virtual_network.example-vnet.id
}

resource "azurerm_private_dns_zone" "example-endpoint-dns-private-zone" {
  name = "${var.example-dns-privatelink}.database.windows.net"
  resource_group_name = azurerm_resource_group.example.name
}

resource "azurerm_mssql_server" "example" {
  name                         = "example-sqlserver-av2501"
  resource_group_name          = azurerm_resource_group.example.name
  location                     = azurerm_resource_group.example.location
  version                      = "12.0"
  administrator_login          = "4dm1n157r470r"
  administrator_login_password = "4-v3ry-53cr37-p455w0rd"
  public_network_access_enabled = false
}

resource "azurerm_mssql_database" "test" {
  name           = "acctest-db-d"
  server_id      = azurerm_mssql_server.example.id
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  license_type   = "LicenseIncluded"
  max_size_gb    = 2
  read_scale     = false
  sku_name       = "Basic"
  zone_redundant = false

  tags = {
    foo = "bar"
  }
}

resource "azurerm_private_endpoint" "example-db-endpoint" {
  depends_on = [azurerm_mssql_server.example]
  name = "example-sql-db-endpoint"
  location = azurerm_resource_group.example.location
  resource_group_name = azurerm_resource_group.example.name
  subnet_id = azurerm_subnet.example-db-subnet.id
  private_service_connection {
    name = "example-sql-db-endpoint"
    is_manual_connection = "false"
    private_connection_resource_id = azurerm_mssql_server.example.id
    subresource_names = ["sqlServer"]
  }
}

data "azurerm_private_endpoint_connection" "example-endpoint-connection" {
  depends_on = [azurerm_private_endpoint.example-db-endpoint]
  name = azurerm_private_endpoint.example-db-endpoint.name
  resource_group_name = azurerm_resource_group.example.name
}

resource "azurerm_private_dns_a_record" "example-endpoint-dns-a-record" {
  depends_on = [azurerm_mssql_server.example]
  name = lower(azurerm_mssql_server.example.name)
  zone_name = azurerm_private_dns_zone.example-endpoint-dns-private-zone.name
  resource_group_name = azurerm_resource_group.example.name
  ttl = 300
  records = [data.azurerm_private_endpoint_connection.example-endpoint-connection.private_service_connection.0.private_ip_address]
}

resource "azurerm_private_dns_zone_virtual_network_link" "dns-zone-to-vnet-link" {
  name = "example-sql-db-vnet-link"
  resource_group_name = azurerm_resource_group.example.name
  private_dns_zone_name = azurerm_private_dns_zone.example-endpoint-dns-private-zone.name
  virtual_network_id = azurerm_virtual_network.example-vnet.id
}

resource "azurerm_service_plan" "example" {
  name                = "example"
  resource_group_name = azurerm_resource_group.example.name
  location            = azurerm_resource_group.example.location
  os_type             = "Linux"
  sku_name            = "F1"
}

resource "azurerm_linux_web_app" "example" {
  name                = "example-av2501"
  resource_group_name = azurerm_resource_group.example.name
  location            = azurerm_service_plan.example.location
  service_plan_id     = azurerm_service_plan.example.id

  site_config {
	always_on = false
    application_stack {
		dotnet_version = "6.0"
	}
  }
}
output "sql_private_link_endpoint_ip" {
  description = "SQL Private Link Endpoint IP"
  value = data.azurerm_private_endpoint_connection.example-endpoint-connection.private_service_connection.0.private_ip_address
}
output "sql_db" {
  description = "SQL Server DB and Database"
  value = "${azurerm_mssql_server.example.name}/${azurerm_mssql_database.test.name}"
}
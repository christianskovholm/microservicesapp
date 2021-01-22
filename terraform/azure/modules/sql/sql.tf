locals {
  sql_name = "sql-${var.suffix}"
}

resource "azurerm_sql_server" "sql" {
  name                         = local.sql_name
  location                     = var.location
  resource_group_name          = var.rg_name
  version                      = "12.0"

  administrator_login          = var.sql_admin_username
  administrator_login_password = var.sql_admin_password

  tags = var.tags
}

resource "azurerm_sql_database" "organization_service" {
  name                = "OrganizationsDb"
  location            = var.location
  resource_group_name = var.rg_name
  server_name         = azurerm_sql_server.sql.name

  edition             = "Basic"
  max_size_gb         = 2

  tags = var.tags

  depends_on = [azurerm_sql_server.sql]
}
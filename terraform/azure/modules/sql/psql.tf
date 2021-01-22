locals {
  psql_name         = "psql-${var.suffix}"
  psql_db_collation = "English_United States.1252"
  psql_db_charset   = "UTF8"
}

resource "azurerm_postgresql_server" "psql" {
  name                = local.psql_name
  location            = var.location
  resource_group_name = var.rg_name

  administrator_login          = var.psql_admin_username
  administrator_login_password = var.psql_admin_password

  version    = var.psql_version
  sku_name   = "GP_Gen5_2"
  storage_mb = 5120

  auto_grow_enabled            = false
  ssl_enforcement_enabled      = false
  geo_redundant_backup_enabled = false
}

resource "azurerm_postgresql_database" "activity_service" {
  name                = "activity_service"
  resource_group_name = var.rg_name
  server_name         = azurerm_postgresql_server.psql.name
  charset             = local.psql_db_charset
  collation           = local.psql_db_collation

  depends_on = [azurerm_postgresql_server.psql]
}

resource "azurerm_postgresql_database" "idea_service" {
  name                = "idea_service"
  resource_group_name = var.rg_name
  server_name         = azurerm_postgresql_server.psql.name
  charset             = local.psql_db_charset
  collation           = local.psql_db_collation

  depends_on = [azurerm_postgresql_server.psql]
}
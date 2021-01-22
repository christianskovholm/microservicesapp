locals {
  acr_name = replace("acr${var.suffix}", "-", "")
}

resource "azurerm_container_registry" "acr" {
  name                     = local.acr_name
  resource_group_name      = var.rg_name
  location                 = var.location
  sku                      = "Basic"
  admin_enabled            = false
}
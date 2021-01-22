locals {
  aks_name = "aks-${var.suffix}"
}

resource "azurerm_kubernetes_cluster" "aks" {
  name                = local.aks_name
  resource_group_name = var.rg_name
  location            = var.location

  kubernetes_version  = var.aks_kubernetes_version
  node_resource_group = local.rg_aks_name
  dns_prefix          = "aks"

  linux_profile {
    admin_username = var.aks_linux_profile_admin_username

    ssh_key {
      key_data = file(var.aks_public_ssh_key_path)
    }
  }

  addon_profile {
    http_application_routing {
      enabled = false
    }

    kube_dashboard {
      enabled = true
    }
  }

  default_node_pool {
    name            = "agentpool"
    node_count      = var.aks_node_count
    vm_size         = var.aks_node_vm_size
    os_disk_size_gb = var.aks_node_disk_size
  }

  service_principal {
    client_id     = var.aks_sp_app_id
    client_secret = var.aks_sp_client_secret
  }

  network_profile {
    network_plugin     = "azure"
    dns_service_ip     = "10.0.0.10"
    docker_bridge_cidr = "172.17.0.1/16"
    service_cidr       = "10.0.0.0/16"
  }

  tags = var.tags
}

resource "azurerm_role_assignment" "network_contributor" {
  scope                = azurerm_kubernetes_cluster.aks.id
  role_definition_name = "Network Contributor"
  principal_id         = var.aks_sp_object_id

  depends_on = [azurerm_kubernetes_cluster.aks]
}
variable "sp_app_id" {
    description = "Application ID of the main Service Principal."
}

variable "sp_client_secret" {
    description = "Client secret of the main Service Principal."
}

variable "sp_object_id" {
    description = "Object ID of the main Service Principal."
}

variable "sql_name" {
    description = "Name of the SQL Server."
}

variable "sql_admin_username" {
    description = "Username for the SQL Server admin user."
}

variable "sql_admin_password" {
    description = "Password for the SQL Server admin user."
}

variable "psql_admin_username" {
  description = "Username of the postgresql server admin."
}

variable "psql_admin_password" {
  description = "Password of the postgresql server admin."
}
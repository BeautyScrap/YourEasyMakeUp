provider "mongodbatlas" {
  public_key  = var.atlas_pub_key
  private_key = var.atlas_priv_key
}

resource "mongodbatlas_project" "yourEasyRent" {
  name   = var.project_id
  org_id = var.atlas_org_id
}

resource "mongodbatlas_project_ip_access_list" "acl" {
  project_id = mongodbatlas_project.yourEasyRent.id
  cidr_block = "0.0.0.0/0"
}

# currently free tier is used, can be migrated to a serverless instance
resource "mongodbatlas_cluster" "cluster" {
  project_id = mongodbatlas_project.yourEasyRent.id
  name       = var.project_id

  provider_name               = "TENANT"
  backing_provider_name       = "GCP"
  provider_region_name        = var.atlas_cluster_region
  provider_instance_size_name = var.atlas_cluster_tier
}

resource "mongodbatlas_database_user" "user" {
  project_id         = mongodbatlas_project.yourEasyRent.id
  auth_database_name = "admin"

  username = var.db_user
  password = random_string.mongodb_password.result

  roles {
    role_name     = "readWrite"
    database_name = var.db_name
  }
}

locals {
  # the demo app only takes URIs with the credentials embedded and the atlas
  # provider doesn't give us a good way to get the hostname without the protocol
  # part so we end up doing some slicing and dicing to get the creds into the URI
  atlas_uri = replace(
    mongodbatlas_cluster.cluster.srv_address,
    "://",
    "://${var.db_user}:${mongodbatlas_database_user.user.password}@"
  )
}
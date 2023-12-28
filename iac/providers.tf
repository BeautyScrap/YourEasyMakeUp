terraform {
  backend "gcs" {
    bucket = "beauty-scrp-bucket-tfstate"
    prefix = "terraform/state"
  }

  required_version = ">= 0.14"

  required_providers {
    google = {
      version = ">= 3.3"
      source  = "hashicorp/google"
    }

    mongodbatlas = {
      version = "~> 1.4.5"
      source  = "mongodb/mongodbatlas"
    }
  }
}

provider "google" {
  project = var.project_id
  region  = var.region
}

# provider "mongodbatlas" {
#   public_key  = var.atlas_pub_key
#   private_key = var.atlas_priv_key
# }